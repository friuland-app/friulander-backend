const request = require('supertest');
const app = require('../../server');

describe('API Integration Tests', () => {
  it('GET / should return 200', async () => {
    const res = await request(app).get('/');
    expect(res.statusCode).toBe(200);
  });

  it('GET /api/poi should return POIs', async () => {
    const res = await request(app).get('/api/poi');
    expect(res.statusCode).toBe(200);
  });

  it('GET /api/creatures should return creatures', async () => {
    const res = await request(app).get('/api/creatures');
    expect(res.statusCode).toBe(200);
  });
});
