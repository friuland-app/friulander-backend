const express = require('express');
const cors = require('cors');
const admin = require('firebase-admin');
const helmet = require('helmet');
const swaggerUi = require('swagger-ui-express');
const swaggerSpec = require('./src/swagger');
const http = require('http');
const { Server } = require('socket.io');
const poiRoutes = require('./src/routes/poiRoutes');
const creatureRoutes = require('./src/routes/creatureRoutes');
const playerRoutes = require('./src/routes/playerRoutes');
const authRoutes = require('./src/routes/authRoutes');
const mapRoutes = require('./src/routes/mapRoutes');
const battleRoutes = require('./src/routes/battleRoutes');
const SocketManager = require('./src/services/socketManager');
const {
  rateLimiter,
  strictRateLimiter,
  validateGPS,
  detectTeleport,
  sanitizeInput,
  enforceHTTPS,
  logSuspiciousActivity,
  checkBanStatus
} = require('./src/middleware/security');
require('dotenv').config();

const app = express();
const PORT = process.env.PORT || 3000;

const server = http.createServer(app);
const io = new Server(server, {
  cors: {
    origin: '*',
    methods: ['GET', 'POST']
  }
});

// Initialize Socket Manager
const socketManager = new SocketManager(io);

// Middleware
app.use(helmet());
app.use(enforceHTTPS);
app.use(cors({
  origin: process.env.CORS_ORIGIN || '*',
  methods: ['GET', 'POST', 'PUT', 'DELETE'],
  allowedHeaders: ['Content-Type', 'Authorization']
}));
app.use(express.json());
app.use(sanitizeInput);
app.use(logSuspiciousActivity);
app.use(rateLimiter);

// API Routes
app.use('/api/auth', strictRateLimiter, authRoutes);
app.use('/api/poi', poiRoutes);
app.use('/api/creatures', creatureRoutes);
app.use('/api/players', checkBanStatus, playerRoutes);
app.use('/api/map', validateGPS, detectTeleport, mapRoutes);
app.use('/api/battle', checkBanStatus, battleRoutes);

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
server.listen(PORT, () => {
  console.log(`Furlan Go Backend running on port ${PORT}`);
  console.log(`Environment: ${process.env.NODE_ENV || 'development'}`);
  console.log(`WebSocket server initialized`);
});

module.exports = app;
