# Digital Bank Api
## Projeto Final - Mentoria Lead C#
**Resumo do projeto**: Com a finalidade de praticar os conhecimentos aprendidos e desenvolvidos durante a **mentoria de C# da [Lead Dell](https://leadfortaleza.com.br/portal)**, este projeto tem por intuito representar uma API de um banco digital por meio das funcionalidades implementadas no mesmo.
 
## Digital Bank API tem as seguintes funcionalidades:

### Cliente:
- Cadastro de cliente.
- Atualização de dados de cliente.
- Listagem de todos os clientes cadastrados.
- Busca por cliente específico.
- Apagamento de um cliente.

 ### Conta Bancária:
 - Criação de conta bancária.
 - Listagem de todas as contas bancárias cadastradas.
 - Busca por conta bancária específica.
 - Depósito em  uma conta bancária.
 - Débito em uma conta bancária.
 - Transferência de uma conta bancaria para outra.
 - Visualização de extrato bancário de uma conta.
 - Apagamento de conta bancária.
 
 ## Regras e limitações:
 1. Cada cliente pode ter apenas uma conta bancária cadastrada.
 2. O cliente deve ser maior de idade.
 3. Não deverá ser permitido Débitos ou Transferências que ultrapassem o valor do saldo atual da conta bancária.

## Conhecimentos utilizados no projeto:
- Estruturação de uma Web API e suas camadas.
- Programação Orientada a Objetos (POO).
- Injeção de dependência.
- Boas práticas.
- Fundamentos de banco de dados.
- Swagger UI.
- Documentação XML.
- Rotas e Requisições HTTP.
- Testes unitários.

## Configuração das entidades no banco de dados:
Para rodar o projeto você precisa ter as mesmas configurações de banco de dados **(*SQL Server*)**. Para gerar um banco de dados com o setup necessário para o código basta usar esse schema:
~~~sql
USE [DigitalBankApi]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 19/11/2022 22:39:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Cpf] [varchar](100) NOT NULL,
	[Idade] [int] NOT NULL,
 CONSTRAINT [pk_Cliente_ID] PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContaBancaria]    Script Date: 19/11/2022 22:39:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContaBancaria](
	[NumeroConta] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[Saldo] [decimal](18, 0) NOT NULL,
 CONSTRAINT [pk_NumeroConta_ID] PRIMARY KEY CLUSTERED 
(
	[NumeroConta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transacao]    Script Date: 19/11/2022 22:39:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transacao](
	[IdTransacao] [int] IDENTITY(1,1) NOT NULL,
	[NumeroConta] [int] NOT NULL,
	[_TipoTransacao] [int] NOT NULL,
	[ValorTransacao] [decimal](18, 0) NOT NULL,
	[DataTransacao] [datetime] NOT NULL,
 CONSTRAINT [pk_Transacao_ID] PRIMARY KEY CLUSTERED 
(
	[IdTransacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContaBancaria]  WITH CHECK ADD  CONSTRAINT [fk_ContaBancaria_IdCliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Cliente] ([IdCliente])
GO
ALTER TABLE [dbo].[ContaBancaria] CHECK CONSTRAINT [fk_ContaBancaria_IdCliente]
GO
ALTER TABLE [dbo].[Transacao]  WITH CHECK ADD  CONSTRAINT [fk_Transacao_NumeroConta] FOREIGN KEY([NumeroConta])
REFERENCES [dbo].[ContaBancaria] ([NumeroConta])
GO
ALTER TABLE [dbo].[Transacao] CHECK CONSTRAINT [fk_Transacao_NumeroConta]
GO
~~~
O schema também está na basta **DbSchema** do repositório.

Além disso, também é necessário que modifique a **SOURCE** da conexão do banco de dados em **DigitalBankApi/Data/Context.cs**
```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=SuaConexaoComOBancoDeDadosAqui;Initial Catalog=DigitalBankApi;Integrated Security=True");
        }
```