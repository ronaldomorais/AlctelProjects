
CREATE FUNCTION ufn_R_122_STAT_RES (
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
)
RETURNS @tR_123_STAT_RES TABLE (
	[data] DATETIME
	,agente_nome VARCHAR(255)
	,agente_dbid NUMERIC(10,0)
	,agente_id VARCHAR(255)
	,agente_cod VARCHAR(20)
	,agente_aplicacoes INT
	,discadas DECIMAL(15,0)
	,chamadas_receptivas DECIMAL(15,0)
	,chamadas_ativas DECIMAL(15,0)
	,chamadas_internas DECIMAL(15,0)
	,tempoLogado DECIMAL(15,0)
	,tempoAtendimentoTotal DECIMAL(15,0)
	,tempoAtendimentoEntrada DECIMAL(15,0)
	,tempoAtendimentoSaida DECIMAL(15,0)
	,tempoAtendimentoInterno DECIMAL(15,0)
)
AS
BEGIN

	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal
		,@par_site INT = @parSite	


	INSERT INTO @tR_123_STAT_RES
	SELECT 
		CONVERT(DATETIME, CONCAT(LEFT(REPLACE(TIME_KEY, 'BRT', ''), 8), ' ', RIGHT(REPLACE(TIME_KEY, 'BRT', ''), 2), ':00:00'), 121) AS data_hora
		,A.agente_nome
		,A.agente_dbid
		,A.agente_id
		,A.agente_cod
		,A.agente_aplicacoes
		,R.N_DIALMADE AS discadas
		,R.N_IN AS chamadas_receptivas
		,R.N_OUT AS chamadas_ativas
		,R.N_INT AS chamadas_internas
		,R.T_LOGIN AS tempoLogado
		,R.T_TALK AS tempoAtendimentoTotal
		,R.T_TALK_IN AS tempoAtendimentoEntrada
		,R.T_TALK_OUT AS tempoAtendimentoSaida
		,R.T_TALK_INT AS tempoAtendimentoInterno
	FROM DB_GENESYS_DM..R_122_STAT_RES AS R WITH (NOLOCK)
		INNER JOIN AlctelVSS.dbo.ufn_Agentes() AS A ON R.[OBJECT_ID] = A.agente_cod
	WHERE
		TIME_KEY BETWEEN CONCAT(REPLACE(@data_inicio, '-', ''), '00BRT') AND CONCAT(REPLACE(@data_final, '-', ''), '23BRT')
	ORDER BY A.agente_nome, R.TIME_KEY

	RETURN
END
