USE [AlctelVSS]
GO

/****** Object:  UserDefinedFunction [dbo].[ufn_GetChamadas]    Script Date: 23/03/2023 16:28:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO











CREATE FUNCTION [dbo].[ufn_GetChamadas] (
	@data_inicio DATETIME = '2023-02-15'
	,@data_fim DATETIME   = '2023-02-15'
	,@site_id INT = 0
) 
RETURNS @tChamadas TABLE (
	cha_id VARCHAR(50)
	,cha_ini DATETIME
	,cha_fim DATETIME
	,cha_tipo_chamada VARCHAR(10)
	,cha_origem VARCHAR(50)
	,cha_fila VARCHAR(100)
	,fila_dbid INT
	,cha_inFila DATETIME
	,cha_outFila DATETIME
	,status_ligacao VARCHAR(20)
	,age_fullname VARCHAR(255)
	,age_agentid VARCHAR(100)
	,age_dbid NUMERIC(10,0)
	,age_extensao VARCHAR(50)
	,age_ini DATETIME
	,age_fim DATETIME
	,age_nao_atende INT
	,evt_transferida INT
	,cha_destino VARCHAR(50)
	,evt_protocolo VARCHAR(100)
	,evt_opcoes_ura VARCHAR(MAX)
	,evt_desconexao VARCHAR(10)
	,[site] VARCHAR(50)
)
AS
BEGIN
	DECLARE
		@_data_inicio DATETIME = CAST(@data_inicio AS DATE)
		,@_data_fim DATETIME = CAST(@data_fim AS DATE)
		,@_site_id INT = @site_id
		,@CHAMADAS_ENTRADA INT = 2
		,@CHAMADAS_SAIDA INT = 3

	SET @_data_fim = DATEADD(SECOND, 86399, @_data_fim)

	--DECLARE @tChamadasUra TABLE (
	--	cha_id VARCHAR(50)
	--	,call_type INT
	--	,cha_origem VARCHAR(50)
	--	,cha_destino VARCHAR(20)
	--	,cha_ini DATETIME
	--	,cha_fim DATETIME
	--	,evt_desconexao INT
	--	,evt_abandono INT
	--)

	--DECLARE @tChamadasResultadoFinal TABLE (
	--	cha_id VARCHAR(50)
	--	,[data] DATETIME
	--	,cha_tipo_chamada VARCHAR(10)
	--	,cha_origem VARCHAR(50)
	--	,cha_fila VARCHAR(100)
	--	,cha_inFila DATETIME
	--	,cha_outFila DATETIME
	--	,status_ligacao VARCHAR(15)
	--	,age_fullname VARCHAR(255)
	--	,age_agentid VARCHAR(100)
	--	,age_extensao VARCHAR(50)
	--	,age_ini DATETIME
	--	,age_fim DATETIME
	--	,age_nao_atende INT
	--	,evt_transferida INT
	--	,cha_destino VARCHAR(50)
	--	,evt_protocolo VARCHAR(100)
	--	,evt_opcoes_ura VARCHAR(MAX)
	--)

	DECLARE @tChamadasFila TABLE (
		cha_id VARCHAR(50)
		,cha_origem VARCHAR(50)
		,cha_fila VARCHAR(100)
		,cha_inFila DATETIME
		,cha_outFila DATETIME
		,evt_number INT
	)

	DECLARE @tChamdasGeral TABLE (
		cha_id VARCHAR(50)
		,cha_ini DATETIME
		,cha_fim DATETIME
		,call_type INT
		,cha_origem VARCHAR(50)
		,cha_destino VARCHAR(20)
		,age_agentid VARCHAR(100)
		,age_extensao VARCHAR(50)
		,age_ini DATETIME
		,age_fim DATETIME
		,evt_desconexao INT
		,evt_abandono INT
		,evt_number INT
	)

	DECLARE @tChamadasGeralConversacao TABLE (
		cha_id VARCHAR(50)
		,cha_destino VARCHAR(20)
		,age_agentid VARCHAR(100)
		,age_extensao VARCHAR(50)
		,age_ini DATETIME
		,age_fim DATETIME
		,evt_transferencia INT
		,evt_number INT		
	)

	DECLARE @tChamadasGeralNaoAtende TABLE (
		cha_id VARCHAR(50)
		,age_agentid VARCHAR(100)
		,age_extensao VARCHAR(50)
		,age_ini DATETIME
		,age_fim DATETIME
		,evt_number INT
	)

	DECLARE @tChamadasProcessadas TABLE (
		cha_id VARCHAR(50)
		,cha_origem VARCHAR(50)
		,cha_fila VARCHAR(100)
		,cha_inFila DATETIME
		,cha_outFila DATETIME 
		,age_agentid VARCHAR(100)
		,age_extensao VARCHAR(50)
		,age_ini DATETIME
		,age_fim DATETIME
		,age_nao_atende BIT
		,evt_transferida BIT 
		,cha_destino VARCHAR(20)
		,cha_evento_hora DATETIME
		,cha_evento_nome VARCHAR(50)
		,cha_remove_evento BIT
	)

	DECLARE @tSiteFilaRelacionamento TABLE (fila VARCHAR(50), fila_dbid INT, [site] VARCHAR(50), site_id INT)
		
	--INSERT INTO @tChamadasUra
	--SELECT 
	--	cha_id
	--	,call_type
	--	,cha_origem
	--	,cha_destino
	--	,cha_ini
	--	,cha_fim
	--	,evt_desconexao
	--	,evt_abandonado
	--FROM AlctelSRVCTI.dbo.Alc_chamadasGeralUra WITH (NOLOCK)
	--WHERE
	--	cha_ini BETWEEN @_data_inicio AND @_data_fim

	INSERT INTO @tChamadasFila
	SELECT 
		cha_id
		,cha_origem
		,cha_fila
		,cha_inFila
		,cha_outFila
		,evt_number
	FROM AlctelSRVCTI.dbo.Alc_chamadasGeralFilas WITH (NOLOCK)
	WHERE
		cha_inFila BETWEEN @_data_inicio AND @_data_fim

	INSERT INTO @tChamdasGeral
	SELECT 
		cha_id
		,cha_ini
		,cha_fim
		,call_type
		,cha_origem
		,cha_destino
		,age_agentid
		,age_extensao
		,age_ini
		,age_fim
		,evt_desconexao
		,evt_abandonado
		,evt_number
	FROM AlctelSRVCTI.dbo.Alc_chamadasGeral WITH (NOLOCK)
	WHERE
		cha_ini BETWEEN @_data_inicio AND @_data_fim

	INSERT INTO @tChamadasGeralConversacao
	SELECT 
		cha_id
		,cha_destino
		,age_agentid
		,age_extensao
		,age_ini
		,age_fim
		,evt_transferencia
		,evt_number
	FROM AlctelSRVCTI.dbo.Alc_chamadasGeralConversacao WITH (NOLOCK)
	WHERE
		age_ini BETWEEN @_data_inicio AND @_data_fim

	INSERT INTO @tChamadasGeralNaoAtende
	SELECT 
		cha_id
		,age_agentid
		,age_extensao
		,age_ini
		,age_fim
		,evt_number
	FROM AlctelSRVCTI.dbo.Alc_chamadasGeralNaoAtende WITH (NOLOCK)
	WHERE
		age_ini BETWEEN @_data_inicio AND @_data_fim		


--Treho acrescido para remover chamadas que foram identificadas como transferencias para o mesmo agente
--Deve ser corrigdo no processamento do CTI: 006D034A877E3DD6


	DECLARE @tVerificaTransferidas TABLE (
		cha_id VARCHAR(50)
		,age_ini DATETIME		
		,age_agentid VARCHAR(100)
		,transferDe VARCHAR(100)
	)

	INSERT INTO @tVerificaTransferidas
	SELECT 
		cha_id
		,age_ini
		,age_agentid
		,LAG(age_agentid, 1, '') OVER(PARTITION BY cha_id ORDER BY age_ini) AS transferDe
	FROM (

		SELECT 
			cha_id
			,age_ini
			,age_agentid
		FROM @tChamdasGeral

		UNION 

		SELECT 
			cha_id
			,age_ini
			,age_agentid
		FROM @tChamadasGeralConversacao
		WHERE
			evt_transferencia = 1

	) AS S
	--WHERE
	--	cha_id = '006D034A877E3DD6'
	ORDER BY S.cha_id, S.age_agentid

	DELETE C
	FROM @tVerificaTransferidas AS T
		INNER JOIN @tChamadasGeralConversacao AS C ON T.cha_id = C.cha_id AND T.age_agentid = C.age_agentid
	WHERE
		T.age_agentid = transferDe

--Treho acrescido para remover chamadas que foram identificadas como transferencias para o mesmo agente	

	INSERT INTO @tChamadasProcessadas
	SELECT 
		S.cha_id
		,S.cha_origem
		,S.cha_fila
		,S.cha_inFila
		,S.cha_outFila
		--,S.age_agentid
		--,S.age_extensao
		--,S.age_ini
		--,S.age_fim
		,CASE S.cha_evento_nome
			WHEN '1 - FILA'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '2 - AGENTE_NAO_ATENDE' THEN LEAD(S.age_agentid, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						WHEN '3 - AGENTE_ATENDE' THEN LEAD(S.age_agentid, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						WHEN '4 - TRANSFER' THEN LEAD(S.age_agentid, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
					END
			WHEN '3 - AGENTE_ATENDE'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN LEAD(S.age_agentid, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						ELSE 
							CASE LAG(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
								WHEN '2 - AGENTE_NAO_ATENDE' THEN S.age_agentid
							END
					END	
			WHEN '4 - TRANSFER'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN LEAD(S.age_agentid, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
					END	
		END AS age_agentid
		,CASE S.cha_evento_nome
			WHEN '1 - FILA'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '2 - AGENTE_NAO_ATENDE' THEN LEAD(S.age_extensao, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						WHEN '3 - AGENTE_ATENDE' THEN LEAD(S.age_extensao, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						WHEN '4 - TRANSFER' THEN LEAD(S.age_extensao, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
					END
			WHEN '3 - AGENTE_ATENDE'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN LEAD(S.age_extensao, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						ELSE 
							CASE LAG(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
								WHEN '2 - AGENTE_NAO_ATENDE' THEN S.age_extensao
							END
					END	
			WHEN '4 - TRANSFER'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN LEAD(S.age_extensao, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
					END	
		END AS age_extensao
		,CASE S.cha_evento_nome
			WHEN '1 - FILA'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '2 - AGENTE_NAO_ATENDE' THEN LEAD(S.age_ini, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						WHEN '3 - AGENTE_ATENDE' THEN LEAD(S.age_ini, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						WHEN '4 - TRANSFER' THEN LEAD(S.age_ini, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
					END
			WHEN '3 - AGENTE_ATENDE'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN LEAD(S.age_ini, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						ELSE 
							CASE LAG(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
								WHEN '2 - AGENTE_NAO_ATENDE' THEN S.age_ini
							END
					END	
			WHEN '4 - TRANSFER'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN LEAD(S.age_ini, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
					END	
		END AS age_ini
		,CASE S.cha_evento_nome
			WHEN '1 - FILA'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '2 - AGENTE_NAO_ATENDE' THEN LEAD(S.age_fim, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						WHEN '3 - AGENTE_ATENDE' THEN LEAD(S.age_fim, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						WHEN '4 - TRANSFER' THEN LEAD(S.age_fim, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
					END
			WHEN '3 - AGENTE_ATENDE'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN LEAD(S.age_fim, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
						ELSE 
							CASE LAG(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
								WHEN '2 - AGENTE_NAO_ATENDE' THEN S.age_fim
							END
					END	
			WHEN '4 - TRANSFER'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN LEAD(S.age_fim, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome) 
					END	
		END AS age_fim
		,CASE S.cha_evento_nome
			WHEN '1 - FILA'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '2 - AGENTE_NAO_ATENDE' THEN 1
						WHEN '3 - AGENTE_ATENDE' THEN 0
						WHEN '4 - TRANSFER' THEN 0
						ELSE 0
					END
			WHEN '3 - AGENTE_ATENDE' THEN 0
			WHEN '4 - TRANSFER' THEN 0
		END AS age_nao_atende		
		,CASE S.cha_evento_nome
			WHEN '1 - FILA'
				THEN 
					CASE LEAD(S.cha_evento_nome, 2, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN 1
						WHEN '1 - FILA' 
							THEN
								CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
									WHEN '2 - AGENTE_NAO_ATENDE' THEN 0
									ELSE 1
								END
						ELSE 0
					END
			--WHEN '3 - AGENTE_ATENDE'
			--	THEN 
			--		CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
			--			WHEN '4 - TRANSFER' THEN 1
			--			ELSE 0
			--		END	
			WHEN '4 - TRANSFER'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN 1
					END	
		END AS evt_transferida
		,S.cha_destino
		,S.cha_evento_hora
		,S.cha_evento_nome		
		,CASE S.cha_evento_nome
			WHEN '1 - FILA' THEN 0
			WHEN '2 - AGENTE_NAO_ATENDE' THEN 1
			WHEN '3 - AGENTE_ATENDE'
				THEN
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN 0						
						ELSE 
							CASE LAG(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
								WHEN '2 - AGENTE_NAO_ATENDE' THEN 0
								ELSE 1
							END
					END
			WHEN '4 - TRANSFER'
				THEN
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN 0
						ELSE 1
					END
		END AS cha_remove_evento
	FROM (

		SELECT 
			cha_id
			,cha_origem
			,cha_fila
			,cha_inFila
			,cha_outFila
			,'' AS age_agentid
			,'' AS age_extensao
			,NULL AS age_ini
			,NULL AS age_fim	
			,'' AS cha_destino
			,cha_inFila AS cha_evento_hora
			,'1 - FILA' AS cha_evento_nome
		FROM @tChamadasFila

		UNION ALL

		SELECT 
			cha_id
			,'' AS cha_origem
			,'' AS cha_fila
			,NULL AS cha_inFila
			,NULL AS cha_outFila
			,age_agentid
			,age_extensao
			,age_ini
			,age_fim
			,'' AS cha_destino
			,age_ini AS cha_evento_hora
			,'2 - AGENTE_NAO_ATENDE' AS cha_evento_nome
		FROM @tChamadasGeralNaoAtende

		UNION ALL

		SELECT 
			cha_id
			,cha_origem
			,'' AS cha_fila
			,NULL AS cha_inFila
			,NULL AS cha_outFila
			,age_agentid
			,age_extensao
			,age_ini
			,age_fim
			,cha_destino
			,age_ini AS cha_evento_hora
			,'3 - AGENTE_ATENDE' AS cha_evento_nome
		FROM @tChamdasGeral
		WHERE
			call_type = @CHAMADAS_ENTRADA

		UNION ALL

		SELECT 
			cha_id
			,'' AS cha_origem
			,'' AS cha_fila
			,NULL AS cha_inFila
			,NULL AS cha_outFila
			,age_agentid
			,age_extensao
			,age_ini
			,age_fim
			,cha_destino
			,age_ini AS cha_evento_hora
			,'4 - TRANSFER' AS cha_evento_nome
		FROM @tChamadasGeralConversacao
		WHERE
			evt_transferencia = 1

	) AS S
	--ORDER BY S.cha_evento_hora, S.cha_seq, S.cha_evento_nome
	ORDER BY cha_id, S.cha_evento_hora, cha_evento_nome

	INSERT INTO @tSiteFilaRelacionamento
	SELECT 
		F.fila
		,F.[dbid]
		,S.[site]
		,S.sequencial AS site_id
	FROM AlctelVSS..Alc_mtr001Filas AS F WITH (NOLOCK)
		LEFT JOIN AlctelVSS..Alc_mtr001Sites AS S WITH (NOLOCK) ON F.[site] = S.[site]
	
	INSERT INTO @tChamadas
	SELECT 
		S.cha_id
		--,CASE WHEN S.cha_tipo_chamada = 'ENTRADA' THEN S.cha_inFila ELSE S.age_ini END [data]
		,CASE S.status_ligacao 
			WHEN 'ABANDONADA_URA' THEN S.cha_inFila
			WHEN 'ABANDONADA' THEN S.cha_inFila
			ELSE G.cha_ini
		END AS cha_ini
		,CASE S.status_ligacao 
			WHEN 'ABANDONADA_URA' THEN S.cha_outFila
			WHEN 'ABANDONADA' THEN S.cha_outFila
			ELSE G.cha_fim
		END AS cha_fim		
		--,ISNULL(G.cha_ini, S.cha_inFila) AS [data]
		,S.cha_tipo_chamada
		,CASE WHEN S.cha_origem = '***' THEN 'numero oculto' ELSE S.cha_origem END AS cha_origem
		,S.cha_fila
		,R.fila_dbid
		,CASE WHEN S.status_ligacao = 'ABANDONADA_URA' THEN NULL ELSE S.cha_inFila END AS cha_inFila
		,CASE WHEN S.status_ligacao = 'ABANDONADA_URA' THEN NULL ELSE CASE WHEN S.cha_inFila > S.cha_outFila THEN S.cha_inFila ELSE S.cha_outFila END END AS cha_outFila 
		,S.status_ligacao
		,S.age_fullname
		,S.age_agentid
		,S.[dbid] AS age_dbid
		,S.age_extensao
		,S.age_ini
		--,S.age_fim
		,CASE WHEN S.age_ini > S.age_fim THEN S.age_ini ELSE S.age_fim END AS age_fim
		,S.age_nao_atende
		,S.evt_transferida
		,CASE WHEN S.cha_destino = '***' THEN 'numero oculto' ELSE S.cha_destino END AS cha_destino
		--,S.cha_evento_hora
		,ISNULL(P.evt_protocolo, LAG(P.evt_protocolo, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_seq)) AS evt_protocolo
		,ISNULL(O.cha_opcoes_ura, '') AS cha_opcoes_ura
		,CASE WHEN ISNULL(G.evt_desconexao, 1) = 1 THEN	'CLIENTE' ELSE 'ATENDENTE' END AS desconexao
		,ISNULL(R.[site], '') AS [site]
		--,S.cha_seq
	FROM (

		SELECT 
			C.cha_id
			,'ENTRADA' AS cha_tipo_chamada
			,C.cha_origem
			--,cha_fila
			,CASE WHEN C.cha_evento_nome = '3 - AGENTE_ATENDE' AND C.cha_fila = ''
				THEN LAG(C.cha_fila, 1, '') OVER(PARTITION BY C.cha_id ORDER BY C.cha_evento_hora)
				ELSE C.cha_fila
			END AS cha_fila
			--,cha_inFila
			,CASE WHEN C.cha_evento_nome = '3 - AGENTE_ATENDE' AND C.cha_inFila IS NULL
				THEN LAG(C.cha_inFila, 1, '') OVER(PARTITION BY C.cha_id ORDER BY C.cha_evento_hora)
				ELSE C.cha_inFila
			END AS cha_inFila
			--,cha_outFila
			,CASE WHEN C.cha_evento_nome = '3 - AGENTE_ATENDE' AND C.cha_inFila IS NULL
				THEN LAG(C.cha_outFila, 1, '') OVER(PARTITION BY C.cha_id ORDER BY C.cha_evento_hora)
				ELSE C.cha_outFila
			END AS cha_outFila
			,CASE WHEN C.age_agentid IS NULL THEN 'ABANDONADA' ELSE CASE WHEN age_nao_atende = 0 THEN 'ATENDIDA' ELSE 'NAO_ATENDIDA' END END AS status_ligacao
			,CONCAT(A.first_name, ' ', A.last_name) AS age_fullname
			,C.age_agentid
			,A.[dbid]
			,C.age_extensao
			,C.age_ini
			,C.age_fim
			,C.age_nao_atende
			,C.evt_transferida
			,C.cha_destino
			--,C.cha_evento_hora
			,ROW_NUMBER() OVER(PARTITION BY C.cha_id ORDER BY C.cha_evento_hora) AS cha_seq
			--,P.evt_protocolo
			--,O.cha_opcoes_ura
		FROM @tChamadasProcessadas AS C
			LEFT JOIN Alctel_Gen_CONFIG..cfg_person AS A  WITH (NOLOCK) ON C.age_agentid = A.employee_id
		WHERE
			C.cha_remove_evento = 0
			--AND cha_id = '006D034A877E297E'

		UNION ALL

		SELECT 
			C.cha_id
			,'SAIDA' AS cha_tipo_chamada
			,C.cha_origem
			,'REALIZADA' AS cha_fila
			,C.age_ini AS cha_inFila
			,C.age_ini AS cha_outFila
			,CASE WHEN DATEDIFF(SECOND, C.age_ini, C.age_fim) > 0 THEN 'ATENDIDA' ELSE 'NAO_ATENDIDA' END AS status_ligacao
			,CONCAT(A.first_name, ' ', A.last_name) AS age_fullname
			,C.age_agentid
			,A.[dbid]
			,C.age_extensao
			,C.age_ini
			,C.age_fim
			,0 AS age_nao_atende
			,0 AS evt_transferida
			,C.cha_destino
			--,C.age_ini AS cha_evento_hora
			--,'' AS evt_protocolo
			--,'' AS cha_opcoes_ura
			,1 AS cha_seq
		FROM @tChamdasGeral AS C 
			LEFT JOIN Alctel_Gen_CONFIG..cfg_person AS A  WITH (NOLOCK) ON C.age_agentid = A.employee_id
		WHERE
			call_type = @CHAMADAS_SAIDA

		UNION ALL

		SELECT 
			cha_id
			--,cha_ini
			,'ENTRADA' AS cha_tipo_chamada
			,cha_origem
			,'' AS cha_fila
			,cha_ini 
			,cha_fim
			,'ABANDONADA_URA' AS status_ligacao
			,'' AS age_fullname
			,'' AS age_agentid
			,NULL AS [dbid]
			,'' AS age_extensao
			,NULL AS age_ini
			,NULL AS age_fim
			,0 AS age_nao_atende
			,0 AS evt_transferida
			,cha_destino
			--,'' AS evt_protocolo
			--,'' AS cha_opcoes_ura
			,1 AS cha_seq
		FROM AlctelSRVCTI..Alc_chamadasGeralUra
		WHERE
			cha_ini BETWEEN @_data_inicio AND @_data_fim
			AND cha_id NOT IN (SELECT cha_id FROM @tChamadasProcessadas)
			AND DATEDIFF(SECOND, cha_ini, cha_fim) > 0

	) AS S
		LEFT JOIN @tChamdasGeral AS G ON S.cha_id = G.cha_id
		LEFT JOIN AlctelVSS..ufn_GetProtocolo(@data_inicio, @data_fim) AS P ON S.cha_id = P.evt_connid AND P.evt_seq = S.cha_seq
		LEFT JOIN AlctelVSS..ufn_GetOpcoesUra(@data_inicio, @data_fim, 'CONCATENADO') AS O ON S.cha_id = O.cha_id
		LEFT JOIN @tSiteFilaRelacionamento AS R ON S.cha_fila = R.fila
	WHERE
		(@_site_id = 0 OR R.site_id = @_site_id)	

	RETURN 
END










GO


