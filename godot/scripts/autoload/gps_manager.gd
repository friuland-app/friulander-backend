extends Node

# GPS Manager - Gestione posizione reale

signal location_updated(lat: float, lng: float)
signal location_error(error: String)

var current_lat: float = 0.0
var current_lng: float = 0.0
var is_tracking: bool = false
var update_interval: float = 5.0  # Update every 5 seconds

var timer: Timer

func _ready():
	_setup_timer()
	_request_permissions()

func _setup_timer():
	timer = Timer.new()
	timer.wait_time = update_interval
	timer.timeout.connect(_on_timer_timeout)
	add_child(timer)

func _request_permissions():
	# Request GPS permissions
	if OS.has_feature("android"):
		# Android permissions
		OS.request_permission("android.permission.ACCESS_FINE_LOCATION")
		OS.request_permission("android.permission.ACCESS_COARSE_LOCATION")
	elif OS.has_feature("ios"):
		# iOS permissions handled in Info.plist
		pass

func start_tracking():
	is_tracking = true
	_get_current_location()
	timer.start()

func stop_tracking():
	is_tracking = false
	timer.stop()

func _get_current_location():
	# JavaScript bridge for web
	if OS.has_feature("web"):
		_call_javascript_gps()
	# Mobile GPS
	elif OS.has_feature("android") or OS.has_feature("ios"):
		_call_mobile_gps()
	# Desktop simulation
	else:
		_simulate_location()

func _call_javascript_gps():
	# Call JavaScript geolocation API
	var js_code = """
	navigator.geolocation.getCurrentPosition(
		function(position) {
			godot_bridge.emit_signal('location_updated', position.coords.latitude, position.coords.longitude);
		},
		function(error) {
			godot_bridge.emit_signal('location_error', error.message);
		},
		{ enableHighAccuracy: true, timeout: 10000 }
	);
	"""
	JavaScriptBridge.eval(js_code)

func _call_mobile_gps():
	# Use Godot's mobile GPS via JavaScriptBridge or platform-specific API
	if OS.has_feature("android"):
		# Android GPS implementation
		var js_code = """
		if (window.cordova && window.cordova.plugins.locationAccuracy) {
			cordova.plugins.locationAccuracy.request(function() {
				navigator.geolocation.getCurrentPosition(
					function(position) {
						godot_bridge.emit_signal('location_updated', position.coords.latitude, position.coords.longitude);
					},
					function(error) {
						godot_bridge.emit_signal('location_error', error.message);
					}
				);
			});
		}
		"""
		JavaScriptBridge.eval(js_code)
	elif OS.has_feature("ios"):
		# iOS GPS implementation
		var js_code = """
		navigator.geolocation.getCurrentPosition(
			function(position) {
				godot_bridge.emit_signal('location_updated', position.coords.latitude, position.coords.longitude);
			},
			function(error) {
				godot_bridge.emit_signal('location_error', error.message);
			},
			{ enableHighAccuracy: true }
		);
		"""
		JavaScriptBridge.eval(js_code)
	else:
		location_error.emit("GPS not supported on this platform")

func _simulate_location():
	# Simulation for desktop testing
	current_lat = 46.0631  # Udine
	current_lng = 13.2361
	location_updated.emit(current_lat, current_lng)

func _on_timer_timeout():
	if is_tracking:
		_get_current_location()

func get_distance_to(target_lat: float, target_lng: float) -> float:
	# Haversine formula for distance calculation
	var R = 6371000.0  # Earth radius in meters
	var lat1 = deg_to_rad(current_lat)
	var lat2 = deg_to_rad(target_lat)
	var dlat = deg_to_rad(target_lat - current_lat)
	var dlng = deg_to_rad(target_lng - current_lng)
	
	var a = sin(dlat/2) * sin(dlat/2) + cos(lat1) * cos(lat2) * sin(dlng/2) * sin(dlng/2)
	var c = 2 * atan2(sqrt(a), sqrt(1-a))
	
	return R * c  # Distance in meters

func deg_to_rad(deg: float) -> float:
	return deg * PI / 180.0

func is_near_poi(poi_lat: float, poi_lng: float, threshold: float = 100.0) -> bool:
	return get_distance_to(poi_lat, poi_lng) <= threshold

func get_current_position() -> Dictionary:
	return {"lat": current_lat, "lng": current_lng}
