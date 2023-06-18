USE AlctelVSS
GO

CREATE FUNCTION ufn_R_ALC_QUEUE_DAY(
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
)
RETURNS @tR_ALC_QUEUE_DAY TABLE (
	[data] DATETIME
	,aplicacoes INT
	,NomeGrupo VARCHAR(30)
	,CodFila VARCHAR(30)
	,nomeFila VARCHAR(255)
	,Recebidas DECIMAL(15,0)
	,Atendidas DECIMAL(15,0)
	,Atendidas30 DECIMAL(15,0)
	,Atendidas31 DECIMAL(15,0)
	,Atendidas61 DECIMAL(15,0)
	,Atendidas91 DECIMAL(15,0)
	,Atendidas121 DECIMAL(15,0)
	,Abandonadas DECIMAL(15,0)
	,Abandonadas30 DECIMAL(15,0)
	,Abandonadas31 DECIMAL(15,0)
	,Abandonadas61 DECIMAL(15,0)
	,Abandonadas91 DECIMAL(15,0)
	,Abandonadas121 DECIMAL(15,0)
	,TempoEspera DECIMAL(15,0)
	,TempoAbandono FLOAT
)
AS
BEGIN

	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal

	INSERT INTO @tR_ALC_QUEUE_DAY
	SELECT 
		CONVERT(DATETIME, TIME_KEY, 103) AS [data]
		,G.aplicacoes
		,G.NomeGrupo
		,G.CodFila
		,G.nomeFila	
		,R.N_ENTERED AS Recebidas
		,R.N_ANSWER AS Atendidas
		,R.N_ANSWER_30 AS Atendidas30
		,R.N_ANSWER_31 AS Atendidas31
		,R.N_ANSWER_61 AS Atendidas61
		,R.N_ANSWER_91 AS Atendidas91
		,R.N_ANSWER_121 AS Atendidas121
		,R.N_ABAND AS Abandonadas
		,R.N_ABAND_30 AS Abandonadas30
		,R.N_ABAND_31 AS Abandonadas31
		,R.N_ABAND_61 AS Abandonadas61
		,R.N_ABAND_91 AS Abandonadas91
		,R.N_ABAND_121 AS Abandonadas121
		,R.T_WAIT AS TempoEspera
		,R.A_ABAND AS TempoAbandono
	FROM AlctelVSS..ufn_Grupos() AS G
		LEFT JOIN DB_GENESYS_DM..R_ALC_QUEUE_DAY AS R WITH (NOLOCK) ON G.CodFila = R.[OBJECT_ID]	
	WHERE
		TIME_KEY BETWEEN @data_inicio AND @data_final
	ORDER BY G.NomeGrupo, TIME_KEY

	RETURN

END

