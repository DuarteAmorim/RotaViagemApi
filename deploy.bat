@echo off
docker build -t simulador-rotas-voo .
docker stop simulador-rotas-voo
docker rm simulador-rotas-voo
docker run -d -p 5000:80 --name simulador-rotas-voo simulador-rotas-voo
echo Aplicação rodando em http://localhost:5000/swagger
pause