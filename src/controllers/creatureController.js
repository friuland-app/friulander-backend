const db = require('../config/database');
const admin = require('firebase-admin');
const spawnService = require('../services/spawnService');

const getCreatures = async (req, res) => {
  try {
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

const getSpawnedCreatures = async (req, res) => {
  try {
    const snapshot = await db.collection('world_creatures')
      .where('expiry_time', '>', new Date().toISOString())
      .get();
    
    const creatures = [];
    snapshot.forEach(doc => {
      creatures.push({ id: doc.id, ...doc.data() });
    });
    
    res.json(creatures);
  } catch (error) {
    console.error('Error fetching spawned creatures:', error);
    res.status(500).json({ error: 'Failed to fetch spawned creatures' });
  }
};

const catchCreature = async (req, res) => {
  try {
    const { spawnId, trapType, playerId } = req.body;
    
    const spawnDoc = await db.collection('world_creatures').doc(spawnId).get();
    if (!spawnDoc.exists) {
      return res.status(404).json({ error: 'Spawn not found' });
    }
    
    const spawn = spawnDoc.data();
    const captureChance = spawnService.calculateCaptureChance(spawn, trapType);
    const roll = Math.random();
    
    if (roll <= captureChance) {
      await db.collection('users').doc(playerId).update({
        'inventory.creatures': admin.firestore.FieldValue.arrayUnion(spawn.creature_id)
      });
      await db.collection('world_creatures').doc(spawnId).delete();
      
      res.json({ success: true, creature: spawn.creature_id });
    } else {
      res.json({ success: false, chance: captureChance });
    }
  } catch (error) {
    console.error('Error catching creature:', error);
    res.status(500).json({ error: 'Failed to catch creature' });
  }
};

const evolveCreature = async (req, res) => {
  try {
    const { id } = req.params;
    const { newStage } = req.body;
    
    await db.collection('creatures').doc(id).update({
      evolution_stage: newStage
    });
    
    res.json({ id, evolution_stage: newStage });
  } catch (error) {
    console.error('Error evolving creature:', error);
    res.status(500).json({ error: 'Failed to evolve creature' });
  }
};

const getNearbyCreatures = async (req, res) => {
  try {
    const { lat, lng, radius = 0.01 } = req.query;
    
    const snapshot = await db.collection('world_creatures')
      .where('expiry_time', '>', new Date().toISOString())
      .get();
    
    const nearby = [];
    snapshot.forEach(doc => {
      const creature = doc.data();
      const distance = spawnService.calculateDistance(
        lat, lng,
        creature.location.latitude,
        creature.location.longitude
      );
      
      if (distance <= radius) {
        nearby.push({ id: doc.id, ...creature, distance });
      }
    });
    
    res.json(nearby);
  } catch (error) {
    console.error('Error fetching nearby creatures:', error);
    res.status(500).json({ error: 'Failed to fetch nearby creatures' });
  }
};

module.exports = {
  getCreatures,
  getCreatureById,
  getSpawnedCreatures,
  catchCreature,
  evolveCreature,
  getNearbyCreatures
};
