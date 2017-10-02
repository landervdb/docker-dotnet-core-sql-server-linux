# ASP.NET Docker image

## Docker compose

### Applicatie klaarmaken

```bash
$ cd app
$ dotnet restore
$ dotnet publish -c Release -o out
$ cd ..
```

### Hele stack runnen

```bash
$ docker-compose up -d
```

### Stack stoppen

```bash
$ docker-compose stop
```

### Stack stoppen en images verwijderen

```bash
$ docker-compose down
```

## Handmatig

### Image builden

```bash
$ cd app
$ dotnet restore
$ dotnet publish -c Release -o out
$ cd ..
$ docker build -t landervdb/dotnet-demo .
```

### Container aanmaken

```bash
$ docker run --name dotnet-demo -p 5000:5000 --link sql-server-demo -e SQLSERVER_HOST=sql-server-demo -d landervdb/dotnet-demo
```

Dit commando gaat er van uit dat er reeds een sql-server container draait met de naam `sql-server-demo` (zie de submap sql-server).

### Container stoppen

```bash
$ docker stop dotnet-demo
```

### Container starten

```bash
$ docker start dotnet-demo
```

## Gemaakte aanpassingen in source code applicatie

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

- Algemeen:

```bash
$ dotnet migrate
```

## Bronnen

- http://blog.kontena.io/dot-net-core-and-sql-server-in-docker/
- https://github.com/DataGrip/docker-env/tree/master/mssql-server-linux
- https://github.com/twright-msft/mssql-node-docker-demo-app
- https://docs.docker.com/compose/gettingstarted
