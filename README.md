# NextBuy

NextBuy est une application web de dropshipping complète construite avec ASP.NET Core 9.0 et Razor Pages.

## 🚀 Fonctionnalités

*   **Boutique en ligne** : Catalogue de produits, recherche et filtrage.
*   **Panier d'achat** : Gestion du panier et persistance de session.
*   **Authentification** : Système complet d'inscription et de connexion (Identity).
*   **Administration** : Tableau de bord pour la gestion des produits, des catégories et des commandes.
*   **Design** : Interface utilisateur moderne et responsive.

## 🛠 Technologies Utilisées

*   **Framework** : .NET 9.0 (ASP.NET Core)
*   **Architecture** : Razor Pages (MVC)
*   **Base de données** : SQL Server
*   **ORM** : Entity Framework Core
*   **Frontend** : HTML5, CSS3, JavaScript (Bootstrap intégré)


2.  **Configuration de la Base de Données**
    Assurez-vous que la chaîne de connexion dans `appsettings.json` pointe vers votre instance SQL Server locale.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NextBuyDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

3.  **Appliquer les Migrations**
    Créez la base de données et appliquez le schéma initial.
    ```bash
    dotnet ef database update
    ```

4.  **Lancer l'application**
    ```bash
    dotnet run
    ```
    L'application sera accessible généralement sur `https://localhost:7001` ou `http://localhost:5000`.

## 📦 Structure du Projet

*   **Pages/** : Contient les pages Razor (Vues et Logique).
*   **Models/** : Classes du modèle de données.
*   **Data/** : Contexte de base de données (DbContext) et migrations.
*   **wwwroot/** : Fichiers statiques (CSS, JS, Images).
*   **Areas/** : Sections distinctes comme l'interface d'administration (si applicable).
