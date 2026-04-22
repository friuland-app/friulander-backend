# Map HUD Specification

## Overview
Specifiche per l'HUD (Heads-Up Display) della schermata mappa principale.

---

## Layout

```
┌─────────────────────────────────┐
│ [Avatar] Nome  Lv.15            │
│ [====XP====] 1500/2000          │
│                                 │
│                  [Bussola N]    │
│                                 │
│                                 │
│                                 │
│         [Mappa]                 │
│                                 │
│                                 │
│                                 │
│                                 │
│                                 │
│                                 │
│              [Quest]            │
│                                 │
│    [Prof]       [AR]     [Inv]  │
└─────────────────────────────────┘
```

---

## HUD Elements

### Top Left: Player Info

#### Avatar
- Size: 48x48px
- Shape: Circle
- Border: 2px solid Hill Gold (#D4A017)
- Background: Forest Green (#2D5A27)
- Image: Player avatar or default icon

#### Player Name
- Font: Poppins, 16px, 600
- Color: White (#FFFFFF)
- Position: Right of avatar
- Max width: 120px
- Truncation: Ellipsis

#### Level Badge
- Background: Hill Gold (#D4A017)
- Text: "Lv.15"
- Font: Inter, 12px, 700
- Color: White
- Border radius: 8px
- Padding: 4px 8px
- Position: Below name

#### XP Bar
- Width: 150px
- Height: 6px
- Background: Dolomite Grey (#6B7280)
- Fill: Tagliamento Blue (#4A90E2)
- Border radius: 3px
- Text: "1500/2000 XP" (12px, below bar)

---

### Top Right: Compass

#### Compass Container
- Size: 64x64px
- Shape: Circle
- Background: Semi-transparent white (rgba(255,255,255,0.9))
- Border: 2px solid Forest Green (#2D5A27)
- Border radius: 32px

#### Compass Needle
- Size: 48x48px
- Rotation: Based on player heading
- North indicator: Red
- South indicator: White

#### Direction Labels
- N, E, S, W labels
- Font: Inter, 10px, 700
- Color: Forest Green (#2D5A27)

---

### Center Bottom: AR Button

#### AR Button
- Size: 72x72px
- Shape: Circle
- Background: Hill Gold (#D4A017)
- Border: 4px solid White
- Icon: Camera/AR icon (32px)
- Shadow: 0 4px 12px rgba(0,0,0,0.3)
- Animation: Pulse when AR available

#### AR Badge
- Position: Top-right of button
- Background: Vineyard Purple (#7C3AED)
- Text: "NEW"
- Font: Inter, 10px, 700
- Color: White
- Border radius: 8px

---

### Bottom Right: Inventory Button

#### Inventory Button
- Size: 56x56px
- Shape: Rounded square
- Border radius: 12px
- Background: Forest Green (#2D5A27)
- Icon: Backpack/Inventory (28px)
- Position: 16px from right, 16px from bottom

#### Item Count Badge
- Position: Top-right corner
- Background: Error Red (#EF4444)
- Text: "5"
- Font: Inter, 12px, 700
- Color: White
- Border radius: 10px
- Size: 20x20px

---

### Bottom Left: Profile Button

#### Profile Button
- Size: 56x56px
- Shape: Rounded square
- Border radius: 12px
- Background: Forest Green (#2D5A27)
- Icon: User/Profile (28px)
- Position: 16px from left, 16px from bottom

---

### Right Side: Quests Button

#### Quests Button
- Size: 56x56px
- Shape: Rounded square
- Border radius: 12px
- Background: Forest Green (#2D5A27)
- Icon: Scroll/Quest (28px)
- Position: 16px from right, 80px from bottom

#### Active Quest Badge
- Position: Top-right corner
- Background: Success Green (#10B981)
- Text: "3"
- Font: Inter, 12px, 700
- Color: White
- Border radius: 10px
- Size: 20x20px

---

## Notifications

### Notification Panel
- Position: Top center (below player info)
- Width: 300px
- Background: Semi-transparent black (rgba(0,0,0,0.8))
- Border radius: 12px
- Padding: 12px
- Animation: Slide down from top

### Notification Types

#### Creature Spawned
- Icon: Creature sprite
- Text: "Nuova creatura nelle vicinanze!"
- Color: Vineyard Purple (#7C3AED)
- Duration: 3 seconds

#### Quest Completed
- Icon: Trophy
- Text: "Missione completata! +50 XP"
- Color: Hill Gold (#D4A017)
- Duration: 4 seconds

#### Level Up
- Icon: Star
- Text: "Livello 16 raggiunto!"
- Color: Success Green (#10B981)
- Duration: 5 seconds

#### POI Discovered
- Icon: Map pin
- Text: "Nuovo POI scoperto: Cividale"
- Color: Tagliamento Blue (#4A90E2)
- Duration: 3 seconds

---

## Animations

### Button Press
- Scale: 0.9
- Duration: 100ms
- Easing: ease-out

### XP Bar Fill
- Duration: 500ms
- Easing: ease-out
- Color transition

### Compass Rotation
- Duration: 300ms
- Easing: ease-in-out
- Smooth interpolation

### Notification Slide
- Duration: 300ms
- Easing: ease-out-back
- Slide from top (-50px to 0)

### Badge Pulse
- Scale: 1.0 → 1.2 → 1.0
- Duration: 1s
- Repeat: Yes

---

## Responsive Design

### Breakpoints

#### Mobile (xs, sm)
- Avatar: 40x40px
- Buttons: 48x48px
- AR Button: 64x64px
- Font sizes: -2px
- Spacing: -4px

#### Tablet (md)
- Avatar: 48x48px
- Buttons: 56x56px
- AR Button: 72x72px
- Font sizes: Base
- Spacing: Base

#### Desktop (lg, xl)
- Avatar: 56x56px
- Buttons: 64x64px
- AR Button: 80x80px
- Font sizes: +2px
- Spacing: +4px

### Safe Areas
- Top: 44px (status bar)
- Bottom: 34px (home indicator)
- Sides: 16px

### Touch Targets
- Minimum: 48x48px
- Recommended: 56x56px
- Spacing: 8px between elements

---

## States

### Normal
- All elements visible
- Full opacity
- Interactive

### Battle Mode
- Hide: Quest button, Inventory button
- Show: Battle HUD
- Dim: Map overlay

### AR Mode
- Hide: All HUD elements
- Show: AR controls
- Transparent: Map overlay

### Menu Open
- Dim: Map
- Keep: Player info, AR button
- Hide: Other buttons

---

## Accessibility

- Minimum tap target: 48x48px
- Color contrast: 4.5:1 minimum
- Screen reader labels
- Focus indicators
- Reduce motion support
