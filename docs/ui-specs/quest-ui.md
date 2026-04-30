# Quest Screen Specification

## Overview
Specifiche per la schermata missioni con daily, weekly e story quests.

---

## Layout

```
┌─────────────────────────────────┐
│ [←]          Missioni           │
│                                 │
│  [Giornaliere] [Settimanali] [Storia]│
│                                 │
│  Reset tra: 12:34:56            │
│                                 │
│  ┌─────────────────────────┐   │
│  │ Cattura 5 creature      │   │
│  │ [====Progress====] 3/5  │   │
│  │ Ricompensa: 50 XP       │   │
│  │ [Ritira]                │   │
│  └─────────────────────────┘   │
│  ┌─────────────────────────┐   │
│  │ Visita 3 POI            │   │
│  │ [====Progress====] 2/3  │   │
│  │ Ricompensa: 30 XP       │   │
│  │ [Ritira]                │   │
│  └─────────────────────────┘   │
│                                 │
│  ┌─────────────────────────┐   │
│  │ Storia: Le Origini      │   │
│  │ Scopri la leggenda del  │   │
│  │ Tagliamento             │   │
│  │ [Inizia]                │   │
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
1. **Giornaliere**: Daily quests with timer
2. **Settimanali**: Weekly quests
3. **Storia**: Story quests with narrative

---

## Daily Quests

### Timer Display
- Position: Below tabs
- Font: Inter, 12px, 500
- Color: Warning Orange (#F59E0B)
- Format: "Reset tra: HH:MM:SS"
- Animation: Update every second

### Quest Card
- Height: 100px
- Background: White
- Border radius: 12px
- Border: 2px solid Dolomite Grey (#6B7280)
- Padding: 12px
- Margin: 8px

### Quest States
- **Active**: White background, grey border
- **Completed**: Light green tint, green border
- **Claimed**: Grey background, disabled
- **New**: Gold border, pulse animation

### Card Elements
- Quest icon: 32x32px
- Quest title: 16px, 600
- Quest description: 12px, 400 (secondary)
- Progress bar: 4px height
- Progress text: "3/5"
- Reward: "50 XP"
- Claim button: 80x32px

### Progress Bar
- Height: 4px
- Background: Dolomite Grey (#6B7280)
- Fill: Tagliamento Blue (#4A90E2)
- Border radius: 2px
- Animation: Smooth transition (300ms)

### Claim Button
- Background: Hill Gold (#D4A017)
- Border radius: 16px
- Text: "Ritira"
- Font: Inter, 12px, 600
- Color: White
- Animation: Scale on press

---

## Weekly Quests

### Week Display
- Position: Below tabs
- Font: Inter, 12px, 500
- Color: Forest Green (#2D5A27)
- Format: "Settimana 15 - Reset in 3 giorni"

### Quest Card
- Same as daily quests
- Larger rewards
- More challenging objectives

### Weekly Progress
- Overall progress bar
- Format: "4/7 completate"
- Position: Top of section

---

## Story Quests

### Quest Card
- Height: 120px
- Background: Gradient (Forest Green to Tagliamento Blue)
- Border radius: 12px
- Padding: 16px

### Card Elements
- Chapter title: 18px, 600 (white)
- Story preview: 14px, 400 (white)
- Friulian narrative text
- Start/Continue button
- Progress indicator (if started)

### Story Narrative
- Based on Friulian culture and history
- Examples:
  - "Le Origini del Tagliamento"
  - "I Castelli del Friuli"
  - "La Leggenda di Cividale"
  - "I Vini del Collio"

### Chapter Progress
- Chapter indicator: "Capitolo 3/10"
- Story progress bar
- Unlock condition display

---

## Reward Claim Animation

### Button Animation
- Scale: 1.0 → 1.1 → 1.0
- Duration: 300ms
- Easing: ease-out-back

### Reward Popup
- Position: Center screen
- Background: Semi-transparent Forest Green
- Border radius: 16px
- Animation: Scale in + fade
- Duration: 500ms

### Reward Elements
- Icon: Trophy or XP icon
- Text: "+50 XP"
- Confetti particles
- Duration: 2s

---

## HUD Badge

### Quest Button Badge
- Position: Top-right of quest button
- Background: Error Red (#EF4444)
- Size: 20x20px
- Border radius: 10px
- Text: Number of available rewards
- Font: Inter, 12px, 700
- Color: White

### Badge States
- **Hidden**: No rewards available
- **Visible**: Rewards to claim
- **Pulse**: New quest available

---

## In-App Notifications

### Notification Banner
- Position: Top of screen
- Height: 60px
- Background: Hill Gold (#D4A017)
- Animation: Slide down from top
- Duration: 3s

### Notification Types
- **New Quest**: "Nuova missione disponibile!"
- **Quest Completed**: "Missione completata!"
- **Reward Ready**: "Ricompensa pronta!"

### Notification Elements
- Icon: Quest icon
- Title: 14px, 600
- Message: 12px, 400
- Dismiss button

---

## Animations

### Tab Switch
- Duration: 300ms
- Animation: Fade in/out
- Easing: ease-in-out

### Progress Bar Fill
- Duration: 300ms
- Easing: ease-out
- Color transition

### Quest Card Selection
- Duration: 200ms
- Animation: Scale + border color
- Easing: ease-out

### Claim Reward
- Duration: 500ms
- Animation: Scale + confetti
- Easing: ease-out-back

### Notification Slide
- Duration: 300ms
- Animation: Slide down
- Easing: ease-out-back

---

## Quest Examples

### Daily Quests
1. "Cattura 5 creature" - 50 XP
2. "Visita 3 POI" - 30 XP
3. "Vinci 2 battaglie" - 40 XP
4. "Percorri 5 km" - 25 XP

### Weekly Quests
1. "Cattura 50 creature" - 500 XP
2. "Scopri 20 POI" - 300 XP
3. "Vinci 20 battaglie" - 400 XP
4. "Percorri 50 km" - 250 XP

### Story Quests
1. "Le Origini del Tagliamento" - 100 XP
2. "I Castelli del Friuli" - 150 XP
3. "La Leggenda di Cividale" - 200 XP

---

## States

### Normal
- All tabs accessible
- Quests displayed
- Progress updated

### Quest Completed
- Card highlighted
- Claim button enabled
- Badge updated

### All Claimed
- Badge hidden
- "Tutte le ricompense ritirate" message

### Story Unlocked
- New story card highlighted
- "Nuova storia disponibile!" notification

---

## Accessibility

- Minimum tap target: 48x48px
- Color contrast: 4.5:1 minimum
- Screen reader labels
- Keyboard navigation support
- Focus indicators
