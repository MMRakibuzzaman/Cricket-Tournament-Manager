# Cricket Tournament Manager 🏏

A full-stack ASP.NET Core MVC application designed to manage cricket tournaments, teams, and player squads. This project demonstrates modern .NET architecture patterns including **Generic Repository**, **Dependency Injection**, and **Master-Detail** relationships.

## 🚀 Features

* **Team Management:** Create and edit teams with logos, revenue data, and established dates.
* **Roster Management:** Master-Detail interface to add/remove players dynamically while editing a team.
* **Tournament Scheduling:** Link teams to tournaments and schedule matches.
* **Squad Logic:** Validates that teams cannot play against themselves and must be registered in the tournament to play.
* **Dynamic UI:** Uses AJAX for smooth updates without full page reloads (e.g., removing players, uploading logos).

## 🛠️ Tech Stack

* **Framework:** ASP.NET Core 8 MVC
* **Database:** SQL Server / Entity Framework Core
* **Architecture:** Repository Pattern (Generic)
* **Frontend:** Bootstrap 5, jQuery, AJAX
* **Tools:** JetBrains Rider, Docker (Optional)

## 🏗️ Architecture Highlights

### The Generic Repository Pattern
I implemented a `GenericRepository<T>` to handle standard CRUD operations, keeping Controllers clean and focused on business logic.
```csharp
// Example: Creating a Team with the Repository
await _teamRepo.CreateAsync(team);
```
## ⚙️ Getting Started

*    **Clone the repository.**

*    **Update the connection string in appsettings.json.**

*    **Run Entity Framework Migrations**

*    **Update-Database**