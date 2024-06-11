# DotNetLibrary
University mandatory project for the `Enterprise` half of the `Advanced Programming Paradigms` exam.

The project consists of a simple RESTful API written with `.NET 8.0`, `Entity Framework`, `ASP.NET Core` and `Swashbuckle`.

> **Disclaimer**<br>
> This application is **NOT** to be used in an actual real-world scenario.<br>
> Despite implementing some basic security measures, like password hashing and use of `JWT` tokens, it is far from perfect and likely still has many unfound bugs and security vulnerabilities.<br>
> This application does not use `https`, does not revoke authentication tokens when a user changes their password or gets deleted (essentially because there isn't a refresh token), does not use an additional database secret on top of the per-user salt for hashing, and does not have any kind of rate limiting or IP banning features, nor does it apply email verification for new users registration.<br>
> It moreover does not use secrets or ignored files to hide the database password, because this is, once again, only an educational application, developer for a university exam.

The application models a very trivial use case for a library, while also providing a `Swagger` web interface to interact with the API.<br>
The modeled library itself is very much simplified, it only having one many-to-many relationship between books and categories they belong to. The separation of authors and publishers from books was not required, and is left as a potential improvement to the project, as well as `https` support and some other minor details such as improved filtering and sorting.

Since the project was developed on `ArchLinux`, deploying it is made simpler by containerization of the whole infrastructure.<br>
To run the application, simply use `docker-compose -f docker-compose.yml up -d` from inside the [`DotNetLibrary`](DotNetLibrary) inner directory (where the [`.yml`](DotNetLibrary/docker-compose.yml) file is located).<br>
The configured services will spin up the `MySQL` database and populate it using [`init.sql`](DotNetLibrary/init.sql) (exposing port `50666` for database connections), then build the [`DotNetLibrary.API`](DotNetLibrary/DotNetLibrary.API) [`Dockerfile`](DotNetLibrary/DotNetLibrary.API/Dockerfile) and run it (exposing port `50667` for the `Swagger` interface).

Some scripts, both bash and powershell, are provided to:
- start docker services;
- start docker services while re-populating the database (deleting the previously created volume) and re-building the API project;
- stop docker services.

To test the application, visit [`http://localhost:50667/swagger`](http://localhost:50667/swagger) using any browser.
