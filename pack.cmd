del *.*nupkg
dotnet build -c Release /p:ContinuousIntegrationBuild=true
dotnet pack -c Release --include-symbols --include-source .\src\FluentAuthorization\FluentAuthorization.csproj --no-build --output release
dotnet pack -c Release --include-symbols --include-source .\src\FluentAuthorization.DependencyInjection\FluentAuthorization.DependencyInjection.csproj --no-build --output release