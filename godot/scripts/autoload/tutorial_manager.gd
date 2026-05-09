extends Node

# Tutorial Manager - Gestisce il tutorial e onboarding

const TUTORIAL_COMPLETED_KEY = "tutorial_completed"
const TUTORIAL_STEP_KEY = "tutorial_step"
const TOTAL_STEPS = 5

var current_step: int = 0
var is_completed: bool = false
var is_skipped: bool = false

signal tutorial_started
signal tutorial_step_changed(step: int)
signal tutorial_completed

func _ready():
	_load_tutorial_state()

func _load_tutorial_state():
	is_completed = false  # In a real game, would load from save file
	is_skipped = false
	current_step = 0

func start_tutorial():
	current_step = 0
	tutorial_started.emit()
	_show_step(0)

func _show_step(step: int):
	if step >= TOTAL_STEPS:
		_complete_tutorial()
		return
	
	current_step = step
	tutorial_step_changed.emit(step)
	
	var step_titles = [
		"Benvenuto in Friulander!",
		"Esplora la Mappa",
		"La tua Prima Creatura!",
		"Cattura in AR",
		"Inventario e Missioni"
	]
	
	var step_descriptions = [
		"Il Friuli è una terra magica abitata da creature straordinarie.",
		"Usa il GPS per trovare creature e POI nei dintorni.",
		"C'è una creatura nelle vicinanze! Tocca per catturarla.",
		"Usa la fotocamera per vedere le creature nel mondo reale.",
		"Gestisci le tue creature nell'inventario e completa missioni."
	]
	
	print("Tutorial Step ", step + 1, "/", TOTAL_STEPS, ": ", step_titles[step])
	print(step_descriptions[step])

func next_step():
	_show_step(current_step + 1)

func skip_tutorial():
	is_skipped = true
	tutorial_completed.emit()

func _complete_tutorial():
	is_completed = true
	tutorial_completed.emit()
	_save_tutorial_state()

func _save_tutorial_state():
	# Would save to file in real implementation
	pass

func reset_tutorial():
	is_completed = false
	is_skipped = false
	current_step = 0
	start_tutorial()

func is_tutorial_completed() -> bool:
	return is_completed

func should_show_tutorial() -> bool:
	return not is_completed and not is_skipped
