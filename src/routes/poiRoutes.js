const express = require('express');
const router = express.Router();
const { getPois, createPoi } = require('../controllers/poiController');

router.get('/', getPois);
router.post('/', createPoi);

module.exports = router;
