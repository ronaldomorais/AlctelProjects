USE AlctelVSS
GO


ALTER PROCEDURE Alc_PAAE_sp_RCC_SLAHoraAtivoAgente
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
	,@aplicacoes INT = 1
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal
		,@par_site INT = @parSite

	IF OBJECT_ID('tempdb..#tLogin') IS NOT NULL
		DROP TABLE #tLogin
	IF OBJECT_ID('tempdb..#tGrupos') IS NOT NULL
		DROP TABLE #tGrupos
	IF OBJECT_ID('tempdb..#tChamadas') IS NOT NULL
		DROP TABLE #tChamadas

	SELECT DISTINCT
		aplicacoes		 
		,NomeGrupo
		,CASE UPPER(LEFT(nomeFila, 3))
			WHEN 'SEG' THEN CASE WHEN NomeGrupo = 'OUVIDORIA' THEN 'OUV' ELSE UPPER(LEFT(nomeFila, 3)) END
			WHEN 'VEN' THEN 'FVE'
			ELSE UPPER(LEFT(nomeFila, 3))
		END AS fila_sufixo
		--,UPPER(LEFT(nomeFila, 3)) AS fila_sufixo
	INTO #tGrupos
	FROM AlctelVSS..ufn_Grupos()

	
	SELECT 
		L.agente_nome
		,CONCAT(LEFT(RIGHT(CONVERT(VARCHAR(19), L.agente_login, 121), 8), 2), ':00:00 - ', LEFT(RIGHT(CONVERT(VARCHAR(19), DATEADD(HOUR, 1, L.agente_login), 121), 8), 2), ':00:00') AS hora_intervalo
		,L.agente_dbid
		,L.agente_id
		,CAST(L.agente_login AS TIME(0)) AS [login]
		,CAST(L.agente_logout AS TIME(0)) AS logout
		,L.login_duracao AS tempo_logado
		,UPPER(LEFT(agente_id, 3)) AS agente_sufixo
	INTO #tLogin
	FROM AlctelVSS..ufn_Login(@data_inicio, @data_final, @par_site, -1) AS L


	SELECT 
		CONCAT(RIGHT(CONVERT(VARCHAR(19), [data], 121), 8), ' - ', RIGHT(CONVERT(VARCHAR(19), DATEADD(HOUR, 1, [data]), 121), 8)) AS hora_intervalo
		,R.agente_dbid
		--,SUM(R.tempoAtendimentoTotal) AS tempoAtendimentoTotal
		--,SUM(R.tempoAtendimentoEntrada) AS tempoAtendimentoEntrada
		,SUM(R.tempoAtendimentoSaida) AS tempoAtendimentoSaida
		--,SUM(R.chamadas_receptivas) AS chamadas_receptivas
		,SUM(R.chamadas_ativas) AS chamadas_ativas
	INTO #tChamadas
	FROM AlctelVSS..ufn_R_122_STAT_RES(@data_inicio, @data_final, @par_site) AS R
	WHERE
		tempoLogado > 0
	GROUP BY R.[data], R.agente_dbid
	--ORDER BY CAST(R.[data] AS DATE), R.agente_dbid

	SELECT 
		L.agente_nome
		,L.hora_intervalo
		--,L.agente_dbid
		--,L.agente_id
		--,L.[login]
		--,L.logout
		--,dbo.ToHHMMSS(L.tempo_logado) AS tempo_logado
		,C.chamadas_ativas AS atendidas
		,CASE WHEN C.chamadas_ativas > 0
			THEN AlctelVSS.dbo.ToHHMMSS(C.tempoAtendimentoSaida / C.chamadas_ativas) 
			ELSE '00:00:00'
		END AS TempoMediaAtendimento
		--,C.chamadas_ativas
		,G.NomeGrupo
	FROM #tLogin AS L
		LEFT JOIN #tGrupos AS G ON L.agente_sufixo = G.fila_sufixo
		LEFT JOIN #tChamadas AS C ON L.hora_intervalo = C.hora_intervalo AND L.agente_dbid = C.agente_dbid
	ORDER BY L.agente_nome

END