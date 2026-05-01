# Friulander - Godot Frontend

Frontend del gioco Friulander con Godot Engine 4.x

## Requisiti

- Godot Engine 4.x
- Backend in esecuzione su `http://localhost:3000`

## Struttura

```
godot/
├── project.godot          # Progetto Godot
├── scenes/                # Scene UI
├── scripts/               # Script GDScript
│   ├── autoload/          # Singletons (GameManager, ApiClient, etc.)
│   └── ui/                # Script UI
├── theme/                 # Tema UI
└── fonts/                 # Font (da aggiungere)
```

## Come Usare

1. **Avvia il backend:**
   ```bash
   cd ..
   npm start
   ```

2. **Apri in Godot:**
   - Godot > Importa > Seleziona `project.godot`

3. **Aggiungi Font:**
   - Metti `Inter-Regular.ttf` e `Poppins-Bold.ttf` in `fonts/`

4. **Esegui:**
   - Premi F5 in Godot

## Scene Create

- ✅ Splash Screen
- ✅ Auth Screen (login/registrazione)
- ✅ Map HUD (mappa principale)
- ✅ Creature/POI Markers
- 🔄 Inventory Screen (in corso)
- 🔄 Profile Screen (in corso)
- 🔄 Quests Screen (in corso)
- 🔄 AR Capture (in corso)
- 🔄 Battle Screen (in corso)
- 🔄 Settings Screen (in corso)

## Autoload

- **GameManager:** Stato globale gioco
- **ApiClient:** Chiamate API backend
- **AudioManager:** Gestione audio
- **SettingsManager:** Impostazioni
- **TutorialManager:** Tutorial
