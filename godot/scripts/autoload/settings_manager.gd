extends Node

# Settings Manager - Gestisce le impostazioni del gioco

var music_volume: float = 0.5
var sfx_volume: float = 0.7
var master_volume: float = 0.8
var notifications_enabled: bool = true
var gps_enabled: bool = true

func _ready():
	_load_settings()

func _load_settings():
	# Would load from config file in real implementation
	pass

func set_music_volume(value: float):
	music_volume = clamp(value, 0.0, 1.0)
	AudioManager.set_music_volume(music_volume)

func set_sfx_volume(value: float):
	sfx_volume = clamp(value, 0.0, 1.0)
	AudioManager.set_sfx_volume(sfx_volume)

func set_master_volume(value: float):
	master_volume = clamp(value, 0.0, 1.0)
	AudioManager.set_master_volume(master_volume)

func toggle_notifications():
	notifications_enabled = !notifications_enabled

func toggle_gps():
	gps_enabled = !gps_enabled
