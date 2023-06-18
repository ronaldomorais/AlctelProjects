	
CREATE FUNCTION ufn_Agentes()
RETURNS
	@tPersons TABLE (
		agente_nome VARCHAR(255)
		,agente_cod VARCHAR(20)
		,agente_dbid NUMERIC(10,0)
		,agente_id VARCHAR(255)
		,agente_aplicacoes INT
	)
AS	
BEGIN
	INSERT INTO @tPersons
		SELECT 
			CONCAT(first_name, ' ', last_name) AS agente_nome
			,'0_0_' + CAST([dbid] AS VARCHAR(5)) AS agente_cod
			,[dbid] AS agente_dbid
			,employee_id AS agente_id
			,CASE LEFT(employee_id, 3)
				WHEN 'FVE' THEN 1
				WHEN 'FSA' THEN 1
				WHEN 'FAL' THEN 1
				WHEN 'SEG' THEN 2
				ELSE 0
			END AS agente_aplicacoes
		FROM DB_GENESYS_DAT..cfg_person WITH (NOLOCK)
		WHERE
			is_agent = 2

	RETURN
END