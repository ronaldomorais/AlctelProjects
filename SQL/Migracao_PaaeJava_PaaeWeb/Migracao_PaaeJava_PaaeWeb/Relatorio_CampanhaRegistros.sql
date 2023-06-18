USE AlctelVSS
GO

CREATE PROCEDURE JAVA_hiRegistrosCampanha
	@parUser VARCHAR(50) = ''
	,@dataInicial NVARCHAR(20) = '2021-01-14'
	,@dataFinal NVARCHAR(20) = '2021-01-15'
	,@parAplicacao INT = 1
	,@parSite INT
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE @tCampanhas TABLE (
		NomeCampanha VARCHAR(20)
	)

	INSERT INTO @tCampanhas VALUES
		('Campanha 1')
		,('Campanha 2')
		,('Campanha 3')
		,('Campanha 4')
		,('Campanha 5')
		,('Campanha 6')
		,('Campanha 7')
		,('Campanha 8')
		,('Campanha X')


	SELECT 
		C.NomeCampanha AS ColunaTipoStr01
		,ISNULL(COUNT(R.Registro), 0) AS ColunaTipoInt01
		,ISNULL(SUM(R.RegistrosDisponiveis), 0) AS ColunaTipoInt02
		,ISNULL(SUM(R.RegistrosFinalizados), 0) AS ColunaTipoInt03
	FROM (

		SELECT
			'Campanha 1' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_1

		UNION ALL

		SELECT
			'Campanha 2' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_2

		UNION ALL

		SELECT
			'Campanha 3' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_3


		UNION ALL

		SELECT
			'Campanha 4' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_4


		UNION ALL

		SELECT
			'Campanha 5' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_5


		UNION ALL

		SELECT
			'Campanha 6' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_6


		UNION ALL

		SELECT
			'Campanha 7' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_7


		UNION ALL

		SELECT
			'Campanha 8' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_8


		UNION ALL

		SELECT
			'Campanha X' AS Campanha
			,1 AS Registro
			,CASE WHEN record_status = 1 OR record_status = 2 THEN 1 ELSE 0 END AS RegistrosDisponiveis
			,CASE WHEN record_status <> 1 AND record_status <> 2 THEN 1 ELSE 0 END AS RegistrosFinalizados
		FROM DB_GENESYS_OCS..Campanha_X


	) AS R
	RIGHT JOIN @tCampanhas AS C ON R.Campanha = C.NomeCampanha
	GROUP BY C.NomeCampanha
	ORDER BY C.NomeCampanha
END

	