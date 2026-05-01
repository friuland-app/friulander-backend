extends Control

# Map HUD - Main game screen with map and navigation

@onready var map_view: ColorRect = $MapView
@onready var compass: Control = $Compass
@onready var player_name: Label = $TopBar/PlayerInfo/InfoContainer/PlayerName
@onready var level_label: Label = $TopBar/PlayerInfo/InfoContainer/LevelLabel
@onready var xp_bar: ProgressBar = $TopBar/PlayerInfo/InfoContainer/XPBar
@onready var weather_label: Label = $TopBar/WeatherWidget/WeatherLabel
@onready var avatar_button: Button = $TopBar/PlayerInfo/AvatarButton
@onready var profile_button: Button = $BottomBar/ProfileButton
@onready var inventory_button: Button = $BottomBar/InventoryButton
@onready var ar_button: Button = $BottomBar/ARButton
@onready var quests_button: Button = $BottomBar/QuestsButton
@onready var settings_button: Button = $BottomBar/SettingsButton
@onready var quest_badge: Panel = $BottomBar/QuestsButton/QuestBadge
@onready var badge_label: Label = $BottomBar/QuestsButton/QuestBadge/BadgeLabel
@onready var notification_panel: Panel = $NotificationPanel
@onready var notification_label: Label = $NotificationPanel/NotificationLabel
@onready var notification_timer: Timer = $NotificationTimer
@onready var gps_timer: Timer = $GPSTimer
@onready var spawn_timer: Timer = $SpawnTimer
@onready var loading_overlay: ColorRect = $LoadingOverlay
@onready var creature_spawns: Node2D = $CreatureSpawns
@onready var poi_markers: Node2D = $POIMarkers

var creature_scene = preload("res://scenes/creature_marker.tscn")
var poi_scene = preload("res://scenes/poi_marker.tscn")

var nearby_creatures: Array = []
var nearby_pois: Array = []

func _ready():
	_setup_ui()
	_setup_event_listeners()
	_start_gps_updates()
	_fetch_nearby_data()
	
	# Connect to GameManager signals
	GameManager.player_data_updated.connect(_on_player_data_updated)
	GameManager.creature_captured.connect(_on_creature_captured)
	GameManager.poi_discovered.connect(_on_poi_discovered)

func _setup_ui():
	_update_player_ui()
	_update_quest_badge()

func _setup_event_listeners():
	avatar_button.pressed.connect(_on_avatar_pressed)
	profile_button.pressed.connect(_on_profile_pressed)
	inventory_button.pressed.connect(_on_inventory_pressed)
	ar_button.pressed.connect(_on_ar_pressed)
	quests_button.pressed.connect(_on_quests_pressed)
	settings_button.pressed.connect(_on_settings_pressed)
	
	notification_timer.timeout.connect(_hide_notification)
	gps_timer.timeout.connect(_on_gps_update)
	spawn_timer.timeout.connect(_on_spawn_update)

func _start_gps_updates():
	gps_timer.start()
	spawn_timer.start()

func _fetch_nearby_data():
	_show_loading(true)
	
	var pos = GameManager.get_player_position_dict()
	if pos["lat"] != 0.0 or pos["lon"] != 0.0:
		ApiClient.get_creatures_nearby(pos["lat"], pos["lon"], 500.0)
		ApiClient.get_pois_nearby(pos["lat"], pos["lon"], 1000.0)
	else:
		# Use default position if GPS not available
		ApiClient.get_creatures_nearby(46.0626, 13.2381, 500.0)
		ApiClient.get_pois_nearby(46.0626, 13.2381, 1000.0)

func _update_player_ui():
	var data = GameManager.player_data
	player_name.text = data.get("username", "Giocatore")
	level_label.text = "Liv. " + str(data.get("level", 1))
	
	var xp = data.get("xp", 0)
	var max_xp = data.get("max_xp", 100)
	xp_bar.value = float(xp) / float(max_xp) * 100.0

func _update_quest_badge():
	var available_rewards = 0  # Calculate from quests
	
	if available_rewards > 0:
		quest_badge.visible = true
		badge_label.text = str(available_rewards)
	else:
		quest_badge.visible = false

