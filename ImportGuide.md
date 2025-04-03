# Guide för att importera TheLoadingBean-databasen

## Förutsättningar
- MongoDB Community Server installerad
- MongoDB Compass installerad

## Steg för att importera databasen

1. **Öppna MongoDB Compass**
   - Starta MongoDB Compass
   - Anslut till: `mongodb://localhost:27017`

2. **Skapa ny databas**
   - Klicka på "Create Database"
   - Ange databasnamn: `TheLoadingBean`
   - Ange första collection-namn: `products` (eller valfri collection)
   - Klicka på "Create Database"

3. **Importera collections**
   - Välj databasen "TheLoadingBean"
   - För varje collection (products, customers, orders):
     - Klicka på collection-namnet
     - Klicka på "Import Data"
     - Välj JSON-format
     - Välj motsvarande JSON-fil från den exporterade databasen
     - Klicka på "Import"

4. **Verifiera importen**
   - Kontrollera att alla collections har importerats
   - Verifiera att data finns i varje collection
   - Kontrollera att antalet dokument stämmer

## Collections i databasen

### Products
- Innehåller alla kaffeprodukter
- Fält: id, name, description, price, stockQuantity, category, imageUrl

### Customers
- Innehåller alla kunder
- Fält: id, name, email, phone, address, password (hashed), role

### Orders
- Innehåller alla beställningar
- Fält: id, customerId, items, totalAmount, status, orderDate

## Testdata
Databasen innehåller följande testdata:
- 5 kaffeprodukter med olika kategorier
- 2 testanvändare:
  - Admin: admin@theloadingbean.com
  - Kund: customer@example.com

## Felsökning
Om du stöter på problem:
1. Kontrollera att MongoDB-tjänsten kör
2. Verifiera att du har rätt behörigheter
3. Kontrollera att JSON-filerna är korrekt formaterade
4. Se till att alla collections har rätt schema 