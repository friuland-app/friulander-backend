extends Node

# API Client - Handles all HTTP requests to the backend

const API_BASE_URL: String = "http://localhost:3000/api"  # Change to production URL

var http_request: HTTPRequest
var pending_requests: Dictionary = {}

signal request_completed(response: Dictionary)
signal request_failed(error: String)
signal auth_success(token: String)
signal auth_error(message: String)

func _ready():
	http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(_on_request_completed)

# Generic request method
func _make_request(endpoint: String, method: HTTPClient.Method, body: Dictionary = {}, headers: Array = []) -> String:
	var url: String = API_BASE_URL + endpoint
	var request_id: String = str(Time.get_unix_time_from_system()) + str(randi())
	
	var request_headers: Array = headers.duplicate()
	request_headers.append("Content-Type: application/json")
	
	if GameManager.auth_token != "":
		request_headers.append("Authorization: Bearer " + GameManager.auth_token)
	
	var body_json: String = ""
	if body.size() > 0:
		body_json = JSON.stringify(body)
	
	var error = http_request.request(url, request_headers, method, body_json)
	if error != OK:
		request_failed.emit("Failed to make request: " + str(error))
		return ""
	
	pending_requests[request_id] = {
		"endpoint": endpoint,
		"method": method,
		"body": body
	}
	
	return request_id

func _on_request_completed(result: int, response_code: int, headers: PackedStringArray, body: PackedByteArray):
	if result != HTTPRequest.RESULT_SUCCESS:
		request_failed.emit("Request failed with result: " + str(result))
		return
	
	var json = JSON.new()
	var error = json.parse(body.get_string_from_utf8())
	
	if error != OK:
		request_failed.emit("Failed to parse JSON response")
		return
	
	var response = json.get_data()
	
	if response_code >= 200 and response_code < 300:
		request_completed.emit(response)
		
		# Call callback if exists
		for request_id in pending_requests.keys():
			if pending_requests[request_id].has("callback"):
				pending_requests[request_id]["callback"].call(response)
				pending_requests.erase(request_id)
				break
	else:
		request_failed.emit(response.get("message", "Unknown error"))

# Authentication
func register(email: String, password: String, username: String):
	var body = {
		"email": email,
		"password": password,
		"username": username
	}
	var request_id = _make_request("/auth/register", HTTPClient.METHOD_POST, body)
	if request_id != "":
		pending_requests[request_id]["callback"] = func(response):
			if response.has("token"):
				GameManager.set_auth_token(response["token"])
				auth_success.emit(response["token"])
			elif response.has("message"):
				auth_error.emit(response["message"])
			else:
				auth_error.emit("Registration failed")

func login(email: String, password: String):
	var body = {
		"email": email,
		"password": password
	}
	var request_id = _make_request("/auth/login", HTTPClient.METHOD_POST, body)
	if request_id != "":
		pending_requests[request_id]["callback"] = func(response):
			if response.has("token"):
				GameManager.set_auth_token(response["token"])
				auth_success.emit(response["token"])
			elif response.has("message"):
				auth_error.emit(response["message"])
			else:
				auth_error.emit("Login failed")

func logout():
	_make_request("/auth/logout", HTTPClient.METHOD_POST)
	GameManager.clear_auth()

func google_auth(id_token: String):
	var body = {
		"id_token": id_token
	}
	var request_id = _make_request("/auth/google", HTTPClient.METHOD_POST, body)
	if request_id != "":
		pending_requests[request_id]["callback"] = func(response):
			if response.has("token"):
				GameManager.set_auth_token(response["token"])
				auth_success.emit(response["token"])
			elif response.has("message"):
				auth_error.emit(response["message"])
			else:
				auth_error.emit("Google auth failed")

# Player
func get_player_profile():
	_make_request("/player/profile", HTTPClient.METHOD_GET)

func update_player_position(lat: float, lon: float):
	var body = {
		"lat": lat,
		"lon": lon
	}
	_make_request("/player/position", HTTPClient.METHOD_POST, body)

func update_player_stats(stats: Dictionary):
	_make_request("/player/stats", HTTPClient.METHOD_POST, stats)

# Creatures
func get_all_creatures():
	_make_request("/creatures", HTTPClient.METHOD_GET)

func get_creatures_nearby(lat: float, lon: float, radius: float = 500.0):
	var body = {
		"lat": lat,
		"lon": lon,
		"radius": radius
	}
	_make_request("/creatures/nearby", HTTPClient.METHOD_POST, body)

func capture_creature(creature_id: String, lat: float, lon: float, trap_type: String):
	var body = {
		"creature_id": creature_id,
		"lat": lat,
		"lon": lon,
		"trap_type": trap_type
	}
	_make_request("/creatures/capture", HTTPClient.METHOD_POST, body)

# POI
func get_all_pois():
	_make_request("/poi", HTTPClient.METHOD_GET)

func get_pois_nearby(lat: float, lon: float, radius: float = 1000.0):
	var body = {
		"lat": lat,
		"lon": lon,
		"radius": radius
	}
	_make_request("/poi/nearby", HTTPClient.METHOD_POST, body)

func interact_with_poi(poi_id: String):
	var body = {
		"poi_id": poi_id
	}
	_make_request("/poi/interact", HTTPClient.METHOD_POST, body)

# Battle
func start_battle(creature_id: String, is_pvp: bool = false, opponent_id: String = ""):
	var body = {
		"creature_id": creature_id,
		"is_pvp": is_pvp
	}
	if opponent_id != "":
		body["opponent_id"] = opponent_id
	_make_request("/battle/start", HTTPClient.METHOD_POST, body)

func make_battle_move(battle_id: String, move: String):
	var body = {
		"battle_id": battle_id,
		"move": move
	}
	_make_request("/battle/move", HTTPClient.METHOD_POST, body)

func flee_battle(battle_id: String):
	var body = {
		"battle_id": battle_id
	}
	_make_request("/battle/flee", HTTPClient.METHOD_POST, body)

# Inventory
func get_inventory():
	_make_request("/player/inventory", HTTPClient.METHOD_GET)

func add_item(item_id: String, quantity: int = 1):
	var body = {
		"item_id": item_id,
		"quantity": quantity
	}
	_make_request("/player/inventory/add", HTTPClient.METHOD_POST, body)

# Quests
func get_active_quests():
	_make_request("/player/quests", HTTPClient.METHOD_GET)

func complete_quest(quest_id: String):
	var body = {
		"quest_id": quest_id
	}
	_make_request("/player/quests/complete", HTTPClient.METHOD_POST, body)

# Leaderboard
func get_leaderboard(scope: String = "local"):
	_make_request("/player/leaderboard?scope=" + scope, HTTPClient.METHOD_GET)
