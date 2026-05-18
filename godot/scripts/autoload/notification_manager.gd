extends Node

# Notification Manager

signal notification_received(data: Dictionary)

var fcm_token: String = ""

func _ready():
	_request_permissions()
	_initialize_fcm()

func _request_permissions():
	if OS.has_feature("android"):
		OS.request_permission("android.permission.POST_NOTIFICATIONS")

func _initialize_fcm():
	# FirebaseMessaging is a plugin that needs to be installed
	# For now, we'll use platform-specific native notifications
	if OS.has_feature("android"):
		# Android notifications handled via NotificationServer
		pass
	elif OS.has_feature("ios"):
		# iOS notifications handled via APNs
		pass

func _on_token_received(token: String):
	fcm_token = token
	_send_token_to_backend(token)

func _on_message_received(data: Dictionary):
	notification_received.emit(data)

func _send_token_to_backend(token: String):
	# Send to backend API
	pass

func send_notification(title: String, body: String):
	if OS.has_feature("android"):
		NotificationServer.notify(title, body)
	elif OS.has_feature("ios"):
		# iOS notifications handled via push
		pass
	else:
		print("Notification: ", title, " - ", body)
