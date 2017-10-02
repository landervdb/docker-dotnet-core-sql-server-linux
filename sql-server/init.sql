CREATE LOGIN demouser WITH PASSWORD = 'DemoPass12'
GO

CREATE USER demouser FOR LOGIN demouser;
GO

ALTER SERVER ROLE sysadmin ADD MEMBER [demouser];
GO

CREATE DATABASE demoapp;
GO
