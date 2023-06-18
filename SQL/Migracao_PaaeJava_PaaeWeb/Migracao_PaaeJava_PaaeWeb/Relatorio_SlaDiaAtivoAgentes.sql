USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[Alc_Paae_sp_RCC_SLADiaAtivoAgente]    Script Date: 1/16/2023 9:22:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[JAVA_hiSLADiaAtivoAgentes]
	@parUser VARCHAR(50) = ''
	,@dataInicial NVARCHAR(20) = '2021-01-14'
	,@dataFinal NVARCHAR(20) = '2021-01-15'
	,@parAplicacao INT = 1
	,@parSite INT
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	DECLARE 
		@par_user VARCHAR(50) = @parUser
		,@data_inicio DATE = @dataInicial
		,@data_final DATE = @dataFinal
		,@par_aplicacao INT = @parAplicacao
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
		,CAST(L.agente_login AS DATE) AS [data]
		,L.agente_dbid
		,L.agente_id
		,CAST(L.agente_login AS TIME(0)) AS [login]
		,CAST(L.agente_logout AS TIME(0)) AS logout
		,L.login_duracao AS tempo_logado
		,UPPER(LEFT(agente_id, 3)) AS agente_sufixo 
		,ROW_NUMBER() OVER(PARTITION BY CAST(L.agente_login AS DATE), L.agente_dbid ORDER BY L.agente_login) AS login_seq
	INTO #tLogin
	FROM AlctelVSS..ufn_Login(@data_inicio, @data_final, @par_site, -1) AS L

	SELECT 
		CAST(R.[data] AS DATE) AS [data]
		,R.agente_dbid
		--,SUM(R.tempoAtendimentoTotal) AS tempoAtendimentoTotal
		--,SUM(R.tempoAtendimentoEntrada) AS tempoAtendimentoEntrada
		,SUM(R.tempoAtendimentoSaida) AS tempoAtendimentoSaida
		--,SUM(R.chamadas_receptivas) AS chamadas_receptivas
		,SUM(R.chamadas_ativas) AS chamadas_ativas
	INTO #tChamadas
	FROM AlctelVSS..ufn_R_123_STAT_RES(@data_inicio, @data_final, @par_site) AS R
	WHERE
		tempoLogado > 0
	GROUP BY CAST(R.[data] AS DATE), R.agente_dbid
	ORDER BY CAST(R.[data] AS DATE), R.agente_dbid

	SELECT 
		L.agente_nome AS ColunaTipoStr01
		,L.[data] AS ColunaTipoDateTime01
		--,L.agente_dbid
		--,L.agente_id
		--,L.[login]
		--,L.logout
		--,dbo.ToHHMMSS(L.tempo_logado) AS tempo_logado
		,C.chamadas_ativas AS ColunaTipoInt01
		,CASE WHEN C.tempoAtendimentoSaida > 0
			THEN C.tempoAtendimentoSaida / C.chamadas_ativas
			ELSE 0
		END AS ColunaTipoInt02
		--,C.chamadas_ativas
		--,G.NomeGrupo
	FROM #tLogin AS L
		LEFT JOIN #tGrupos AS G ON L.agente_sufixo = G.fila_sufixo
		LEFT JOIN #tChamadas AS C ON L.[data] = C.[data] AND L.agente_dbid = C.agente_dbid
	WHERE
		L.login_seq = 1
	ORDER BY L.agente_nome, L.[data]

END
GO


