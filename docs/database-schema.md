# Firestore Database Schema

## Collections Structure

### users/{userId}
```json
{
  "name": "string",
  "email": "string",
  "level": "number",
  "xp": "number",
  "inventory": {
    "items": ["item_id"],
    "creatures": ["creature_id"]
  },
  "active_squad": ["creature_id"],
  "titles": ["title_id"],
  "badges": ["badge_id"],
  "created_at": "timestamp",
  "updated_at": "timestamp"
}
```

### creatures/{creatureId}
```json
{
  "name_friulian": "string",
  "name_italian": "string",
  "type": "string",
  "stats": {
    "hp": "number",
    "attack": "number",
    "defense": "number",
    "speed": "number"
  },
  "rarity": "string",
  "spawn_zone": "string",
  "lore": "string"
}
```

### world_creatures/{spawnId}
```json
{
  "creature_id": "string",
  "location": {
    "latitude": "number",
    "longitude": "number"
  },
  "level": "number",
  "spawn_time": "timestamp",
  "expiry_time": "timestamp",
  "rarity": "string"
}
```

### poi/{poiId}
```json
{
  "name": "string",
  "type": "string",
  "location": {
    "latitude": "number",
    "longitude": "number"
  },
  "description": "string",
  "image_url": "string"
}
```

### battles/{battleId}
```json
{
  "player_id": "string",
  "creature_id": "string",
  "result": "string",
  "timestamp": "timestamp",
  "xp_gained": "number"
}
```

### quests/{questId}
```json
{
  "type": "string",
  "title": "string",
  "description": "string",
  "objective": "number",
  "progress": "number",
  "reward_xp": "number",
  "reward_items": ["item_id"]
}
```
