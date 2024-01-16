
### Prerequisites
1 Ensure that you have .NET 8 installed on your machine
2 This solution makes use of sql server database. Ensure that you have sql server installed on your machine
3 Modify connection string in appsettings.json to point to your sql server instance
4 Modify the Default & Master connection string in appsettings.json located in the Crud.Migration project to point to your sql server instance.
  The master database is a system database in SQL Server that contains system tables and metadata. When creating or managing databases, you often need administrative privileges, and connecting to the master database is a common practice.
  The default database is the database that the user is connected to when connecting to SQL Server. 
  DbUp is primarily designed for database schema upgrades and doesn't have built-in functionality for ensuring the existence of a database.
5 Set Crud.Migration as startup project and run the project to create database and table this leverages on https://dbup.readthedocs.io/en/latest/
6 Run the Crud.Api project to start the Web Api MVC project