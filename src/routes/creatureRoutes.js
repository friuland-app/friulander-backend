const express = require('express');
const router = express.Router();
const {
  getCreatures,
  getCreatureById,
  getSpawnedCreatures,
  catchCreature,
  evolveCreature,
  getNearbyCreatures
} = require('../controllers/creatureController');
const { authenticate } = require('../middleware/auth');

router.get('/', getCreatures);
router.get('/nearby', getNearbyCreatures);
router.get('/spawn/active', getSpawnedCreatures);
router.post('/catch', authenticate, catchCreature);
router.put('/:id/evolve', authenticate, evolveCreature);
router.get('/:id', getCreatureById);

module.exports = router;
