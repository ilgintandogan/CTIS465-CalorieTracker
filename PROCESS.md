# Process

We use this file to keep track of what we did, what we tested and where we are in the project. We update it while we work.

## Where we are now

Phase 1 is finished and it is on the `main` branch. We are working on the final part on the `final-improvements` branch so that main stays the same as what we submitted for phase 1.

Right now we are at step 14 (JWT authentication). We added the JWT classes to CORE and next we will add the Users projects.

## Phase 1 (main branch)

- [x] Formed the group and chose our own domain (calorie tracker)
- [x] Sent the E-R diagram and got it approved (Category, Food, Meal, MealFood)
- [x] Created the entity classes in Calories.APP Domain
- [x] Added the DbSets to CaloriesDb
- [x] Added the connection string to appsettings.json
- [x] Registered CaloriesDb in Program.cs
- [x] Created the database with migration v1
- [x] Added a seed controller (`api/SeedDb`) to fill the tables with test data
- [x] Wrote the request, response and handler classes with MediatR for Categories, Foods and Meals
- [x] Registered MediatR in Program.cs
- [x] Copied the Templates folder to Calories.API
- [x] Scaffolded the controllers for Categories, Foods and Meals
- [x] Tested the endpoints with Swagger

## Final (final-improvements branch)

- [ ] Step 14: JWT authentication with token and refresh token
  - [x] Added the JwtBearer package and the JWT models and service to CORE
  - [ ] Users.APP with User, Role, UserRole and Status
  - [ ] Token and RefreshToken handlers
  - [ ] Users.API with TokensController and JWT and Swagger settings in Program.cs
- [ ] Step 15: Admin role authorization, one GET action with AllowAnonymous
- [ ] Step 16: Testing the endpoints with JWT
- [ ] Step 17: Gateway project with Ocelot

## Tests

### Phase 1

We ran Calories.API and tested everything from Swagger.

1. Ran `GET /api/SeedDb` first so the tables have data.
2. Categories: get all, get by id, add, update and delete worked. Deleting a category that has foods gives an error message, which is what we wanted.
3. Foods: get all, get by id, add (with a categoryId), update and delete worked.
4. Meals: get all, get by id, add (with foodIds), update and delete worked. The foods of a meal are listed in the response.

### Final

No tests yet. We will write them here after we finish the JWT part.

## Notes

- If the solution is opened with Visual Studio 2019 the projects do not load because the project is .NET 8. It has to be opened with Visual Studio 2022.
- The database file is not in the repo, so after cloning `update-database` has to be run once (it is explained in the README).
