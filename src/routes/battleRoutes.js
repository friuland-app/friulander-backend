const express = require('express');
const router = express.Router();
const {
  startBattle,
  executeMove,
  getBattleState,
  fleeBattle,
  endBattle
} = require('../controllers/battleController');
const { authenticate } = require('../middleware/auth');

router.post('/start', authenticate, startBattle);
router.post('/:id/move', authenticate, executeMove);
router.get('/:id/state', authenticate, getBattleState);
router.post('/:id/flee', authenticate, fleeBattle);
router.post('/:id/end', authenticate, endBattle);

module.exports = router;
