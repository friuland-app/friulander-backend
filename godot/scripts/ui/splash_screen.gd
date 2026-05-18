extends Control

# Splash Screen - First scene, shows loading progress

@onready var progress_bar: ProgressBar = $ProgressBar
@onready var loading_label: Label = $LoadingLabel
@onready var quote_label: Label = $QuoteLabel
@onready var animation_player: AnimationPlayer = $AnimationPlayer
@onready var loading_timer: Timer = $LoadingTimer
@onready var scene_transition_timer: Timer = $SceneTransitionTimer

var quotes: Array = [
	"\"Il Friuli è una terra magica\"",
	"\"Il Tagliamento nasconde segreti antichi\"",
	"\"I castelli del Friuli raccontano storie epiche\"",
	"\"Il Collio produce vini straordinari\"",
	"\"Cividale fu capitale longobarda\"",
	"\"La cucina friulana è un tesoro gastronomico\"",
	"\"Il Friuli Venezia Giulia unisce terre e culture\"",
	"\"Il mare, i colli, le montagne: tre anime in una regione\"",
	"\"Aquileia fu una delle più grandi città dell'Impero Romano\"",
	"\"Il Friuli è patria di grandi artisti e scrittori\""
]

var loading_steps: Array = [
	"Connessione al server...",
	"Caricamento asset...",
	"Preparazione mappa...",
	"Finalizzazione..."
]

var current_step: int = 0
var progress: float = 0.0
var is_loading_complete: bool = false

func _ready():
	_setup_ui()
	_show_random_quote()
	_start_loading()
	_animate_logo()

func _setup_ui():
	progress_bar.value = 0
	loading_label.text = loading_steps[0]
	
func _show_random_quote():
	var random_index = randi() % quotes.size()
	quote_label.text = quotes[random_index]

func _animate_logo():
	# Logo scale animation
	var logo = $LogoContainer/VBoxContainer/LogoPlaceholder
	logo.scale = Vector2.ZERO
	
	var tween = create_tween()
	tween.set_ease(Tween.EASE_OUT)
	tween.set_trans(Tween.TRANS_BACK)
	tween.tween_property(logo, "scale", Vector2.ONE, 1.0)
	
	# Title fade in
	var title = $LogoContainer/VBoxContainer/TitleLabel
	title.modulate.a = 0
	
	var title_tween = create_tween()
	title_tween.tween_property(title, "modulate:a", 1.0, 0.5).set_delay(0.5)

func _start_loading():
	loading_timer.timeout.connect(_on_loading_step)
	loading_timer.start()
	
	scene_transition_timer.timeout.connect(_on_loading_complete)

func _on_loading_step():
	if is_loading_complete:
		return
	
	# Simulate loading progress
	progress += randf_range(2.0, 8.0)
	progress = min(progress, 100.0)
	progress_bar.value = progress
	
	# Update loading text based on progress
	var step_index = int((progress / 100.0) * loading_steps.size())
	step_index = clamp(step_index, 0, loading_steps.size() - 1)
	
	if step_index != current_step:
		current_step = step_index
		loading_label.text = loading_steps[current_step]
		_animate_loading_text()
	
	# Check if loading is complete
	if progress >= 100.0:
		is_loading_complete = true
		loading_label.text = "Completato!"
		loading_timer.stop()
		scene_transition_timer.start()
		return
	
	# Continue loading
	loading_timer.start()

func _animate_loading_text():
	var tween = create_tween()
	tween.tween_property(loading_label, "modulate:a", 0.0, 0.1)
	tween.tween_property(loading_label, "modulate:a", 1.0, 0.2)

func _on_loading_complete():
	_transition_to_next_scene()

func _transition_to_next_scene():
	# Fade out
	var tween = create_tween()
	tween.tween_property(self, "modulate:a", 0.0, 0.5)
	tween.finished.connect(_change_scene)

func _change_scene():
	# Go directly to main game scene (no login required)
	get_tree().change_scene_to_file("res://scenes/map_hud.tscn")
