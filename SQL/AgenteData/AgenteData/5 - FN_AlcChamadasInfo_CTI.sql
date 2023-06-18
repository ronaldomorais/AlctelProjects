DECLARE 
	@tenant_dbid NUMERIC(10,0) = 111
	,@data_inicio DATETIME = '2022-12-20 00:00:00'
	,@data_fim DATETIME = '2022-12-21 23:59:59'
	,@site_id INT = 0


	DECLARE
		@_tenant_dbid NUMERIC(10,0) = @tenant_dbid
		,@_data_inicio DATETIME = @data_inicio
		,@_data_fim DATETIME = @data_fim
		,@_site_id INT = @site_id
		,@CHAMADAS_ENTRADA INT = 2


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
	--FROM AlctelSRVCTI_Aeromedica.dbo.Alc_chamadasGeralUra WITH (NOLOCK)
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
	FROM AlctelSRVCTI_Aeromedica.dbo.Alc_chamadasGeralFilas WITH (NOLOCK)
	WHERE
		cha_inFila BETWEEN @_data_inicio AND @_data_fim

	INSERT INTO @tChamdasGeral
	SELECT 
		cha_id
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
	FROM AlctelSRVCTI_Aeromedica.dbo.Alc_chamadasGeral WITH (NOLOCK)
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
	FROM AlctelSRVCTI_Aeromedica.dbo.Alc_chamadasGeralConversacao WITH (NOLOCK)
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
	FROM AlctelSRVCTI_Aeromedica.dbo.Alc_chamadasGeralNaoAtende WITH (NOLOCK)
	WHERE
		age_ini BETWEEN @_data_inicio AND @_data_fim		

	

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
			WHEN '3 - AGENTE_ATENDE'
				THEN 
					CASE LEAD(S.cha_evento_nome, 1, '') OVER(PARTITION BY S.cha_id ORDER BY S.cha_evento_hora, S.cha_evento_nome)
						WHEN '4 - TRANSFER' THEN 1
						ELSE 0
					END	
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
						ELSE 1
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

	SELECT 
		ROW_NUMBER() OVER(PARTITION BY cha_id ORDER BY cha_evento_hora, cha_evento_nome) AS cha_seq
		,cha_id
		,2 AS call_type
		,cha_origem
		,cha_fila
		,cha_inFila
		,cha_outFila
		,age_agentid
		,age_extensao
		,age_ini
		,age_fim
		,age_nao_atende
		,evt_transferida
		,cha_destino
		,cha_evento_hora
		,cha_evento_nome
		,cha_remove_evento
	FROM @tChamadasProcessadas
	WHERE
		--cha_evento_nome IN('1 - FILA', '4 - TRANSFER')	
		--cha_remove_evento  = 0
		cha_id = '028A033D398630D9'
	ORDER BY cha_id, cha_evento_hora, cha_evento_nome


	SELECT 
		cha_id
		,call_type
		,cha_origem
		,'' AS cha_fila
		,NULL AS cha_inFila
		,NULL AS cha_outFila
		,age_agentid
		,age_extensao
		,age_ini
		,age_fim
		,0 AS age_nao_atende
		,0 AS evt_transferida
		,cha_destino
		,age_ini AS cha_evento_hora
		,'OUTBOUND' AS cha_evento_nome
		,0 AS cha_remove_evento
	FROM @tChamdasGeral
	WHERE
		call_type <> @CHAMADAS_ENTRADA
	ORDER BY cha_id
