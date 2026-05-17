extends Control

# Auth Screen - Login and registration

@onready var login_tab: Button = $MainContainer/Panel/VBoxContainer/TabContainer/LoginTab
@onready var register_tab: Button = $MainContainer/Panel/VBoxContainer/TabContainer/RegisterTab
@onready var login_form: VBoxContainer = $MainContainer/Panel/VBoxContainer/LoginForm
@onready var register_form: VBoxContainer = $MainContainer/Panel/VBoxContainer/RegisterForm

# Login inputs
@onready var login_email: LineEdit = $MainContainer/Panel/VBoxContainer/LoginForm/EmailInput
@onready var login_password: LineEdit = $MainContainer/Panel/VBoxContainer/LoginForm/PasswordInput
@onready var login_button: Button = $MainContainer/Panel/VBoxContainer/LoginForm/LoginButton

# Register inputs
@onready var register_email: LineEdit = $MainContainer/Panel/VBoxContainer/RegisterForm/EmailInput
@onready var register_username: LineEdit = $MainContainer/Panel/VBoxContainer/RegisterForm/UsernameInput
@onready var register_password: LineEdit = $MainContainer/Panel/VBoxContainer/RegisterForm/PasswordInput
@onready var register_confirm: LineEdit = $MainContainer/Panel/VBoxContainer/RegisterForm/ConfirmPasswordInput
@onready var register_button: Button = $MainContainer/Panel/VBoxContainer/RegisterForm/RegisterButton

# Social buttons
@onready var google_button: Button = $MainContainer/Panel/VBoxContainer/SocialButtons/GoogleButton
@onready var apple_button: Button = $MainContainer/Panel/VBoxContainer/SocialButtons/AppleButton

# Error label
@onready var error_label: Label = $MainContainer/Panel/VBoxContainer/ErrorLabel

# Loading spinner
@onready var loading_spinner: Control = $LoadingSpinner

enum AuthMode { LOGIN, REGISTER }
var current_mode: int = AuthMode.LOGIN

func _ready():
	_setup_event_listeners()
	_show_login_form()
	
	# Connect to API signals
	ApiClient.auth_success.connect(_on_auth_success)
	ApiClient.auth_error.connect(_on_auth_error)
	ApiClient.request_completed.connect(_on_request_completed)
	ApiClient.request_failed.connect(_on_request_failed)
	
	# Connect Google auth signals (if using JavaScript bridge)
	if OS.has_feature("android") or OS.has_feature("ios"):
		_connect_google_signals()

func _setup_event_listeners():
	login_tab.pressed.connect(_show_login_form)
	register_tab.pressed.connect(_show_register_form)
	
	login_button.pressed.connect(_on_login_pressed)
	register_button.pressed.connect(_on_register_pressed)
	
	google_button.pressed.connect(_on_google_pressed)
	apple_button.pressed.connect(_on_apple_pressed)

func _connect_google_signals():
	# This would be connected via JavaScript bridge signals
	# For now, we'll handle it manually in the OAuth flow
	pass

func _show_login_form():
	current_mode = AuthMode.LOGIN
	login_form.visible = true
	register_form.visible = false
	
	login_tab.button_mask = 0  # Disabled look
	register_tab.button_mask = 1  # Normal look
	
	_clear_error()
	_animate_form_transition()

func _show_register_form():
	current_mode = AuthMode.REGISTER
	login_form.visible = false
	register_form.visible = true
	
	login_tab.button_mask = 1  # Normal look
	register_tab.button_mask = 0  # Disabled look
	
	_clear_error()
	_animate_form_transition()

func _animate_form_transition():
	var target_form = login_form if current_mode == AuthMode.LOGIN else register_form
	
	target_form.modulate.a = 0
	target_form.scale = Vector2(0.95, 0.95)
	
	var tween = create_tween()
	tween.set_parallel(true)
	tween.tween_property(target_form, "modulate:a", 1.0, 0.3)
	tween.tween_property(target_form, "scale", Vector2.ONE, 0.3).set_trans(Tween.TRANS_BACK)

func _on_login_pressed():
	var email = login_email.text.strip_edges()
	var password = login_password.text
	
	if not _validate_login_input(email, password):
		return
	
	_show_loading(true)
	ApiClient.login(email, password)

func _on_register_pressed():
	var email = register_email.text.strip_edges()
	var username = register_username.text.strip_edges()
	var password = register_password.text
	var confirm = register_confirm.text
	
	if not _validate_register_input(email, username, password, confirm):
		return
	
	_show_loading(true)
	ApiClient.register(email, password, username)

