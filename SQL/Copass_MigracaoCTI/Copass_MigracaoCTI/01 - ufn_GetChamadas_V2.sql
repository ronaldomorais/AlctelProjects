USE [AlctelVSS]
GO

/****** Object:  UserDefinedFunction [dbo].[ufn_GetChamadas]    Script Date: 23/03/2023 16:28:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO










DECLARE
--ALTER FUNCTION [dbo].[ufn_GetChamadas_V2] (
	@data_inicio DATETIME = '2023-02-01'
	,@data_fim DATETIME   = '2023-02-01'
	,@site_id INT = 0
--) 
--RETURNS @tChamadas TABLE (
--	cha_id VARCHAR(50)
--	,cha_ini DATETIME
--	,cha_fim DATETIME
--	,cha_tipo_chamada VARCHAR(10)
--	,cha_origem VARCHAR(50)
--	,cha_fila VARCHAR(100)
--	,fila_dbid INT
--	,cha_inFila DATETIME
--	,cha_outFila DATETIME
--	,status_ligacao VARCHAR(20)
--	,age_fullname VARCHAR(255)
--	,age_agentid VARCHAR(100)
--	,age_dbid NUMERIC(10,0)
--	,age_extensao VARCHAR(50)
--	,age_ini DATETIME
--	,age_fim DATETIME
--	,age_nao_atende INT
--	,evt_transferida INT
--	,cha_destino VARCHAR(50)
--	,evt_protocolo VARCHAR(100)
--	,evt_opcoes_ura VARCHAR(MAX)
--	,evt_desconexao VARCHAR(10)
--	,[site] VARCHAR(50)
--)
--AS
BEGIN
	DECLARE
		@_data_inicio DATETIME = CAST(@data_inicio AS DATE)
		,@_data_fim DATETIME = CAST(@data_fim AS DATE)
		,@_site_id INT = @site_id
		,@CHAMADAS_ENTRADA INT = 2
		,@CHAMADAS_SAIDA INT = 3

	SET @_data_fim = DATEADD(SECOND, 86399, @_data_fim)

	SET @_site_id = 0 --Exite somente um site no cliente.

	DECLARE @tChamadasUra TABLE (
		cha_id VARCHAR(50)
		,call_type INT
		,cha_origem VARCHAR(50)
		,cha_destino VARCHAR(20)
		,cha_ini DATETIME
		,cha_fim DATETIME
		,evt_desconexao INT
		,evt_abandono INT
		,evt_number INT
	)

	DECLARE @tChamadasResultadoFinal TABLE (
		evt_number INT		
		,cha_evento_hora DATETIME
		,cha_evento VARCHAR(20)
		,cha_id VARCHAR(50)
		,call_type INT
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

	DECLARE @tChamadasSequencia TABLE (
		evt_number INT
		,evt_number_pos INT
		,cha_id VARCHAR(50)
		,cha_evento_hora DATETIME
		,cha_evento VARCHAR(20)
		,call_type INT
		,cha_origem VARCHAR(50)
		,cha_ini DATETIME
		,cha_fim DATETIME
		,cha_fila VARCHAR(100)
		,cha_inFila DATETIME
		,cha_outFila DATETIME 
		,age_agentid VARCHAR(100)
		,age_extensao VARCHAR(50)
		,age_ini DATETIME
		,age_fim DATETIME
		,cha_destino VARCHAR(20)
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

	INSERT INTO @tSiteFilaRelacionamento
	SELECT 
		F.fila
		,F.[dbid]
		,S.[site]
		,S.sequencial AS site_id
	FROM AlctelVSS..Alc_mtr001Filas AS F WITH (NOLOCK)
		LEFT JOIN AlctelVSS..Alc_mtr001Sites AS S WITH (NOLOCK) ON F.[site] = S.[site]
		
	INSERT INTO @tChamadasUra
	SELECT 
		S.cha_id
		,S.call_type
		,S.cha_origem
		,S.cha_destino
		,S.cha_ini
		,S.cha_fim
		,S.evt_desconexao
		,S.evt_abandonado
		,S.sequencial
	FROM (

		SELECT 
			cha_id
			,call_type
			,cha_origem
			,cha_destino
			,cha_ini
			,cha_fim
			,DATEDIFF(SECOND, cha_ini, cha_fim) AS cha_duracao
			,evt_desconexao
			,evt_abandonado
			,sequencial
		FROM AlctelSRVCTI.dbo.Alc_chamadasGeralUra WITH (NOLOCK)
		WHERE
			cha_ini BETWEEN @_data_inicio AND @_data_fim

	) AS S
	WHERE
		S.cha_duracao > 0

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

	
	INSERT INTO @tChamadasSequencia(evt_number, cha_evento_hora, cha_evento, evt_number_pos, cha_id)
	SELECT 	
		evt_number
		,cha_evento_hora
		,cha_evento
		,LEAD(evt_number, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number) AS cha_evento_pos
		,cha_id
		--,call_type
		--,cha_origem
		--,cha_ini
		--,cha_fim
		--,cha_fila
		--,cha_inFila
		--,cha_outFila
		--,age_agentid
		--,age_extensao
		--,age_ini
		--,age_fim
		--,cha_destino
		--,evt_desconexao
	FROM (
		
		SELECT 
			evt_number
			,cha_ini AS cha_evento_hora
			,'URA' AS cha_evento
			,cha_id
			--,call_type
			--,cha_origem
			--,cha_ini
			--,cha_fim
			--,'' AS cha_fila
			--,NULL AS cha_inFila
			--,NULL AS cha_outFila
			--,'' AS age_agentid
			--,'' AS age_extensao
			--,NULL AS age_ini
			--,NULL AS age_fim
			--,cha_destino
			--,evt_desconexao
		FROM @tChamadasUra			

		UNION ALL

		SELECT 
			evt_number
			,cha_inFila AS cha_evento_hora
			,'FILA' AS cha_evento
			,cha_id
			--,0 AS call_type
			--,cha_origem
			--,NULL AS cha_ini
			--,NULL AS cha_fim
			--,cha_fila
			--,cha_inFila
			--,cha_outFila
			--,'' AS age_agentid
			--,'' AS age_extensao
			--,NULL AS age_ini
			--,NULL AS age_fim
			--,'' AS cha_destino
			--,-1 AS evt_desconexao
		FROM @tChamadasFila

		UNION ALL

		SELECT 
			evt_number
			,age_ini AS cha_evento_hora
			,'AGENTE' AS cha_evento
			,cha_id
			--,call_type
			--,cha_origem
			--,cha_ini
			--,cha_fim
			--,'' AS cha_fila
			--,NULL AS cha_inFila
			--,NULL AS cha_outFila
			--,age_agentid
			--,age_extensao
			--,age_ini
			--,age_fim
			--,cha_destino
			--,evt_desconexao
		FROM @tChamdasGeral
			
		UNION ALL

		SELECT 
			evt_number
			,age_ini AS cha_evento_hora
			,'AGENTE_NAO_ATENDE' AS cha_evento
			,cha_id
			--,0 AS call_type
			--,'' AS cha_origem
			--,NULL AS cha_ini
			--,NULL AS cha_fim
			--,'' AS cha_fila
			--,NULL AS cha_inFila
			--,NULL AS cha_outFila
			--,age_agentid
			--,age_extensao
			--,age_ini
			--,age_fim
			--,'' AS cha_destino
			--,-1 AS evt_desconexao
		FROM @tChamadasGeralNaoAtende

		UNION ALL

		SELECT 
			evt_number
			,age_ini AS cha_evento_hora
			,'TRANSFER' AS cha_evento
			,cha_id
			--,0 AS call_type
			--,'' AS cha_origem
			--,NULL AS cha_ini
			--,NULL AS cha_fim
			--,'' AS cha_fila
			--,NULL AS cha_inFila
			--,NULL AS cha_outFila
			--,age_agentid
			--,age_extensao
			--,age_ini
			--,age_fim
			--,cha_destino
			--,-1 AS evt_desconexao		
		FROM @tChamadasGeralConversacao
		WHERE
			evt_transferencia = 1

	) AS S
	--WHERE
	--	S.cha_id = '006D034A877E2A47'
	ORDER BY S.cha_id, S.cha_evento_hora, S.evt_number

------------------------------------------------------------------------------------------------------------------
--Processando ChamadasUra (URA)
	INSERT INTO @tChamadasResultadoFinal(evt_number, cha_evento_hora, cha_evento, cha_id, cha_origem, cha_ini, cha_fim, cha_fila, cha_inFila, cha_outFila)
	SELECT 
		evt_number
		,cha_ini AS cha_evento_hora
		,'URA'
		,cha_id
		,cha_origem
		,cha_ini 
		,cha_fim 
		,'' AS cha_fila
		,cha_ini AS cha_inFila	
		,cha_fim AS cha_outFila	
	FROM @tChamadasUra
	

------------------------------------------------------------------------------------------------------------------
--Processando ChamadasFilas (Filas)
	INSERT INTO @tChamadasResultadoFinal(evt_number, cha_evento_hora, cha_evento, cha_id, cha_origem, cha_fila, cha_inFila, cha_outFila)
	SELECT 
		evt_number
		,cha_inFila AS cha_evento_hora
		,'FILA' AS cha_evento
		,cha_id
		,cha_origem
		,cha_fila
		,cha_inFila
		,cha_outFila
	FROM @tChamadasFila



------------------------------------------------------------------------------------------------------------------
--Processando ChamadasGeral (Atendidas)

	--SELECT *
	--	S.cha_id
	--	,C.call_type
	--	,R.cha_origem
	--	,C.cha_ini
	--	,C.cha_fim
	--	,R.cha_fila
	--	,R.cha_inFila
	--	,R.cha_outFila
	--	,C.age_agentid
	--	,C.age_extensao
	--	,C.age_ini
	--	,C.age_fim
	--	,C.cha_destino
	UPDATE R SET
		--R.cha_evento_hora = S.cha_evento_hora
		R.call_type = C.call_type
		,R.status_ligacao = 'ATENDIDA'
		--,R.cha_origem
		,R.cha_ini = C.cha_ini
		,R.cha_fim = C.cha_fim
		--,R.cha_fila
		--,R.cha_inFila
		--,R.cha_outFila
		,R.age_agentid = C.age_agentid
		,R.age_extensao = C.age_extensao
		,R.age_ini = C.age_ini
		,R.age_fim = C.age_fim
		,R.cha_destino = C.cha_destino
	FROM @tChamadasSequencia AS S
		INNER JOIN @tChamadasResultadoFinal AS R ON S.cha_id = R.cha_id AND S.evt_number = R.evt_number
		INNER JOIN @tChamdasGeral AS C ON S.cha_id = C.cha_id AND S.evt_number_pos = C.evt_number
	WHERE
		S.cha_evento = 'FILA'
	--	S.cha_id = '006D034A877E297E'
	--ORDER BY S.cha_id, S.cha_evento_hora, S.evt_number
	
	
		
	DELETE FROM C
	FROM @tChamadasSequencia AS S
		INNER JOIN @tChamadasResultadoFinal AS R ON S.cha_id = R.cha_id AND S.evt_number = R.evt_number
		INNER JOIN @tChamdasGeral AS C ON S.cha_id = C.cha_id AND S.evt_number_pos = C.evt_number
	WHERE
		S.cha_evento = 'FILA'


	INSERT INTO @tChamadasResultadoFinal(evt_number, cha_evento_hora, cha_evento, cha_id, cha_ini, cha_fim, call_type, cha_origem, age_agentid, age_extensao, age_ini, age_fim, cha_destino, status_ligacao)
	SELECT 
		C.evt_number 
		,S.cha_evento_hora
		,S.cha_evento
		,C.cha_id
		,C.cha_ini
		,C.cha_fim
		,C.call_type
		,C.cha_origem
		,C.age_agentid
		,C.age_extensao
		,C.age_ini
		,C.age_fim
		,C.cha_destino
		,'ATENDIDA' AS status_ligacao
	FROM @tChamadasSequencia AS S 
		INNER JOIN @tChamdasGeral AS C ON S.cha_id = C.cha_id AND S.evt_number = C.evt_number
	--WHERE cha_id = '006D034A877E297E' 


------------------------------------------------------------------------------------------------------------------
--Processando ChamadasGeralNaoAtende (Nao Atendidas)

	UPDATE R SET 
		R.cha_evento_hora = S.cha_evento_hora
		,R.age_agentid = C.age_agentid
		,R.age_extensao = C.age_extensao
		,R.age_ini = C.age_ini
		,R.age_fim = C.age_fim
		,R.status_ligacao = 'AGENTE_NAO_ATENDE'
	FROM @tChamadasSequencia AS S
		INNER JOIN @tChamadasResultadoFinal AS R ON S.cha_id = R.cha_id AND S.evt_number = R.evt_number
		INNER JOIN @tChamadasGeralNaoAtende AS C ON S.cha_id = C.cha_id AND S.evt_number_pos = C.evt_number
	WHERE
		S.cha_evento = 'FILA'

	--INSERT INTO @tChamadasResultadoFinal(evt_number, cha_evento_hora, cha_id, age_agentid, age_extensao, age_ini, age_fim, cha_destino, status_ligacao)
	--SELECT 
	--	C.evt_number
	--	,S.cha_evento_hora
	--	,C.cha_id
	--	--,C.cha_ini
	--	--,C.cha_fim
	--	--,C.call_type
	--	--,C.cha_origem
	--	,C.age_agentid
	--	,C.age_extensao
	--	,C.age_ini
	--	,C.age_fim
	--	,C.cha_destino
	--	,'ATENDIDA' AS status_ligacao
	--FROM @tChamadasSequencia AS S
	--	INNER JOIN @tChamadasGeralConversacao AS C ON S.cha_id = C.cha_id AND S.evt_number_pos = C.evt_number
	--WHERE
	--	cha_evento = 'AGENTE_NAO_ATENDE'


	DELETE FROM C
	FROM @tChamadasSequencia AS S
		INNER JOIN @tChamadasResultadoFinal AS R ON S.cha_id = R.cha_id AND S.evt_number = R.evt_number
		INNER JOIN @tChamadasGeralNaoAtende AS C ON S.cha_id = C.cha_id AND S.evt_number_pos = C.evt_number
	WHERE
		S.cha_evento = 'FILA'


------------------------------------------------------------------------------------------------------------------
--Processando ChamadasGeralConversacaoo (Transferidas)

	UPDATE R SET 
		R.cha_evento_hora = S.cha_evento_hora
		,R.age_agentid = C.age_agentid
		,R.age_extensao = C.age_extensao
		,R.age_ini = C.age_ini
		,R.age_fim = C.age_fim
		,R.status_ligacao = 'TRANSFER'
	FROM @tChamadasSequencia AS S
		INNER JOIN @tChamadasResultadoFinal AS R ON S.cha_id = R.cha_id AND S.evt_number = R.evt_number
		INNER JOIN @tChamadasGeralConversacao AS C ON S.cha_id = C.cha_id AND S.evt_number_pos = C.evt_number
	WHERE
		S.cha_evento = 'FILA'

	DELETE FROM C
	FROM @tChamadasSequencia AS S
		INNER JOIN @tChamadasResultadoFinal AS R ON S.cha_id = R.cha_id AND S.evt_number = R.evt_number
		INNER JOIN @tChamadasGeralConversacao AS C ON S.cha_id = C.cha_id AND S.evt_number_pos = C.evt_number
	WHERE
		S.cha_evento = 'FILA'

	INSERT INTO @tChamadasResultadoFinal(evt_number, cha_evento_hora, cha_id, age_agentid, age_extensao, age_ini, age_fim, cha_destino, status_ligacao)
	SELECT 
		S.evt_number
		,S.cha_evento_hora
		,C.cha_id
		,C.age_agentid
		,C.age_extensao
		,C.age_ini
		,C.age_fim
		,C.cha_destino
		,'TRANSFER' AS status_ligacao
	FROM @tChamadasSequencia AS S
		INNER JOIN @tChamadasGeralConversacao AS C ON S.cha_id = C.cha_id AND S.evt_number = C.evt_number


	--INSERT INTO @tChamadas
	SELECT 
		--cha_evento_hora
		--,evt_number
		S.cha_id
		,S.cha_ini
		,S.cha_fim		
		,S.cha_tipo_chamada
		,CASE WHEN S.cha_origem = '***' THEN 'numero oculto' ELSE S.cha_origem END AS cha_origem
		,S.cha_fila
		,S.fila_dbid
		,S.cha_inFila
		,S.cha_outFila
		,S.status_ligacao
		,CONCAT(A.first_name, ' ', A.last_name) AS age_fullname
		,S.age_agentid
		,A.[dbid] AS age_dbid
		,S.age_extensao
		,S.age_ini
		,S.age_fim
		,S.age_nao_atende
		,S.evt_transferida
		--,ISNULL(S.cha_destino, '') AS cha_destino
		,ISNULL(CASE WHEN evt_transferida = 1 AND LEAD(cha_inFila, 1, NULL) OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.evt_number) = S.cha_inFila
			THEN LEAD(age_extensao, 1, NULL) OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.evt_number)
			ELSE LEAD(cha_fila, 1, NULL) OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.evt_number)
		END, '') cha_destino
		,ISNULL(P.evt_protocolo, '') AS evt_protocolo
		,ISNULL(O.cha_opcoes_ura, '') AS cha_opcoes_ura
		,S.desconexao
		--,ISNULL(R.[site], '') AS [site]
		,'Copass' AS [site]
	FROM (

	SELECT 
		cha_evento_hora
		,evt_number
		,cha_id
		,CASE WHEN cha_ini IS NULL AND status_ligacao = 'AGENTE_NAO_ATENDE'
			THEN LEAD(cha_ini, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
			ELSE 
				CASE WHEN cha_ini IS NULL AND cha_inFila IS NOT NULL
					THEN cha_inFila
					ELSE 
						CASE WHEN cha_ini IS NULL AND status_ligacao = 'TRANSFER'
							THEN LAG(cha_ini, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
							ELSE cha_ini
						END
				END
		END AS cha_ini
		,CASE WHEN cha_fim IS NULL AND status_ligacao = 'AGENTE_NAO_ATENDE'
			THEN LEAD(cha_fim, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
			ELSE 
				CASE WHEN cha_ini IS NULL AND cha_outFila IS NOT NULL
					THEN cha_outFila
					ELSE 
						CASE WHEN cha_fim IS NULL AND status_ligacao = 'TRANSFER'
							THEN LAG(cha_fim, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
							ELSE cha_fim
						END
				END
		END AS cha_fim
		,CASE WHEN call_type IS NULL AND status_ligacao = 'AGENTE_NAO_ATENDE'
			THEN 
				CASE LEAD(call_type, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
					WHEN 1 THEN ''
					WHEN 2 THEN 'ENTRADA'
					WHEN 3 THEN 'SAIDA'
					WHEN 4 THEN ''
				END
			ELSE 
				CASE call_type
					WHEN 1 THEN ''
					WHEN 2 THEN 
						CASE WHEN LEN(cha_origem) < 8 THEN 'INTERNA' ELSE 'ENTRADA' END 
					WHEN 3 THEN 'SAIDA'
					WHEN 4 THEN ''
					ELSE
						CASE WHEN cha_fila IS NOT NULL AND cha_fila <> ''
							THEN 'ENTRADA'
							ELSE
								CASE WHEN status_ligacao = 'TRANSFER'
									THEN 
										CASE LAG(call_type, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
											WHEN 1 THEN ''
											WHEN 2 THEN 'ENTRADA'
											WHEN 3 THEN 'SAIDA'
											WHEN 4 THEN ''
										END			
										ELSE CASE WHEN cha_evento = 'URA' THEN 'ENTRADA' END
								END
						END
				END
		END AS cha_tipo_chamada
		,CASE WHEN cha_origem IS NULL AND status_ligacao = 'TRANSFER'
			THEN LAG(cha_origem, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
			ELSE cha_origem
		END AS cha_origem
		,CASE WHEN call_type = 3
			THEN 'REALIZADA'
			ELSE 
				CASE WHEN cha_fila IS NULL AND status_ligacao = 'ATENDIDA'
					THEN ISNULL(LAG(cha_fila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number), '')
					ELSE 
						CASE WHEN cha_fila IS NULL AND status_ligacao = 'TRANSFER'
							--THEN LAG(cha_fila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
							THEN ''
							ELSE cha_fila
						END
			END
		END AS cha_fila
		,NULL AS fila_dbid
		,CASE WHEN call_type = 3
			THEN age_ini
			ELSE
				CASE WHEN cha_inFila IS NULL AND status_ligacao = 'ATENDIDA'
					THEN CASE WHEN LAG(cha_inFila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number) IS NULL AND LEN(cha_origem) < 8
						THEN age_ini
						ELSE CASE WHEN LAG(cha_inFila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number) IS NULL AND cha_fila IS NULL
							THEN age_ini
							ELSE LAG(cha_inFila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
						END
					END
					ELSE 
						CASE WHEN cha_inFila IS NULL AND status_ligacao = 'TRANSFER'
							THEN LAG(cha_inFila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
							ELSE cha_inFila
						END
			END
		END AS cha_inFila
		,CASE WHEN call_type = 3
			THEN age_fim
			ELSE
				CASE WHEN cha_outFila IS NULL AND status_ligacao = 'ATENDIDA'
					THEN CASE WHEN LAG(cha_outFila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number) IS NULL AND LEN(cha_origem) < 8
						THEN age_fim
						ELSE CASE WHEN LAG(cha_outFila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number) IS NULL AND cha_fila IS NULL
							THEN age_fim
							ELSE LAG(cha_outFila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
						END
					END
					ELSE 
						CASE WHEN cha_outFila IS NULL AND status_ligacao = 'TRANSFER'
							THEN LAG(cha_outFila, 1, NULL) OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number)
							ELSE cha_outFila
						END
			END
		END AS cha_outFila
		,CASE ISNULL(status_ligacao, '') 
			WHEN '' THEN CASE WHEN cha_evento = 'URA' THEN 'ABANDONADA_URA' ELSE 'ABANDONADA' END
			WHEN 'TRANSFER' THEN CASE WHEN DATEDIFF(SECOND, age_ini, age_fim) > 0 THEN 'ATENDIDA' ELSE 'ABANDONADA' END
			ELSE status_ligacao
		END AS status_ligacao
		--,CASE WHEN status_ligacao IS NULL
		--	THEN 
		--		CASE WHEN cha_fila IS NOT NULL AND cha_fila <> ''
		--			THEN 'ABANDONADA'
		--		END
		--	ELSE status_ligacao
		--END AS status_ligacao
		--,'' AS age_fullname
		,age_agentid
		--,NULL AS age_dbid
		,age_extensao
		,age_ini
		,age_fim
		,CASE WHEN status_ligacao = 'AGENTE_NAO_ATENDE' THEN 1 ELSE 0 END AS age_nao_atende
		,CASE WHEN LEAD(status_ligacao, 1, '') OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, evt_number) = 'TRANSFER'
			THEN 1
			ELSE 0
		END AS evt_transferida
		,CASE WHEN cha_destino = '***' THEN '' ELSE cha_destino END AS cha_destino
		--,'' AS evt_protocolo
		--,'' AS cha_opcoes_ura
		,'' AS desconexao
		--,'' AS [site]
	FROM @tChamadasResultadoFinal
	--WHERE cha_id = '006D034A877E297E'
	--WHERE cha_id = '006D034A877E2A20'
	--WHERE cha_id = '006D034A877E29C5'
	--WHERE cha_id = '006D034A877E29FF'
	--WHERE cha_id = '006D034A877E2A47' --Erro no processamento
	--WHERE cha_id = '006D034A877E2A38' --Chamada call_type = 2 sem fila
	--WHERE cha_id = '006D034A877E2A8E' --Erro no processamento
	--WHERE cha_id = '006D034A877E297E'

	) AS S
	LEFT JOIN Alctel_Gen_CONFIG..cfg_person AS A ON S.age_agentid = A.employee_id
	LEFT JOIN AlctelVSS..ufn_GetProtocolo(@data_inicio, @data_fim) AS P ON S.cha_id = P.evt_connid
	LEFT JOIN AlctelVSS..ufn_GetOpcoesUra(@data_inicio, @data_fim, 'CONCATENADO') AS O ON S.cha_id = O.cha_id
	LEFT JOIN @tSiteFilaRelacionamento AS R ON S.cha_fila = R.fila
	--WHERE
	--	S.cha_outFila IS NULL 
	--	--S.cha_id = '006D034A877E2991'
	ORDER BY S.cha_id, S.cha_evento_hora, S.evt_number

	
	RETURN 
END










GO


