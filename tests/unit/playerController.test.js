const request = require('supertest');
const app = require('../../server');

describe('Player Controller', () => {
  describe('GET /api/players/leaderboard', () => {
    it('should return leaderboard', async () => {
      const res = await request(app)
        .get('/api/players/leaderboard');
      
      expect(res.statusCode).toBe(200);
      expect(Array.isArray(res.body)).toBe(true);
    });
  });

  describe('GET /api/players/:id', () => {
    it('should return player profile', async () => {
      const res = await request(app)
        .get('/api/players/test-id')
        .set('Authorization', 'Bearer test-token');
      
      expect([200, 401, 404]).toContain(res.statusCode);
    });
  });
});
