extends Control

# Inventory Screen

@onready var back_button: Button = $Header/BackButton
@onready var creatures_tab: Button = $TabContainer/CreaturesTab
@onready var items_tab: Button = $TabContainer/ItemsTab
@onready var medals_tab: Button = $TabContainer/MedalsTab
@onready var creatures_panel: Panel = $CreaturesPanel
@onready var items_panel: Panel = $ItemsPanel
@onready var medals_panel: Panel = $MedalsPanel
@onready var creatures_grid: GridContainer = $CreaturesPanel/ScrollContainer/CreaturesGrid

var creature_card_scene = preload("res://scenes/creature_card.tscn")

var current_tab: int = 0

func _ready():
	back_button.pressed.connect(_on_back)
	creatures_tab.pressed.connect(func(): _switch_tab(0))
	items_tab.pressed.connect(func(): _switch_tab(1))
	medals_tab.pressed.connect(func(): _switch_tab(2))
	_load_creatures()

func _switch_tab(tab: int):
	current_tab = tab
	creatures_panel.visible = tab == 0
	items_panel.visible = tab == 1
	medals_panel.visible = tab == 2

func _on_back():
	AudioManager.play_click()
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")

func _load_creatures():
	var creatures = GameManager.player_data.get("inventory", {}).get("creatures", [])
	for creature in creatures:
		var card = creature_card_scene.instantiate()
		card.set_creature(creature)
		creatures_grid.add_child(card)
