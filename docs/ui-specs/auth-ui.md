# Auth UI Specification

## Overview
Specifiche per le schermate di autenticazione: login, registrazione, recupero password e onboarding.

---

## Login Screen

### Layout
```
┌─────────────────────────┐
│    Friulander Logo      │
│                         │
│    Benvenuto!           │
│   Accedi al tuo account │
│                         │
│  [Email Input]          │
│  [Password Input]       │
│                         │
│  [Accedi]               │
│                         │
│  ─────── oppure ─────── │
│                         │
│  [Continua con Google]  │
│  [Continua con Apple]   │
│                         │
│  Password dimenticata?  │
│  [Registrati]           │
└─────────────────────────┘
```

### Components

#### Email Input
- Height: 48px
- Border radius: 12px
- Border: 1px solid Dolomite Grey (#6B7280)
- Placeholder: "La tua email"
- Validation: Email format in real-time
- Error color: Error Red (#EF4444)
- Success color: Success Green (#10B981)

#### Password Input
- Height: 48px
- Border radius: 12px
- Border: 1px solid Dolomite Grey (#6B7280)
- Placeholder: "La tua password"
- Show/Hide toggle icon
- Validation: Min 8 characters
- Error color: Error Red (#EF4444)

#### Primary Button (Accedi)
- Height: 48px
- Background: Forest Green (#2D5A27)
- Text: White, 14px, 600
- Border radius: 12px
- Loading state: Spinner + disabled

#### Social Buttons
- Height: 48px
- Background: White
- Border: 1px solid Dolomite Grey
- Text: Dolomite Grey, 14px, 500
- Icon: Google/Apple logo (24px)
- Border radius: 12px

---

## Registration Screen

### Layout
```
┌─────────────────────────┐
│    Friulander Logo      │
│                         │
│    Crea il tuo account  │
│                         │
│  [Nome Input]           │
│  [Email Input]          │
│  [Password Input]       │
│  [Conferma Password]     │
│                         │
│  ✓ Accetto i termini    │
│                         │
│  [Registrati]           │
│                         │
│  Hai già un account?    │
│  [Accedi]               │
└─────────────────────────┘
```

### Components

#### Name Input
- Height: 48px
- Border radius: 12px
- Placeholder: "Il tuo nome"
- Validation: Min 2 characters, letters only

#### Password Requirements
- Min 8 characters
- At least 1 uppercase
- At least 1 number
- At least 1 special character

#### Password Strength Indicator
- Weak: Error Red (#EF4444)
- Medium: Warning Orange (#F59E0B)
- Strong: Success Green (#10B981)

---

## Password Recovery Screen

### Layout
```
┌─────────────────────────┐
│    [←]                 │
│                         │
│    Password dimenticata?│
│                         │
│  Inserisci la tua email │
│  per reimpostare la     │
│  password               │
│                         │
│  [Email Input]          │
│                         │
│  [Invia email]          │
│                         │
│  Torna al login         │
└─────────────────────────┘
```

### Components

#### Email Input
- Same as login screen
- Validation: Email format

#### Success State
- Checkmark icon (Success Green)
- "Email inviata!" message
- "Controlla la tua inbox" subtitle

---

## Onboarding Screen

### Layout
```
┌─────────────────────────┐
│    [Skip]               │
│                         │
│    [Illustration]       │
│                         │
│    Benvenuto in         │
│    Friulander!          │
│                         │
│    Esplora il Friuli,   │
│    cattura creature     │
│    e diventa il         │
│    miglior allenatore   │
│                         │
│    ● ● ● ● ●            │
│                         │
│    [Continua]           │
└─────────────────────────┘
```

### Onboarding Slides

#### Slide 1: Welcome
- Title: "Benvenuto in Friulander!"
- Description: "Esplora il Friuli, cattura creature e diventa il miglior allenatore"
- Illustration: Map with creatures

#### Slide 2: Explore
- Title: "Esplora il Friuli"
- Description: "Scopri luoghi unici e creature rare nelle diverse zone della regione"
- Illustration: GPS/Map icon

#### Slide 3: Catch
- Title: "Cattura Creature"
- Description: "Usa le tue trappole per catturare creature e aggiungile alla tua collezione"
- Illustration: Creature catching

#### Slide 4: Battle
- Title: "Combatti e Vinci"
- Description: "Sfida altri allenatori e sali in classifica per diventare il campione"
- Illustration: Battle scene

#### Slide 5: Start
- Title: "Inizia l'avventura!"
- Description: "Il Friuli ti aspetta. Crea il tuo account e inizia a giocare"
- Illustration: Trophy/Celebration

### Components

#### Pagination Dots
- Size: 8px
- Active: Hill Gold (#D4A017)
- Inactive: Dolomite Grey (#6B7280)
- Spacing: 8px

#### Skip Button
- Text: "Salta"
- Color: Dolomite Grey
- Position: Top right

#### Continue Button
- Same as primary button
- Text: "Continua" (or "Inizia" on last slide)

---

## Validation Rules

### Email
- Format: Standard email regex
- Real-time validation on blur
- Error: "Email non valida"

### Password
- Min length: 8 characters
- 1 uppercase required
- 1 number required
- 1 special character required
- Real-time validation on input
- Error: "Password non sicura"

### Name
- Min length: 2 characters
- Letters and spaces only
- Real-time validation on input
- Error: "Nome non valido"

---

## Visual Feedback

### Success States
- Green border on input
- Checkmark icon
- Success message below input

### Error States
- Red border on input
- Error icon
- Error message below input
- Shake animation

### Loading States
- Button disabled
- Spinner animation
- Text: "Caricamento..."

---

## Transitions

### Screen Transitions
- Duration: 300ms
- Easing: ease-in-out
- Direction: Slide right/left

### Input Focus
- Border color: Tagliamento Blue (#4A90E2)
- Shadow: 0 0 0 3px rgba(74, 144, 226, 0.2)
- Duration: 150ms

### Button Press
- Scale: 0.95
- Duration: 100ms
- Easing: ease-out

---

## Accessibility

- Minimum tap target: 48x48px
- Color contrast: 4.5:1 minimum
- Screen reader labels
- Keyboard navigation support
- Focus indicators
