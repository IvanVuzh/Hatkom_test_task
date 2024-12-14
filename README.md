# Database

To create setup a database, run `docker-compose up` inside Database folder of this project in your Terminal while running docker.
<br />
Run `dotnet ef database update` from cli or `update-database` from Package Manager Console to apply migrations.


# Api

To start the api, make sure if your database is running and is up to date. 
Open `appsettings.json` and check if default connection string is correct.
<br />
Open Terminal in /Api folder and run `dotnet build` and `dotnet run`.
<br />
You should see output like this:<br /> ![image](https://github.com/user-attachments/assets/e8818bcc-0e61-4907-86cf-9670db87d739)
<br />
<br />
Now Api is running and you can navigate to `http://localhost:xxxx/swagger/index.html` (replace xxxx with "now listening" port)
