# Movies Web API
A simple web API project to process movie-related data.

## How to use
1. Clone the repo and open the project in your IDE (I use Visual Studio).
2. Create a new database, in this case, I'm using SQL Server. You can find the schema in `db_schema.sql` file.
3. Get the "connection string" of your newly created database. 
4. Navigate to `appsettings.json` file in the project folder and change the value for `DefaultConnection` with your database connection string.
5. Run the project.

## Additional Instructions
- Create a sample account using the POST: `api/Accounts` endpoint. Include this json in the request body.   
`{ "username": "#usernamehere","password": "#passwordhere"}`
- It'll give you a response containing the API key you can use to access the available endpoints.
