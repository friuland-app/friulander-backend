extends Control

# Compass - Shows player heading and direction

@onready var background: ColorRect = $CompassBackground
@onready var label: Label = $CompassLabel

var target_rotation: float = 0.0
var current_rotation: float = 0.0
var smooth_speed: float = 5.0

# Device compass (would integrate with actual device sensors)
var device_heading: float = 0.0

func _ready():
	_update_compass_display()

func _process(delta):
	# Smooth rotation
	current_rotation = lerp_angle(current_rotation, target_rotation, smooth_speed * delta)
	_update_compass_rotation()

func _update_compass_rotation():
	# Rotate the label to show direction
	label.rotation = -current_rotation
	
	# Update label text based on heading
	var heading = rad_to_deg(current_rotation)
	if heading < 0:
		heading += 360
	
	var direction = _heading_to_direction(heading)
	label.text = direction

func _heading_to_direction(heading: float) -> String:
	if heading >= 337.5 or heading < 22.5:
		return "N"
	elif heading >= 22.5 and heading < 67.5:
		return "NE"
	elif heading >= 67.5 and heading < 112.5:
		return "E"
	elif heading >= 112.5 and heading < 157.5:
		return "SE"
	elif heading >= 157.5 and heading < 202.5:
		return "S"
	elif heading >= 202.5 and heading < 247.5:
		return "SW"
	elif heading >= 247.5 and heading < 292.5:
		return "W"
	elif heading >= 292.5 and heading < 337.5:
		return "NW"
	return "N"

func _update_compass_display():
	# Could update visual elements based on heading
	pass

func set_heading(heading: float):
	# Heading in degrees (0-360)
	device_heading = heading
	target_rotation = deg_to_rad(heading)

func update_from_gps(new_position: Vector2, old_position: Vector2):
	# Calculate heading from GPS movement
	if old_position != new_position:
		var direction = new_position - old_position
		var angle = direction.angle()
		set_heading(rad_to_deg(angle))
