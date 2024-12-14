# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image to build the API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy the entire solution directory (which includes all projects) into the Docker image.
COPY . ./

# Restore, build, and publish the Api project.
RUN dotnet restore "Api/Api.csproj"
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish 

# Define the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
