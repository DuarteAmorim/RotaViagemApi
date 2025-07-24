@echo off
docker build -t simulador-rotas-voo:prod .
docker stop simulador-rotas-voo:prod
docker rm simulador-rotas-voo:prod
docker run -d -p 5000:80 --name simulador-rotas-voo-prod simulador-rotas-voo:prod
echo Aplicação rodando em http://localhost:5000/swagger
pause