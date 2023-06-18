
CREATE FUNCTION ufn_R_ALC_QUEUE_HOUR(
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
	,Abandonadas DECIMAL(15,0)
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
		CONVERT(DATETIME, CONCAT(LEFT(REPLACE(TIME_KEY, 'BRT', ''), 8), ' ', RIGHT(REPLACE(TIME_KEY, 'BRT', ''), 2), ':00:00'), 121) AS data_hora		,G.aplicacoes
		,G.NomeGrupo
		,G.CodFila
		,G.nomeFila	
		,N_ENTERED AS Recebidas
		,N_ANSWER AS Atendidas
		,N_ABAND AS Abandonadas
		,T_WAIT AS TempoEspera
		,A_ABAND AS TempoAbandono
	FROM AlctelVSS..ufn_Grupos() AS G
		LEFT JOIN DB_GENESYS_DM..R_ALC_QUEUE_HOUR AS R WITH (NOLOCK) ON G.CodFila = R.[OBJECT_ID]	
	WHERE
		TIME_KEY BETWEEN CONCAT(REPLACE(@data_inicio, '-', ''), '00BRT') AND CONCAT(REPLACE(@data_final, '-', ''), '23BRT')
	ORDER BY G.NomeGrupo, TIME_KEY

	RETURN

END

