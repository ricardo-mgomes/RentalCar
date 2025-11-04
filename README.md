# RentalCar
Este é um sistema de gestão de aluguer de veículos desenvolvido em ASP.NET Core com Entity Framework Core.  
Permite gerir clientes, veículos e contratos de aluguer.

Alterar Credenciais para as credenciais do server SQL no ficheiro appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=SERVER;Database=RentalCarDB;User Id=USERID;Password=PASSWORD;Encrypt=False;TrustServerCertificate=True;"
}

dotnet restore

dotnet tool install --global dotnet-ef

dotnet ef database update

dotnet run
