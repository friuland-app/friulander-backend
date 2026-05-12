extends Node

# Object Pool Manager - Ottimizzazione memoria mobile

var creature_pool: Array = []
var poi_pool: Array = []
var particle_pool: Array = []

var pool_size: int = 20

func _ready():
	_initialize_pools()

func _initialize_pools():
	for i in range(pool_size):
		var creature = preload("res://scenes/creature_marker.tscn").instantiate()
		creature.visible = false
		add_child(creature)
		creature_pool.append(creature)
	
	for i in range(10):
		var poi = preload("res://scenes/poi_marker.tscn").instantiate()
		poi.visible = false
		add_child(poi)
		poi_pool.append(poi)

func get_creature() -> Node:
	for creature in creature_pool:
		if not creature.visible:
			creature.visible = true
			return creature
	return null

func return_creature(creature: Node):
	creature.visible = false
	creature.position = Vector2.ZERO

func get_poi() -> Node:
	for poi in poi_pool:
		if not poi.visible:
			poi.visible = true
			return poi
	return null

func return_poi(poi: Node):
	poi.visible = false
	poi.position = Vector2.ZERO
