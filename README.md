# API Hospedagem

API para gerenciamento de hospedagens, reservas e clientes. Ideal para hotéis, pousadas ou sistemas de aluguel de imóveis por temporada.

## Funcionalidades

- Cadastro, consulta, atualização e remoção de hóspedes
- Registro e controle de reservas
- Gerenciamento de quartos ou unidades disponíveis
- Consulta de disponibilidade por datas
- Histórico de reservas por cliente
- Integração com sistemas de pagamento (opcional)
- Documentação interativa via Swagger

## Tecnologias Utilizadas

- **.NET 6.0 (ASP.NET Core)**
- **Entity Framework Core 6** (com suporte a SQL Server)
- **AutoMapper**
- **Swashbuckle.AspNetCore** (Swagger)
- **Docker** (containerização)
- **Microsoft Visual Studio** (opcional, integração facilitada)
- **Testes:** xUnit, Moq, FluentAssertions, Microsoft.NET.Test.Sdk, EntityFrameworkCore.InMemory, Coverlet Collect
- **Banco de dados:** SQL Server
- **Gerenciamento de dependências:** NuGet

## Instalação e Execução Local

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/JonFalleNBR/api-hospedagem.git
   cd api-hospedagem/API.Hospedagem
   ```

2. **Configuração do Banco de Dados:**
   - Edite a string de conexão no arquivo de variáveis de ambiente ou utilize a padrão do Docker:
     ```
     DB_CONNECTION="Server=host.docker.internal;Database=HotelDB;User=sa;Password=SENHA;"
     ```
   - Altere a senha conforme necessário.

3. **Executando via Docker:**
   ```bash
   docker build -t apihospedagem .
   docker run -p 80:80 -p 443:443 --env DB_CONNECTION="..." apihospedagem
   ```

4. **Ou execução local (requer .NET 6 SDK):**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

5. **Acesse a documentação Swagger:**  
   [http://localhost:80/swagger](http://localhost:80/swagger) ou conforme porta configurada.

## Endpoints Principais

| Método | Rota         | Descrição                |
|--------|--------------|--------------------------|
| GET    | /hospedes    | Lista todos os hóspedes  |
| POST   | /hospedes    | Cria um novo hóspede     |
| GET    | /reservas    | Lista todas as reservas  |
| POST   | /reservas    | Cria uma nova reserva    |
| ...    | ...          | ...                      |

> Veja a documentação Swagger para detalhes completos e exemplos.

## Testes

- Os testes automatizados utilizam xUnit, Moq, FluentAssertions e Coverlet para cobertura.
- Para rodar os testes:
  ```bash
  dotnet test
  ```

## Como contribuir

1. Faça um fork do projeto.
2. Crie uma branch: `git checkout -b minha-feature`
3. Faça commit das alterações: `git commit -m 'Minha feature'`
4. Envie a branch: `git push origin minha-feature`
5. Abra um Pull Request.

## Licença

MIT

## Autor

- [JonFalleNBR](https://github.com/JonFalleNBR)
