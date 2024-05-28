Role-Based Data Access Testing Manual


Introduction


This manual guides you through testing a role-based authentication and authorization system implemented in ASP.NET Core 7. This system controls user access to specific data within the application's database, depending on their assigned role.

Prerequisites



Seeded Roles: The database should be pre-populated with the required roles: ADMIN, SALES, and DEVS. (This has already been done in the development environment.)
Postman: Ensure Postman is installed and configured for API testing.
Test Procedures
User Registration and Initial Role Assignment

Register a new user with the required details: first name, last name, username, email, and a secure password.
Expected Result: The new user will be automatically assigned the "Sales" role.
User Login and JWT Token Acquisition

Log in using the newly registered user's credentials.
Expected Result: The API will return a JSON Web Token (JWT) upon successful authentication. This token is essential for accessing protected endpoints.
Using the JWT in Postman

Open Postman and create a new request for the desired endpoint.
In the "Authorization" tab, select "Bearer Token" as the type and paste the JWT obtained in step 2. This is crucial for testing endpoints that are restricted to specific roles (ADMIN, SALES, DEVS).
Role Management

Utilize the make-admin and make-dev endpoints to modify the user's role. Simply provide the username in the request body.
Expected Behavior: The system will remove any existing roles for the user and assign them exclusively to the new role specified in the request.
Endpoint Testing

GET /api/Users (Get Users): Retrieve a list of all registered users.
GET /api/Users/getRoles (Get Roles): Retrieve a list of all available roles within the system.
GET /api/Users/byfirstname/{username} (Get Users by Username): (Accessible only to ADMIN users) Retrieve user details based on the provided username.
GET /api/Users/allDataForSales: (Accessible only to SALES users) Remember to include the JWT in Postman's "Bearer Token" field to authorize as a "Sales" user. Retrieve data accessible to users with the "Sales" role.
GET /api/Users/allDataForDevs: (Accessible only to DEVS users) Remember to include the JWT in Postman's "Bearer Token" field to authorize as a "Devs" user. Retrieve data accessible to users with the "Devs" role.
GET /api/Users/allDataForAdmin: (Accessible only to ADMIN users) Remember to include the JWT in Postman's "Bearer Token" field to authorize as an "Admin" user. Retrieve data accessible to users with the "Admin" role.