func _show_notification(message: String, duration: float = 3.0):
	notification_label.text = message
	notification_panel.visible = true
	
	# Animate in
	notification_panel.position.y = -100
	var tween = create_tween()
	tween.set_ease(Tween.EASE_OUT)
	tween.set_trans(Tween.TRANS_BACK)
	tween.tween_property(notification_panel, "position:y", 16, 0.5)
	
	notification_timer.wait_time = duration
	notification_timer.start()

func _hide_notification():
	var tween = create_tween()
	tween.set_ease(Tween.EASE_IN)
	tween.tween_property(notification_panel, "position:y", -100, 0.3)
	tween.finished.connect(func(): notification_panel.visible = false)

func _show_loading(show: bool):
	loading_overlay.visible = show

func _on_player_data_updated(data: Dictionary):
	_update_player_ui()

func _on_creature_captured(creature_id: String):
	_show_notification("Creatura catturata!")
	_fetch_nearby_data()

func _on_poi_discovered(poi_id: String):
	_show_notification("POI scoperto!")

func _on_avatar_pressed():
	AudioManager.play_click()
	_go_to_profile()

func _on_profile_pressed():
	AudioManager.play_click()
	_go_to_profile()

func _on_inventory_pressed():
	AudioManager.play_click()
	get_tree().change_scene_to_file("res://scenes/inventory_screen.tscn")

func _on_ar_pressed():
	AudioManager.play_click()
	# Check if creatures nearby
	if nearby_creatures.size() > 0:
		get_tree().change_scene_to_file("res://scenes/ar_capture.tscn")
	else:
		_show_notification("Nessuna creatura nelle vicinanze!")

func _on_quests_pressed():
	AudioManager.play_click()
	get_tree().change_scene_to_file("res://scenes/quests_screen.tscn")

func _on_settings_pressed():
	AudioManager.play_click()
	get_tree().change_scene_to_file("res://scenes/settings_screen.tscn")

func _go_to_profile():
	get_tree().change_scene_to_file("res://scenes/profile_screen.tscn")

func _on_gps_update():
	# Update player position from GPS
	# This would use actual device GPS
	pass

func _on_spawn_update():
	# Check for new spawns
	_fetch_nearby_data()

func update_creature_spawns(creatures: Array):
	# Clear existing markers
	for child in creature_spawns.get_children():
		child.queue_free()
	
	nearby_creatures = creatures
	
	# Create new markers
	for creature in creatures:
		var marker = creature_scene.instantiate()
		marker.set_creature_data(creature)
		marker.creature_selected.connect(_on_creature_selected)
		creature_spawns.add_child(marker)
		
		# Position marker (simplified - would use actual GPS coords)
		var pos = _world_to_screen(
			creature.get("lat", 46.0626),
			creature.get("lon", 13.2381)
		)
		marker.position = pos

func update_poi_markers(pois: Array):
	# Clear existing markers
	for child in poi_markers.get_children():
		child.queue_free()
	
	nearby_pois = pois
	
	# Create new markers
	for poi in pois:
		var marker = poi_scene.instantiate()
		marker.set_poi_data(poi)
		marker.poi_selected.connect(_on_poi_selected)
		poi_markers.add_child(marker)
		
		# Position marker
		var pos = _world_to_screen(
			poi.get("lat", 46.0626),
			poi.get("lon", 13.2381)
		)
		marker.position = pos

func _world_to_screen(lat: float, lon: float) -> Vector2:
	# Simplified projection - in real game would use proper map projection
	var viewport_size = get_viewport_rect().size
	
	# Center of map (approximate center of Friuli)
	var center_lat = 46.0626
	var center_lon = 13.2381
	
	# Scale factor
	var scale = 100000.0
	
	var x = (lon - center_lon) * scale + viewport_size.x / 2
	var y = (center_lat - lat) * scale + viewport_size.y / 2
	
	return Vector2(x, y)

func _on_creature_selected(creature: Dictionary):
	AudioManager.play_click()
	_show_notification("Creatura: " + creature.get("name", "Sconosciuta"))
	# Could show creature details or start capture

func _on_poi_selected(poi: Dictionary):
	AudioManager.play_click()
	_show_notification("POI: " + poi.get("name", "Sconosciuto"))
