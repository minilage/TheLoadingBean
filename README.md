# The Loading Bean - Kaffekedja

En webbapplikation för en kaffekedja som hanterar beställningar, produktkatalog och kundhantering.

## Funktioner

- **Produkthantering**
  - Visa produktkatalog
  - Hantera produkter (admin)
  - Kategorisering av kaffeprodukter
  - Lagerhantering

- **Kundhantering**
  - Kundregistrering
  - Inloggning
  - Profilhantering
  - Orderhistorik

- **Beställningshantering**
  - Skapa beställningar
  - Spåra orderstatus
  - Hantera beställningar (admin)

- **Säkerhet**
  - JWT-autentisering
  - Rollbaserad behörighet
  - Säker lösenordshantering

## Teknisk Stack

### Backend
- .NET 8
- MongoDB
- JWT för autentisering
- Repository Pattern
- Unit of Work Pattern

### Frontend
- Blazor WebAssembly
- Bootstrap
- Blazored.Toast för notifikationer

## Installation

### Förutsättningar
- .NET 8 SDK
- MongoDB Community Server
- MongoDB Compass

### Konfiguration

1. **Databas**
   - Starta MongoDB Compass
   - Anslut till: `mongodb://localhost:27017`
   - Skapa ny databas: `TheLoadingBean`
   - Importera collections från de medföljande JSON-filerna:
     - Products
     - Customers
     - Orders

2. **API**
   - Navigera till `TheLoadingBean.API`
   - Uppdatera `appsettings.json` med din MongoDB-connection string
   - Kör `dotnet run`

3. **Client**
   - Navigera till `TheLoadingBean.Client`
   - Kör `dotnet run`

## Testning

### Unit Tests
- Kör `dotnet test` i `TheLoadingBean.Tests` för att köra alla unit tester
- Tester finns för:
  - Controllers
  - Repositories
  - Services
  - DTOs

## API Endpoints

### Produkter
- GET /api/Product - Hämta alla produkter
- GET /api/Product/{id} - Hämta specifik produkt
- POST /api/Product - Skapa ny produkt (admin)
- PUT /api/Product/{id} - Uppdatera produkt (admin)
- DELETE /api/Product/{id} - Ta bort produkt (admin)
- GET /api/Product/search?query=namnEllerNummer - Sök produkt med namn eller nummer

### Kunder
- GET /api/Customer - Hämta alla kunder
- GET /api/Customer/{id} - Hämta kund med ID
- POST /api/Customer - Skapa ny kund
- PUT /api/Customer/{id} - Uppdatera kund
- DELETE /api/Customer/{id} - Ta bort kund
- GET /api/Customer/search?email=adress - Sök kund med e-post

### Ordrar
- GET /api/Order - Hämta alla ordrar
- GET /api/Order/{id} - Hämta en specifik order
- POST /api/Order - Skapa ny order
- DELETE /api/Order/{id} - Ta bort en order

## Datamodeller

### Produkt
```json
{
  "ProductNumber": "KAFFE001",
  "Name": "Nerd Roast",
  "Description": "Mörkrostad blandning för kodare",
  "Price": 89.9,
  "Category": "Kaffe",
  "IsAvailable": true
}
```

### Kund
```json
{
  "FirstName": "Tina",
  "LastName": "Lagesson",
  "Email": "tina@loadingbean.dev",
  "Phone": "070-1234567",
  "Address": "Buggvägen 1, Kodstad"
}
```

### Order
```json
{
  "CustomerId": "123abc",
  "ProductIds": ["456def", "789ghi"],
  "OrderDate": "2025-03-20T09:30:00Z",
  "TotalAmount": 258.0
}
```

## Testanvändare

### Admin
- Email: admin@theloadingbean.com
- Lösenord: Admin123!

### Kund
- Email: customer@example.com
- Lösenord: Customer123!

## Utvecklare

- Tina Lagesson
- tina@loadingbean.dev

## Licens
Detta projekt är skapat som en del av en utbildningsuppgift. 