@echo off

setlocal EnableDelayedExpansion

:: Define the image name
set image_name=vbellos/invoice-api

echo Using image: %image_name%

:: Stop and remove existing containers
docker-compose down

:: Remove old images
docker-compose rm -f -v
for /f "tokens=*" %%i in ('docker images -q %image_name%') do docker rmi -f %%i

:: Build and start the containers
docker-compose build --no-cache 
docker-compose up -d  --remove-orphans

:end
endlocal