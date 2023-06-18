USE [AlctelVSS]
GO

/****** Object:  UserDefinedFunction [dbo].[ufn_GetProtocolo]    Script Date: 15/03/2023 14:26:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[ufn_GetProtocolo] (
	@dataInicio DATETIME = '2023-02-01'
	,@dataFinal DATETIME = '2023-02-01'
)
RETURNS @tProtocolos TABLE (
	evt_connid VARCHAR(50)
	,evt_hora DATETIME
	,evt_protocolo VARCHAR(100)
  ,evt_seq INT
)
AS
BEGIN

	DECLARE
		@inicio_dt DATETIME = CAST(@dataInicio AS DATE)
		,@fim_dt DATETIME = CAST(@dataFinal AS DATE)

	SET @fim_dt = DATEADD(SECOND, 86399, @fim_dt)


	DECLARE @tLogChamadas TABLE (
		evt_connid VARCHAR(50)
		,evt_hora DATETIME
		,evt_dado VARCHAR(100)
		,evt_valor VARCHAR(100)
	)

	INSERT INTO @tLogChamadas
	SELECT 
		evt_connid
		,evt_hora
		,evt_dado
		,evt_valor
	FROM AlctelSRVCTI..Alc_mtr001LogChamadas WITH (NOLOCK)
	WHERE
		evt_hora BETWEEN @inicio_dt AND @fim_dt

	INSERT INTO @tProtocolos
	SELECT 
		evt_connid
		,evt_hora
		--,evt_dado
		,evt_valor AS evt_protocolo
		,ROW_NUMBER() OVER(PARTITION BY evt_connid ORDER BY evt_hora) AS evt_seq
	FROM @tLogChamadas
	WHERE
		evt_dado = 'Alc_Nav_Protocolo'

	RETURN
END

GO


