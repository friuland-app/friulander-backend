# POI Screen Specification

## Overview
Specifiche per la schermata POI (Punti di Interesse) e esplorazione.

---

## POI Proximity Popup

### Layout
```
┌─────────────────────────────────┐
│         [POI Icon]              │
│                                 │
│    Nuovo POI nelle vicinanze!    │
│    Castello di Udine            │
│    Distanza: 150m              │
│                                 │
│         [Esplora]               │
│         [Ignora]                │
└─────────────────────────────────┘
```

### Popup Container
- Position: Top center
- Width: 300px
- Height: 200px
- Background: Semi-transparent Forest Green (rgba(45, 90, 39, 0.95))
- Border radius: 16px
- Padding: 16px
- Animation: Slide down from top

### Popup Elements
- POI icon: 64x64px
- Title: 18px, 600 (white)
- Distance: 14px, 400 (white)
- Explore button: 120x40px
- Ignore button: 120x40px

### Animation
- Duration: 500ms
- Easing: ease-out-back
- Auto-dismiss: 10s

---

## POI Detail Card

### Layout
```
┌─────────────────────────────────┐
│ [←]          Castello di Udine  │
│                                 │
│    [Photo Gallery]             │
│    ○ ○ ○ ○                     │
│                                 │
│    Descrizione Storica         │
│    Il castello di Udine,       │
│    costruito nel XV secolo...  │
│                                 │
│    Distanza: 150m              │
│    [====Progress====] 75%      │
│                                 │
│    Ricompense                  │
│    +50 XP                      │
│    +1 Medaglia                 │
│    +1 Creature Rara            │
│                                 │
│    [Interagisci] [Enciclopedia]│
└─────────────────────────────────┘
```

### Card Container
- Position: Bottom sheet
- Height: 70% of screen
- Background: White
- Border radius: 16px top
- Animation: Slide up from bottom

### Photo Gallery
- Height: 200px
- Background: Gradient
- Pagination dots: 4 dots
- Swipe navigation

### Historical Description
- Font: Inter, 14px, 400
- Color: Primary text (#111827)
- Max lines: 5
- Expandable: "Leggi di più"

### Distance Indicator
- Font: Inter, 16px, 600
- Color: Forest Green (#2D5A27)
- Icon: Location pin (20px)

### Progress Bar
- Height: 8px
- Background: Dolomite Grey (#6B7280)
- Fill: Tagliamento Blue (#4A90E2)
- Border radius: 4px
- Text: "75% completato"

### Rewards Section
- Background: Friuli White (#F9FAFB)
- Border radius: 12px
- Padding: 12px
- Items: XP, Medaglia, Creature

### Action Buttons
- Interagisci: Forest Green (#2D5A27)
- Enciclopedia: Tagliamento Blue (#4A90E2)
- Size: 140x48px each

---

## POI Interaction Animation

### Animation Sequence
1. **Approach**: POI icon grows as player gets closer
2. **Discovery**: Burst effect when POI is discovered
3. **Interaction**: Pulse animation on tap
4. **Completion**: Confetti + success message

### Approach Animation
- Scale: 0.5 → 1.0
- Duration: Based on distance
- Easing: ease-out

### Discovery Animation
- Scale: 1.0 → 1.5 → 1.0
- Color: White → Gold → White
- Duration: 1s
- Particles: Burst effect

### Interaction Animation
- Scale: 1.0 → 1.2 → 1.0
- Duration: 300ms
- Easing: ease-out-back

### Completion Animation
- Confetti: Burst effect
- Success overlay: Fade in
- Duration: 2s

---

## Distance Indicator

### On-Map Indicator
- Position: Above POI marker
- Background: Semi-transparent black
- Border radius: 12px
- Padding: 4px 8px
- Text: "150m"
- Font: Inter, 12px, 600
- Color: White

### Direction Arrow
- Position: Edge of screen
- Points toward POI
- Size: 48x48px
- Color: Hill Gold (#D4A017)
- Animation: Pulse when close

---

## Mini-Encyclopedia

### Layout
```
┌─────────────────────────────────┐
│ [←]     Enciclopedia Friuli    │
│                                 │
│  [Cerca POI...]                │
│                                 │
│  Categoria:                    │
│  [Castelli] [Chiese] [Musei]  │
│  [Natura] [Città] [Altro]     │
│                                 │
│  POI Scoperti: 23/50           │
│                                 │
│  ┌─────────────────────────┐   │
│  │ [Photo] Castello di Udine│   │
│  │ Udine • Castello         │   │
│  │ ⭐⭐⭐⭐⭐              │   │
│  └─────────────────────────┘   │
│  ┌─────────────────────────┐   │
│  │ [Photo] Cividale        │   │
│  │ Cividale • Longobardi   │   │
│  │ ⭐⭐⭐⭐☆              │   │
│  └─────────────────────────┘   │
└─────────────────────────────────┘
```

### Search Bar
- Height: 48px
- Background: Friuli White (#F9FAFB)
- Border radius: 24px
- Placeholder: "Cerca POI..."
- Icon: Search (20px)

### Category Filters
- Grid: 3x2
- Button size: 100x40px
- Background: Forest Green (selected), White (unselected)
- Border radius: 20px

### POI Grid
- Columns: 2 (mobile), 3 (tablet), 4 (desktop)
- Spacing: 12px
- Padding: 16px

### POI Card
- Height: 150px
- Border radius: 12px
- Background: White
- Border: 2px solid Dolomite Grey (#6B7280)

### Card Elements
- Photo: 100x80px
- Name: 14px, 600
- Location: 12px, 400 (secondary)
- Rating: 5 stars
- Discovered badge: Green

---

## Cultural Content Links

### Link Types
- **Wikipedia**: External link to Wikipedia page
- **YouTube**: Video documentary
- **Audio Guide**: Audio narration
- **AR Experience**: AR visualization
- **Photo Gallery**: Historical photos

### Link Button
- Size: 40x40px
- Border radius: 8px
- Background: Tagliamento Blue (#4A90E2)
- Icon: External link (20px)
- Position: Top-right of card

### Content Modal
- Full screen overlay
- Background: White
- Animation: Fade in
- Content: Embedded media

---

## Animations

### Popup Slide
- Duration: 500ms
- Easing: ease-out-back
- Direction: Top to center

### Detail Sheet
- Duration: 400ms
- Easing: ease-out-back
- Direction: Bottom to 70%

### Photo Swipe
- Duration: 300ms
- Easing: ease-out
- Transition: Slide

### Encyclopedia Filter
- Duration: 200ms
- Animation: Scale + color
- Easing: ease-out

---

## States

### Normal
- POI markers visible on map
- Distance indicators active
- No popup shown

### POI Nearby
- Popup appears
- Distance indicator pulses
- Direction arrow active

### POI Discovered
- Discovery animation
- Detail card available
- Progress updated

### POI Completed
- Rewards claimed
- Confetti animation
- Marked as completed

---

## Accessibility

- Minimum tap target: 48x48px
- Color contrast: 4.5:1 minimum
- Screen reader labels
- Keyboard navigation support
- Focus indicators
