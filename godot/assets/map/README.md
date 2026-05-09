# Texture Mappa Friulander

Tile e texture per la mappa di gioco, ispirate al territorio del Friuli-Venezia Giulia.

## Tile Disponibili

### Terreno Base

| Tile | Descrizione | File |
|------|-------------|------|
| **Erba** | Pianura agricola friulana | `tile_grass.svg` |
| **Collina** | Vigneti del Collio | `tile_hills.svg` |
| **Montagna** | Dolomiti Friulane | `tile_mountain.svg` |
| **Acqua** | Fiume Tagliamento | `tile_water.svg` |
| **Città** | Centri urbani (Udine, Pordenone) | `tile_city.svg` |
| **Foresta** | Boschi della Carnia | `tile_forest.svg` |

### Caratteristiche

- **Dimensione**: 64x64 pixel (ottimale per tile map)
- **Formato**: SVG vettoriale
- **Pattern**: Texture ripetibili per tile map infinite
- **Palette**: Colori ispirati al territorio reale

## Uso in Godot

### TileMap Node
```gdscript
# Crea una TileMap
var tile_map = TileMap.new()

# Assegna le texture
tile_map.tile_set = preload("res://assets/map/tile_set.tres")

# Posiziona tile
tile_map.set_cell(0, Vector2i(0, 0), 0)  # Grass
tile_map.set_cell(0, Vector2i(1, 0), 1)  # Hills
```

### Texture singole
```gdscript
var grass_texture = load("res://assets/map/tile_grass.svg")
$Sprite2D.texture = grass_texture
```

## Mappa del Territorio

```
┌─────────────────────────────────────────┐
│  🏔️ Alpi Carniche                       │
│     (tile_mountain)                     │
│                                         │
│  🌲 Foresta Carnica     🏠 Udine        │
│   (tile_forest)        (tile_city)      │
│                                         │
│  🌿 Collio            🏠 Pordenone      │
│  (tile_hills)         (tile_city)       │
│                                         │
│  🌊 Tagliamento       🏖️ Mare           │
│  (tile_water)                          │
└─────────────────────────────────────────┘
```

## Aggiungere Nuove Tile

1. Crea file SVG 64x64 in questa cartella
2. Nomina: `tile_nome.svg`
3. Usa pattern ripetibili per texture seamless
4. Aggiungi alla tabella sopra
5. Commit e push

## Ottimizzazione Mobile

- Le tile SVG verranno importate come texture 64x64
- Godot gestisce automaticamente il batching
- Per mappe grandi, considera l'uso di `CanvasItem` con culling
