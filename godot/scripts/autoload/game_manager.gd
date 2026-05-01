extends Node

# Game Manager - Autoload singleton for global game state

signal player_data_updated(data: Dictionary)
signal creature_captured(creature_id: String)
signal battle_started(enemy_creature: Dictionary)
signal battle_ended(won: bool)
signal poi_discovered(poi_id: String)
signal quest_completed(quest_id: String)

# Player Data
var player_data: Dictionary = {
	"id": "",
	"username": "",
	"email": "",
	"level": 1,
	"xp": 0,
	"max_xp": 100,
	"creatures_caught": 0,
	"distance_traveled": 0.0,
	"battles_won": 0,
	"medals": [],
	"inventory": {
		"creatures": [],
		"items": [],
		"medals": []
	}
}

# Authentication
var auth_token: String = ""
var is_logged_in: bool = false

# Game State
var current_scene: String = ""
var previous_scene: String = ""
var is_in_battle: bool = false
var current_battle: Dictionary = {}

# GPS
var player_position: Vector2 = Vector2.ZERO
var player_altitude: float = 0.0
var gps_accuracy: float = 10.0

# Creatures nearby
var nearby_creatures: Array = []
var nearby_pois: Array = []

# Quests
var active_quests: Array = []
var completed_quests: Array = []

func _ready():
	print("GameManager initialized")
	_load_saved_data()

func set_auth_token(token: String):
	auth_token = token
	is_logged_in = true
	_save_auth_token()

func clear_auth():
	auth_token = ""
	is_logged_in = false
	player_data = {}
	_save_auth_token()

func update_player_data(data: Dictionary):
	player_data.merge(data, true)
	player_data_updated.emit(player_data)
	_save_player_data()

func add_creature_to_inventory(creature: Dictionary):
	player_data["inventory"]["creatures"].append(creature)
	player_data["creatures_caught"] += 1
	creature_captured.emit(creature["id"])
	update_player_data(player_data)

func start_battle(enemy_creature: Dictionary):
	is_in_battle = true
	current_battle = {
		"enemy": enemy_creature,
		"turn": 1,
		"player_hp": 100,
		"enemy_hp": enemy_creature.get("hp", 100)
	}
	battle_started.emit(enemy_creature)

func end_battle(won: bool):
	is_in_battle = false
	if won:
		player_data["battles_won"] += 1
	battle_ended.emit(won)
	update_player_data(player_data)
	current_battle = {}

func discover_poi(poi_id: String):
	poi_discovered.emit(poi_id)

func complete_quest(quest_id: String):
	quest_completed.emit(quest_id)
	for quest in active_quests:
		if quest["id"] == quest_id:
			completed_quests.append(quest)
			active_quests.erase(quest)
			break

func update_player_position(lat: float, lon: float, alt: float = 0.0, accuracy: float = 10.0):
	player_position = Vector2(lon, lat)
	player_altitude = alt
	gps_accuracy = accuracy

func get_player_position_dict() -> Dictionary:
	return {
		"lat": player_position.y,
		"lon": player_position.x,
		"altitude": player_altitude,
		"accuracy": gps_accuracy
	}

func calculate_distance_to(lat: float, lon: float) -> float:
	var target = Vector2(lon, lat)
	# Simplified distance calculation (Haversine would be better)
	return player_position.distance_to(target) * 111000  # Convert to meters

# Save/Load
func _save_auth_token():
	var config = ConfigFile.new()
	config.set_value("auth", "token", auth_token)
	config.save("user://auth.cfg")

func _save_player_data():
	var config = ConfigFile.new()
	config.set_value("player", "data", player_data)
	config.save("user://player.cfg")

func _load_saved_data():
	var config = ConfigFile.new()
	var err = config.load("user://auth.cfg")
	if err == OK:
		auth_token = config.get_value("auth", "token", "")
		is_logged_in = auth_token != ""
	
	err = config.load("user://player.cfg")
	if err == OK:
		player_data = config.get_value("player", "data", player_data)

func reset_all_data():
	player_data = {
		"id": "",
		"username": "",
		"email": "",
		"level": 1,
		"xp": 0,
		"max_xp": 100,
		"creatures_caught": 0,
		"distance_traveled": 0.0,
		"battles_won": 0,
		"medals": [],
		"inventory": {
			"creatures": [],
			"items": [],
			"medals": []
		}
	}
	auth_token = ""
	is_logged_in = false
	_save_auth_token()
	_save_player_data()
