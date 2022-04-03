FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR src/SiGameReborn.Tokens.Api

COPY .. ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR src/SiGameReborn.Tokens.Api
COPY --from=build-env src/SiGameReborn.Tokens.Api/out .
ENTRYPOINT ["dotnet", "SiGameReborn.Tokens.Api.dll"]