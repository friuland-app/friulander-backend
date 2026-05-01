extends Control

@onready var back_button = $BackButton
@onready var daily_tab = $DailyTab
@onready var weekly_tab = $WeeklyTab
@onready var story_tab = $StoryTab
@onready var quests_container = $QuestsContainer

var quest_card_scene = preload("res://scenes/quest_card.tscn")

func _ready():
	back_button.pressed.connect(_on_back)
	daily_tab.pressed.connect(func(): _load_quests("daily"))
	weekly_tab.pressed.connect(func(): _load_quests("weekly"))
	story_tab.pressed.connect(func(): _load_quests("story"))
	_load_quests("daily")

func _load_quests(type: String):
	for child in quests_container.get_children():
		child.queue_free()
	
	var quests = _get_quests_by_type(type)
	for quest in quests:
		var card = quest_card_scene.instantiate()
		card.set_quest(quest)
		quests_container.add_child(card)

func _get_quests_by_type(type: String) -> Array:
	return GameManager.active_quests.filter(func(q): return q.get("type") == type)

func _on_back():
	AudioManager.play_click()
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")
