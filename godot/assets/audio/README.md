# Musica e Audio di Friulander

## Struttura Cartelle

```
assets/audio/
├── music/           # Musica di sottofondo
└── sfx/             # Effetti sonori
```

## Formati Supportati

- **Musica**: OGG Vorbis (.ogg) - Qualità buona, file piccoli
- **SFX**: WAV (.wav) - Bassa latenza, qualità alta

## Tracce Musicali Richieste

### Musica di Sottofondo (music/)

| File | Uso | Stile Suggerito |
|------|-----|-----------------|
| `main_theme.ogg` | Menu principale, splash screen | Folk/orchestrale, tema friulano |
| `map_ambient.ogg` | Mappa esplorazione | Ambient/nature, rilassante |
| `battle_theme.ogg` | Battaglie | Orchestrale epico, ritmato |
| `ar_capture.ogg` | Cattura AR | Elettronica/etnica, tesa |
| `victory.ogg` | Vittoria battaglia | Trionfale, breve |
| `defeat.ogg` | Sconfitta | Melanconica, breve |

### Effetti Sonori (sfx/)

| File | Uso |
|------|-----|
| `ui_click.ogg` | Click pulsanti UI |
| `ui_hover.ogg` | Hover pulsanti |
| `capture_success.ogg` | Creatura catturata |
| `capture_fail.ogg` | Cattura fallita |
| `battle_start.ogg` | Inizio battaglia |
| `attack_hit.ogg` | Colpo a segno |
| `attack_miss.ogg` | Colpo mancato |
| `level_up.ogg` | Level up giocatore/creatura |
| `quest_complete.ogg` | Missione completata |
| `notification.ogg` | Notifica push |
| `error.ogg` | Errore/azione non valida |
| `creature_appear.ogg` | Creatura appare sulla mappa |
| `poi_discover.ogg` | POI scoperto |

## Dove Trovare Audio

### Musica Gratuita/Libre
- **OpenGameArt.org** - CC0 music packs
- **Freesound.org** - Campioni audio
- **Incompetech** - Musica royalty-free
- **FreePD.com** - Musica public domain

### Generazione AI
- **Suno AI** - Musica generativa
- **Udio** - Tracce musicali
- **AIVA** - Musica orchestrale

### Strumenti
- **Audacity** - Editing audio gratuito
- **LMMS** - DAW open source
- **GarageBand** (macOS)
- **FL Studio**

## Specifiche Tecniche

### Musica
- **Formato**: OGG Vorbis
- **Sample rate**: 44.1 kHz
- **Bit rate**: 128-192 kbps
- **Canali**: Stereo (2.0)
- **Loop**: Seamless looping per tracce ambient

### SFX
- **Formato**: WAV PCM
- **Sample rate**: 44.1 kHz
- **Bit depth**: 16-bit
- **Canali**: Mono o Stereo
- **Durata**: 0.1-2 secondi

## Implementazione in Godot

### Caricamento
```gdscript
var music = preload("res://assets/audio/music/main_theme.ogg")
AudioManager.play_music(music)
```

### Volume
```gdscript
AudioManager.set_music_volume(0.7)
AudioManager.set_sfx_volume(0.8)
```

### Bus Audio
Godot usa bus audio separati:
- **Master** - Volume globale
- **Music** - Musica di sottofondo
- **SFX** - Effetti sonori

Configura in Project > Audio > Bus Layout.

## Sostituire Placeholder

1. Scarica/crea audio in formato corretto
2. Rinomina secondo convenzione sopra
3. Sovrascrivi file placeholder
4. Godot importa automaticamente
5. Testa in gioco
