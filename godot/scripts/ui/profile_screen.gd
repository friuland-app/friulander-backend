extends Control

@onready var back_button = $BackButton
@onready var name_label = $Name
@onready var level_label = $Level
@onready var xp_bar = $XPBar

func _ready():
	back_button.pressed.connect(_on_back)
	_update_profile()

func _update_profile():
	var data = GameManager.player_data
	name_label.text = data.get("username", "Giocatore")
	level_label.text = "Livello " + str(data.get("level", 1))
	xp_bar.value = float(data.get("xp", 0)) / data.get("max_xp", 100) * 100

func _on_back():
	AudioManager.play_click()
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")
