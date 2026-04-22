const geoService = require('./geoService');

class SocketManager {
  constructor(io) {
    this.io = io;
    this.playerRooms = new Map();
    this.setupEventHandlers();
  }

  setupEventHandlers() {
    this.io.on('connection', (socket) => {
      console.log('Client connected:', socket.id);

      socket.on('join_zone', (data) => {
        this.handleJoinZone(socket, data);
      });

      socket.on('leave_zone', (data) => {
        this.handleLeaveZone(socket, data);
      });

      socket.on('update_position', (data) => {
        this.handleUpdatePosition(socket, data);
      });

      socket.on('chat_message', (data) => {
        this.handleChatMessage(socket, data);
      });

      socket.on('disconnect', () => {
        this.handleDisconnect(socket);
      });
    });
  }

  handleJoinZone(socket, data) {
    const { playerId, lat, lng } = data;
    const zoneId = this.getZoneId(lat, lng);
    
    socket.join(zoneId);
    this.playerRooms.set(socket.id, { playerId, zoneId, lat, lng });
    
    console.log(`Player ${playerId} joined zone ${zoneId}`);
    
    socket.emit('zone_joined', { zoneId, players: this.getZonePlayers(zoneId) });
    socket.to(zoneId).emit('player_joined', { playerId, lat, lng });
  }

  handleLeaveZone(socket, data) {
    const { playerId } = data;
    const roomData = this.playerRooms.get(socket.id);
    
    if (roomData) {
      socket.leave(roomData.zoneId);
      socket.to(roomData.zoneId).emit('player_left', { playerId });
      this.playerRooms.delete(socket.id);
    }
  }

  handleUpdatePosition(socket, data) {
    const { playerId, lat, lng } = data;
    const roomData = this.playerRooms.get(socket.id);
    
    if (roomData) {
      const newZoneId = this.getZoneId(lat, lng);
      
      if (newZoneId !== roomData.zoneId) {
        socket.leave(roomData.zoneId);
        socket.to(roomData.zoneId).emit('player_left', { playerId });
        
        socket.join(newZoneId);
        socket.to(newZoneId).emit('player_joined', { playerId, lat, lng });
        
        roomData.zoneId = newZoneId;
        roomData.lat = lat;
        roomData.lng = lng;
      }
      
      socket.to(roomData.zoneId).emit('player_moved', { playerId, lat, lng });
    }
  }

  handleChatMessage(socket, data) {
    const { playerId, message } = data;
    const roomData = this.playerRooms.get(socket.id);
    
    if (roomData) {
      this.io.to(roomData.zoneId).emit('chat_message', {
        playerId,
        message,
        timestamp: new Date().toISOString()
      });
    }
  }

  handleDisconnect(socket) {
    const roomData = this.playerRooms.get(socket.id);
    
    if (roomData) {
      socket.to(roomData.zoneId).emit('player_left', { playerId: roomData.playerId });
      this.playerRooms.delete(socket.id);
    }
    
    console.log('Client disconnected:', socket.id);
  }

  getZoneId(lat, lng) {
    const gridSize = 0.01;
    const latZone = Math.floor(lat / gridSize);
    const lngZone = Math.floor(lng / gridSize);
    return `zone_${latZone}_${lngZone}`;
  }

  getZonePlayers(zoneId) {
    const players = [];
    this.playerRooms.forEach((data, socketId) => {
      if (data.zoneId === zoneId) {
        players.push({ playerId: data.playerId, lat: data.lat, lng: data.lng });
      }
    });
    return players;
  }

  broadcastCreatureSpawn(creatureData) {
    const zoneId = this.getZoneId(creatureData.location.latitude, creatureData.location.longitude);
    this.io.to(zoneId).emit('creature_spawned', creatureData);
  }

  broadcastCreatureDespawn(spawnId, location) {
    const zoneId = this.getZoneId(location.latitude, location.longitude);
    this.io.to(zoneId).emit('creature_despawned', { spawnId });
  }

  broadcastSpecialEvent(eventData) {
    this.io.emit('special_event', eventData);
  }

  broadcastToZone(zoneId, event, data) {
    this.io.to(zoneId).emit(event, data);
  }
}

module.exports = SocketManager;
