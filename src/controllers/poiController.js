const db = require('../config/database');

const getPois = async (req, res) => {
  try {
    if (!db) {
      return res.status(503).json({ error: 'Database not configured' });
    }
    
    const snapshot = await db.collection('pois').get();
    const pois = [];
    snapshot.forEach(doc => {
      pois.push({ id: doc.id, ...doc.data() });
    });
    
    res.json(pois);
  } catch (error) {
    console.error('Error fetching POIs:', error);
    res.status(500).json({ error: 'Failed to fetch POIs' });
  }
};

const createPoi = async (req, res) => {
  try {
    if (!db) {
      return res.status(503).json({ error: 'Database not configured' });
    }
    
    const poiData = req.body;
    const docRef = await db.collection('pois').add(poiData);
    
    res.json({ id: docRef.id, ...poiData });
  } catch (error) {
    console.error('Error creating POI:', error);
    res.status(500).json({ error: 'Failed to create POI' });
  }
};

module.exports = { getPois, createPoi };
