INSERT INTO AlctelVSS..Alc_mtr001ConfigReport (
	nome
	,titulo
	,perfil
	,tipo
	,refresh
	,ativo
	,col_agrupamento
	,col_ordem
	,label_agrupamento
	,label_estilo
	,paginate
	,timeout_processamento
) VALUES
	--('JAVA_hiAbandonadas', 'Abandonadas', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiCampanhas', 'Campanha', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiRegistrosCampanha', 'Campanha Registros', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiContatosReagendados', 'Contatos Reagendados', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiDiarioAgentes', 'Diario Agentes', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiFilas', 'Filas', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiLogin', 'Login', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiPausas', 'Pausas', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiSLADiaAtivoAgente', 'SLA Dia Ativo - Agentes', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	('JAVA_hiSLASemanaAtivoAgentes', 'SLA Semana Ativo - Agentes', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	
	--,('JAVA_hiURA', 'URA', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiSLADiaAtivoOutbound', 'SLA Dia Ativo - Outbound', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiSLASemanaAtivoOutbound', 'SLA Semana Ativo - Outbound', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiSLASemanaReceptivo', 'SLA Semana Receptivo', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)
	--,('JAVA_hiSLADiaReceptivo', 'SLA Dia Receptivo', 1, 2, 5000, 1, -1, 0, 'Subtotal', '***', 1, 1200)

SELECT *
FROM AlctelVSS..Alc_mtr001ConfigReport

RETURN

TRUNCATE TABLE AlctelVSS..Alc_mtr001ConfigReport







