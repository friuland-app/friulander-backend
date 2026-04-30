# Tutorial & Onboarding Specification

## Overview
Specifiche per il tutorial e onboarding dei nuovi giocatori.

---

## Tutorial Flow

### Step 1: Introduction to Friulander Lore

#### Layout
```
┌─────────────────────────────────┐
│      [Skip]                    │
│                                 │
│         [Illustration]         │
│                                 │
│    Benvenuto in Friulander!     │
│                                 │
│    Il Friuli è una terra        │
│    magica abitata da           │
│    creature straordinarie.      │
│    Diventa il miglior          │
│    allenatore e scopri         │
│    i segreti della regione.    │
│                                 │
│         [Continua]              │
│                                 │
│         1/5                     │
└─────────────────────────────────┘
```

#### Content
- Title: "Benvenuto in Friulander!"
- Description: Lore introduction about magical creatures
- Illustration: Map of Friuli with creatures
- Duration: 30s
- Skipable: Yes

---

### Step 2: Map & GPS Explanation

#### Layout
```
┌─────────────────────────────────┐
│      [Skip]                    │
│                                 │
│    [Map Highlight]             │
│         ○                      │
│                                 │
│    Esplora la Mappa            │
│                                 │
│    Usa il GPS per trovare      │
│    creature e POI.             │
│    Muoviti nel mondo reale     │
│    per scoprire nuovi luoghi.  │
│                                 │
│         [Continua]              │
│                                 │
│         2/5                     │
└─────────────────────────────────┘
```

#### Content
- Title: "Esplora la Mappa"
- Description: GPS usage explanation
- Highlight: Map area with pulsing effect
- Duration: 45s
- Skipable: Yes

---

### Step 3: First Creature Guaranteed

#### Layout
```
┌─────────────────────────────────┐
│      [Skip]                    │
│                                 │
│    [Creature Sprite]           │
│         ★                      │
│                                 │
│    La tua Prima Creatura!      │
│                                 │
│    C'è una creatura nelle      │
│    vicinanze! Segui il         │
│    indicatore per trovarla.    │
│                                 │
│         [Vai alla Creatura]     │
│                                 │
│         3/5                     │
└─────────────────────────────────┘
```

#### Content
- Title: "La tua Prima Creatura!"
- Description: Guaranteed creature nearby
- Highlight: Direction arrow to creature
- Duration: 60s
- Skipable: No (required step)

---

### Step 4: AR Capture Tutorial

#### Layout
```
┌─────────────────────────────────┐
│      [Skip]                    │
│                                 │
│    [AR View Mockup]            │
│         ○                      │
│                                 │
│    Cattura in AR               │
│                                 │
│    Usa la fotocamera per        │
│    vedere le creature nel       │
│    mondo reale. Lancia una      │
│    trappola per catturarle!    │
│                                 │
│         [Prova AR]             │
│                                 │
│         4/5                     │
└─────────────────────────────────┘
```

#### Content
- Title: "Cattura in AR"
- Description: AR capture mechanics
- Highlight: AR button with pulse
- Duration: 90s
- Skipable: No (required step)

---

### Step 5: Inventory & Quests Explanation

#### Layout
```
┌─────────────────────────────────┐
│      [Skip]                    │
│                                 │
│    [Inventory Icon] [Quest Icon]│
│                                 │
│    Inventario e Missioni        │
│                                 │
│    Gestisci le tue creature     │
│    nell'inventario e completa   │
│    missioni per guadagnare      │
│    ricompense.                 │
│                                 │
│         [Inizia a Giocare]      │
│                                 │
│         5/5                     │
└─────────────────────────────────┘
```

#### Content
- Title: "Inventario e Missioni"
- Description: Inventory and quests overview
- Highlight: Inventory and quest buttons
- Duration: 30s
- Skipable: Yes

---

## Contextual Tooltips

### Tooltip Types

#### Map Tooltip
- Trigger: First time opening map
- Position: Top center
- Text: "Tocca per vedere la mappa"
- Duration: 3s
- Icon: Map pin

#### AR Button Tooltip
- Trigger: First time AR button appears
- Position: Above button
- Text: "Tocca per attivare la AR"
- Duration: 3s
- Icon: Camera

#### Creature Tooltip
- Trigger: First creature nearby
- Position: Above creature marker
- Text: "Tocca per vedere i dettagli"
- Duration: 5s
- Icon: Creature

#### Quest Tooltip
- Trigger: First quest available
- Position: Above quest button
- Text: "Nuova missione disponibile!"
- Duration: 5s
- Icon: Scroll

#### Inventory Tooltip
- Trigger: First item obtained
- Position: Above inventory button
- Text: "Tocca per vedere l'inventario"
- Duration: 3s
- Icon: Backpack

### Tooltip Animation
- Fade in: 300ms
- Display: 3-5s
- Fade out: 300ms
- Easing: ease-in-out

---

## Tutorial States

### Not Started
- Tutorial available
- "Inizia Tutorial" button in settings
- No tooltips shown

### In Progress
- Tutorial steps shown sequentially
- Contextual tooltips enabled
- Skip button available (where allowed)
- Progress indicator visible

### Completed
- Tutorial marked as complete
- "Ripeti Tutorial" button in settings
- Tooltips disabled (unless re-enabled)
- All features unlocked

### Skipped
- Tutorial marked as skipped
- "Ripeti Tutorial" button in settings
- Basic tooltips still shown
- All features unlocked

---

## Tutorial Persistence

### Local Storage
- Tutorial state: PlayerPrefs
- Current step: PlayerPrefs
- Completion status: PlayerPrefs
- Skip status: PlayerPrefs

### Cloud Sync
- Sync with Firebase
- Cross-device consistency
- Automatic backup

---

## Accessibility

### Tutorial Options
- Text size: Respects accessibility settings
- High contrast: Respects accessibility settings
- Reduce motion: Respects accessibility settings
- Screen reader: Full support

### Skip Options
- Skip entire tutorial
- Skip specific steps
- Resume from skipped step
- Repeat tutorial anytime

---

## Tutorial Manager Features

### Step Management
- Track current step
- Validate step completion
- Allow step skipping
- Support step replay

### Progress Tracking
- Overall progress (0-100%)
- Step-by-step progress
- Completion detection
- Achievement unlock

### Tooltip Management
- Show contextual tooltips
- Track tooltip shown status
- Disable after tutorial
- Re-enable on request

### State Management
- Not started → In progress → Completed/Skipped
- Persist state across sessions
- Support state reset
- Cloud synchronization
