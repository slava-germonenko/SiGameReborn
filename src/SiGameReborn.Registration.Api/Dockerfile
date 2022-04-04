FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR src/SiGameReborn.Registration.Api

COPY .. ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR src/SiGameReborn.Registration.Api
COPY --from=build-env src/SiGameReborn.Registration.Api/out .
ENTRYPOINT ["dotnet", "SiGameReborn.Registration.Api.dll"]