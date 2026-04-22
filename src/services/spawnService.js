const db = require('../config/database');

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

const calculateCaptureChance = (spawn, trapType) => {
  let baseChance = 0.3;
  
  if (trapType === 'rare') baseChance += 0.15;
  if (trapType === 'special') baseChance += 0.3;
  
  if (spawn.rarity === 'legendary') baseChance -= 0.3;
  if (spawn.rarity === 'epic') baseChance -= 0.2;
  if (spawn.rarity === 'rare') baseChance -= 0.1;
  
  return Math.max(0.05, Math.min(0.95, baseChance));
};

const spawnCreature = async (location, weather, hour) => {
  const modifiers = getSpawnModifiers(weather, hour);
  
  const spawnData = {
    creature_id: selectCreatureType(modifiers),
    location: {
      latitude: location.lat,
      longitude: location.lng
    },
    level: Math.floor(Math.random() * 50) + 1,
    spawn_time: new Date().toISOString(),
    expiry_time: new Date(Date.now() + 30 * 60 * 1000).toISOString(),
    rarity: selectRarity(modifiers)
  };
  
  await db.collection('world_creatures').add(spawnData);
  return spawnData;
};

const getSpawnModifiers = (weather, hour) => {
  const modifiers = {
    water: 1.0,
    darkness: 1.0,
    rare: 1.0
  };
  
  if (weather === 'rain') modifiers.water = 2.0;
  if (hour >= 20 || hour < 6) {
    modifiers.darkness = 1.5;
    modifiers.rare = 1.3;
  }
  if (weather === 'fog') modifiers.rare = 2.0;
  
  return modifiers;
};

const selectCreatureType = (modifiers) => {
  const types = ['water', 'earth', 'fire', 'air', 'legend'];
  const weights = [1, 1, 1, 1, 0.1];
  
  weights[0] *= modifiers.water;
  weights[4] *= modifiers.darkness;
  
  const totalWeight = weights.reduce((a, b) => a + b, 0);
  let random = Math.random() * totalWeight;
  
  for (let i = 0; i < types.length; i++) {
    random -= weights[i];
    if (random <= 0) return types[i];
  }
  
  return types[0];
};

const selectRarity = (modifiers) => {
  const rarities = ['common', 'rare', 'epic', 'legendary'];
  const weights = [0.6, 0.25, 0.1, 0.05];
  
  weights[1] *= modifiers.rare;
  weights[2] *= modifiers.rare;
  weights[3] *= modifiers.rare;
  
  const totalWeight = weights.reduce((a, b) => a + b, 0);
  let random = Math.random() * totalWeight;
  
  for (let i = 0; i < rarities.length; i++) {
    random -= weights[i];
    if (random <= 0) return rarities[i];
  }
  
  return rarities[0];
};

module.exports = {
  calculateDistance,
  calculateCaptureChance,
  spawnCreature,
  getSpawnModifiers
};
