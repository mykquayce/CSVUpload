FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
ARG TARGETARCH
WORKDIR /app
COPY . .
RUN dotnet restore ./CSVUpload.Website/CSVUpload.Website.csproj --arch $TARGETARCH --source https://api.nuget.org/v3/index.json
RUN dotnet publish ./CSVUpload.Website/CSVUpload.Website.csproj --arch $TARGETARCH --configuration Release --no-restore --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
EXPOSE 443/tcp
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "./CSVUpload.Website.dll"]
