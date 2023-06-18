USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[Alc_BancoAlfa_sp_relatorioFilaDetalhada]    Script Date: 1/16/2023 2:25:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[JAVA_hiFilas] 
	-- Add the parameters for the stored procedure here
	@parUser VARCHAR(50) = ''
	,@dataInicial NVARCHAR(20) = '2021-01-14'
	,@dataFinal NVARCHAR(20) = '2021-01-15'
	,@parAplicacao INT = 1
	,@parSite INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE 
		@par_user VARCHAR(50) = @parUser
		,@data_inicio DATE = @dataInicial
		,@data_final DATE = @dataFinal
		,@par_aplicacao INT = @parAplicacao
		,@par_site INT = @parSite

	select  
		ch.sequencial as sequencial,
		ch.aplicacao as aplicacao,
		ch.data,
		replace(replace(ch.origem, '@10.10.5.240',''),'sip:','') as origem,
		ch.hora_inicio as hora_inicio_ura, 
		ch.hora_fim as hora_fim_ura,
		isnull(inb.fila,'') as fila, 
		isnull(inb.outFila,'') as hora_inicio_fila, 
		isnull(inb.inFila,'') as hora_fim_fila, 
		isnull(inb.processamento,'') as processamento, 
		isnull(inb.agente,'') as agente,
		isnull(inb.inicio,'') as inicio_atendimento, 
		isnull(inb.fim,'') as fim_atendimento
	from
		AlctelVSS.dbo.Alc_Chamadas as ch
	left join
		AlctelVSS.dbo.Alc_mtrInbound as inb
	on 
		inb.chamada = ch.sequencial
	where 
		convert(date,ch.data,111) between @data_inicio and @data_final
		--and ch.aplicacao = @aplicacoes
	order by
		ch.hora_inicio
END













GO


