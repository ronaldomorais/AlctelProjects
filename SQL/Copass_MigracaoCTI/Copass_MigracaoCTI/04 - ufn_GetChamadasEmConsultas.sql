USE [AlctelVSS]
GO

/****** Object:  UserDefinedFunction [dbo].[ufn_GetChamadasEmConsultas]    Script Date: 15/03/2023 14:27:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[ufn_GetChamadasEmConsultas] (
	@dataInicio DATETIME = '2023-02-01'
	,@dataFinal DATETIME = '2023-02-01'
)
RETURNS @tChamadasEmConsulta TABLE (
	evt_connid VARCHAR(50)
	,age_agentid VARCHAR(100)
	,age_ini DATETIME
	,age_fim DATETIME
	,age_extensao VARCHAR(50)
	,cha_destino VARCHAR(50)
	,age_agentid_destino VARCHAR(100)
)
AS
BEGIN

	DECLARE
		@inicio_dt DATETIME = CAST(@dataInicio AS DATE)
		,@fim_dt DATETIME = CAST(@dataFinal AS DATE)
		,@CHAMADAS_EM_CONSULTA INT = 0

	SET @fim_dt = DATEADD(SECOND, 86399, @fim_dt)

	INSERT INTO @tChamadasEmConsulta
	SELECT 
		cha_id
		,age_agentid
		,age_ini
		,age_fim
		,age_extensao
		,cha_destino
		,age_agentid_destino
	FROM AlctelSRVCTI..Alc_chamadasGeralConversacao
	WHERE
		age_ini BETWEEN @inicio_dt AND @fim_dt
		AND evt_transferencia = @CHAMADAS_EM_CONSULTA
	
	RETURN
END


GO


