extends Control

@onready var enemy_health: ProgressBar = $EnemyHealth
@onready var player_health: ProgressBar = $PlayerHealth
@onready var enemy_name: Label = $EnemyName
@onready var player_name: Label = $PlayerName
@onready var battle_log: Label = $BattleLog
@onready var move1: Button = $MovesPanel/Move1
@onready var move2: Button = $MovesPanel/Move2
@onready var move3: Button = $MovesPanel/Move3
@onready var flee_button: Button = $MovesPanel/Flee

var enemy_creature: Dictionary = {}
var player_creature: Dictionary = {}
var battle_active: bool = true

func _ready():
	_setup_battle()
	move1.pressed.connect(_on_move.bind("attack"))
	move2.pressed.connect(_on_move.bind("defend"))
	move3.pressed.connect(_on_move.bind("special"))
	flee_button.pressed.connect(_on_flee)

func _setup_battle():
	enemy_creature = GameManager.current_battle.get("enemy", {})
	player_creature = GameManager.player_data.get("inventory", {}).get("creatures", [{}])[0]
	
	enemy_name.text = enemy_creature.get("name", "Nemico")
	player_name.text = player_creature.get("name", "Tua Creatura")
	
	enemy_health.value = enemy_creature.get("hp", 100)
	player_health.value = player_creature.get("hp", 100)

func _on_move(move_type: String):
	if not battle_active:
		return
	
	var damage = _calculate_damage(move_type)
	enemy_health.value -= damage
	battle_log.text = "Hai usato " + move_type + "! Danno: " + str(damage)
	
	if enemy_health.value <= 0:
		_end_battle(true)
	else:
		_enemy_turn()

func _calculate_damage(move_type: String) -> int:
	match move_type:
		"attack": return randi_range(10, 20)
		"special": return randi_range(20, 30)
		_: return 5

func _enemy_turn():
	var damage = randi_range(5, 15)
	player_health.value -= damage
	battle_log.text += "\nIl nemico attacca! Danno: " + str(damage)
	
	if player_health.value <= 0:
		_end_battle(false)

func _on_flee():
	_end_battle(false)

func _end_battle(won: bool):
	battle_active = false
	GameManager.end_battle(won)
	
	if won:
		battle_log.text = "Vittoria!"
	else:
		battle_log.text = "Sconfitta!"
	
	await get_tree().create_timer(2.0).timeout
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")
