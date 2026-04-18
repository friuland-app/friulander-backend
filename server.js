const express = require('express');
const cors = require('cors');
const admin = require('firebase-admin');
require('dotenv').config();

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors());
app.use(express.json());

// Firebase initialization (will be configured with actual credentials)
let db = null;
let storage = null;
let auth = null;
try {
  if (process.env.FIREBASE_SERVICE_ACCOUNT_KEY) {
    const serviceAccount = JSON.parse(process.env.FIREBASE_SERVICE_ACCOUNT_KEY);
    admin.initializeApp({
      credential: admin.credential.cert(serviceAccount),
      databaseURL: `https://${process.env.FIREBASE_PROJECT_ID}-default-rtdb.firebaseio.com`,
      storageBucket: `${process.env.FIREBASE_PROJECT_ID}.appspot.com`
    });
    db = admin.firestore();
    storage = admin.storage();
    auth = admin.auth();
    console.log('Firebase initialized successfully for project:', process.env.FIREBASE_PROJECT_ID);
  } else {
    console.log('Firebase not configured - running in development mode');
  }
} catch (error) {
  console.error('Firebase initialization error:', error);
}

// Routes
app.get('/', (req, res) => {
  res.json({
    message: 'Furlan Go Backend API',
    version: '1.0.0',
    status: 'running'
  });
});

// Health check
app.get('/health', (req, res) => {
  res.json({
    status: 'healthy',
    timestamp: new Date().toISOString(),
    firebase: {
      connected: db ? true : false,
      project: process.env.FIREBASE_PROJECT_ID || 'not configured',
      firestore: db ? 'connected' : 'not configured',
      storage: storage ? 'connected' : 'not configured',
      auth: auth ? 'connected' : 'not configured'
    }
  });
});

// POI endpoints
app.get('/api/poi', async (req, res) => {
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
});

// Creature endpoints
app.get('/api/creatures', async (req, res) => {
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
});

// Player endpoints
app.post('/api/players', async (req, res) => {
  try {
    if (!db) {
      return res.status(503).json({ error: 'Database not configured' });
    }
    
    const playerData = req.body;
    const docRef = await db.collection('players').add(playerData);
    
    res.json({ id: docRef.id, ...playerData });
  } catch (error) {
    console.error('Error creating player:', error);
    res.status(500).json({ error: 'Failed to create player' });
  }
});

// Start server
app.listen(PORT, () => {
  console.log(`Furlan Go Backend running on port ${PORT}`);
  console.log(`Environment: ${process.env.NODE_ENV || 'development'}`);
});

module.exports = app;
