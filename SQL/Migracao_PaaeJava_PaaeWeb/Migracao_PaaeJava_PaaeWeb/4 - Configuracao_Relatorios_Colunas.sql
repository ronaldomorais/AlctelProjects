USE AlctelVSS
GO

INSERT INTO AlctelVSS..Alc_mtr001ConfigCol (
	seq_configreport
	,col_indice
	,col_visivel
	,col_nome
	,col_alinhamento
	,col_dado
	,col_formato
	,col_totalizar
	,col_limites
	,col_abstrata
	,col_fixa
	,col_ativa
	,col_totalizar_ref
	,col_numerador
	,col_denominador
	,col_expansao
	,col_width
	,col_expindice
)VALUES
--Abandonadas
	(1, 1, 1, 'DATA', 1, 'ColunaTipoDateTime01', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(1, 2, 1, 'ORIGEM', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(1, 3, 1, 'FILA', 1, 'ColunaTipoStr02', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(1, 4, 1, 'INFILA', 1, 'ColunaTipoDateTime02', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(1, 5, 1, 'OUTFILA', 1, 'ColunaTipoDateTime03', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

--Campanha
	,(2, 1, 1, 'CAMPANHA', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 2, 1, 'DATA', 1, 'ColunaTipoDateTime01', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 3, 1, 'QTD. LIGAÇÕES', 1, 'ColunaTipoInt01', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 4, 1, 'ATENDIDAS', 1, 'ColunaTipoInt02', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 5, 1, '% AT', 1, 'ColunaTipoDouble01', 'float(6,2)', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 6, 1, 'AGENDADAS', 1, 'ColunaTipoInt03', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 7, 1, 'NAO AT', 1, 'ColunaTipoInt04', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 8, 1, '% NAO AT', 1, 'ColunaTipoDouble02', 'float(6,2)', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 9, 1, 'OCUPADAS', 1, 'ColunaTipoInt05', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 10, 1, 'TEL INEXISTENTES', 1, 'ColunaTipoInt06', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 11, 1, 'CAIXA POSTAL', 1, 'ColunaTipoInt07', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 12, 1, 'FAX', 1, 'ColunaTipoInt08', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 13, 1, 'ERRO LIGACOES', 1, 'ColunaTipoInt09', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(2, 14, 1, 'OUTROS', 1, 'ColunaTipoInt10', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

--Campanha Registros
	,(3, 1, 1, 'CAMPANHA', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(3, 2, 1, 'TOTAL REGISTROS', 1, 'ColunaTipoInt01', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(3, 3, 1, 'REG. DISPONIVEIS', 1, 'ColunaTipoInt02', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(3, 4, 1, 'REG. FINALIZADOS', 1, 'ColunaTipoInt03', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)


--Contatos Reagendados
---?????????

--Diario Agente
	,(5, 1, 1, 'AGENTE', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 2, 1, 'ID', 1, 'ColunaTipoStr02', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 3, 1, 'DATA', 1, 'ColunaTipoDateTime01', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 4, 1, 'LOGIN', 1, 'ColunaTipoDateTime02', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 5, 1, 'LOGOUT', 1, 'ColunaTipoDateTime03', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 6, 1, 'TEMPO LOGADO', 1, 'ColunaTipoInt01', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 7, 1, 'TEMPO DISPONIVEL', 1, 'ColunaTipoInt02', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 8, 1, 'TEMPO PAUSA', 1, 'ColunaTipoInt03', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 9, 1, 'TEMPO ATENDIMENTO', 1, 'ColunaTipoInt04', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 10, 1, 'RECEBIDAS', 1, 'ColunaTipoInt05', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(5, 11, 1, 'SAIDAS', 1, 'ColunaTipoInt06', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

--Filas
	,(6, 1, 1, 'FILA', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(6, 2, 1, 'DATA', 1, 'ColunaTipoDateTime01', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(6, 3, 1, 'RECEBIDAS', 1, 'ColunaTipoInt01', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(6, 4, 1, 'ATENDIDAS', 1, 'ColunaTipoInt02', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(6, 5, 1, '% AT', 1, 'ColunaTipoDouble01', 'float(6,2)', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(6, 6, 1, 'ABANDONADAS', 1, 'ColunaTipoInt03', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(6, 7, 1, '% AB', 1, 'ColunaTipoDouble02', 'float(6,2)', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

--Login 
	,(7, 1, 1, 'AGENTE', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 2, 1, 'ID', 1, 'ColunaTipoStr02', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 3, 1, 'DATA', 1, 'ColunaTipoDateTime01', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 4, 1, 'LOGIN', 1, 'ColunaTipoDateTime02', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 5, 1, 'LOGOUT', 1, 'ColunaTipoDateTime03', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 6, 1, 'TEMPO LOGADO', 1, 'ColunaTipoInt01', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 7, 1, 'TEMPO DISPONIVEL', 1, 'ColunaTipoInt02', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 8, 1, 'TEMPO PAUSA', 1, 'ColunaTipoInt03', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 9, 1, 'TEMPO ATENDIMENTO', 1, 'ColunaTipoInt04', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 10, 1, 'RECEBIDAS', 1, 'ColunaTipoInt05', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(7, 11, 1, 'SAIDAS', 1, 'ColunaTipoInt06', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	  
--Pausas
	,(8, 1, 1, 'AGENTE', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 2, 1, 'ID', 1, 'ColunaTipoStr02', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 3, 1, 'DATA', 1, 'ColunaTipoDateTime01', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 4, 1, 'LOGIN', 1, 'ColunaTipoDateTime02', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 5, 1, 'LOGOUT', 1, 'ColunaTipoDateTime03', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 6, 1, 'TEMPO LOGADO', 1, 'ColunaTipoInt01', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 7, 1, 'TEMPO DISPONIVEL', 1, 'ColunaTipoInt02', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 8, 1, 'TEMPO ATENDIMENTO', 1, 'ColunaTipoInt03', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 9, 1, 'NUMNERO', 1, 'ColunaTipoInt04', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 10, 1, 'INICIO', 1, 'ColunaTipoDateTime04', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 11, 1, 'FIM', 1, 'ColunaTipoDateTime05', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 12, 1, 'DURAÇÃO', 1, 'ColunaTipoInt05', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(8, 13, 1, 'MOTIVO', 1, 'ColunaTipoStr03', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)


--Dia Ativo Agente
	,(9, 1, 1, 'AGENTE', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(9, 2, 1, 'DATA', 1, 'ColunaTipoDateTime01', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(9, 3, 1, 'ATENDIDAS', 1, 'ColunaTipoInt01', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(9, 4, 1, 'TEMPO MEDIA AT.', 1, 'ColunaTipoInt02', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)


--Semana Ativo Agente
	,(10, 1, 1, 'AGENTE', 1, 'ColunaTipoStr01', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(10, 2, 1, 'DATA', 1, 'ColunaTipoStr02', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(10, 3, 1, 'ATENDIDAS', 1, 'ColunaTipoInt01', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
	,(10, 4, 1, 'TEMPO MEDIA AT.', 1, 'ColunaTipoInt02', 'hh:mm:ss', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

----URA
--	,(2, 1, 1, 'DATA', 1, 'data', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(2, 2, 1, 'ORIGEM', 1, 'origem', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(2, 3, 1, 'URA INICIO', 1, 'hora_inicio_ura', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(2, 4, 1, 'URA FIM', 1, 'hora_fim_ura', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(2, 5, 1, 'AGENTE', 1, 'agente', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(2, 6, 1, 'INICIO ATEND.', 1, 'inicio_atendimento', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(2, 7, 1, 'FIM ATEND.', 1, 'fim_atendimento', 'time', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)







----Dia Ativo Outbound
--	,(11, 1, 1, 'DATA', 1, 'data', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(11, 2, 1, 'DISCADAS', 1, 'Discadas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(11, 3, 1, 'ATENDIDAS', 1, 'Atendidas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(11, 4, 1, 'SLA', 1, 'SLA', 'float(6,3)', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(11, 5, 1, 'QTD. LOGADOS', 1, 'AgentesLogados', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(11, 6, 1, 'TEMPO MEDIA AT.', 1, 'TempoMediaAtendimento', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

----Semana Ativo Outbound
--	,(12, 1, 1, 'DIA SEMANA', 1, 'DiaDaSemana', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(12, 2, 1, 'DISCADAS', 1, 'Discadas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(12, 3, 1, 'ATENDIDAS', 1, 'Atendidas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(12, 4, 1, 'SLA', 1, 'SLA', 'float(6,3)', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(12, 5, 1, 'QTD. LOGADOS', 1, 'AgentesLogados', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(12, 6, 1, 'TEMPO MEDIA AT.', 1, 'TempoMediaAtendimento', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

----SLA Semana Receptivo
--	,(13, 1, 1, 'DIA SEMANA', 1, 'DiaDaSemana', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(13, 2, 1, 'RECEBIDAS', 1, 'Recebidas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(13, 3, 1, 'ATENDIDAS', 1, 'Atendidas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(13, 4, 1, 'ABANDONADAS', 1, 'Abandonadas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(13, 5, 1, 'SLA', 1, 'SLA', 'float(6,3)', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(13, 6, 1, 'QTD. LOGADOS', 1, 'AgentesLogados', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(13, 7, 1, 'TEMPO MEDIO ESPERA', 1, 'TempoMedioEspera', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(13, 8, 1, 'TEMPO MEDIO AB.', 1, 'TempoMedioAbandono', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(13, 9, 1, 'TEMPO MEDIA AT.', 1, 'TempoMediaAtendimento', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

----SLA Dia Receptivo
--	,(14, 1, 1, 'DATA', 1, 'data', 'date', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(14, 2, 1, 'RECEBIDAS', 1, 'Recebidas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(14, 3, 1, 'ATENDIDAS', 1, 'Atendidas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(14, 4, 1, 'ABANDONADAS', 1, 'Abandonadas', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(14, 5, 1, 'SLA', 1, 'SLA', 'float(6,3)', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(14, 6, 1, 'QTD. LOGADOS', 1, 'AgentesLogados', 'int', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(14, 7, 1, 'TEMPO MEDIO ESPERA', 1, 'TempoMedioEspera', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(14, 8, 1, 'TEMPO MEDIO AB.', 1, 'TempoMedioAbandono', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)
--	,(14, 9, 1, 'TEMPO MEDIA AT.', 1, 'TempoMediaAtendimento', 'string', 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0)

SELECT 
	R.titulo
	,R.nome
	,C.sequencial
	,C.seq_configreport
	,C.col_indice
	,C.col_visivel
	,C.col_nome
	,C.col_alinhamento
	,C.col_dado
	,C.col_formato
	,C.col_totalizar
	,C.col_limites
	,C.col_abstrata
	,C.col_fixa
	,C.col_ativa
	,C.col_totalizar_ref
	,C.col_numerador
	,C.col_denominador
	,C.col_expansao
	,C.col_width
	,C.col_expindice
FROM AlctelVSS..Alc_mtr001ConfigCol AS C
	INNER JOIN AlctelVSS..Alc_mtr001ConfigReport AS R ON C.seq_configreport = R.sequencial
ORDER BY seq_configreport, col_visivel

RETURN

TRUNCATE TABLE AlctelVSS..Alc_mtr001ConfigCol