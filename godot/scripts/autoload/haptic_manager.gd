extends Node

# Haptic Manager - Gestione feedback tattile

var is_enabled: bool = true

func _ready():
	_check_haptic_support()

func _check_haptic_support():
	# Check if haptic feedback is supported on this platform
	is_enabled = OS.has_feature("android") or OS.has_feature("ios")
	if not is_enabled:
		print("Haptic feedback non supportato su questo dispositivo")

func vibrate(duration_ms: int = 100):
	if not is_enabled:
		return
	Input.vibrate_handheld(duration_ms)

func vibrate_pattern(pattern: Array):
	if not is_enabled:
		return
	for i in range(pattern.size()):
		var duration = pattern[i]
		vibrate(duration)
		await get_tree().create_timer(duration / 1000.0).timeout

# Pattern predefiniti
func click():
	vibrate(50)

func capture_success():
	vibrate_pattern([100, 50, 100])

func capture_fail():
	vibrate_pattern([50, 50, 50, 50])

func battle_hit():
	vibrate(150)

func battle_win():
	vibrate_pattern([100, 100, 200])

func level_up():
	vibrate_pattern([50, 50, 100, 50, 50, 200])

func quest_complete():
	vibrate_pattern([100, 50, 100, 50, 100])

func notification_vibrate():
	vibrate_pattern([50, 100])

func error():
	vibrate_pattern([200, 100, 200])

func set_enabled(enabled: bool):
	is_enabled = enabled
