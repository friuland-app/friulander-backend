const db = require('../config/database');
const admin = require('firebase-admin');
const battleService = require('../services/battleService');

const startBattle = async (req, res) => {
  try {
    const { playerId, enemyCreatureId, playerCreatureId } = req.body;
    
    const enemyDoc = await db.collection('creatures').doc(enemyCreatureId).get();
    if (!enemyDoc.exists) {
      return res.status(404).json({ error: 'Enemy creature not found' });
    }
    
    const playerDoc = await db.collection('users').doc(playerId).get();
    if (!playerDoc.exists) {
      return res.status(404).json({ error: 'Player not found' });
    }
    
    const battleData = {
      player_id: playerId,
      player_creature_id: playerCreatureId,
      enemy_creature_id: enemyCreatureId,
      enemy_level: Math.floor(Math.random() * 50) + 1,
      player_hp: 100,
      enemy_hp: 100,
      turn: 'player',
      state: 'active',
      created_at: new Date().toISOString(),
      moves: []
    };
    
    const docRef = await db.collection('battles').add(battleData);
    
    res.json({ id: docRef.id, ...battleData });
  } catch (error) {
    console.error('Error starting battle:', error);
    res.status(500).json({ error: 'Failed to start battle' });
  }
};

const executeMove = async (req, res) => {
  try {
    const { id } = req.params;
    const { playerId, moveType, damage } = req.body;
    
    const battleDoc = await db.collection('battles').doc(id).get();
    if (!battleDoc.exists) {
      return res.status(404).json({ error: 'Battle not found' });
    }
    
    const battle = battleDoc.data();
    
    const validation = battleService.validateMove(battle, { player_id: playerId, hp: battle.player_hp });
    if (!validation.valid) {
      return res.status(400).json({ error: validation.reason });
    }
    
    const attacker = battle.turn === 'player' ? 'player' : 'enemy';
    const defender = battle.turn === 'player' ? 'enemy' : 'player';
    
    const calculatedDamage = battleService.calculateDamage(
      { stats: { attack: 10 } },
      { stats: { defense: 10 }, type: 'water' },
      moveType
    );
    
    if (attacker === 'player') {
      battle.enemy_hp = Math.max(0, battle.enemy_hp - calculatedDamage);
    } else {
      battle.player_hp = Math.max(0, battle.player_hp - calculatedDamage);
    }
    
    battle.moves.push({
      player: attacker,
      type: moveType,
      damage: calculatedDamage,
      timestamp: new Date().toISOString()
    });
    
    const battleEnd = battleService.checkBattleEnd(battle);
    if (battleEnd.ended) {
      battle.state = 'ended';
      battle.winner = battleEnd.winner;
      battle.xp_reward = battleService.calculateXpReward(battle, battleEnd.winner);
    } else {
      battle.turn = defender;
    }
    
    await db.collection('battles').doc(id).update(battle);
    
    res.json(battle);
  } catch (error) {
    console.error('Error executing move:', error);
    res.status(500).json({ error: 'Failed to execute move' });
  }
};

const getBattleState = async (req, res) => {
  try {
    const { id } = req.params;
    const doc = await db.collection('battles').doc(id).get();
    
    if (!doc.exists) {
      return res.status(404).json({ error: 'Battle not found' });
    }
    
    res.json({ id: doc.id, ...doc.data() });
  } catch (error) {
    console.error('Error fetching battle state:', error);
    res.status(500).json({ error: 'Failed to fetch battle state' });
  }
};

const fleeBattle = async (req, res) => {
  try {
    const { id } = req.params;
    const { playerId } = req.body;
    
    const battleDoc = await db.collection('battles').doc(id).get();
    if (!battleDoc.exists) {
      return res.status(404).json({ error: 'Battle not found' });
    }
    
    const battle = battleDoc.data();
    
    if (battle.player_id !== playerId) {
      return res.status(403).json({ error: 'Not your battle' });
    }
    
    await db.collection('battles').doc(id).update({
      state: 'fled',
      ended_at: new Date().toISOString()
    });
    
    res.json({ success: true, state: 'fled' });
  } catch (error) {
    console.error('Error fleeing battle:', error);
    res.status(500).json({ error: 'Failed to flee battle' });
  }
};

const endBattle = async (req, res) => {
  try {
    const { id } = req.params;
    const { playerId, result } = req.body;
    
    const battleDoc = await db.collection('battles').doc(id).get();
    if (!battleDoc.exists) {
      return res.status(404).json({ error: 'Battle not found' });
    }
    
    const battle = battleDoc.data();
    
    if (battle.player_id !== playerId) {
      return res.status(403).json({ error: 'Not your battle' });
    }
    
    const xpReward = battleService.calculateXpReward(battle, result);
    
    await db.collection('users').doc(playerId).update({
      xp: admin.firestore.FieldValue.increment(xpReward)
    });
    
    await db.collection('battles').doc(id).update({
      state: 'ended',
      winner: result,
      xp_reward: xpReward,
      ended_at: new Date().toISOString()
    });
    
    res.json({ success: true, xp_reward: xpReward });
  } catch (error) {
    console.error('Error ending battle:', error);
    res.status(500).json({ error: 'Failed to end battle' });
  }
};

module.exports = {
  startBattle,
  executeMove,
  getBattleState,
  fleeBattle,
  endBattle
};
