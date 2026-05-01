extends Control

# POI Marker - Map marker for Point of Interest

signal poi_selected(poi: Dictionary)

@onready var button: Button = $Button
@onready var icon_label: Label = $Button/IconLabel
@onready var name_label: Label = $Button/NameLabel
@onready var discovered_badge: Panel = $DiscoveredBadge

var poi_data: Dictionary = {}
var poi_id: String = ""
var is_discovered: bool = false

func _ready():
	button.pressed.connect(_on_pressed)

func set_poi_data(data: Dictionary):
	poi_data = data
	poi_id = data.get("id", "")
	
	var name = data.get("name", "POI")
	var type = data.get("type", "landmark")
	is_discovered = data.get("discovered", false)
	
	name_label.text = name
	icon_label.text = _get_poi_icon(type)
	
	discovered_badge.visible = is_discovered

func _get_poi_icon(type: String) -> String:
	match type:
		"castle": return "🏰"
		"church": return "⛪"
		"museum": return "🏛️"
		"park": return "🌳"
		"restaurant": return "🍽️"
		"shop": return "🛍️"
		"landmark": return "📍"
		"nature": return "🌲"
		"viewpoint": return "👁️"
		_: return "📍"

func _on_pressed():
	AudioManager.play_click()
	
	# Pulse animation
	var tween = create_tween()
	tween.tween_property(self, "scale", Vector2(1.2, 1.2), 0.1)
	tween.tween_property(self, "scale", Vector2.ONE, 0.1)
	
	poi_selected.emit(poi_data)

func mark_as_discovered():
	is_discovered = true
	discovered_badge.visible = true
	poi_data["discovered"] = true

func get_distance() -> float:
	return poi_data.get("distance", 999.0)
