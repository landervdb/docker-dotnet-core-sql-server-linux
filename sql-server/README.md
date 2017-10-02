# SQL Server Docker image

## Build image

```bash
$ docker build --nocache -t landervdb/sql-server-demo .
```

## Create container

```bash
$ docker run -d --name sql-server-demo -p 1433:1433 landervdb/sql-server-demo
```

## Stop container

```bash
$ docker stop sql-server-demo
```

## Start container

```bash
$ docker start sql-server-demo
```
