# Teste Prático - Função Sistemas

Implementação do campo CPF no cadastro de clientes e do cadastro de beneficiários, sobre o projeto base fornecido pela Função Sistemas.

## Tecnologias

- ASP.NET MVC (.NET Framework 4.8)
- SQL Server Express LocalDB 2019
- jQuery / Bootstrap

## Como executar

1. Abrir `FI.WebAtividadeEntrevista.sln` no Visual Studio 2022
2. Restaurar os pacotes NuGet e recompilar a solução
3. Executar o projeto `FI.WebAtividadeEntrevista` (F5)

O banco (`App_Data/BancoDeDados.mdf`) já contém as alterações abaixo.

## Alterações no banco

```sql
ALTER TABLE CLIENTES
ADD CPF VARCHAR(14) NOT NULL;

ALTER TABLE BENEFICIARIOS
ALTER COLUMN CPF VARCHAR(14) NOT NULL;

CREATE UNIQUE INDEX UX_CLIENTES_CPF ON CLIENTES (CPF);
CREATE UNIQUE INDEX UX_BENEFICIARIOS_CLIENTE_CPF ON BENEFICIARIOS (IDCLIENTE, CPF);
```
Os índices únicos garantem no próprio banco que não haja CPF repetido entre clientes, nem beneficiários com o mesmo CPF para o mesmo cliente.

O CPF é gravado formatado (`000.000.000-00`). A coluna `CPF` de `BENEFICIARIOS` foi ajustada de 11 para 14 caracteres para manter o mesmo padrão de `CLIENTES`.

## Procedures alteradas

| Procedure | Alteração |
|---|---|
| `FI_SP_IncClienteV2` | Inclusão do parâmetro `@CPF` |
| `FI_SP_AltCliente` | Inclusão do parâmetro `@CPF` |
| `FI_SP_ConsCliente` | Retorno da coluna `CPF` |
| `FI_SP_VerificaCliente` | Novo parâmetro `@ID`, para ignorar o próprio cliente na alteração |
| `FI_SP_AltBenef` | Parâmetro `@CPF` ajustado para 14 caracteres |

## Procedures criadas

| Procedure | Função |
|---|---|
| `FI_SP_IncBenef` | Inclui um beneficiário e retorna o Id gerado |
| `FI_SP_ConsBenef` | Lista os beneficiários de um cliente |
| `FI_SP_DelBenef` | Exclui um beneficiário |
| `FI_SP_VerificaBenef` | Verifica se o CPF já está cadastrado como beneficiário do cliente (disponível, não utilizada no fluxo atual) |

Os scripts estão na pasta `Procedures` de cada entidade, usando `CREATE OR ALTER`.

## Regras implementadas

### Cliente

- CPF obrigatório, com máscara `999.999.999-99`
- Validação dos dígitos verificadores no backend (`BLL/ValidadorCpf.cs`), reaproveitada para clientes e beneficiários
- Dois clientes não podem ter o mesmo CPF; a verificação é feita no banco, pelo método `VerificarExistencia` do `BoCliente`

### Beneficiários

- Botão **Beneficiários** na tela de cliente, que abre um pop-up com CPF, nome e grid com as opções Alterar e Excluir
- CPF com máscara e validação dos dígitos verificadores
- O mesmo cliente não pode ter dois beneficiários com o mesmo CPF; a verificação é feita na lista, no JavaScript e no controller
- As inclusões, alterações e exclusões ficam em memória na tela e só são gravadas no banco ao clicar em **Salvar** no cadastro do cliente
- No servidor, a lista enviada é validada antes de gravar o cliente e comparada com a lista do banco: os beneficiários removidos são excluídos, os existentes são alterados e os novos são incluídos

## Estrutura

| Camada | Arquivos |
|---|---|
| DML | `Beneficiario.cs` |
| DAL | `Beneficiarios/DaoBeneficiario.cs` |
| BLL | `BoBeneficiario.cs`, `ValidadorCpf.cs` |
| Web | `BeneficiarioModel.cs`, `Views/Cliente/ModalBeneficiarios.cshtml`, `Scripts/Clientes/FI.Beneficiarios.js` |

## Observações

- O arquivo `FI_SP_IncCliente.sql` define a procedure `FI_SP_IncClienteV2`, que é a utilizada pelo `DaoCliente`.
- A procedure `FI_SP_IncCliente`, existente no banco, não é utilizada pelo sistema e foi mantida sem alteração.
- As procedures `FI_SP_VerificaCliente` e `FI_SP_AltBenef` existiam apenas no banco e foram adicionadas à solução por terem sido alteradas.
- A máscara de CPF utiliza o pacote NuGet Inputmask.
- Caso alterações de JavaScript não apareçam no navegador, limpe o cache (Ctrl+F5).
