const express = require('express');
const router = express.Router();
const {
  registerEmailPassword,
  loginEmailPassword,
  googleSignIn,
  appleSignIn,
  refreshToken,
  resetPassword
} = require('../controllers/authController');

router.post('/register', registerEmailPassword);
router.post('/login', loginEmailPassword);
router.post('/google', googleSignIn);
router.post('/apple', appleSignIn);
router.post('/refresh', refreshToken);
router.post('/reset-password', resetPassword);

module.exports = router;
