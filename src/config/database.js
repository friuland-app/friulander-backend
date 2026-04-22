const admin = require('firebase-admin');
require('dotenv').config();

let db = null;

try {
  if (process.env.FIREBASE_SERVICE_ACCOUNT_KEY) {
    const serviceAccount = JSON.parse(process.env.FIREBASE_SERVICE_ACCOUNT_KEY);
    admin.initializeApp({
      credential: admin.credential.cert(serviceAccount),
      databaseURL: `https://${process.env.FIREBASE_PROJECT_ID}-default-rtdb.firebaseio.com`
    });
    db = admin.firestore();
    console.log('Firebase initialized');
  }
} catch (error) {
  console.error('Firebase error:', error);
}

module.exports = db;
