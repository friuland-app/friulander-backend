const fs = require('fs');
const path = require('path');

// Script di bilanciamento creature

function calculateBST(stats) {
  return stats.hp + stats.atk + stats.def + stats.spa + stats.spd + stats.spe;
}

function balanceCreature(creature) {
  const bst = calculateBST(creature.base_stats);
  const idealBST = 300 + (creature.rarity === 'rare' ? 100 : 0);
  
  console.log(`${creature.id}: BST=${bst} (target: ${idealBST})`);
  
  return {
    ...creature,
    bst,
    balanced: Math.abs(bst - idealBST) < 30
  };
}

async function runBalance() {
  try {
    const statsData = JSON.parse(
      fs.readFileSync(path.join(__dirname, '../data/creature_stats.json'), 'utf8')
    );

    console.log('Bilanciamento creature:');
    statsData.creatures.forEach(balanceCreature);
    
  } catch (error) {
    console.error('Errore:', error.message);
  }
}

runBalance();
