# Inventory Screen Specification

## Overview
Specifiche per la schermata inventario con tab multiple.

---

## Layout

```
┌─────────────────────────────────┐
│ [←]          Inventario         │
│                                 │
│  [Creature] [Oggetti] [Medaglie]│
│                                 │
│  [Filtri: Tipo ▼ Rarità ▼]      │
│                                 │
│  ┌────┐ ┌────┐ ┌────┐ ┌────┐   │
│  │ C1 │ │ C2 │ │ C3 │ │ C4 │   │
│  └────┘ └────┘ └────┘ └────┘   │
│  ┌────┐ ┌────┐ ┌────┐ ┌────┐   │
│  │ C5 │ │ C6 │ │ C7 │ │ C8 │   │
│  └────┘ └────┘ └────┘ └────┘   │
│                                 │
│  [Dettaglio Creatura]           │
│  ┌─────────────────────────┐   │
│  │     [Modello 3D]        │   │
│  │                         │   │
│  │  ATK: 50 DEF: 30        │   │
│  │  Livello: 15            │   │
│  │                         │   │
│  │  Mosse:                 │   │
│  │  [M1] [M2] [M3] [M4]    │   │
│  │                         │   │
│  │  [Evolvi]               │   │
│  └─────────────────────────┘   │
└─────────────────────────────────┘
```

---

## Tab Navigation

### Tab Container
- Position: Top, below header
- Height: 48px
- Background: Forest Green (#2D5A27)
- Border radius: 0

### Tab Buttons
- Width: 1/3 of container
- Height: 48px
- Font: Inter, 14px, 500
- Color: White (inactive), Hill Gold (active)
- Border bottom: 3px solid Hill Gold (active)

### Tabs
1. **Creature**: Grid of captured creatures
2. **Oggetti**: Traps, potions, items
3. **Medaglie**: Achievement badges

---

## Creature Tab

### Filter Bar
- Position: Below tabs
- Height: 40px
- Background: Friuli White (#F9FAFB)
- Padding: 8px

### Filter Dropdowns
- Type: Fire, Water, Grass, Electric, Psychic
- Rarity: Common, Rare, Epic, Legendary
- Level: 1-10, 11-20, 21-30, 31+
- Sort: Level, Name, Rarity, CP

### Creature Grid
- Columns: 4 (mobile), 6 (tablet), 8 (desktop)
- Spacing: 12px
- Padding: 16px

### Creature Card
- Size: 80x100px
- Border radius: 12px
- Background: White
- Border: 2px solid Dolomite Grey (#6B7280)
- Selected: 4px solid Hill Gold (#D4A017)

### Card Elements
- Creature sprite: 60x60px
- Name: 12px, 500
- Level badge: 10px, 700
- Rarity indicator: Border color
- CP: 10px, 400

### Rarity Colors
- Common: Grey (#6B7280)
- Rare: Blue (#4A90E2)
- Epic: Purple (#7C3AED)
- Legendary: Gold (#D4A017)

---

## Items Tab

### Item Grid
- Columns: 3 (mobile), 4 (tablet), 5 (desktop)
- Spacing: 12px
- Padding: 16px

### Item Card
- Size: 100x80px
- Border radius: 12px
- Background: White
- Border: 2px solid Dolomite Grey

### Item Types
- **Traps**: Basic, Rare, Epic, Legendary
- **Potions**: Health, Stamina, XP Boost
- **Items**: Revive, Escape Rope, etc.

### Card Elements
- Item icon: 48x48px
- Name: 12px, 500
- Count badge: 20x20px, red background
- Description: 10px, 400

---

## Medals Tab

### Medal Grid
- Columns: 3 (mobile), 4 (tablet), 6 (desktop)
- Spacing: 16px
- Padding: 16px

### Medal Card
- Size: 100x120px
- Border radius: 16px
- Background: White
- Border: 2px solid Hill Gold

### Medal States
- **Locked**: Grey, opacity 0.5
- **Unlocked**: Gold, full opacity
- **New**: Gold, pulse animation

### Card Elements
- Medal icon: 64x64px
- Name: 12px, 600
- Description: 10px, 400
- Date unlocked: 10px, 400 (if unlocked)

---

## Creature Detail Panel

### Panel Container
- Position: Bottom, above navigation
- Height: 300px
- Background: Semi-transparent white (rgba(255,255,255,0.95))
- Border radius: 16px top
- Animation: Slide up from bottom

### 3D Model Viewer
- Size: 150x150px
- Position: Center top
- Background: Gradient (light blue to white)
- Border radius: 12px
- Interaction: Rotate with touch/drag

### Statistics
- Font: Inter, 14px, 500
- Color: Primary text (#111827)
- Layout: 2 columns
- Stats: ATK, DEF, SPD, HP, CP

### Level Progress
- Level badge: Hill Gold background
- XP bar: Tagliamento Blue fill
- Text: "Livello 15 - 1500/2000 XP"

### Moves Section
- Grid: 2x2
- Move card: 80x40px
- Background: Forest Green
- Border radius: 8px
- Font: 12px, 500

### Evolution Button
- Position: Bottom right
- Size: 120x48px
- Background: Vineyard Purple (#7C3AED)
- Border radius: 24px
- Text: "Evolvi"
- Font: Inter, 16px, 600
- Color: White
- Disabled: Dolomite Grey

---

## Animations

### Tab Switch
- Duration: 300ms
- Animation: Fade in/out
- Easing: ease-in-out

### Card Selection
- Duration: 200ms
- Animation: Scale + border color
- Easing: ease-out

### Scroll
- Duration: 300ms
- Animation: Smooth scroll
- Easing: ease-out-cubic

### Detail Panel
- Duration: 400ms
- Animation: Slide up from bottom
- Easing: ease-out-back

### 3D Model Rotation
- Duration: Based on drag speed
- Animation: Smooth rotation
- Damping: 0.1

### Medal Unlock
- Duration: 1s
- Animation: Scale + pulse + shine
- Repeat: 3 times

---

## Filters

### Type Filter
- All types selected by default
- Multi-select enabled
- Visual: Colored badges

### Rarity Filter
- All rarities selected by default
- Single select
- Visual: Colored badges

### Level Filter
- Range slider
- Min: 1, Max: 50
- Visual: Range indicator

### Sort Options
- Level: High to low
- Name: A to Z
- Rarity: Legendary to Common
- CP: High to low

---

## States

### Normal
- All tabs accessible
- Filters enabled
- Detail panel hidden

### Creature Selected
- Detail panel visible
- 3D model loaded
- Statistics displayed

### Evolution Available
- Evolution button enabled
- Purple glow effect
- "Evolvi" text

### Empty State
- Empty illustration
- "Nessuna creatura" message
- "Cattura creature per iniziare" subtitle

---

## Accessibility

- Minimum tap target: 48x48px
- Color contrast: 4.5:1 minimum
- Screen reader labels
- Keyboard navigation support
- Focus indicators
