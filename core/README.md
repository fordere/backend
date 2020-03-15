# Fordere.ch Backend 

This is the .NET Core 3.1 Version of the fordere backend.

## Requirements
The following tools have to be installed on your developent machine:

- dotnet (https://dotnet.microsoft.com/download)
- mysql (8.x) (https://dev.mysql.com/downloads/installer/)

## How to run

### Install ServiceStack licence
Copy the provided licence key into a file called licence.txt directly into the "forderebackend" directory.

### Setup Database
Setup a database with the name "fordere". Then excute the SQL-setup script you can find in the directory "forderebackend.DbMigration". 

### Run the project "forderebackend"
The backend should now be running on localhost:5001

## Notes
- For most of the requests you have to provide a divison-id. This id has to be set as a header called "divison_id"
- Currently the complete AppSettings-thing is not yet ported to this version. Therefore alle configurations for sending mails, payment, sms, .. will not be available!
- You can find the service with the logic in the project forerebackend.ServiceInterface. 
- Every fordere "Service" extends the BaseService which provides some basic informations like the current DivisonId or access to the Database (Db-Object)
- The routes are defined on the message-objects which you can find in the project "forderebackend.ServiceModel"