func _validate_login_input(email: String, password: String) -> bool:
	if email.is_empty():
		_show_error("Inserisci la tua email")
		return false
	
	if password.is_empty():
		_show_error("Inserisci la tua password")
		return false
	
	if not email.contains("@"):
		_show_error("Inserisci un'email valida")
		return false
	
	return true

func _validate_register_input(email: String, username: String, password: String, confirm: String) -> bool:
	if email.is_empty():
		_show_error("Inserisci la tua email")
		return false
	
	if username.is_empty():
		_show_error("Inserisci un username")
		return false
	
	if password.is_empty():
		_show_error("Inserisci una password")
		return false
	
	if password != confirm:
		_show_error("Le password non coincidono")
		return false
	
	if password.length() < 6:
		_show_error("La password deve essere di almeno 6 caratteri")
		return false
	
	if not email.contains("@"):
		_show_error("Inserisci un'email valida")
		return false
	
	return true

func _on_google_pressed():
	_show_loading(true)
	_initiate_google_oauth()

func _initiate_google_oauth():
	if OS.has_feature("web"):
		_call_google_oauth_web()
	elif OS.has_feature("android"):
		_call_google_oauth_android()
	elif OS.has_feature("ios"):
		_call_google_oauth_ios()
	else:
		_show_loading(false)
		_show_error("Google OAuth non supportato su questa piattaforma")

func _call_google_oauth_web():
	var js_code = """
	var googleAuthUrl = 'https://accounts.google.com/o/oauth2/v2/auth?client_id=YOUR_CLIENT_ID&redirect_uri=http://localhost:3000/auth/google/callback&response_type=code&scope=email%20profile';
	window.open(googleAuthUrl, '_blank');
	"""
	JavaScriptBridge.eval(js_code)
	_show_loading(false)
	_show_error("OAuth inizializzato - completa nel browser")

func _call_google_oauth_android():
	var js_code = """
	if (window.cordova && window.cordova.plugins.googleplus) {
		window.cordova.plugins.googleplus.login(
			{},
			function(obj) {
				godot_bridge.emit_signal('google_auth_success', obj.idToken);
			},
			function(msg) {
				godot_bridge.emit_signal('google_auth_error', msg);
			}
		);
	} else {
		godot_bridge.emit_signal('google_auth_error', 'Google plugin non disponibile');
	}
	"""
	JavaScriptBridge.eval(js_code)

func _call_google_oauth_ios():
	var js_code = """
	if (window.cordova && window.cordova.plugins.googleplus) {
		window.cordova.plugins.googleplus.login(
			{},
			function(obj) {
				godot_bridge.emit_signal('google_auth_success', obj.idToken);
			},
			function(msg) {
				godot_bridge.emit_signal('google_auth_error', msg);
			}
		);
	} else {
		godot_bridge.emit_signal('google_auth_error', 'Google plugin non disponibile');
	}
	"""
	JavaScriptBridge.eval(js_code)

func _on_apple_pressed():
	_show_error("Social login non ancora implementato")

func _on_google_auth_success(id_token: String):
	ApiClient.google_auth(id_token)

func _on_google_auth_error(message: String):
	_show_loading(false)
	_show_error("Google auth error: " + message)

func _on_auth_success(token: String):
	_show_loading(false)
	AudioManager.play_success()
	
	# Transition to main scene
	var tween = create_tween()
	tween.tween_property(self, "modulate:a", 0.0, 0.5)
	tween.finished.connect(func(): get_tree().change_scene_to_file("res://scenes/map_hud.tscn"))

func _on_auth_error(message: String):
	_show_loading(false)
	_show_error(message)
	AudioManager.play_error()

func _on_request_completed(response: Dictionary):
	_show_loading(false)
	
	if response.has("token"):
		GameManager.set_auth_token(response["token"])
		GameManager.update_player_data(response.get("player", {}))
		_on_auth_success(response["token"])

func _on_request_failed(error: String):
	_show_loading(false)
	_show_error(error)

func _show_error(message: String):
	error_label.text = message
	error_label.visible = true
	
	# Animate error shake
	var tween = create_tween()
	for i in range(5):
		tween.tween_property(error_label, "position:x", error_label.position.x + 5, 0.05)
		tween.tween_property(error_label, "position:x", error_label.position.x - 5, 0.05)
	tween.tween_property(error_label, "position:x", error_label.position.x, 0.05)

func _clear_error():
	error_label.visible = false
	error_label.text = ""

func _show_loading(show: bool):
	loading_spinner.visible = show
	if show:
		# Animate spinner rotation
		var spinner = loading_spinner.get_node("Spinner")
		var tween = create_tween()
		tween.set_loops()
		tween.tween_property(spinner, "rotation", spinner.rotation + TAU, 1.0)
	else:
		loading_spinner.get_node("Spinner").rotation = 0
