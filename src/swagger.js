const swaggerJsdoc = require('swagger-jsdoc');

const options = {
  definition: {
    openapi: '3.0.0',
    info: {
      title: 'Friulander Backend API',
      version: '1.0.0',
      description: 'API for Friulander AR game'
    },
    servers: [
      {
        url: 'http://localhost:3000',
        description: 'Development server'
      }
    ]
  },
  apis: ['./src/routes/*.js']
};

const specs = swaggerJsdoc(options);
module.exports = specs;
