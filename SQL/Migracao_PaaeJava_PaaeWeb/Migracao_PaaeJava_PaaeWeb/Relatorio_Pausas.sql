USE AlctelVSS
GO


CREATE PROCEDURE JAVA_hiPausas
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
	IF OBJECT_ID('tempdb..#tPausas') IS NOT NULL
		DROP TABLE #tPausas

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
		,CAST(L.agente_login AS DATE) AS [data]
		,L.agente_dbid
		,L.agente_id
		,CAST(L.agente_login AS TIME(0)) AS [login]
		,CAST(L.agente_logout AS TIME(0)) AS logout
		,L.login_duracao AS tempo_logado
		,UPPER(LEFT(agente_id, 3)) AS agente_sufixo
	INTO #tLogin
	FROM AlctelVSS..ufn_Login(@data_inicio, @data_final, @par_site, -1) AS L

	SELECT 
		CAST(R.[data] AS DATE) AS [data]
		,R.agente_dbid
		,SUM(R.tempoAtendimentoTotal) AS tempoAtendimentoTotal
		,SUM(R.chamadas_receptivas) AS chamadas_receptivas
		,SUM(R.chamadas_ativas) AS chamadas_ativas
	INTO #tChamadas
	FROM AlctelVSS..ufn_R_123_STAT_RES(@data_inicio, @data_final, @par_site) AS R
	WHERE
		tempoLogado > 0
	GROUP BY CAST(R.[data] AS DATE), R.agente_dbid
	ORDER BY CAST(R.[data] AS DATE), R.agente_dbid


	SELECT 
		ROW_NUMBER() OVER(PARTITION BY agente_dbid ORDER BY pausa_inicio) AS numero
		,agente_dbid
		,pausa_nome
		,pausa_inicio
		,pausa_fim
		,pausa_duracao
	INTO #tPausas
	FROM AlctelVSS.dbo.ufn_Pausas(@data_inicio, @data_final, @par_site)


	SELECT 
		L.agente_nome AS ColunaTipoStr01
		,L.[data] AS ColunaTipoDateTime01
		--,L.agente_dbid
		,L.agente_id AS ColunaTipoStr02
		,L.[login] AS ColunaTipoDateTime02
		,L.logout AS ColunaTipoDateTime03
		--,dbo.ToHHMMSS(L.tempo_logado) AS tempo_logado
		,L.tempo_logado AS ColunaTipoInt01
		,0 AS ColunaTipoInt02
		--,dbo.ToHHMMSS(C.tempoAtendimentoTotal) AS tempoAtendimentoTotal
		,C.tempoAtendimentoTotal AS ColunaTipoInt03
		----,'00:00:00' AS tempo_pausa
		----,C.chamadas_receptivas
		----,C.chamadas_ativas
		,P.numero AS ColunaTipoInt04
		,P.pausa_inicio AS ColunaTipoDateTime04
		,P.pausa_fim AS ColunaTipoDateTime05
		--,dbo.ToHHMMSS(P.pausa_duracao) AS pausa_duracao
		,P.pausa_duracao AS ColunaTipoInt05
		,P.pausa_nome AS ColunaTipoStr03
		--,G.NomeGrupo
	FROM #tLogin AS L
		LEFT JOIN #tGrupos AS G ON L.agente_sufixo = G.fila_sufixo
		LEFT JOIN #tChamadas AS C ON L.[data] = C.[data] AND L.agente_dbid = C.agente_dbid
		LEFT JOIN #tPausas AS P ON L.agente_dbid = P.agente_dbid
	ORDER BY L.agente_nome, L.[data], P.numero

END