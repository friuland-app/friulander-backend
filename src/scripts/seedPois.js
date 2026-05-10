const fs = require('fs');
const path = require('path');

// Script per popolare il database con i POI del Friuli

async function seedPois() {
  try {
    const poisData = JSON.parse(
      fs.readFileSync(path.join(__dirname, '../data/pois.json'), 'utf8')
    );

    console.log(`Trovati ${poisData.pois.length} POI da inserire`);
    console.log('\nPOI disponibili:');

    poisData.pois.forEach(poi => {
      console.log(`- ${poi.name} (${poi.id}): ${poi.type} in ${poi.location.city}`);
    });

    console.log('\nPer inserire nel database:');
    console.log('1. Assicurati che Firebase sia configurato');
    console.log('2. Esegui: node src/scripts/seedPois.js --import');
    
  } catch (error) {
    console.error('Errore:', error.message);
  }
}

seedPois();
