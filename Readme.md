# Pinewood Task

## Prerequisites

1) Ensure that you have .NET 8 installed on your machine.

2) This solution makes use of sql server database. Ensure that you have an instance of sql server installed on your machine

## Running the App
1) Modify connection string in appsettings.json in the Crud.Api project to point to your sql server instance 

2) Modify the Default & Master connection string in appsettings.json located in the Crud.Migration project to point to your sql server instance. 

3) Set Crud.Migration as startup project and run the project to create database and table this leverages on a migration tool called [DbUp](https://github.com/user/repo/blob/branch/other_file.md).
(This step 3 is only needed when you are running the application for the very first time, subsequent runs requires only step 4)

4) Run the Crud.Api project to start the Web Api MVC project

### N.B
- The master database is a system database in SQL Server that contains system tables and metadata. When creating or managing databases, you often need administrative privileges, and connecting to the master database is a common practice. 
- The default database is the database that the user is connected to when connecting to SQL Server. 
- DbUp is primarily designed for database schema upgrades and doesn't have built-in functionality for ensuring the existence of a database.


