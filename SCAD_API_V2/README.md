# SCAD API v2

SCAD API v2 � uma API RESTful desenvolvida em ASP.NET Core (.NET 8) para gerenciar clientes, licen�as, usu�rios e v�nculos entre m�quinas e licen�as. A API � organizada em controllers, cada um respons�vel por uma entidade do dom�nio. Os endpoints s�o versionados e suportam multi-banco de dados via o par�metro de rota `{banco}`.

## Sum�rio

- [Controllers e Endpoints](#controllers-e-endpoints)
  - [ClienteController](#clientecontroller)
  - [LicencaController](#licencacontroller)
  - [UsuarioController](#usuariocontroller)
  - [VinculoController](#vinculocontroller)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Executar](#como-executar)
- [Swagger](#swagger)
- [Observa��es](#observa��es)

---

## Controllers e Endpoints

### ClienteController

**Rota Base:** `api/licenca/v2/{banco}/cliente`

| M�todo HTTP | Rota                              | Descri��o                                                                 |
|-------------|-----------------------------------|---------------------------------------------------------------------------|
| GET         | `/`                               | Lista todos os clientes.                                                  |
| GET         | `/{usuarioId:int}`                | Busca cliente pelo ID do usu�rio.                                         |
| GET         | `/cpf/{cpfCnpj}`                  | Busca cliente pelo CPF ou CNPJ.                                           |
| GET         | `/email/{email}`                  | Busca cliente pelo e-mail.                                                |
| GET         | `/nome/{nome}`                    | Busca clientes pelo nome.                                                 |
| GET         | `/telefone/{telefone}`            | Busca clientes pelo telefone.                                             |
| POST        | `/`                               | Cria um novo cliente. Retorna 409 se j� existir cliente com o CPF/CNPJ.   |
| PUT         | `/`                               | Atualiza um cliente existente. Retorna 404 se n�o encontrado.             |
| DELETE      | `/{usuarioId:int}`                | Remove um cliente pelo ID do usu�rio. Retorna 404 se n�o encontrado.      |

---

### LicencaController

**Rota Base:** `api/licenca/v2/{banco}/licenca`

| M�todo HTTP | Rota                              | Descri��o                                                                 |
|-------------|-----------------------------------|---------------------------------------------------------------------------|
| GET         | `/`                               | Lista todas as licen�as.                                                  |
| GET         | `/{licencaId:int}`                | Busca licen�a pelo ID.                                                    |
| GET         | `/key/{licencaKey}`               | Busca licen�a pela chave da licen�a.                                      |
| GET         | `/cliente/{clienteId:int}`        | Busca licen�as pelo ID do cliente.                                        |
| POST        | `/`                               | Cria uma nova licen�a.                                                    |
| PUT         | `/`                               | Atualiza uma licen�a existente. Retorna 404 se n�o encontrada.            |
| DELETE      | `/{licencaId:int}`                | Remove uma licen�a pelo ID. Retorna 404 se n�o encontrada.                |

---

### UsuarioController

**Rota Base:** `api/licenca/v2/usuario`

| M�todo HTTP | Rota                              | Descri��o                                                                 |
|-------------|-----------------------------------|---------------------------------------------------------------------------|
| GET         | `/`                               | Lista todos os usu�rios.                                                  |
| GET         | `/{usuarioId:int}`                | Busca usu�rio pelo ID.                                                    |
| GET         | `/email/{email}`                  | Busca usu�rio pelo e-mail.                                                |
| GET         | `/nome/{nome}`                    | Busca usu�rios pelo nome.                                                 |
| POST        | `/`                               | Cria um novo usu�rio.                                                     |
| PUT         | `/`                               | Atualiza um usu�rio existente. Retorna 404 se n�o encontrado.             |
| DELETE      | `/{usuarioId:int}`                | Remove um usu�rio pelo ID. Retorna 404 se n�o encontrado.                 |

---

### VinculoController

**Rota Base:** `api/licenca/v2/{banco}/vinculo`

| M�todo HTTP | Rota                                  | Descri��o                                                                 |
|-------------|---------------------------------------|---------------------------------------------------------------------------|
| GET         | `/`                                   | Lista todos os v�nculos (associa��es licen�a-m�quina).                    |
| GET         | `/{id:int}`                           | Busca v�nculo pelo ID.                                                    |
| GET         | `/licenca/{licenca}`                  | Busca v�nculo pela chave da licen�a.                                      |
| GET         | `/maquina/{maquina}/{softwareId:int}` | Busca v�nculo por m�quina e ID do software.                               |
| GET         | `/licencaId/{licencaId:int}`          | Busca v�nculo pelo ID da licen�a.                                         |
| GET         | `/nomeMaquina/{nomeMaquina}`          | Busca v�nculos pelo nome da m�quina.                                      |
| POST        | `/`                                   | Cria um novo v�nculo. Retorna 400 se a licen�a j� estiver vinculada.      |
| PUT         | `/`                                   | Atualiza um v�nculo existente. Retorna 404 se n�o encontrado.             |
| DELETE      | `/{id:int}`                           | Remove um v�nculo pelo ID. Retorna 404 se n�o encontrado.                 |

---


## Estrutura do Projeto
---

## Como Executar

1. **Pr�-requisitos:** .NET 8 SDK instalado.
2. **Configura��o:** Ajuste o `appsettings.json` conforme necess�rio para os bancos de dados.
3. **Build:** Execute `dotnet build` na raiz do projeto.
4. **Run:** Execute `dotnet run` para iniciar a API.
5. **Acesso:** A API estar� dispon�vel em `https://localhost:5001` (ou porta configurada).

---

## Swagger

A documenta��o interativa dos endpoints est� dispon�vel via Swagger em `/swagger` ap�s iniciar a aplica��o. Use para testar e explorar os endpoints.

---

## Observa��es

- O par�metro `{banco}` na rota permite selecionar o banco de dados de destino.
- Os endpoints retornam c�digos HTTP apropriados para sucesso e falha (404, 409, 400, etc).
- O projeto segue boas pr�ticas de versionamento e separa��o de responsabilidades.
- Para d�vidas ou contribui��es, abra uma issue ou pull request.

---
