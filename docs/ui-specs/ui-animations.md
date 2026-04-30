# UI Animations & Feedback Specification

## Overview
Specifiche per animazioni globali, haptic feedback e effetti sonori UI.

---

## DOTween Global Settings

### Configuration
- Time Scale: 1.0 (normal), 0.5 (slow motion)
- Easing: Default ease-out
- Auto-Kill: True (on complete)
- Recyclable: True
- Update Type: Normal

### Animation Types
- Fade: Alpha 0 → 1 or 1 → 0
- Scale: Vector3 0 → 1 or 1 → 0
- Move: Position A → B
- Rotate: Rotation A → B
- Punch: Temporary displacement
- Shake: Random oscillation
- Color: Color A → B

---

## Screen Transitions

### Slide Left
- Duration: 300ms
- Easing: ease-in-out
- Direction: Left to right
- Used: Navigation forward

### Slide Right
- Duration: 300ms
- Easing: ease-in-out
- Direction: Right to left
- Used: Navigation back

### Fade In
- Duration: 400ms
- Easing: ease-out
- Alpha: 0 → 1
- Used: Modal dialogs

### Fade Out
- Duration: 300ms
- Easing: ease-in
- Alpha: 1 → 0
- Used: Modal close

### Scale In
- Duration: 400ms
- Easing: ease-out-back
- Scale: 0 → 1
- Used: Popups, cards

### Scale Out
- Duration: 300ms
- Easing: ease-in-back
- Scale: 1 → 0
- Used: Popup dismiss

---

## Popup Animations

### Notification Popup
- Duration: 500ms
- Easing: ease-out-back
- Animation: Slide down from top
- Auto-dismiss: 3s

### Alert Popup
- Duration: 400ms
- Easing: ease-out-back
- Animation: Scale in from center
- Manual dismiss

### Bottom Sheet
- Duration: 400ms
- Easing: ease-out-back
- Animation: Slide up from bottom
- Manual dismiss

### Tooltip
- Duration: 200ms
- Easing: ease-out
- Animation: Fade in
- Auto-dismiss: 2s

---

## Button Feedback

### Press Animation
- Duration: 100ms
- Easing: ease-out
- Scale: 1.0 → 0.95 → 1.0
- Trigger: OnPointerDown/Up

### Hover Animation (Desktop)
- Duration: 200ms
- Easing: ease-out
- Scale: 1.0 → 1.05
- Trigger: OnPointerEnter

### Disabled State
- Alpha: 0.5
- Interactable: False
- No animation

### Success Animation
- Duration: 500ms
- Easing: ease-out-back
- Scale: 1.0 → 1.2 → 1.0
- Color: Green flash

### Error Animation
- Duration: 300ms
- Easing: ease-out
- Shake: 5 times
- Color: Red flash

---

## Progress Bar Animations

### Fill Animation
- Duration: 500ms
- Easing: ease-out
- Fill: 0 → target value
- Smooth transition

### Pulse Animation
- Duration: 1s
- Easing: ease-in-out
- Scale: 1.0 → 1.05 → 1.0
- Repeat: Yes (while loading)

### Complete Animation
- Duration: 300ms
- Easing: ease-out-back
- Scale: 1.0 → 1.1 → 1.0
- Color: Green flash

---

## Haptic Feedback

### iOS Haptic Patterns
- **Light**: UIImpactFeedbackStyle.Light
- **Medium**: UIImpactFeedbackStyle.Medium
- **Heavy**: UIImpactFeedbackStyle.Heavy
- **Success**: UINotificationFeedbackType.Success
- **Warning**: UINotificationFeedbackType.Warning
- **Error**: UINotificationFeedbackType.Error

### Android Vibration Patterns
- **Light**: 50ms
- **Medium**: 100ms
- **Heavy**: 200ms
- **Success**: 100ms pause 50ms 100ms
- **Warning**: 50ms pause 50ms 50ms
- **Error**: 200ms pause 100ms 200ms

### Haptic Triggers
- Button press: Light
- Screen transition: None
- Notification: Medium
- Success: Success pattern
- Error: Error pattern
- Achievement: Heavy

---

## UI Sound Effects

### Sound Types
- **Click**: Short tap sound (100ms)
- **Navigate**: Whoosh sound (200ms)
- **Notification**: Chime sound (500ms)
- **Success**: Positive chime (400ms)
- **Error**: Negative buzz (300ms)
- **Achievement**: Fanfare (1s)

### Sound Settings
- Volume: Controlled by settings
- Mute: Respects mute toggle
- Priority: UI sounds > Game sounds
- Pool: 5 concurrent sounds max

### Audio Clips
- Format: WAV or MP3
- Quality: 44.1kHz, 16-bit
- Compression: VBR
- Size: < 50KB per clip

---

## Performance Optimization

### 60 FPS Guarantee
- Use DOTween's optimized tweens
- Avoid nested tweens
- Use sequences for complex animations
- Kill tweens on scene unload
- Use object pooling for particles

### Optimization Techniques
- Cache DOTween references
- Use SetRecyclable(true)
- Use SetAutoKill(true)
- Avoid Update() in animations
- Use coroutines for delays

### Profiling
- Monitor frame time
- Track tween count
- Log performance warnings
- Optimize heavy animations

---

## Animation States

### Normal
- All animations enabled
- Full frame rate
- Haptic feedback enabled
- Sound effects enabled

### Reduce Motion
- Fade transitions only
- No scale animations
- No shake effects
- No haptic feedback
- Sound effects reduced

### Low Power
- Slower animations (2x duration)
- Fewer particles
- Reduced haptic
- Sound effects muted

---

## Accessibility

### Animation Preferences
- Respect system reduce motion setting
- Provide animation toggle
- Allow animation speed adjustment
- Support custom easing

### Haptic Preferences
- Respect system haptic setting
- Provide haptic toggle
- Allow intensity adjustment
- Support custom patterns

### Sound Preferences
- Respect system mute setting
- Provide sound toggle
- Allow volume adjustment
- Support custom sounds
