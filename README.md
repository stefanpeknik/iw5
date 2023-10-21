## Lokální development
### Prerekvizity
- nainstalovaný Docker

### Kroky
1. Ve složce `project` spustit `docker-compose up -d` a ujistit se že MS SQL běží
2. Vytvořit `appsettings.Development.json` a přidat do něj connection string
```json
// appsettings.Development.json example
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "SQLCONNSTR_DefaultConnection": "<your connection string>"
  }
}
```


