USE AlctelVSS
GO


CREATE FUNCTION ufn_Pausas (
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
)
RETURNS @tPausas TABLE (
	agente_dbid NUMERIC(10,0)
	,pausa_cod VARCHAR(255)
	,pausa_nome VARCHAR(255)
	,pausa_inicio DATETIME
	,pausa_fim DATETIME
	,pausa_duracao INT
	,pausa_ishardware INT
	,pausa_workmode INT
)
AS
BEGIN


	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal
		,@par_site INT = @parSite
		,@timestamp_inicio INT = DATEDIFF(SECOND, '1970-01-01 00:00:00', CAST(@dataInicio AS DATETIME))
		,@timestamp_final INT = DATEDIFF(SECOND, '1970-01-01 00:00:00', DATEADD(SECOND, 86399, CAST(@dataFinal AS DATETIME)))

	INSERT INTO @tPausas
	SELECT 
		R.agente_dbid
		,R.pausa_cod
		,R.pausa_nome
		,R.pausa_inicio
		,R.pausa_fim
		,R.pausa_duracao
		,R.pausa_ishardware
		,R.pausa_workmode
	FROM (
		SELECT 
			V.AGENT_DBID AS agente_dbid
			--,V.REASON_VALUE
			,C.code AS pausa_cod
			,C.[name] AS pausa_nome
			,DATEADD(SECOND, V.START_TIME-10800, '1970-01-01') AS pausa_inicio
			,DATEADD(SECOND, V.END_TIME-10800, '1970-01-01') AS pausa_fim
			,V.DURATION AS pausa_duracao
			,V.IS_HARDWARE AS pausa_ishardware
			,V.WORK_MODE AS pausa_workmode
		FROM DB_GENESYS_DM..VOICE_REASONS AS V WITH (NOLOCK)
			LEFT JOIN DB_GENESYS_DAT..cfg_action_code AS C WITH (NOLOCK) ON V.REASON_VALUE = C.code
		WHERE
			START_TIME BETWEEN @timestamp_inicio AND @timestamp_final
			AND IS_HARDWARE = 0 AND WORK_MODE = 6
			--AND REASON_KEY = 'ReasonCode'

		UNION ALL

		SELECT 
			V.AGENT_DBID AS agente_dbid
			--,V.REASON_VALUE
			,C.code AS pausa_cod
			,C.[name] AS pausa_nome
			,DATEADD(SECOND, V.START_TIME-10800, '1970-01-01') AS pausa_inicio
			,DATEADD(SECOND, V.END_TIME-10800, '1970-01-01') AS pausa_fim
			,V.DURATION AS pausa_duracao
			,V.IS_HARDWARE AS pausa_ishardware
			,V.WORK_MODE AS pausa_workmode
		FROM DB_GENESYS_DM..VOICE_REASONS AS V
			LEFT JOIN DB_GENESYS_DAT..cfg_action_code AS C ON V.REASON_VALUE = C.code
		WHERE
			START_TIME BETWEEN @timestamp_inicio AND @timestamp_final
			AND IS_HARDWARE = 1 AND WORK_MODE = 0
			--AND REASON_KEY = 'ReasonCode'
	) AS R
	ORDER BY R.agente_dbid, R.pausa_inicio

	RETURN
END