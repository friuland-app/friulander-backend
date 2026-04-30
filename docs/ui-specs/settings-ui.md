# Settings Screen Specification

## Overview
Specifiche per la schermata impostazioni e accessibilità.

---

## Layout

```
┌─────────────────────────────────┐
│ [←]          Impostazioni      │
│                                 │
│  Audio                          │
│  ┌─────────────────────────┐   │
│  │ Musica        [====50%====]│   │
│  │ Effetti       [====70%====]│   │
│  │ Volume        [====80%====]│   │
│  └─────────────────────────┘   │
│                                 │
│  Grafica                        │
│  ┌─────────────────────────┐   │
│  │ Qualità: [Alta ▼]        │   │
│  │ Frame Rate: [60 FPS ▼]  │   │
│  └─────────────────────────┘   │
│                                 │
│  Notifiche                      │
│  ┌─────────────────────────┐   │
│  │ Push Notifications [✓]  │   │
│  │ In-App Notifications [✓]│   │
│  └─────────────────────────┘   │
│                                 │
│  Privacy                        │
│  ┌─────────────────────────┐   │
│  │ GPS [✓]                 │   │
│  │ Fotocamera [✓]           │   │
│  └─────────────────────────┘   │
│                                 │
│  Account                        │
│  ┌─────────────────────────┐   │
│  │ [Logout]                 │   │
│  │ [Elimina Account]        │   │
│  └─────────────────────────┘   │
│                                 │
│  Accessibilità                 │
│  ┌─────────────────────────┐   │
│  │ Dimensione Testo [Media ▼]│
│  │ Contrasto Elevato [ ]    │   │
│  │ Riduzione Movimento [ ]  │   │
│  └─────────────────────────┘   │
└─────────────────────────────────┘
```

---

## Settings Sections

### Audio Settings

#### Section Header
- Font: Poppins, 18px, 600
- Color: Primary text (#111827)
- Icon: Speaker (24px)
- Padding: 16px

#### Music Volume
- Label: "Musica"
- Slider: 0-100%
- Icon: Music note (20px)
- Value display: "50%"

#### Sound Effects
- Label: "Effetti Sonori"
- Slider: 0-100%
- Icon: Sound wave (20px)
- Value display: "70%"

#### Master Volume
- Label: "Volume"
- Slider: 0-100%
- Icon: Volume (20px)
- Value display: "80%"

### Graphics Settings

#### Quality Dropdown
- Options: Bassa, Media, Alta, Ultra
- Default: Alta
- Icon: Settings (20px)

#### Frame Rate Dropdown
- Options: 30 FPS, 60 FPS, 120 FPS
- Default: 60 FPS
- Icon: Speed (20px)

### Notification Settings

#### Push Notifications Toggle
- Label: "Push Notifications"
- Description: "Ricevi notifiche push"
- Default: Enabled
- Icon: Bell (20px)

#### In-App Notifications Toggle
- Label: "In-App Notifications"
- Description: "Notifiche durante il gioco"
- Default: Enabled
- Icon: Notification (20px)

### Privacy Settings

#### GPS Toggle
- Label: "GPS"
- Description: "Condividi la tua posizione"
- Default: Enabled
- Icon: Location (20px)

#### Camera Toggle
- Label: "Fotocamera"
- Description: "Accesso alla fotocamera per AR"
- Default: Enabled
- Icon: Camera (20px)

### Account Settings

#### Logout Button
- Background: Warning Orange (#F59E0B)
- Text: "Logout"
- Icon: Log out (20px)
- Size: Full width, 48px height

#### Delete Account Button
- Background: Error Red (#EF4444)
- Text: "Elimina Account"
- Icon: Trash (20px)
- Size: Full width, 48px height

### Accessibility Settings

#### Text Size Dropdown
- Options: Piccola, Media, Grande, Molto Grande
- Default: Media
- Icon: Text (20px)

#### High Contrast Toggle
- Label: "Contrasto Elevato"
- Description: "Aumenta il contrasto dei colori"
- Default: Disabled
- Icon: Contrast (20px)

#### Reduce Motion Toggle
- Label: "Riduzione Movimento"
- Description: "Riduci le animazioni"
- Default: Disabled
- Icon: Animation (20px)

---

## UI Components

### Section Container
- Background: Friuli White (#F9FAFB)
- Border radius: 12px
- Padding: 16px
- Margin: 8px
- Border: 1px solid Dolomite Grey (#6B7280)

### Setting Item
- Height: 60px
- Background: White
- Border radius: 8px
- Padding: 12px
- Layout: Row (label + control)

### Slider
- Height: 4px
- Background: Dolomite Grey (#6B7280)
- Fill: Forest Green (#2D5A27)
- Border radius: 2px
- Thumb: 16x16px circle

### Toggle Switch
- Width: 48px
- Height: 28px
- Background: Dolomite Grey (off), Forest Green (on)
- Border radius: 14px
- Thumb: 24x24px circle

### Dropdown
- Height: 48px
- Background: White
- Border: 1px solid Dolomite Grey
- Border radius: 8px
- Padding: 0 12px

---

## Accessibility Features

### Text Size Scaling
- Small: 12px base
- Medium: 14px base (default)
- Large: 16px base
- Extra Large: 18px base

### High Contrast Mode
- Background: Black (#000000)
- Text: White (#FFFFFF)
- Accents: Yellow (#FFFF00)
- Borders: White (#FFFFFF)

### Reduce Motion
- Disable all animations
- Instant transitions
- No particle effects
- No screen shake

---

## Settings Persistence

### Local Storage
- Audio settings: PlayerPrefs
- Graphics settings: PlayerPrefs
- Notification settings: PlayerPrefs
- Privacy settings: PlayerPrefs
- Accessibility settings: PlayerPrefs

### Cloud Sync
- Sync with Firebase
- Cross-device consistency
- Automatic backup

---

## Confirmation Dialogs

### Logout Confirmation
```
┌─────────────────────────┐
│  Sei sicuro di voler   │
│  fare logout?           │
│                         │
│  [Annulla] [Conferma]   │
└─────────────────────────┘
```

### Delete Account Confirmation
```
┌─────────────────────────┐
│  Eliminare l'account?   │
│  Questa azione è        │
│  irreversibile.         │
│                         │
│  [Annulla] [Elimina]    │
└─────────────────────────┘
```

---

## States

### Normal
- All settings accessible
- Values displayed
- Changes applied immediately

### Loading
- Skeleton loaders
- Spinner animation
- Dimmed content

### Modified
- Unsaved changes indicator
- "Salva" button appears
- Auto-save after 5s

---

## Accessibility

- Minimum tap target: 48x48px
- Color contrast: 4.5:1 minimum (7:1 for high contrast)
- Screen reader labels
- Keyboard navigation support
- Focus indicators
- Reduce motion support
