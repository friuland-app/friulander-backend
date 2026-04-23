# AR Capture Screen Specification

## Overview
Specifiche per la schermata AR e cattura creature.

---

## Layout

```
┌─────────────────────────────────┐
│ [←]                 [Stabilità] │
│                                 │
│         [Mirino Centrale]       │
│              ○                 │
│                                 │
│    [Creatura AR Model]          │
│    [====Vita====] 100/100      │
│                                 │
│                                 │
│    [Trappola 1] [Trappola 2]    │
│    [Trappola 3] [Trappola 4]    │
│                                 │
│              [Fuga]             │
│                                 │
│    Swipe per lanciare trappola  │
└─────────────────────────────────┘
```

---

## AR UI Elements

### Top Left: Back Button
- Size: 48x48px
- Icon: Arrow left
- Background: Semi-transparent black (rgba(0,0,0,0.5))
- Border radius: 24px
- Position: 16px from top, 16px from left

### Top Right: AR Stability Indicator

#### Stability Container
- Size: 80x24px
- Background: Semi-transparent black (rgba(0,0,0,0.5))
- Border radius: 12px
- Padding: 4px

#### Stability Dots
- 3 dots (8px each)
- Spacing: 4px
- States:
  - Poor: 1 red dot
  - Fair: 2 orange dots
  - Good: 3 green dots

#### Stability Text
- Font: Inter, 10px, 500
- Color: White
- Position: Below dots

---

### Center: Animated Crosshair

#### Crosshair Container
- Size: 120x120px
- Position: Center of screen
- Animation: Slow rotation (30s full rotation)

#### Crosshair Elements
- Outer ring: 120px diameter, 2px stroke, Hill Gold (#D4A017)
- Inner ring: 80px diameter, 1px stroke, Tagliamento Blue (#4A90E2)
- Center dot: 8px diameter, Forest Green (#2D5A27)
- Corner markers: 4 markers at 90° intervals

#### Animation States
- Idle: Slow rotation
- Target locked: Pulse animation
- Swipe detected: Scale up + flash

---

### Creature Health Bar

#### Health Bar Container
- Position: Above creature
- Width: 200px
- Height: 16px
- Background: Semi-transparent black (rgba(0,0,0,0.7))
- Border radius: 8px

#### Health Fill
- Height: 12px
- Background: Success Green (#10B981)
- Border radius: 6px
- Animation: Smooth transition (300ms)

#### Health Text
- Font: Inter, 12px, 600
- Color: White
- Position: Right of bar
- Format: "100/100"

---

### Trap Selection

#### Trap Container
- Position: Bottom center
- Horizontal scroll
- Spacing: 12px
- Padding: 16px

#### Trap Card
- Size: 80x100px
- Border radius: 12px
- Background: Semi-transparent white (rgba(255,255,255,0.9))
- Border: 2px solid Forest Green (#2D5A27)
- Selected: 4px solid Hill Gold (#D4A017)

#### Trap Elements
- Icon: 48x48px
- Name: 12px, 500
- Count: Badge (red, 20x20px)
- Rarity indicator: Border color

#### Trap Types
- Basic: Grey border
- Rare: Blue border
- Epic: Purple border
- Legendary: Gold border

---

### Flee Button

#### Button Container
- Position: Bottom center, below traps
- Size: 120x48px
- Background: Error Red (#EF4444)
- Border radius: 24px
- Text: "Fuga"
- Font: Inter, 16px, 600
- Color: White

#### Animation
- Press: Scale 0.95
- Duration: 100ms

---

### Swipe Gesture Indicator

#### Swipe Container
- Position: Bottom center, below flee button
- Width: 200px
- Height: 40px
- Animation: Swipe arrow

#### Swipe Animation
- Arrow: Swipe up icon
- Color: Hill Gold (#D4A017)
- Animation: Move up and fade
- Duration: 1.5s
- Repeat: Yes

---

## Capture Result Feedback

### Success State

#### Celebration Overlay
- Full screen overlay
- Background: Semi-transparent Forest Green (rgba(45, 90, 39, 0.9))
- Animation: Fade in (500ms)

#### Success Elements
- Trophy icon: 120x120px
- Text: "Cattura Riuscita!"
- Creature sprite: 200x200px
- XP gained: "+50 XP"
- New creature badge: "NUOVA!"

#### Celebration Animation
- Confetti particles
- Scale bounce effect
- Sparkle effects
- Duration: 3s

### Failure State

#### Failure Overlay
- Full screen overlay
- Background: Semi-transparent Error Red (rgba(239, 68, 68, 0.9))
- Animation: Fade in (500ms)

#### Failure Elements
- X icon: 120x120px
- Text: "Cattura Fallita"
- Reason: "La creatura è fuggita"
- Retry button: "Riprova"

---

## Visual Effects

### Trap Launch Effect

#### Swipe Trail
- Trail color: Hill Gold (#D4A017)
- Trail width: 4px
- Duration: 0.5s
- Fade out: Yes

#### Launch Animation
- Trap sprite: Scale from 1.0 to 0.5
- Position: From bottom to center
- Duration: 0.3s
- Easing: ease-out

### Impact Effect

#### Impact Particles
- Color: Hill Gold (#D4A017)
- Count: 20 particles
- Duration: 1s
- Spread: 60° cone

#### Screen Shake
- Intensity: Small
- Duration: 0.2s
- Type: Random shake

---

## Animations

### Crosshair Rotation
- Duration: 30s
- Type: Continuous
- Direction: Clockwise

### Stability Pulse
- Duration: 1s
- Type: Scale + opacity
- Repeat: Yes

### Health Bar Update
- Duration: 300ms
- Easing: ease-out
- Color transition

### Celebration Sequence
1. Overlay fade in (0-500ms)
2. Trophy scale up (500-1000ms)
3. Confetti burst (1000-2000ms)
4. XP counter (2000-2500ms)
5. Continue button (2500-3000ms)

---

## AR States

### Searching
- Crosshair: Idle rotation
- Stability: Poor → Good
- Creature: Hidden

### Target Locked
- Crosshair: Pulse animation
- Stability: Good
- Creature: Visible
- Health bar: Show

### Aiming
- Crosshair: Follows swipe
- Trap: Selected
- Swipe indicator: Active

### Launching
- Crosshair: Flash
- Trap: Launch animation
- Swipe indicator: Hide

### Impact
- Crosshair: Hide
- Trap: Impact effect
- Creature: Reaction

### Result
- Success/Failure overlay
- Celebration/Failure animation
- Continue/Retry button

---

## Accessibility

- Minimum tap target: 48x48px
- Color contrast: 4.5:1 minimum
- Screen reader labels
- Reduce motion support
- Alternative input methods
