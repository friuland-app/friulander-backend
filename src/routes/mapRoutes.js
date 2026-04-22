const express = require('express');
const router = express.Router();
const {
  getAllPois,
  getNearbyPois,
  visitPoi,
  getGameZones,
  getMapEvents,
  getMapboxTile
} = require('../controllers/mapController');
const { authenticate } = require('../middleware/auth');

router.get('/poi', getAllPois);
router.get('/poi/nearby', getNearbyPois);
router.post('/poi/:id/visit', authenticate, visitPoi);
router.get('/zones', getGameZones);
router.get('/events', getMapEvents);
router.get('/tiles/:z/:x/:y', getMapboxTile);

module.exports = router;
