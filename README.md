# Calorie Tracker

CTIS 465 Microservice Development with .NET project.

Team members: Ilgın Tandoğan, Yiğitcan Karagöz, Kerem Sönmez

## Projects

| Project | Description |
|---|---|
| `CORE` | Base classes: `Record`, `CommandResponse`, `Service`, `DbService` |
| `Calories.APP` | Domain entities (Category, Food, Meal, MealFood), `CaloriesDb` context, migrations and MediatR request, response and handler classes under `Features` |
| `Calories.API` | Web API with Categories, Foods and Meals controllers, and the `api/SeedDb` action |

Relationships: Category 1 to many Food, Meal many to many Food through MealFood.

## Running the project

The SQLite database file (`CaloriesDB.db`) is not stored in the repository, so it must be created once after cloning.

1. Open `CalorieTracker.sln` in Visual Studio.
2. Create the database:
   - Visual Studio: Tools -> NuGet Package Manager -> Package Manager Console, set `Calories.APP` as Default Project, then run:
     ```
     update-database
     ```
   - Or from the terminal in the solution folder:
     ```
     dotnet tool install -g dotnet-ef
     dotnet ef database update -p Calories.APP -s Calories.API
     ```
3. Set `Calories.API` as the startup project and run it with the `https` profile (F5). Swagger opens at `https://localhost:7176/swagger`.
4. Execute `GET /api/SeedDb` once to fill the tables with initial data.

## Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/SeedDb` | Deletes all data and inserts initial data |
| GET | `/api/Categories`, `/api/Foods`, `/api/Meals` | List all records |
| GET | `/api/{controller}/{id}` | Get one record by id |
| POST | `/api/{controller}` | Create a record |
| PUT | `/api/{controller}` | Update a record (id in the body) |
| DELETE | `/api/{controller}/{id}` | Delete a record |

Example request bodies:

```json
// POST /api/Foods
{ "name": "Broccoli", "servingDescription": "1 cup (91 g)", "calories": 31, "proteinGrams": 2.5, "isVegan": true, "categoryId": 4 }

// POST /api/Meals  (mealType: 0 Breakfast, 1 Lunch, 2 Dinner, 3 Snack)
{ "title": "Lunch Plate", "mealType": 1, "mealDate": "2026-11-10T12:30:00", "notes": "", "foodIds": [1, 3] }
```


