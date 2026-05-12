// Distance Calculator Utility

// Haversine formula for calculating distance between two GPS coordinates
function haversineDistance(lat1, lon1, lat2, lon2) {
  const R = 6371000; // Earth radius in meters
  const dLat = toRadians(lat2 - lat1);
  const dLon = toRadians(lon2 - lon1);
  
  const a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
            Math.cos(toRadians(lat1)) * Math.cos(toRadians(lat2)) *
            Math.sin(dLon / 2) * Math.sin(dLon / 2);
  
  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
  return R * c; // Distance in meters
}

function toRadians(degrees) {
  return degrees * (Math.PI / 180);
}

// Check if user is within range of a POI
function isWithinRange(userLat, userLon, poiLat, poiLon, range = 100) {
  const distance = haversineDistance(userLat, userLon, poiLat, poiLon);
  return distance <= range;
}

// Get distance in human-readable format
function formatDistance(meters) {
  if (meters < 1000) {
    return `${Math.round(meters)}m`;
  }
  return `${(meters / 1000).toFixed(1)}km`;
}

// Find nearest POI
function findNearestPOI(userLat, userLon, pois) {
  let nearest = null;
  let minDistance = Infinity;
  
  for (const poi of pois) {
    const distance = haversineDistance(userLat, userLon, poi.location.lat, poi.location.lng);
    if (distance < minDistance) {
      minDistance = distance;
      nearest = poi;
    }
  }
  
  return { poi: nearest, distance: minDistance };
}

// Filter POIs within range
function filterPOIsInRange(userLat, userLon, pois, range = 500) {
  return pois.filter(poi => 
    isWithinRange(userLat, userLon, poi.location.lat, poi.location.lng, range)
  );
}

module.exports = {
  haversineDistance,
  isWithinRange,
  formatDistance,
  findNearestPOI,
  filterPOIsInRange
};
