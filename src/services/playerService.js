const db = require('../config/database');
const Player = require('../models/Player');

const getPlayerById = async (id) => {
  const doc = await db.collection('players').doc(id).get();
  if (!doc.exists) return null;
  return new Player({ id: doc.id, ...doc.data() });
};

module.exports = { getPlayerById };
