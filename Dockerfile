FROM microsoft/dotnet:1.0.4-runtime

WORKDIR /dotnetapp
COPY app/out .
ENTRYPOINT ["dotnet", "app.dll"]
