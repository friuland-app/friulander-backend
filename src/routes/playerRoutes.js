const express = require('express');
const router = express.Router();
const { createPlayer } = require('../controllers/playerController');
const { authenticate } = require('../middleware/auth');

router.post('/', createPlayer);
router.get('/:id', authenticate, createPlayer);

module.exports = router;
