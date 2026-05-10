const fs = require('fs');
const path = require('path');

// Script per popolare il database con le creature iniziali

async function seedCreatures() {
  try {
    // Leggi dati creature
    const creaturesData = JSON.parse(
      fs.readFileSync(path.join(__dirname, '../data/creatures.json'), 'utf8')
    );

    console.log(`Trovate ${creaturesData.creatures.length} creature da inserire`);

    // Per ora solo stampa le creature (integrazione con DB futura)
    creaturesData.creatures.forEach(creature => {
      console.log(`- ${creature.name} (${creature.id}): ${creature.type.join('/')}`);
    });

    console.log('\nPer inserire nel database:');
    console.log('1. Assicurati che Firebase sia configurato');
    console.log('2. Esegui: node src/scripts/seedCreatures.js --import');
    
  } catch (error) {
    console.error('Errore:', error.message);
  }
}

// Esegui
seedCreatures();
