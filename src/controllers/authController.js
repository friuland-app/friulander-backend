const admin = require('firebase-admin');
const jwt = require('jsonwebtoken');
const db = require('../config/database');

const JWT_SECRET = process.env.JWT_SECRET || 'your-secret-key';
const JWT_REFRESH_SECRET = process.env.JWT_REFRESH_SECRET || 'your-refresh-secret';

const registerEmailPassword = async (req, res) => {
  try {
    const { email, password, name } = req.body;
    
    const userRecord = await admin.auth().createUser({
      email,
      password,
      displayName: name
    });
    
    const userDoc = {
      uid: userRecord.uid,
      name,
      email,
      level: 1,
      xp: 0,
      inventory: { items: [], creatures: [] },
      active_squad: [],
      titles: [],
      badges: [],
      created_at: new Date().toISOString()
    };
    
    await db.collection('users').doc(userRecord.uid).set(userDoc);
    
    const token = generateAccessToken(userRecord.uid);
    const refreshToken = generateRefreshToken(userRecord.uid);
    
    res.json({
      uid: userRecord.uid,
      token,
      refreshToken,
      user: userDoc
    });
  } catch (error) {
    console.error('Registration error:', error);
    res.status(400).json({ error: error.message });
  }
};

const loginEmailPassword = async (req, res) => {
  try {
    const { email, password } = req.body;
    
    const userRecord = await admin.auth().getUserByEmail(email);
    
    const token = generateAccessToken(userRecord.uid);
    const refreshToken = generateRefreshToken(userRecord.uid);
    
    const userDoc = await db.collection('users').doc(userRecord.uid).get();
    
    res.json({
      uid: userRecord.uid,
      token,
      refreshToken,
      user: userDoc.exists ? userDoc.data() : null
    });
  } catch (error) {
    console.error('Login error:', error);
    res.status(401).json({ error: 'Invalid credentials' });
  }
};

const googleSignIn = async (req, res) => {
  try {
    const { idToken } = req.body;
    
    const decodedToken = await admin.auth().verifyIdToken(idToken);
    const uid = decodedToken.uid;
    
    const userDoc = await db.collection('users').doc(uid).get();
    
    if (!userDoc.exists) {
      const newUser = {
        uid,
        name: decodedToken.name || 'User',
        email: decodedToken.email,
        level: 1,
        xp: 0,
        inventory: { items: [], creatures: [] },
        active_squad: [],
        titles: [],
        badges: [],
        created_at: new Date().toISOString()
      };
      await db.collection('users').doc(uid).set(newUser);
    }
    
    const token = generateAccessToken(uid);
    const refreshToken = generateRefreshToken(uid);
    
    res.json({
      uid,
      token,
      refreshToken,
      user: userDoc.exists ? userDoc.data() : newUser
    });
  } catch (error) {
    console.error('Google Sign-In error:', error);
    res.status(401).json({ error: 'Invalid Google token' });
  }
};

const appleSignIn = async (req, res) => {
  try {
    const { idToken } = req.body;
    
    const decodedToken = await admin.auth().verifyIdToken(idToken);
    const uid = decodedToken.uid;
    
    const userDoc = await db.collection('users').doc(uid).get();
    
    if (!userDoc.exists) {
      const newUser = {
        uid,
        name: decodedToken.name || 'User',
        email: decodedToken.email,
        level: 1,
        xp: 0,
        inventory: { items: [], creatures: [] },
        active_squad: [],
        titles: [],
        badges: [],
        created_at: new Date().toISOString()
      };
      await db.collection('users').doc(uid).set(newUser);
    }
    
    const token = generateAccessToken(uid);
    const refreshToken = generateRefreshToken(uid);
    
    res.json({
      uid,
      token,
      refreshToken,
      user: userDoc.exists ? userDoc.data() : newUser
    });
  } catch (error) {
    console.error('Apple Sign-In error:', error);
    res.status(401).json({ error: 'Invalid Apple token' });
  }
};

const refreshToken = async (req, res) => {
  try {
    const { refreshToken } = req.body;
    
    const decoded = jwt.verify(refreshToken, JWT_REFRESH_SECRET);
    const newToken = generateAccessToken(decoded.uid);
    const newRefreshToken = generateRefreshToken(decoded.uid);
    
    res.json({
      token: newToken,
      refreshToken: newRefreshToken
    });
  } catch (error) {
    console.error('Refresh token error:', error);
    res.status(401).json({ error: 'Invalid refresh token' });
  }
};

const resetPassword = async (req, res) => {
  try {
    const { email } = req.body;
    
    const link = await admin.auth().generatePasswordResetLink(email);
    
    res.json({ message: 'Password reset email sent', link });
  } catch (error) {
    console.error('Password reset error:', error);
    res.status(400).json({ error: error.message });
  }
};

const generateAccessToken = (uid) => {
  return jwt.sign({ uid }, JWT_SECRET, { expiresIn: '1h' });
};

const generateRefreshToken = (uid) => {
  return jwt.sign({ uid }, JWT_REFRESH_SECRET, { expiresIn: '7d' });
};

module.exports = {
  registerEmailPassword,
  loginEmailPassword,
  googleSignIn,
  appleSignIn,
  refreshToken,
  resetPassword
};
