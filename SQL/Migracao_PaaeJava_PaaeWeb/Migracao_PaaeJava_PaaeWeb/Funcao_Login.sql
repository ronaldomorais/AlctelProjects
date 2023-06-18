USE AlctelVSS
GO


CREATE FUNCTION ufn_Login (
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
	,@aplicacoes INT = -1
)
RETURNS @tLogin TABLE (
	agente_nome VARCHAR(255)
	,agente_dbid NUMERIC(10,0)
	,agente_id VARCHAR(255)
	,agente_cod VARCHAR(20)
	,agente_aplicacoes INT
	,agente_place VARCHAR(255)
	,agente_login DATETIME
	,agente_logout DATETIME
	,login_duracao INT
)
--AS
BEGIN
	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal
		,@par_site INT = @parSite
		,@timestamp_inicio INT = DATEDIFF(SECOND, '1970-01-01 00:00:00', CAST(@dataInicio AS DATETIME))
		,@timestamp_final INT = DATEDIFF(SECOND, '1970-01-01 00:00:00', DATEADD(SECOND, 86399, CAST(@dataFinal AS DATETIME)))


	INSERT INTO @tLogin
	SELECT 
		R.agente_nome
		,R.agente_dbid
		,R.agente_id
		,R.agente_cod
		,R.agente_aplicacoes
		,R.agente_place
		,R.agente_login
		,R.agente_logout
		,DATEDIFF(SECOND, R.agente_login, R.agente_logout) AS login_duracao
	FROM (
	SELECT 
		A.agente_nome
		,A.agente_dbid
		,A.agente_id
		,A.agente_cod
		,A.agente_aplicacoes
		,P.[name] AS agente_place
		,CASE WHEN L.[STATUS] = 1 THEN DATEADD(SECOND, [TIME]-10800, '1970-01-01') ELSE NULL END AS agente_login 			
		--,CASE WHEN L.[STATUS] = 0 THEN DATEADD(SECOND, [TIME]-10800, '1970-01-01') ELSE NULL END AS agente_logout		
		,CASE WHEN L.[STATUS] = 1 AND LEAD(L.[STATUS], 1, NULL) OVER(PARTITION BY L.AGENTDBID ORDER BY [TIME]) = 0
			THEN LEAD(DATEADD(SECOND, [TIME]-10800, '1970-01-01'), 1, NULL) OVER(PARTITION BY L.AGENTDBID ORDER BY [TIME]) 
			ELSE NULL
		END AS agente_logout
	FROM DB_GENESYS_DM..[LOGIN] AS L WITH (NOLOCK)
		LEFT JOIN AlctelVSS..ufn_Agentes() AS A ON L.AGENTDBID = A.agente_dbid
		LEFT JOIN DB_GENESYS_DAT..cfg_place AS P WITH (NOLOCK) ON L.PLACEDBID = P.[dbid]		
	WHERE 
		[TIME] BETWEEN @timestamp_inicio AND @timestamp_final
	) AS R
	WHERE
		R.agente_login IS NOT NULL AND R.agente_logout IS NOT NULL
		AND (@aplicacoes = -1 OR R.agente_aplicacoes = @aplicacoes)
	ORDER BY R.agente_nome, R.agente_login

	RETURN
END
