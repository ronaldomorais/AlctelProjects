USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[TESTE_Alc_Paae_001_sp_hiChamadasSim]    Script Date: 23/03/2023 16:28:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[TESTE_Alc_Paae_001_sp_hiChamadasSim]   
	@dataInicial datetime = '2023-02-15',  
	@dataFinal datetime   = '2023-02-15',   
	@parSite int = 0
AS 
BEGIN

	DECLARE @site_id INT = @parSite
	
	SET @site_id = 0 --Para trazer todas as informaçoes no relatorios de chamadas

	SELECT 
		cha_id
		,cha_id AS callid
		,cha_origem
		,'' AS calldnis
		,cha_ini AS cha_data
		,cha_fim AS FIM
		,cha_fila
		,CASE WHEN cha_inFila IS NULL THEN cha_ini ELSE cha_inFila END AS cha_inFila
		,CASE WHEN cha_outFila IS NULL THEN cha_fim ELSE cha_outFila END AS cha_outFila
		,0 AS [dbid]
		,ISNULL(age_agentid, '') AS agente
		,age_fullname
		,ISNULL(age_ini, cha_ini) AS age_inicio
		,ISNULL(age_fim, cha_ini) AS age_fim
		,CASE WHEN evt_protocolo = 'Cliente_nao_foi_identificado' THEN '' ELSE ISNULL(evt_protocolo, '') END AS protocolo
		,CASE WHEN evt_transferida = 0 THEN 'NAO' ELSE 'SIM' END AS transferenciaaux
		,ISNULL(DATEDIFF(SECOND, age_ini, age_fim), 0) AS age_duracao
		,ISNULL(DATEDIFF(SECOND, cha_inFila, cha_outFila), 0) AS tfila
		,status_ligacao AS abandonada
		,CASE WHEN cha_tipo_chamada = 'SAIDA' AND status_ligacao = 'NAO_ATENDIDA' THEN 'SIM' ELSE '' END AS transferida
		,evt_desconexao AS desconexao
		,CASE WHEN cha_tipo_chamada = 'SAIDA' 
			THEN '' 
			ELSE 
				CASE WHEN evt_transferida = 1
					THEN 'SIM'
					ELSE ''
				END
		END AS naoatendida
		,cha_destino AS aplicacao
		,CASE WHEN evt_protocolo = 'Cliente_nao_foi_identificado' THEN '' ELSE ISNULL(evt_protocolo, '') END AS cpf
		,CASE WHEN cha_tipo_chamada = 'SAIDA' THEN cha_destino ELSE '' END AS destino_outbound
		,evt_opcoes_ura AS opcao_ura
	FROM AlctelVSS..ufn_GetChamadas (@dataInicial, @dataFinal, @site_id)
	ORDER BY cha_id, cha_ini

END





GO


