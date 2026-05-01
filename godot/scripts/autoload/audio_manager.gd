extends Node

# Audio Manager - Handles all audio playback

@export var music_volume: float = 0.5
@export var sfx_volume: float = 0.7
@export var master_volume: float = 0.8

var music_player: AudioStreamPlayer
var sfx_players: Array = []
var max_sfx_players: int = 5

# Audio resources (to be loaded)
var click_sound: AudioStream
var navigate_sound: AudioStream
var notification_sound: AudioStream
var success_sound: AudioStream
var error_sound: AudioStream
var achievement_sound: AudioStream

var background_music: AudioStream

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
	# These would be loaded from actual audio files
	# click_sound = load("res://audio/click.wav")
	# navigate_sound = load("res://audio/navigate.wav")
	# etc.
	pass

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
