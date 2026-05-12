extends Control

@onready var stability_bar: ProgressBar = $StabilityIndicator
@onready var health_bar: ProgressBar = $CreatureHealthBar
@onready var creature_name: Label = $CreatureName
@onready var flee_button: Button = $FleeButton
@onready var back_button: Button = $BackButton

var selected_trap: String = "basic"
var stability: float = 50.0
var target_creature: Dictionary = {}

func _ready():
	flee_button.pressed.connect(_on_flee)
	back_button.pressed.connect(_on_back)
	CameraManager.start_camera()
	_load_creature()

func _load_creature():
	if GameManager.nearby_creatures.size() > 0:
		target_creature = GameManager.nearby_creatures[0]
	creature_name.text = target_creature.get("name", "Creatura")

func _on_flee():
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")

func _on_back():
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")

func _input(event):
	if event is InputEventScreenTouch and event.pressed:
		_try_capture()

func _try_capture():
	var chance = stability / 100.0
	if randf() < chance:
		GameManager.add_creature_to_inventory(target_creature)
		get_tree().change_scene_to_file("res://scenes/map_hud.tscn")
