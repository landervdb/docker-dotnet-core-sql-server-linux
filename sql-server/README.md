# SQL Server Docker image

## Image builden

```bash
$ docker build --nocache -t landervdb/sql-server-demo .
```

## Container aanmaken

```bash
$ docker run -d --name sql-server-demo -p 1433:1433 landervdb/sql-server-demo
```

## Container stoppen

```bash
$ docker stop sql-server-demo
```

## Container starten

```bash
$ docker start sql-server-demo
```
