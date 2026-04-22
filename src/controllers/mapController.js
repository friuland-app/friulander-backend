const db = require('../config/database');
const admin = require('firebase-admin');
const geoService = require('../services/geoService');

const getAllPois = async (req, res) => {
  try {
    const snapshot = await db.collection('poi').get();
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

const getNearbyPois = async (req, res) => {
  try {
    const { lat, lng, radius = 5 } = req.query;
    
    const snapshot = await db.collection('poi').get();
    const nearby = [];
    
    snapshot.forEach(doc => {
      const poi = doc.data();
      const distance = geoService.calculateDistance(
        lat, lng,
        poi.location.latitude,
        poi.location.longitude
      );
      
      if (distance <= radius) {
        nearby.push({ id: doc.id, ...poi, distance });
      }
    });
    
    res.json(nearby);
  } catch (error) {
    console.error('Error fetching nearby POIs:', error);
    res.status(500).json({ error: 'Failed to fetch nearby POIs' });
  }
};

const visitPoi = async (req, res) => {
  try {
    const { id } = req.params;
    const { playerId } = req.body;
    
    const poiDoc = await db.collection('poi').doc(id).get();
    if (!poiDoc.exists) {
      return res.status(404).json({ error: 'POI not found' });
    }
    
    await db.collection('users').doc(playerId).update({
      'visited_pois': admin.firestore.FieldValue.arrayUnion(id)
    });
    
    res.json({ success: true, poi: id });
  } catch (error) {
    console.error('Error visiting POI:', error);
    res.status(500).json({ error: 'Failed to visit POI' });
  }
};

const getGameZones = async (req, res) => {
  try {
    const snapshot = await db.collection('zones').get();
    const zones = [];
    snapshot.forEach(doc => {
      zones.push({ id: doc.id, ...doc.data() });
    });
    
    res.json(zones);
  } catch (error) {
    console.error('Error fetching zones:', error);
    res.status(500).json({ error: 'Failed to fetch zones' });
  }
};

const getMapEvents = async (req, res) => {
  try {
    const snapshot = await db.collection('events')
      .where('active', '==', true)
      .where('end_time', '>', new Date().toISOString())
      .get();
    
    const events = [];
    snapshot.forEach(doc => {
      events.push({ id: doc.id, ...doc.data() });
    });
    
    res.json(events);
  } catch (error) {
    console.error('Error fetching events:', error);
    res.status(500).json({ error: 'Failed to fetch events' });
  }
};

const getMapboxTile = async (req, res) => {
  try {
    const { x, y, z } = req.params;
    const mapboxToken = process.env.MAPBOX_TOKEN;
    
    const tileUrl = `https://api.mapbox.com/styles/v1/mapbox/streets-v12/tiles/${z}/${x}/${y}?access_token=${mapboxToken}`;
    
    res.redirect(tileUrl);
  } catch (error) {
    console.error('Error fetching Mapbox tile:', error);
    res.status(500).json({ error: 'Failed to fetch tile' });
  }
};

module.exports = {
  getAllPois,
  getNearbyPois,
  visitPoi,
  getGameZones,
  getMapEvents,
  getMapboxTile
};
