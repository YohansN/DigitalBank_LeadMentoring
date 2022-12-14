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
