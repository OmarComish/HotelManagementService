#Add migration
dotnet ef migrations add InitialCreate --project ./src/Infrastructure  --startup-project ./src/API --output-dir ./Data/Migrations

#Update database
dotnet ef database update --project ./src/Infrastructure  --startup-project ./src/API

#Drop database
dotnet ef database drop --project ./src/Infrastructure  --startup-project ./src/API 