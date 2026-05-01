extends Control

@onready var back_button = $BackButton
@onready var music_slider = $MusicSlider
@onready var sfx_slider = $SFXSlider

func _ready():
	back_button.pressed.connect(_on_back)
	music_slider.value_changed.connect(_on_music_changed)
	sfx_slider.value_changed.connect(_on_sfx_changed)

func _on_music_changed(value):
	AudioManager.set_music_volume(value / 100)

func _on_sfx_changed(value):
	AudioManager.set_sfx_volume(value / 100)

func _on_back():
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")
