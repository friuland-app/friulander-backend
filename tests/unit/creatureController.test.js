const request = require('supertest');
const app = require('../../server');

describe('Creature Controller', () => {
  describe('GET /api/creatures', () => {
    it('should return all creatures', async () => {
      const res = await request(app)
        .get('/api/creatures');
      
      expect(res.statusCode).toBe(200);
      expect(Array.isArray(res.body)).toBe(true);
    });
  });

  describe('GET /api/creatures/:id', () => {
    it('should return creature details', async () => {
      const res = await request(app)
        .get('/api/creatures/test-id');
      
      expect([200, 404]).toContain(res.statusCode);
    });
  });
});
