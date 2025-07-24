@echo off
docker build -t simulador-rotas-voo .
docker stop simulador-rotas-voo
docker rm simulador-rotas-voo
docker run -d -p 8080:80 --name simulador-rotas-voo simulador-rotas-voo
echo Aplicação rodando em http://localhost:8080/swagger
pause