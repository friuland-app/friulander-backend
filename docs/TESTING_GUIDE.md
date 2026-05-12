# Guida Testing - Friulander

## Preparazione Device

### Android
1. Abilita Developer Options
2. Abilita USB Debugging
3. Installa APK via ADB o sideload
4. Concedi permessi (GPS, Camera, Notifiche)

### iOS
1. Installa provisioning profile
2. Installa IPA via Xcode o TestFlight
3. Concedi permessi al primo avvio

## Esecuzione Test

### Test Automatici
```gdscript
# Esegui in Godot Editor
var test = preload("res://tests/device_test.gd").new()
test.run_all_tests()
```

### Test Manuali
1. Usa `MANUAL_TEST_CHECKLIST.md`
2. Compila ogni item
3. Registra bug trovati
4. Report risultati

## Report Bug

Formato:
- Titolo: [Categoria] Descrizione breve
- Device: Modello e OS
- Passi: Come riprodurre
- Atteso: Cosa dovrebbe succedere
- Reale: Cosa succede
- Priorità: Alta/Media/Bassa

## Metriche Successo
- FPS > 30
- Caricamento < 3s
- Memoria < 500MB
- Battery drain < 10%/ora
