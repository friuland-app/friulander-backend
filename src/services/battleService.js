const calculateDamage = (attacker, defender, moveType) => {
  const baseDamage = 10;
  const typeEffectiveness = getTypeEffectiveness(moveType, defender.type);
  const attackStat = attacker.stats.attack || 10;
  const defenseStat = defender.stats.defense || 10;
  
  const damage = Math.floor((baseDamage + attackStat) * typeEffectiveness * (attackStat / defenseStat));
  return Math.max(1, damage);
};

const getTypeEffectiveness = (moveType, defenderType) => {
  const effectiveness = {
    'water': { 'fire': 2.0, 'earth': 0.5, 'air': 1.0, 'legend': 1.0 },
    'fire': { 'earth': 2.0, 'water': 0.5, 'air': 1.0, 'legend': 1.0 },
    'earth': { 'air': 2.0, 'fire': 0.5, 'water': 1.0, 'legend': 1.0 },
    'air': { 'water': 2.0, 'earth': 0.5, 'fire': 1.0, 'legend': 1.0 },
    'legend': { 'water': 1.5, 'fire': 1.5, 'earth': 1.5, 'air': 1.5 }
  };
  
  return effectiveness[moveType]?.[defenderType] || 1.0;
};

const validateMove = (battle, moveData) => {
  const currentTurn = battle.turn;
  const expectedPlayer = currentTurn === 'player' ? battle.player_id : battle.enemy_id;
  
  if (moveData.player_id !== expectedPlayer) {
    return { valid: false, reason: 'Not your turn' };
  }
  
  if (battle.state !== 'active') {
    return { valid: false, reason: 'Battle not active' };
  }
  
  if (moveData.hp <= 0 || moveData.hp > 100) {
    return { valid: false, reason: 'Invalid HP value' };
  }
  
  return { valid: true };
};

const checkBattleEnd = (battle) => {
  if (battle.player_hp <= 0) {
    return { ended: true, winner: 'enemy' };
  }
  if (battle.enemy_hp <= 0) {
    return { ended: true, winner: 'player' };
  }
  return { ended: false };
};

const calculateXpReward = (battle, winner) => {
  if (winner === 'player') {
    return 50 + (battle.enemy_level * 10);
  }
  return 25;
};

module.exports = {
  calculateDamage,
  getTypeEffectiveness,
  validateMove,
  checkBattleEnd,
  calculateXpReward
};
