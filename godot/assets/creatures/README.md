# Creature di Friulander

Sprite SVG delle creature del gioco, ispirate al Friuli-Venezia Giulia.

## Creature Disponibili

| Nome | Tipo | Ispirazione | File |
|------|------|-------------|------|
| **Furlanet** | 🔥 Fuoco | Spirito del Furlan tradizionale | `furlanet.svg` |
| **Tagliamon** | 💧 Acqua | Fiume Tagliamento | `tagliamon.svg` |
| **Colliflor** | 🌿 Erba | Vigneti del Collio | `colliflor.svg` |
| **Dolomitec** | 🪨 Roccia | Dolomiti Friulane | `dolomitec.svg` |
| **Aquileon** | ⚡ Elettrico | Aquileia longobarda | `aquileon.svg` |

## Uso in Godot

1. Importa gli SVG in `assets/creatures/`
2. Godot 4 li converte automaticamente in texture
3. Usali nelle scene:
   ```gdscript
   var texture = load("res://assets/creatures/furlanet.svg")
   $Sprite2D.texture = texture
   ```

## Aggiungere Nuove Creature

1. Crea file SVG in questa cartella
2. Nomina: `nomecreatura.svg`
3. Aggiungi riga alla tabella sopra
4. Commit e push

## Dimensioni

Tutti gli sprite sono **128x128 pixel** per consistenza.
