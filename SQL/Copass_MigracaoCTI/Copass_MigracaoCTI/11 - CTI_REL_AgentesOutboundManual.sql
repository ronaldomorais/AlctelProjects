USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[TESTE_CTI_REL_AgentesOutboundManual]    Script Date: 22/03/2023 11:58:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TESTE_CTI_REL_AgentesOutboundManual]   
	@dataInicial datetime = '2023-03-01',
	@dataFinal datetime = '2023-03-10',
	@site varchar(50) = '', 
	@parSite int = 1
AS 
BEGIN

	SELECT 
		cha_id 
		,age_fullname AS fullname
		,age_agentid AS agentId
		,CAST(cha_ini AS DATE) AS [data]
		,'Outbound Manual' as [nomeFila]
		,cha_destino AS destino
		,cha_ini
		,cha_fim
		,age_ini
		,age_fim
		,DATEDIFF(SECOND, cha_ini, cha_fim) AS dchamada
		,DATEDIFF(SECOND, age_ini, age_fim) AS dconversacao
		,CASE status_ligacao
			WHEN 'ATENDIDA' THEN 1
			ELSE 0
		END AS atendimento
	--INTO #tChamadas
	FROM AlctelVSS..ufn_GetChamadas (@dataInicial, @dataFinal, 0)
	WHERE
		cha_tipo_chamada = 'SAIDA'
		AND age_agentid <> '***'

END


GO


