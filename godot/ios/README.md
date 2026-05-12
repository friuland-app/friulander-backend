# iOS Export Configuration

## Certificati e Provisioning Profiles

### 1. Apple Developer Account
- Iscriviti a Apple Developer Program ($99/anno)
- Accedi a https://developer.apple.com/account

### 2. Team ID
- Copia il Team ID dal portale developer
- Inseriscilo in `project.godot` → `application/app_store_team_id`

### 3. Certificato di Sviluppo
- Vai a Certificates, Identifiers & Profiles
- Crea un nuovo certificato di sviluppo
- Scarica e installa nel Keychain

### 4. App ID
- Crea un nuovo App ID con bundle: `com.friuland.friulander`
- Abilita: Push Notifications, Location, Camera

### 5. Provisioning Profile
- Crea provisioning profile di sviluppo
- Associa al certificato e App ID
- Scarica e installa

### 6. Export in Godot
1. Project → Export → iOS
2. Inserisci Team ID
3. Seleziona provisioning profile
4. Esporta progetto Xcode

### 7. Build in Xcode
- Apri il progetto esportato
- Firma con il certificato
- Build e test su dispositivo
