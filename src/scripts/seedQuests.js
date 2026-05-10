const fs = require('fs');
const path = require('path');

// Script per popolare il database con le missioni

async function seedQuests() {
  try {
    const questsData = JSON.parse(
      fs.readFileSync(path.join(__dirname, '../data/quests.json'), 'utf8')
    );

    console.log(`Trovate ${questsData.quests.length} missioni da inserire`);
    console.log('\nMissioni disponibili:');

    questsData.quests.forEach(quest => {
      console.log(`- ${quest.name} (${quest.id}): ${quest.type} - ${quest.difficulty}`);
    });

    console.log('\nPer inserire nel database:');
    console.log('1. Assicurati che Firebase sia configurato');
    console.log('2. Esegui: node src/scripts/seedQuests.js --import');
    
  } catch (error) {
    console.error('Errore:', error.message);
  }
}

seedQuests();
