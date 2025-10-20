# E-Pood .NET 8 WebAPI

See on lihtne e-pood, mille eesmärk on hallata tooteid, kategooriaid ja tellimusi. Projekt on tehtud **.NET 8** ja kasutab **Entity Framework Core** andmebaasihaldust.

## Funktsioonid

- **Tootehaldus (Product)**
  - Nimetus, kirjeldus, hind, pilt
  - Seos kategooriaga
- **Kategooriad (Category)**
  - Üks tase, sisaldab mitut toodet
- **Tellimused (Order & OrderItem)**
  - Tellimuse kuupäev, staatus
  - Tellimuse read koos toote ja hinna info
- **WebAPI kontrollerid**
  - GET Products, Categories, Orders
- **Mediator pattern** päringute ja handlereid jaoks
- **Andmebaas EF Core migrations abil**

## Andmebaas

- SQL Server LocalDB  
- Tabelid: `Products`, `Categories`, `Orders`, `OrderItems`  
- Algandmed kategooriatele: `Electronics`, `Clothing`, `Books`

## Käivitamine

1. Kloneeri hoidla:
```bash
git clone https://github.com/sirlikont/Programmeerimine2.git
```
2. Ava lahendus Visual Studio 2022-s.

3. Veendu, et connection string on appsettings.json korrektne.

4. Käivita Update-Database Package Manager Console abil, et luua andmebaas.

5. Käivita WebAPI (F5 või Start Debugging).

API Näited

GET /api/Products – tagastab kõik tooted

GET /api/Categories – tagastab kõik kategooriad

GET /api/Orders – tagastab kõik tellimused
