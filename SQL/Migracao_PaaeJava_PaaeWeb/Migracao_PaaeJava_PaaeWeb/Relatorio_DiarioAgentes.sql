USE AlctelVSS
GO


CREATE PROCEDURE JAVA_hiDiarioAgentes
	@parUser VARCHAR(50) = ''
	,@dataInicial NVARCHAR(20) = '2021-01-14'
	,@dataFinal NVARCHAR(20) = '2021-01-15'
	,@parAplicacao INT = 1
	,@parSite INT
AS
BEGIN

	DECLARE 
		@par_user VARCHAR(50) = @parUser
		,@data_inicio DATE = @dataInicial
		,@data_final DATE = @dataFinal
		,@par_aplicacao INT = @parAplicacao
		,@par_site INT = @parSite

	EXEC JAVA_hiLogin @par_user, @data_inicio, @data_final, @par_aplicacao, @par_site

END