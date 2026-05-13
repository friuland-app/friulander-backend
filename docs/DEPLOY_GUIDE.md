# Guida Deploy Store - Friulander

## Google Play Store

### Prerequisiti
- Account Google Play Developer ($25 una tantum)
- APK firmato
- Screenshot (2-8)
- Icone (512x512)
- Feature graphic (1024x500)

### Passi

1. **Crea APK**
   - Godot → Export → Android
   - Configura keystore
   - Esporta APK

2. **Crea Console Play**
   - Accedi a https://play.google.com/console
   - Crea nuova app
   - Inserisci dettagli app

3. **Carica APK**
   - Upload APK
   - Aspetta review automatica

4. **Compila Store Listing**
   - Titolo: Friulander
   - Short description (80 char)
   - Full description
   - Carica screenshot
   - Carica icone
   - Aggiungi keywords

5. **Content Rating**
   - Compila questionnaire
   - E for Everyone

6. **Pricing**
   - Gratuito o a pagamento
   - Configura IAP se necessario

7. **Distribuzione**
   - Release track
   - Test interno
   - Test chiuso
   - Produzione

## App Store

### Prerequisiti
- Account Apple Developer ($99/anno)
- IPA firmato
- Screenshot (3-10)
- Icone (1024x1024)
- Certificati e provisioning profiles

### Passi

1. **Crea IPA**
   - Godot → Export → iOS
   - Configura certificati
   - Esporta progetto Xcode
   - Build IPA in Xcode

2. **App Store Connect**
   - Accedi a https://appstoreconnect.apple.com
   - Crea nuova app
   - Inserisci bundle ID

3. **Compila App Information**
   - Nome: Friulander
   - Subtitle (30 char)
   - Description
   - Keywords
   - Support URL
   - Marketing URL

4. **Carica Assets**
   - Screenshot
   - Icone
   - App preview (opzionale)

5. **Pricing**
   - Prezzo
   - Disponibilità geografica

6. **Version Information**
   - Version number
   - Release notes
   - Upload IPA

7. **Submit for Review**
   - Aspetta review Apple (2-3 giorni)
