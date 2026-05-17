extends Node

# Camera Manager - Gestione fotocamera per AR

signal camera_ready
signal camera_error(error: String)

var camera_feed: CameraFeed
var camera_texture: CameraTexture
var is_active: bool = false

func _ready():
	_request_permissions()

func _request_permissions():
	if OS.has_feature("android"):
		OS.request_permission("android.permission.CAMERA")
	elif OS.has_feature("ios"):
		# iOS permissions handled in Info.plist
		pass

func start_camera():
	if is_active:
		return
	
	# In Godot 4.x, feeds() is a method that returns array of CameraFeed objects
	var camera_feeds = CameraServer.feeds()
	if camera_feeds == null or camera_feeds.is_empty():
		camera_error.emit("Nessuna fotocamera disponibile")
		return
	
	# Try to get camera feed from array
	if camera_feeds.size() > 1:
		camera_feed = camera_feeds[1]  # Back camera
	if camera_feed == null and camera_feeds.size() > 0:
		camera_feed = camera_feeds[0]  # Front camera fallback
	
	if camera_feed == null:
		camera_error.emit("Impossibile accedere alla fotocamera")
		return
	
	camera_feed.set_active(true)
	camera_texture = CameraTexture.new()
	camera_texture.set_camera_feed(camera_feed)
	is_active = true
	camera_ready.emit()

func stop_camera():
	if camera_feed != null:
		camera_feed.set_active(false)
		camera_feed = null
	is_active = false

func get_camera_texture() -> CameraTexture:
	return camera_texture

func is_camera_active() -> bool:
	return is_active

func get_camera_resolution() -> Vector2i:
	if camera_feed != null:
		return camera_feed.get_dimensions()
	return Vector2i(1920, 1080)
