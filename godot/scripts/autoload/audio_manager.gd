extends Node

# Audio Manager - Handles all audio playback

@export var music_volume: float = 0.5
@export var sfx_volume: float = 0.7
@export var master_volume: float = 0.8

var music_player: AudioStreamPlayer
var sfx_players: Array = []
var max_sfx_players: int = 5

# Audio resources (to be loaded)
# UI SFX
var click_sound: AudioStream
var hover_sound: AudioStream
var navigate_sound: AudioStream

# Game SFX
var capture_success_sound: AudioStream
var capture_fail_sound: AudioStream
var battle_start_sound: AudioStream
var attack_hit_sound: AudioStream
var attack_miss_sound: AudioStream
var level_up_sound: AudioStream
var quest_complete_sound: AudioStream

# General SFX
var notification_sound: AudioStream
var success_sound: AudioStream
var error_sound: AudioStream
var achievement_sound: AudioStream

# Background music tracks
var main_theme: AudioStream
var map_ambient: AudioStream
var battle_theme: AudioStream
var ar_capture_music: AudioStream
var victory_music: AudioStream
var defeat_music: AudioStream

var background_music: AudioStream
var current_music_track: String = ""

func _ready():
	_setup_audio_players()
	_load_audio_resources()
	_apply_volumes()

func _setup_audio_players():
	# Music player
	music_player = AudioStreamPlayer.new()
	music_player.bus = "Music"
	add_child(music_player)
	
	# SFX players (pool)
	for i in range(max_sfx_players):
		var player = AudioStreamPlayer.new()
		player.bus = "SFX"
		add_child(player)
		sfx_players.append(player)

func _load_audio_resources():
	# Load SFX
	click_sound = load("res://assets/audio/sfx/ui_click.ogg")
	hover_sound = load("res://assets/audio/sfx/ui_hover.ogg")
	navigate_sound = click_sound
	capture_success_sound = load("res://assets/audio/sfx/capture_success.ogg")
	capture_fail_sound = load("res://assets/audio/sfx/capture_fail.ogg")
	battle_start_sound = load("res://assets/audio/sfx/battle_start.ogg")
	attack_hit_sound = load("res://assets/audio/sfx/attack_hit.ogg")
	attack_miss_sound = load("res://assets/audio/sfx/attack_miss.ogg")
	level_up_sound = load("res://assets/audio/sfx/level_up.ogg")
	quest_complete_sound = load("res://assets/audio/sfx/quest_complete.ogg")
	notification_sound = load("res://assets/audio/sfx/notification.ogg")
	success_sound = capture_success_sound
	achievement_sound = quest_complete_sound
	error_sound = load("res://assets/audio/sfx/error.ogg")
	
	# Load music (placeholders - uncomment with actual files)
	# main_theme = load("res://assets/audio/music/main_theme.ogg")
	# map_ambient = load("res://assets/audio/music/map_ambient.ogg")
	# battle_theme = load("res://assets/audio/music/battle_theme.ogg")
	# ar_capture_music = load("res://assets/audio/music/ar_capture.ogg")
	# victory_music = load("res://assets/audio/music/victory.ogg")
	# defeat_music = load("res://assets/audio/music/defeat.ogg")

# Music scene-specific playback
func play_main_theme():
	if main_theme != null:
		play_music(main_theme, true)
		current_music_track = "main_theme"

func play_map_ambient():
	if map_ambient != null:
		play_music(map_ambient, true)
		current_music_track = "map_ambient"

func play_battle_theme():
	if battle_theme != null:
		play_music(battle_theme, true)
		current_music_track = "battle_theme"

func play_ar_capture():
	if ar_capture_music != null:
		play_music(ar_capture_music, true)
		current_music_track = "ar_capture"

func play_victory():
	if victory_music != null:
		play_music(victory_music, false)
		current_music_track = "victory"

func play_defeat():
	if defeat_music != null:
		play_music(defeat_music, false)
		current_music_track = "defeat"

func fade_to_music(new_stream: AudioStream, duration: float = 1.0):
	# Smooth transition between tracks
	var tween = create_tween()
	tween.tween_method(_fade_volume, music_volume, 0.0, duration * 0.5)
	tween.tween_callback(func():
		play_music(new_stream, true)
		_apply_volumes()
	)
	tween.tween_method(_fade_volume, 0.0, music_volume, duration * 0.5)

func _fade_volume(value: float):
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Music"), linear_to_db(value))

func _apply_volumes():
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Master"), linear_to_db(master_volume))
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Music"), linear_to_db(music_volume))
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("SFX"), linear_to_db(sfx_volume))

func play_music(stream: AudioStream, loop: bool = true):
	if stream == null:
		return
	
	background_music = stream
	music_player.stream = stream
	music_player.stream.loop = loop
	music_player.play()

func stop_music():
	music_player.stop()

func play_sfx(stream: AudioStream):
	if stream == null:
		return
	
	# Find available SFX player
	for player in sfx_players:
		if not player.playing:
			player.stream = stream
			player.play()
			return
	
	# If all busy, stop the oldest one
	sfx_players[0].stop()
	sfx_players[0].stream = stream
	sfx_players[0].play()

func play_click():
	play_sfx(click_sound)

func play_hover():
	play_sfx(hover_sound)

func play_navigate():
	play_sfx(navigate_sound)

func play_notification():
	play_sfx(notification_sound)

func play_success():
	play_sfx(success_sound)

func play_error():
	play_sfx(error_sound)

func play_achievement():
	play_sfx(achievement_sound)

# Capture SFX
func play_capture_success():
	play_sfx(capture_success_sound)

func play_capture_fail():
	play_sfx(capture_fail_sound)

# Battle SFX
func play_battle_start():
	play_sfx(battle_start_sound)

func play_attack_hit():
	play_sfx(attack_hit_sound)

func play_attack_miss():
	play_sfx(attack_miss_sound)

# Progression SFX
func play_level_up():
	play_sfx(level_up_sound)

func play_quest_complete():
	play_sfx(quest_complete_sound)

func set_master_volume(value: float):
	master_volume = clamp(value, 0.0, 1.0)
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Master"), linear_to_db(master_volume))

func set_music_volume(value: float):
	music_volume = clamp(value, 0.0, 1.0)
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Music"), linear_to_db(music_volume))

func set_sfx_volume(value: float):
	sfx_volume = clamp(value, 0.0, 1.0)
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("SFX"), linear_to_db(sfx_volume))

func toggle_mute():
	var master_bus = AudioServer.get_bus_index("Master")
	AudioServer.set_bus_mute(master_bus, not AudioServer.is_bus_mute(master_bus))

func is_muted() -> bool:
	return AudioServer.is_bus_mute(AudioServer.get_bus_index("Master"))
