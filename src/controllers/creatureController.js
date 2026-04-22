const db = require('../config/database');

const getCreatures = async (req, res) => {
  try {
    if (!db) {
      return res.status(503).json({ error: 'Database not configured' });
    }
    
    const snapshot = await db.collection('creatures').get();
    const creatures = [];
    snapshot.forEach(doc => {
      creatures.push({ id: doc.id, ...doc.data() });
    });
    
    res.json(creatures);
  } catch (error) {
    console.error('Error fetching creatures:', error);
    res.status(500).json({ error: 'Failed to fetch creatures' });
  }
};

const getCreatureById = async (req, res) => {
  try {
    if (!db) {
      return res.status(503).json({ error: 'Database not configured' });
    }
    
    const { id } = req.params;
    const doc = await db.collection('creatures').doc(id).get();
    
    if (!doc.exists) {
      return res.status(404).json({ error: 'Creature not found' });
    }
    
    res.json({ id: doc.id, ...doc.data() });
  } catch (error) {
    console.error('Error fetching creature:', error);
    res.status(500).json({ error: 'Failed to fetch creature' });
  }
};

module.exports = { getCreatures, getCreatureById };
