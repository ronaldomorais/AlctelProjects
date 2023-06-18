
CREATE FUNCTION ufn_R_ALC_CL_HOUR(
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
)
RETURNS @tR_ALC_CL_DAY TABLE (
	[data] DATETIME
	,[OBJECT_ID] VARCHAR(27)
	,Discadas DECIMAL(15,0)
	,Agendadas DECIMAL(15,0)
	,Atendidas DECIMAL(15,0)
	,Abandonadas DECIMAL(15,0)
	,NaoAtendidas DECIMAL(15,0)
	,Ocupadas DECIMAL(15,0)
	,NumeroInvalido DECIMAL(15,0)
	,CaixaPostal DECIMAL(15,0)
	,Fax DECIMAL(15,0)
	,DicadasComErro DECIMAL(15,0)
)
AS
BEGIN
	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal

	INSERT INTO @tR_ALC_CL_DAY
	SELECT 
		CONVERT(DATETIME, CONCAT(LEFT(REPLACE(TIME_KEY, 'BRT', ''), 8), ' ', RIGHT(REPLACE(TIME_KEY, 'BRT', ''), 2), ':00:00'), 121) AS data_hora
		,[OBJECT_ID]
		,N_DIALMADE AS Discadas
		,N_SCHEDULED AS Agendadas
		,N_ANSWER AS Atendidas
		,N_ABAND AS Abandonadas
		,N_NOANSWER AS NaoAtendidas
		,N_BUSY AS Ocupadas
		,N_INVALIDNUM AS NumeroInvalido
		,N_ANSMACHINE AS CaixaPostal
		,N_FAX AS Fax
		,N_DIALERROR AS DiscadasComErro
	FROM DB_GENESYS_DM..R_ALC_CL_HOUR WITH (NOLOCK)
	WHERE
		TIME_KEY BETWEEN CONCAT(REPLACE(@data_inicio, '-', ''), '00BRT') AND CONCAT(REPLACE(@data_final, '-', ''), '23BRT')

	RETURN
END
