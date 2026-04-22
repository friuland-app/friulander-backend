const rateLimit = require('express-rate-limit');
const helmet = require('helmet');
const xss = require('xss');
const logger = require('../utils/logger');

const suspiciousActivities = new Map();

const rateLimiter = rateLimit({
  windowMs: 60 * 1000,
  max: 100,
  message: { error: 'Too many requests, please try again later' },
  standardHeaders: true,
  legacyHeaders: false
});

const strictRateLimiter = rateLimit({
  windowMs: 60 * 1000,
  max: 20,
  message: { error: 'Too many requests for this endpoint' }
});

const validateGPS = (req, res, next) => {
  const { lat, lng } = req.query;
  
  if (lat !== undefined && lng !== undefined) {
    const latitude = parseFloat(lat);
    const longitude = parseFloat(lng);
    
    if (isNaN(latitude) || isNaN(longitude)) {
      return res.status(400).json({ error: 'Invalid GPS coordinates' });
    }
    
    if (latitude < -90 || latitude > 90) {
      return res.status(400).json({ error: 'Invalid latitude range' });
    }
    
    if (longitude < -180 || longitude > 180) {
      return res.status(400).json({ error: 'Invalid longitude range' });
    }
    
    req.sanitizedLat = latitude;
    req.sanitizedLng = longitude;
  }
  
  next();
};

const detectTeleport = (req, res, next) => {
  const { uid } = req;
  const { lat, lng } = req.body;
  
  if (uid && lat && lng) {
    const lastPosition = suspiciousActivities.get(uid);
    
    if (lastPosition) {
      const distance = calculateDistance(
        lastPosition.lat, lastPosition.lng,
        lat, lng
      );
      const timeDiff = (Date.now() - lastPosition.timestamp) / 1000;
      const speed = distance / timeDiff;
      
      if (speed > 0.5) {
        logger.warn(`Suspicious activity: Possible teleport detected for user ${uid}`);
        suspiciousActivities.set(uid, {
          ...suspiciousActivities.get(uid),
          suspiciousCount: (suspiciousActivities.get(uid).suspiciousCount || 0) + 1
        });
        
        if (suspiciousActivities.get(uid).suspiciousCount > 5) {
          return res.status(403).json({ error: 'Account temporarily suspended due to suspicious activity' });
        }
      }
    }
    
    suspiciousActivities.set(uid, {
      lat,
      lng,
      timestamp: Date.now(),
      suspiciousCount: suspiciousActivities.get(uid)?.suspiciousCount || 0
    });
  }
  
  next();
};

const sanitizeInput = (req, res, next) => {
  if (req.body) {
    for (const key in req.body) {
      if (typeof req.body[key] === 'string') {
        req.body[key] = xss(req.body[key]);
      }
    }
  }
  
  if (req.query) {
    for (const key in req.query) {
      if (typeof req.query[key] === 'string') {
        req.query[key] = xss(req.query[key]);
      }
    }
  }
  
  next();
};

const enforceHTTPS = (req, res, next) => {
  if (process.env.NODE_ENV === 'production' && !req.secure) {
    return res.redirect('https://' + req.headers.host + req.url);
  }
  next();
};

const logSuspiciousActivity = (req, res, next) => {
  const originalSend = res.send;
  
  res.send = function(data) {
    if (res.statusCode >= 400) {
      logger.warn(`Suspicious request: ${req.method} ${req.path} - Status: ${res.statusCode} - IP: ${req.ip}`);
    }
    originalSend.call(this, data);
  };
  
  next();
};

const checkBanStatus = async (req, res, next) => {
  const { uid } = req;
  
  if (uid) {
    const activity = suspiciousActivities.get(uid);
    
    if (activity && activity.suspiciousCount >= 10) {
      return res.status(403).json({ error: 'Account banned due to repeated suspicious activity' });
    }
  }
  
  next();
};

const calculateDistance = (lat1, lon1, lat2, lon2) => {
  const R = 6371;
  const dLat = (lat2 - lat1) * Math.PI / 180;
  const dLon = (lon2 - lon1) * Math.PI / 180;
  const a = Math.sin(dLat/2) * Math.sin(dLat/2) +
    Math.cos(lat1 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180) *
    Math.sin(dLon/2) * Math.sin(dLon/2);
  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
  return R * c;
};

module.exports = {
  rateLimiter,
  strictRateLimiter,
  validateGPS,
  detectTeleport,
  sanitizeInput,
  enforceHTTPS,
  logSuspiciousActivity,
  checkBanStatus
};
