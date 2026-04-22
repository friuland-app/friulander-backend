const db = require('../config/database');

const getPlayer = async (req, res) => {
  try {
    const { id } = req.params;
    const doc = await db.collection('users').doc(id).get();
    
    if (!doc.exists) {
      return res.status(404).json({ error: 'Player not found' });
    }
    
    res.json({ id: doc.id, ...doc.data() });
  } catch (error) {
    console.error('Error fetching player:', error);
    res.status(500).json({ error: 'Failed to fetch player' });
  }
};

const updatePlayer = async (req, res) => {
  try {
    const { id } = req.params;
    const updateData = req.body;
    
    await db.collection('users').doc(id).update(updateData);
    
    res.json({ id, ...updateData });
  } catch (error) {
    console.error('Error updating player:', error);
    res.status(500).json({ error: 'Failed to update player' });
  }
};

const getPlayerInventory = async (req, res) => {
  try {
    const { id } = req.params;
    const doc = await db.collection('users').doc(id).get();
    
    if (!doc.exists) {
      return res.status(404).json({ error: 'Player not found' });
    }
    
    const player = doc.data();
    res.json(player.inventory || { items: [], creatures: [] });
  } catch (error) {
    console.error('Error fetching inventory:', error);
    res.status(500).json({ error: 'Failed to fetch inventory' });
  }
};

const addPlayerXp = async (req, res) => {
  try {
    const { id } = req.params;
    const { xp } = req.body;
    
    const doc = await db.collection('users').doc(id).get();
    if (!doc.exists) {
      return res.status(404).json({ error: 'Player not found' });
    }
    
    const player = doc.data();
    const newXp = (player.xp || 0) + xp;
    
    await db.collection('users').doc(id).update({ xp: newXp });
    
    res.json({ xp: newXp });
  } catch (error) {
    console.error('Error adding XP:', error);
    res.status(500).json({ error: 'Failed to add XP' });
  }
};

const getPlayerQuests = async (req, res) => {
  try {
    const { id } = req.params;
    const snapshot = await db.collection('quests')
      .where('player_id', '==', id)
      .where('status', '==', 'active')
      .get();
    
    const quests = [];
    snapshot.forEach(doc => {
      quests.push({ id: doc.id, ...doc.data() });
    });
    
    res.json(quests);
  } catch (error) {
    console.error('Error fetching quests:', error);
    res.status(500).json({ error: 'Failed to fetch quests' });
  }
};

const getLeaderboard = async (req, res) => {
  try {
    const snapshot = await db.collection('users')
      .orderBy('level', 'desc')
      .orderBy('xp', 'desc')
      .limit(100)
      .get();
    
    const leaderboard = [];
    snapshot.forEach((doc, index) => {
      leaderboard.push({
        rank: index + 1,
        id: doc.id,
        ...doc.data()
      });
    });
    
    res.json(leaderboard);
  } catch (error) {
    console.error('Error fetching leaderboard:', error);
    res.status(500).json({ error: 'Failed to fetch leaderboard' });
  }
};

module.exports = {
  getPlayer,
  updatePlayer,
  getPlayerInventory,
  addPlayerXp,
  getPlayerQuests,
  getLeaderboard
};
