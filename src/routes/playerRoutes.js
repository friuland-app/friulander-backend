const express = require('express');
const router = express.Router();
const {
  getPlayer,
  updatePlayer,
  getPlayerInventory,
  addPlayerXp,
  getPlayerQuests,
  getLeaderboard
} = require('../controllers/playerController');
const { authenticate } = require('../middleware/auth');
const { validatePlayerUpdate, validateAddXp } = require('../middleware/validation');

router.get('/leaderboard', getLeaderboard);
router.get('/:id', authenticate, getPlayer);
router.put('/:id', authenticate, validatePlayerUpdate, updatePlayer);
router.get('/:id/inventory', authenticate, getPlayerInventory);
router.post('/:id/xp', authenticate, validateAddXp, addPlayerXp);
router.get('/:id/quests', authenticate, getPlayerQuests);

module.exports = router;
