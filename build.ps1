docker pull mcr.microsoft.com/dotnet/sdk:8.0-alpine
docker pull mcr.microsoft.com/dotnet/aspnet:8.0-alpine
docker pull mariadb:latest


docker buildx build `
	--file '.\CSVUpload.Website\Dockerfile' `
	--platform 'linux/amd64,linux/arm64' `
	--pull `
	--push `
	--tag 'registry.local/4thu/csv-upload:latest' `
	.
