extends Control

@onready var back_button = $BackButton
@onready var photo = $Photo
@onready var title = $Title
@onready var description = $Description
@onready var interact = $Interact

var poi_data: Dictionary = {}

func _ready():
	back_button.pressed.connect(_on_back)
	interact.pressed.connect(_on_interact)
	_load_poi()

func _load_poi():
	# Get POI data from GameManager
	if GameManager.nearby_pois.size() > 0:
		poi_data = GameManager.nearby_pois[0]
	else:
		poi_data = {
			"id": "test_poi",
			"name": "Castello di Udine",
			"type": "castle",
			"description": "Il Castello di Udine è uno dei simboli della città. Costruito nel XVI secolo, domina la città dalla collina del Castello.",
			"rewards": {"xp": 50, "badge": "visitatore"},
			"distance": 150
		}
	
	title.text = poi_data.get("name", "POI")
	description.text = poi_data.get("description", "Nessuna descrizione disponibile.")

func _on_back():
	AudioManager.play_click()
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")

func _on_interact():
	AudioManager.play_click()
	AudioManager.play_success()
	
	# Reward player
	var rewards = poi_data.get("rewards", {})
	var xp = rewards.get("xp", 0)
	GameManager.player_data["xp"] += xp
	
	GameManager.discover_poi(poi_data.get("id", ""))
	
	# Show feedback
	interact.text = "✓ Scoperto!"
	interact.disabled = true
	
	await get_tree().create_timer(1.5).timeout
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")
