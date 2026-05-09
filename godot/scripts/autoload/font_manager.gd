extends Node

# Font Manager - Gestisce i font del gioco

const FONT_DIR = "res://fonts/"

var fonts_ready: bool = false
signal fonts_loaded

func _ready():
	_check_fonts()

func _check_fonts():
	var dir = DirAccess.open(FONT_DIR)
	if dir == null:
		print("⚠️  Cartella fonts/ non trovata. Creazione...")
		DirAccess.make_dir_absolute(FONT_DIR)
		_show_font_instructions()
		return
	
	var required_fonts = [
		"Inter-Regular.ttf",
		"Inter-Bold.ttf", 
		"Poppins-Regular.ttf",
		"Poppins-Bold.ttf"
	]
	
	var missing_fonts = []
	for font in required_fonts:
		if not FileAccess.file_exists(FONT_DIR + font):
			missing_fonts.append(font)
	
	if missing_fonts.size() > 0:
		print("⚠️  Font mancanti: ", missing_fonts)
		_show_font_instructions()
	else:
		print("✅ Tutti i font sono presenti")
		fonts_ready = true
		fonts_loaded.emit()

func _show_font_instructions():
	push_warning("""
⚠️  FONT NON TROVATI

Scarica i font da:
1. Inter: https://fonts.google.com/specimen/Inter
2. Poppins: https://fonts.google.com/specimen/Poppins

Copia i file .ttf in godot/fonts/
""")

func are_fonts_ready() -> bool:
	return fonts_ready

func get_inter_regular() -> Font:
	if FileAccess.file_exists(FONT_DIR + "Inter-Regular.ttf"):
		return load(FONT_DIR + "Inter-Regular.ttf")
	return ThemeDB.fallback_font

func get_poppins_bold() -> Font:
	if FileAccess.file_exists(FONT_DIR + "Poppins-Bold.ttf"):
		return load(FONT_DIR + "Poppins-Bold.ttf")
	return ThemeDB.fallback_font
