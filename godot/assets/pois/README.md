# Foto POI - Friulander

## Struttura Cartelle

```
assets/pois/
├── castello_udine.jpg
├── castello_miramare.jpg
├── basilica_aquileia.jpg
├── ponte_diavolo.jpg
└── laguna_grado.jpg
```

## POI Disponibili

| POI | Città | Tipo | File |
|-----|-------|------|------|
| Castello di Udine | Udine | Castello | `castello_udine.jpg` |
| Castello di Miramare | Trieste | Castello | `castello_miramare.jpg` |
| Basilica di Aquileia | Aquileia | Chiesa | `basilica_aquileia.jpg` |
| Ponte del Diavolo | Cividale | Landmark | `ponte_diavolo.jpg` |
| Laguna di Grado | Grado | Natura | `laguna_grado.jpg` |

## Fonti Foto Consigliate

### Wikimedia Commons (CC licenses)
- https://commons.wikimedia.org/wiki/Castello_di_Udine
- https://commons.wikimedia.org/wiki/Castello_di_Miramare
- https://commons.wikimedia.org/wiki/Basilica_patriarcale_di_Aquileia
- https://commons.wikimedia.org/wiki/Ponte_del_Diavolo_(Cividale_del_Friuli)

### Foto Personali
- Scatta foto durante visite ai luoghi
- Rispetta normative privacy (no persone riconoscibili)
- Preferibilmente orario "golden hour" per migliori risultati

### Altre Fonti Gratuite
- **Unsplash** - www.unsplash.com
- **Pexels** - www.pexels.com
- **Pixabay** - www.pixabay.com

## Specifiche Tecniche

- **Formato**: JPG o WebP (preferibile WebP)
- **Dimensione**: 1200x800 minimo
- **Qualità**: 80-90%
- **Peso**: < 500KB per foto
- **Orientamento**: Landscape (orizzontale)

## Sostituire Placeholder

1. Scarica foto dalla fonte scelta
2. Rinomina secondo convenzione: `nome_poi.jpg`
3. Ottimizza con strumento come TinyJPG o Squoosh
4. Sovrascrivi file placeholder in `assets/pois/`
5. Godot importerà automaticamente

## Aggiungere Nuovi POI

1. Aggiungi POI in `src/data/pois.json`
2. Crea foto placeholder in questa cartella
3. Aggiorna questo README
4. Commit e push
