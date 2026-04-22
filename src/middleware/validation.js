const Joi = require('joi');

const playerUpdateSchema = Joi.object({
  name: Joi.string().min(2).max(50),
  level: Joi.number().min(1).max(40),
  xp: Joi.number().min(0)
});

const addXpSchema = Joi.object({
  xp: Joi.number().min(0).max(10000).required()
});

const validatePlayerUpdate = (req, res, next) => {
  const { error } = playerUpdateSchema.validate(req.body);
  if (error) {
    return res.status(400).json({ error: error.details[0].message });
  }
  next();
};

const validateAddXp = (req, res, next) => {
  const { error } = addXpSchema.validate(req.body);
  if (error) {
    return res.status(400).json({ error: error.details[0].message });
  }
  next();
};

module.exports = {
  validatePlayerUpdate,
  validateAddXp
};
