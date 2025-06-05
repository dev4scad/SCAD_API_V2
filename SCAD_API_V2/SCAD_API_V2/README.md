# SCAD API v2

SCAD API v2 é uma API RESTful desenvolvida em ASP.NET Core (.NET 8) para gerenciar clientes, licenças, usuários e vínculos entre máquinas e licenças. A API é organizada em controllers, cada um responsável por uma entidade do domínio. Os endpoints são versionados e suportam multi-banco de dados via o parâmetro de rota `{banco}`.

## Sumário

- [Controllers e Endpoints](#controllers-e-endpoints)
  - [ClienteController](#clientecontroller)
  - [LicencaController](#licencacontroller)
  - [UsuarioController](#usuariocontroller)
  - [VinculoController](#vinculocontroller)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Executar](#como-executar)
- [Swagger](#swagger)
- [Observações](#observações)

---

## Controllers e Endpoints

### ClienteController

**Rota Base:** `api/licenca/v2/{banco}/cliente`

| Método HTTP | Rota                              | Descrição                                                                 |
|-------------|-----------------------------------|---------------------------------------------------------------------------|
| GET         | `/`                               | Lista todos os clientes.                                                  |
| GET         | `/{usuarioId:int}`                | Busca cliente pelo ID do usuário.                                         |
| GET         | `/cpf/{cpfCnpj}`                  | Busca cliente pelo CPF ou CNPJ.                                           |
| GET         | `/email/{email}`                  | Busca cliente pelo e-mail.                                                |
| GET         | `/nome/{nome}`                    | Busca clientes pelo nome.                                                 |
| GET         | `/telefone/{telefone}`            | Busca clientes pelo telefone.                                             |
| POST        | `/`                               | Cria um novo cliente. Retorna 409 se já existir cliente com o CPF/CNPJ.   |
| PUT         | `/`                               | Atualiza um cliente existente. Retorna 404 se não encontrado.             |
| DELETE      | `/{usuarioId:int}`                | Remove um cliente pelo ID do usuário. Retorna 404 se não encontrado.      |

---

### LicencaController

**Rota Base:** `api/licenca/v2/{banco}/licenca`

| Método HTTP | Rota                              | Descrição                                                                 |
|-------------|-----------------------------------|---------------------------------------------------------------------------|
| GET         | `/`                               | Lista todas as licenças.                                                  |
| GET         | `/{licencaId:int}`                | Busca licença pelo ID.                                                    |
| GET         | `/key/{licencaKey}`               | Busca licença pela chave da licença.                                      |
| GET         | `/cliente/{clienteId:int}`        | Busca licenças pelo ID do cliente.                                        |
| POST        | `/`                               | Cria uma nova licença.                                                    |
| PUT         | `/`                               | Atualiza uma licença existente. Retorna 404 se não encontrada.            |
| DELETE      | `/{licencaId:int}`                | Remove uma licença pelo ID. Retorna 404 se não encontrada.                |

---

### UsuarioController

**Rota Base:** `api/licenca/v2/usuario`

| Método HTTP | Rota                              | Descrição                                                                 |
|-------------|-----------------------------------|---------------------------------------------------------------------------|
| GET         | `/`                               | Lista todos os usuários.                                                  |
| GET         | `/{usuarioId:int}`                | Busca usuário pelo ID.                                                    |
| GET         | `/email/{email}`                  | Busca usuário pelo e-mail.                                                |
| GET         | `/nome/{nome}`                    | Busca usuários pelo nome.                                                 |
| POST        | `/`                               | Cria um novo usuário.                                                     |
| PUT         | `/`                               | Atualiza um usuário existente. Retorna 404 se não encontrado.             |
| DELETE      | `/{usuarioId:int}`                | Remove um usuário pelo ID. Retorna 404 se não encontrado.                 |

---

### VinculoController

**Rota Base:** `api/licenca/v2/{banco}/vinculo`

| Método HTTP | Rota                                  | Descrição                                                                 |
|-------------|---------------------------------------|---------------------------------------------------------------------------|
| GET         | `/`                                   | Lista todos os vínculos (associações licença-máquina).                    |
| GET         | `/{id:int}`                           | Busca vínculo pelo ID.                                                    |
| GET         | `/licenca/{licenca}`                  | Busca vínculo pela chave da licença.                                      |
| GET         | `/maquina/{maquina}/{softwareId:int}` | Busca vínculo por máquina e ID do software.                               |
| GET         | `/licencaId/{licencaId:int}`          | Busca vínculo pelo ID da licença.                                         |
| GET         | `/nomeMaquina/{nomeMaquina}`          | Busca vínculos pelo nome da máquina.                                      |
| POST        | `/`                                   | Cria um novo vínculo. Retorna 400 se a licença já estiver vinculada.      |
| PUT         | `/`                                   | Atualiza um vínculo existente. Retorna 404 se não encontrado.             |
| DELETE      | `/{id:int}`                           | Remove um vínculo pelo ID. Retorna 404 se não encontrado.                 |

---


## Estrutura do Projeto
---

## Como Executar

1. **Pré-requisitos:** .NET 8 SDK instalado.
2. **Configuração:** Ajuste o `appsettings.json` conforme necessário para os bancos de dados.
3. **Build:** Execute `dotnet build` na raiz do projeto.
4. **Run:** Execute `dotnet run` para iniciar a API.
5. **Acesso:** A API estará disponível em `https://localhost:5001` (ou porta configurada).

---

## Swagger

A documentação interativa dos endpoints está disponível via Swagger em `/swagger` após iniciar a aplicação. Use para testar e explorar os endpoints.

---

## Observações

- O parâmetro `{banco}` na rota permite selecionar o banco de dados de destino.
- Os endpoints retornam códigos HTTP apropriados para sucesso e falha (404, 409, 400, etc).
- O projeto segue boas práticas de versionamento e separação de responsabilidades.
- Para dúvidas ou contribuições, abra uma issue ou pull request.

---
