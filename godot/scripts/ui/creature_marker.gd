extends Control

# Creature Marker - Map marker for creature spawn

signal creature_selected(creature: Dictionary)

@onready var button: Button = $Button
@onready var icon_label: Label = $Button/IconLabel
@onready var name_label: Label = $Button/NameLabel
@onready var animation_player: AnimationPlayer = $AnimationPlayer

var creature_data: Dictionary = {}
var creature_id: String = ""

func _ready():
	button.pressed.connect(_on_pressed)
	_animate_spawn()

func set_creature_data(data: Dictionary):
	creature_data = data
	creature_id = data.get("id", "")
	
	var name = data.get("name", "Creatura")
	var rarity = data.get("rarity", "common")
	
	name_label.text = name
	
	# Set icon based on creature type
	var creature_type = data.get("type", "normal")
	icon_label.text = _get_creature_icon(creature_type)
	
	# Set color based on rarity
	button.modulate = _get_rarity_color(rarity)

func _get_creature_icon(type: String) -> String:
	match type:
		"fire": return "🔥"
		"water": return "💧"
		"grass": return "🌿"
		"electric": return "⚡"
		"ground": return "🌍"
		"flying": return "🦅"
		"ice": return "❄️"
		"rock": return "🪨"
		_: return "🐾"

func _get_rarity_color(rarity: String) -> Color:
	match rarity:
		"common": return Color(1, 1, 1, 1)
		"uncommon": return Color(0.5, 0.8, 0.5, 1)
		"rare": return Color(0.3, 0.5, 0.9, 1)
		"epic": return Color(0.7, 0.3, 0.9, 1)
		"legendary": return Color(1, 0.8, 0.2, 1)
		_: return Color(1, 1, 1, 1)

func _animate_spawn():
	# Scale up animation
	scale = Vector2.ZERO
	
	var tween = create_tween()
	tween.set_ease(Tween.EASE_OUT)
	tween.set_trans(Tween.TRANS_BACK)
	tween.tween_property(self, "scale", Vector2.ONE, 0.5)

func _on_pressed():
	AudioManager.play_click()
	
	# Pulse animation
	var tween = create_tween()
	tween.tween_property(self, "scale", Vector2(1.2, 1.2), 0.1)
	tween.tween_property(self, "scale", Vector2.ONE, 0.1)
	
	creature_selected.emit(creature_data)

func get_distance() -> float:
	return creature_data.get("distance", 999.0)
