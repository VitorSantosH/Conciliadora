Conciliadora - Sistema de Estacionamento
Sistema backend desenvolvido em .NET 8 com PostgreSQL e Entity Framework Core para gestão de estacionamento.

Funcionalidades
Cadastro de clientes (nome, telefone)

Cadastro de veículos (modelo, placa)

Associação de múltiplos veículos a um cliente

Controle de mensalistas

Geração simulada de faturas mensais

Upload de CSV para cadastro em massa de veículos

Tecnologias
ASP.NET Core 8

Entity Framework Core

PostgreSQL

Swagger

Execução
dotnet ef database update
dotnet run

Upload CSV - Exemplo
Placa,Modelo,Cliente
ABC1234,Onix,João Silva
XYZ9876,Corolla,Ana Souza

Decisões Técnicas
Entity Framework Core para ORM

API RESTful com Controllers

Upload CSV via StreamReader
