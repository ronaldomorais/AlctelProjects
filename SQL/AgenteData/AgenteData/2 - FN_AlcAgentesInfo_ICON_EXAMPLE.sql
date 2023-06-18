DECLARE 
	@tenant_dbid NUMERIC(10,0) = 111
	,@data_inicio DATETIME = '2022-12-21 00:00:00'
	,@data_fim DATETIME = '2022-12-21 23:59:59'
	,@site_id INT = 11

SELECT *
FROM AlctelVSS..ufn_AlcAgentesInfo_ICON (@tenant_dbid, @data_inicio, @data_fim, @site_id)