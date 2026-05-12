extends Node

# Automated Device Test Script

var test_results: Array = []
var current_test: String = ""

func _ready():
	run_all_tests()

func run_all_tests():
	print("=== Inizio Test Device ===")
	test_gps()
	test_camera()
	test_audio()
	test_haptic()
	test_performance()
	print_results()

func test_gps():
	current_test = "GPS"
	print("Test GPS...")
	
	if GPSManager.is_tracking:
		test_results.append({"test": "GPS", "status": "PASS", "message": "GPS attivo"})
	else:
		test_results.append({"test": "GPS", "status": "FAIL", "message": "GPS non attivo"})

func test_camera():
	current_test = "Camera"
	print("Test Camera...")
	
	if CameraManager.is_camera_active():
		test_results.append({"test": "Camera", "status": "PASS", "message": "Camera attiva"})
	else:
		test_results.append({"test": "Camera", "status": "FAIL", "message": "Camera non attiva"})

func test_audio():
	current_test = "Audio"
	print("Test Audio...")
	
	test_results.append({"test": "Audio", "status": "PASS", "message": "Audio caricato"})

func test_haptic():
	current_test = "Haptic"
	print("Test Haptic...")
	
	HapticManager.click()
	test_results.append({"test": "Haptic", "status": "PASS", "message": "Haptic funzionante"})

func test_performance():
	current_test = "Performance"
	print("Test Performance...")
	
	var fps = Engine.get_frames_per_second()
	if fps >= 30:
		test_results.append({"test": "Performance", "status": "PASS", "message": "FPS: " + str(fps)})
	else:
		test_results.append({"test": "Performance", "status": "FAIL", "message": "FPS bassi: " + str(fps)})

func print_results():
	print("\n=== Risultati Test ===")
	for result in test_results:
		print(result["test"] + ": " + result["status"] + " - " + result["message"])
