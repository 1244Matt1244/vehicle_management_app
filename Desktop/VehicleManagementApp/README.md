---

```markdown
# Vehicle Management Application

## Pregled

Ovaj projekt je jednostavna ASP.NET MVC aplikacija za upravljanje podacima o vozilima (marke i modeli), koja omogućuje funkcionalnosti poput sortiranja, filtriranja i paginacije. Aplikacija je razvijena korištenjem modernih tehnologija i principa objektno orijentiranog programiranja kako bi demonstrirala osnove C#, osnovnih design patterna te korištenje Entity Frameworka i drugih alata.

## Tehnologije

- **C# / .NET Framework 4.7.2 ili noviji**
- **ASP.NET MVC 5**
- **Entity Framework 6 Code First**
- **Ninject za Dependency Injection**
- **AutoMapper za mapiranje objekata**
- **MSSQL LocalDB**

## Zahtjevi

- **Visual Studio 2019** ili noviji
- **.NET Framework 4.7.2** ili noviji
- **SQL Server LocalDB**

## Instalacija i pokretanje

1. **Kloniranje repozitorija:**

   ```bash
   git clone https://github.com/vaškorisnički/vehicle-management-app.git
   ```

2. **Otvaranje rješenja:**

   Otvorite rješenje `VehicleManagementApp.sln` u Visual Studiu.

3. **Obnova NuGet paketa:**

   Desnom tipkom miša kliknite na rješenje u **Solution Exploreru** i odaberite **Restore NuGet Packages**.

4. **Konfiguracija konekcijskog stringa:**

   Provjerite i, ako je potrebno, uredite konekcijski string u datoteci `Web.config` unutar **Project.MVC** projekta:

   ```xml
   <connectionStrings>
       <add name="VehicleDbConnection" 
            connectionString="Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=VehicleDb;Integrated Security=True;" 
            providerName="System.Data.SqlClient"/>
   </connectionStrings>
   ```

5. **Migracije i kreiranje baze:**

   Otvorite **Package Manager Console**, postavite **Default project** na **Project.Service** i pokrenite sljedeće komande:

   ```powershell
   Enable-Migrations
   Add-Migration InitialCreate
   Update-Database
   ```

6. **Pokretanje aplikacije:**

   Pritisnite **F5** ili kliknite na **Start Debugging** kako biste pokrenuli aplikaciju. Aplikacija bi se trebala otvoriti u web pregledniku.

## Korištenje aplikacije

- **Upravljanje markama vozila (VehicleMake):**

  - Posjetite `/VehicleMake` za upravljanje markama vozila.
  - Omogućene su CRUD operacije (Create, Read, Update, Delete).
  - Podržano je sortiranje po nazivu, filtriranje i paginacija.

- **Upravljanje modelima vozila (VehicleModel):**

  - Posjetite `/VehicleModel` za upravljanje modelima vozila.
  - Omogućene su CRUD operacije.
  - Podržano je filtriranje po markama vozila, sortiranje i paginacija.

## Struktura projekta

- **Project.Service**
  - Sadrži EF modele (`VehicleMake`, `VehicleModel`) i kontekst baze podataka (`VehicleContext`).
  - Implementira servisni sloj s asinkronim (async/await) CRUD metodama kroz `VehicleService`.
  - Definira interfejse (`IVehicleService`) koji omogućavaju unit testiranje i apstrakciju.

- **Project.MVC**
  - ASP.NET MVC aplikacija s kontrolerima (`VehicleMakeController`, `VehicleModelController`), view modelima i Razor view-ovima.
  - Konfiguriran je Dependency Injection putem Ninjecta u `App_Start/NinjectWebCommon.cs`.
  - AutoMapper profil (`VehicleMappingProfile`) se koristi za mapiranje između EF modela i view modela.
  - Folderi `Views/VehicleMake` i `Views/VehicleModel` sadrže view-ove za CRUD operacije.

## Ključne značajke

- **Asinhronost i performanse:**
  - Svi servisni metodi su implementirani asinhrono koristeći `async/await` za bolje performanse i responzivnost aplikacije.

- **Testabilnost:**
  - Korištenje interfejsa i dependency injection omogućava jednostavno pisanje unit testova.
  - Servisni sloj je apstrahiran, što omogućava mocking tijekom testiranja.

- **Inversion of Control (IoC) i Dependency Injection (DI):**
  - Implementirano korištenjem Ninjecta za smanjenje ovisnosti među komponentama i poboljšanje modularnosti koda.

- **Mapiranje modela:**
  - AutoMapper se koristi za konverziju između EF modela i view modela, osiguravajući jasnu separaciju poslovne logike i prezentacijskog sloja.

- **Sortiranje, filtriranje i paginacija:**
  - Implementirane su funkcionalnosti sortiranja podataka po nazivu.
  - Mogućnost filtriranja rezultata pretrage po nazivu ili marki vozila.
  - Podržana je paginacija rezultata s prilagodljivom veličinom stranice.

## Dodatne napomene

- **Razor view-ovi:**
  - View-ovi su implementirani koristeći standardne ASP.NET MVC konvencije i koriste view modele iz **Project.MVC/ViewModels**.

- **Validacija i greške:**
  - Implementirana je validacija korisničkih unosa koristeći data annotations u view modelima.
  - Greške se prikazuju korisniku na intuitivan način.

- **Unit testovi (preporučeno):**
  - Preporuča se implementirati unit testove za servisni sloj koristeći testni okvir poput MSTest ili NUnit te mocking alate kao što je Moq.

- **Dokumentacija:**
  - U repozitoriju se nalazi ovaj README s detaljnim uputama za instalaciju, konfiguraciju i korištenje aplikacije.

## Autori

- [Vaše ime]
- Kontakt: [vaš.email@example.com](mailto:vaš.email@example.com)

## Licenca

Ovaj projekt je objavljen pod **MIT licencom**. Pogledajte datoteku `LICENSE` za više informacija.

```

---