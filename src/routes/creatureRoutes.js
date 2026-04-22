const express = require('express');
const router = express.Router();
const { getCreatures, getCreatureById } = require('../controllers/creatureController');

router.get('/', getCreatures);
router.get('/:id', getCreatureById);

module.exports = router;
