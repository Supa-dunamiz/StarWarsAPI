# StarWarsAPI
This is a .NET 8 Web API powered by Entity Framework Core and SQLite, with Swagger UI for interactive API testing. You can run it either using Docker or locally with the .NET CLI or Visual Studio.
________________________________________
🚀 Prerequisites
----------------------------------------
•	Docker Desktop
•	.NET SDK 8.0+ (required for local development)
________________________________________
🐳 Run with Docker
---------------------------------------

1.	Clone the repository
2.	Ensure Docker configuration is enabled on the project
Update appsettings.json:

  "UseDocker": true

3.	Build and run the container
4.	docker compose up --build
5.	Access Swagger UI
Open your browser:
6.	http://localhost:8080/swagger
________________________________________
💻 Run Locally (Without Docker)
----------------------------------------

1.	Clone the repository
2.	Switch to local configuration in the project
Update appsettings.json:

  "UseDocker": false

3.	Run using .NET CLI
dotnet build
dotnet run
Or with hot reload:
dotnet watch run
4.	Access Swagger UI
5.	https://localhost:7064/swagger

________________________________________
🔐 Authentication
----------------------------------------
To access the endpoints, you must be authenticated.
Admin User:

•	Username: admin
•	Password: 123456

Regular User:

•	Username: User
•	Password: password

ℹ️ Regular users can create and read starships, but cannot update or delete records.
________________________________________
🧪 Unit Testing
----------------------------------------
This project includes a unit test project to validate key methods in the StarshipRepository.
Run tests with:
dotnet test
