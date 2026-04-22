const express = require('express');
const cors = require('cors');
const admin = require('firebase-admin');
const swaggerUi = require('swagger-ui-express');
const swaggerSpec = require('./src/swagger');
const poiRoutes = require('./src/routes/poiRoutes');
const creatureRoutes = require('./src/routes/creatureRoutes');
const playerRoutes = require('./src/routes/playerRoutes');
const authRoutes = require('./src/routes/authRoutes');
const mapRoutes = require('./src/routes/mapRoutes');
require('dotenv').config();

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors());
app.use(express.json());

// API Routes
app.use('/api/auth', authRoutes);
app.use('/api/poi', poiRoutes);
app.use('/api/creatures', creatureRoutes);
app.use('/api/players', playerRoutes);
app.use('/api/map', mapRoutes);

// Swagger Documentation
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerSpec));

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




// Start server
app.listen(PORT, () => {
  console.log(`Furlan Go Backend running on port ${PORT}`);
  console.log(`Environment: ${process.env.NODE_ENV || 'development'}`);
});

module.exports = app;
