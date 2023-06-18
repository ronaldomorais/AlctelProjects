USE AlctelVSS
GO

CREATE FUNCTION ufn_Grupos()
RETURNS
	@tGrupos TABLE(
		aplicacoes INT
		,NomeGrupo VARCHAR(30)
		,CodFila VARCHAR(30)
		,nomeFila VARCHAR(255)
		,nomePresentacao VARCHAR(255)
	)
AS
BEGIN

	INSERT INTO @tGrupos
	SELECT 
		G.aplicacoes
		,G.NomeGrupo
		,C.codFila
		,REPLACE(LEFT(O.[OBJECT_NAME], (CHARINDEX('@', O.[OBJECT_NAME], 0) - 1)), 'VQ_', '') AS nomeFila
		,O.PRESENTATION_NAME
	FROM AlctelVSS..Alc_NomeGrupos AS G WITH (NOLOCK)
		INNER JOIN AlctelVSS..Alc_CodFila AS C WITH (NOLOCK) ON G.NomeGrupo = C.grupo
		INNER JOIN DB_GENESYS_DM..O_ALC_QUEUE_DAY AS O WITH (NOLOCK) ON C.CodFila = O.[OBJECT_ID]
	ORDER BY G.NomeGrupo

	RETURN
END