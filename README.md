## MS SQL lokální development
### Prerekvizity
- nainstalovaný Docker

### Kroky
1. Ve složce `project` spustit `docker-compose up -d` a ujistit se že MS SQL běží
2. Jít do projektu `TaHooK.Api.DAL` a spustit tyto příkazy
```bash
# Přidání connection stringu do User secrets
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<connection string>"

# Migrace databaze
dotnet ef database update
```


