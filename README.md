# ASP.NET Docker image

Author: Lander Van den Bulcke

Disclaimer: This was made as a school assignment. Therefore, no guarantees in terms of quality/functionality. ;)

## Docker compose

### Preparing application

```bash
$ cd app
$ dotnet restore
$ dotnet publish -c Release -o out
$ cd ..
```

### Run entire stack

```bash
$ docker-compose up -d
```

### Stop stack

```bash
$ docker-compose stop
```

### Stop stack and remove images

```bash
$ docker-compose down
```

## Manual

### Building the image

```bash
$ cd app
$ dotnet restore
$ dotnet publish -c Release -o out
$ cd ..
$ docker build -t landervdb/dotnet-demo .
```

### Create container

```bash
$ docker run --name dotnet-demo -p 5000:5000 --link sql-server-demo -e SQLSERVER_HOST=sql-server-demo -d landervdb/dotnet-demo
```

This command assumes there is already a sql-server container running with the name `sql-server-demo` (see `sql-server` folder).

### Stop container

```bash
$ docker stop dotnet-demo
```

### Start container

```bash
$ docker start dotnet-demo
```

## Changes made in application code

- Program.cs:

```csharp
var host = new WebHostBuilder()
                .UseUrls("http://*:5000") // Dit toevoegen
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
```

- Startup.cs:

```csharp
using System;

//...

var hostname = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "localhost";
            var connString = $"Data Source={hostname};Initial Catalog=demoapp;User ID=demouser;Password=DemoPass12;";

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connString));
```

- General:

```bash
$ dotnet migrate
```

## Sources

- http://blog.kontena.io/dot-net-core-and-sql-server-in-docker/
- https://github.com/DataGrip/docker-env/tree/master/mssql-server-linux
- https://github.com/twright-msft/mssql-node-docker-demo-app
- https://docs.docker.com/compose/gettingstarted
