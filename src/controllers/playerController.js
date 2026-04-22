const db = require('../config/database');

const createPlayer = async (req, res) => {
  try {
    const playerData = req.body;
    const docRef = await db.collection('players').add(playerData);
    res.json({ id: docRef.id, ...playerData });
  } catch (error) {
    res.status(500).json({ error: 'Failed to create player' });
  }
};

module.exports = { createPlayer };
