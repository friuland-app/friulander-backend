extends Button

signal creature_selected(creature: Dictionary)

@onready var icon_label: Label = $Icon
@onready var name_label: Label = $Name
@onready var level_label: Label = $Level

var creature_data: Dictionary = {}

func _ready():
	pressed.connect(_on_pressed)

func set_creature(data: Dictionary):
	creature_data = data
	name_label.text = data.get("name", "Creatura")
	level_label.text = "Lv. " + str(data.get("level", 1))
	icon_label.text = _get_icon(data.get("type", "normal"))
	modulate = _get_rarity_color(data.get("rarity", "common"))

func _get_icon(type: String) -> String:
	match type:
		"fire": return "🔥"
		"water": return "💧"
		"grass": return "🌿"
		"electric": return "⚡"
		_: return "🐾"

func _get_rarity_color(rarity: String) -> Color:
	match rarity:
		"common": return Color.WHITE
		"rare": return Color.BLUE
		"epic": return Color.PURPLE
		"legendary": return Color.GOLD
		_: return Color.WHITE

func _on_pressed():
	AudioManager.play_click()
	creature_selected.emit(creature_data)
