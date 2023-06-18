USE [I3_IC_40]
GO

/****** Object:  StoredProcedure [Alc_rel].[ALC_GetCallDataByWorkgroup]    Script Date: 18/06/2023 10:53:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





--DECLARE
--	@dataHoraInicio DATETIME   = '2022-07-27 00:00:00'
--	,@dataHoraTermino DATETIME = '2022-07-27 23:59:59'
--	,@workgroup VARCHAR(100) = NULL
--	,@ExcludeLoginID varchar(MAX)

CREATE PROCEDURE [Alc_rel].[ALC_GetCallDataByWorkgroup]
	@dataHoraInicio DATETIME
	,@dataHoraTermino DATETIME
	,@workgroup VARCHAR(100)
	,@ExcludeLoginID varchar(MAX)
AS
BEGIN 
	-- VXX (Direto em Produção)    19.06.2021

/****************************************************************************************************************************************
* ALCTEL - Cliente: Mercado Livre - Stored procedure base, utilizada nos relatórios do Mercado Livre.
* Alterado em	Alterado por				Código (busca código)			Descrição	
* 22/07/2019	Ronaldo Morais/Mauricio H.	Alteracao_22072019				Inclusão de controle de execução do relatório por Usuário/Workgroup
* 07/08/2019	Ronaldo Morais/Mauricio H.	Ajuste_07082019					Ajuste na seleção dos registros das chamadas transferidas diretamente
*																			do agente para um workgroup.
*24/09/2019		Ronaldo Morais				alteracao_inicio_24092019		Modificado o processo para identificação de chamadas transferidas diretamente para workgroups ou agentes
*											alteracao_termino_24092019
*25/09/2019		Ronaldo Morais				ajuste_inicio_25092019			Existem algumas chamadas, que não foram registradas na tabela InteractionSummary e, portanto, não foi possível coletar a data de término da chamada.
											ajuste_termino_25092019			Sendo assim, para esse dado, foi coletado o último registro dessa chamada, na tabela IVRHistory.
*26/09/2019		Ronaldo Morais				ajuste_inicio_26092019			Reavalida a lógica para as chamadas DI (transferidas diretamente), para identificar, cronologicamente, os segmentos das chamadas que passaram pela URA (entrada e transferidas)
											ajuste_termino_26092019			e que foram transferidas para um agente (tabelas IntxSegment e Intx_Participant), para realizar a relação entre as tabelas, para identificar as chamadas que foram transferidas
																			diretamente (sem passar pelo ponto de transferência que é na URA), que são as chamadas transferidas erroneamente.
*27/09/2019		Ronaldo Morais				ajuste_inicio_27092019			Existem chamadas que foram roteadas para dois agentes ao mesmo tempo (3001643486D0181018), ficando na tabela Intx_Participant com o mesmo tempo na coluna StartDateTime.
											ajuste_termino_27092019			Desta forma, na ordenação, que está utilizando essa coluna, ora trazia ordenado de uma forma, ora de outra. Sendo assim, essas chamadas foram tratadas à parte
																			e realizada a ordenação, utilizando a coluna IntxID. Talvez poderia utilizar essa coluna para criar a SequenciaCallId, porém, não sabemos se pode dar erros posteriores.
*07/10/2019		Ronaldo Morais				ajuste_inicio_07102019			O último ajuste (27/09/2019) realizado acima, que foi feito apenas para chamadas como mesmo valor para StartDateTime na tabela Intx_Participant, foi aplicado para todas as
											ajuste_termino_07102019			chamadas. 
*****************************************************************************************************************************************/

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	--DECLARE
	--	        @dataHoraInicio DATETIME = '2019-04-17 00:00:00'
	--	        ,@dataHoraTermino DATETIME = '2019-04-17 23:59:59'
	--	        ,@workgroup VARCHAR(100) = NULL

	DECLARE
		@dataInicial DATETIME = @dataHoraInicio
		,@dataFinal DATETIME = @dataHoraTermino

	IF OBJECT_ID('tempdb..#tbIVRDataToProcessor') IS NOT NULL
		DROP TABLE #tbIVRDataToProcessor

	IF OBJECT_ID('tempdb..#tbIVRData') IS NOT NULL
		DROP TABLE #tbIVRData

	IF OBJECT_ID('tempdb..#tbIVR') IS NOT NULL
		DROP TABLE #tbIVR

	IF OBJECT_ID('tempdb..#tbWorkgroupDataFromIVRToProcess') IS NOT NULL
		DROP TABLE #tbWorkgroupDataFromIVRToProcess

	IF OBJECT_ID('tempdb..#tbWorkgroupDataFromIVR') IS NOT NULL
		DROP TABLE #tbWorkgroupDataFromIVR

	IF OBJECT_ID('tempdb..#tbParticipant') IS NOT NULL
		DROP TABLE #tbParticipant

	IF OBJECT_ID('tempdb..#tbWorkgroupDataToCompare') IS NOT NULL
		DROP TABLE #tbWorkgroupDataToCompare

	IF OBJECT_ID('tempdb..#tbParticipantDataToCompare') IS NOT NULL
		DROP TABLE #tbParticipantDataToCompare

	IF OBJECT_ID('tempdb..#tbCallDataToProcess') IS NOT NULL
		DROP TABLE #tbCallDataToProcess

	IF OBJECT_ID('tempdb..#tbCallData') IS NOT NULL
		DROP TABLE #tbCallData

	IF OBJECT_ID('tempdb..#tbCallsToProcess') IS NOT NULL
		DROP TABLE #tbCallsToProcess

	IF OBJECT_ID('tempdb..#tbCalls') IS NOT NULL
		DROP TABLE #tbCalls

	IF OBJECT_ID('tempdb..#tbCallsWithHoldNotAssignedYet') IS NOT NULL
		DROP TABLE #tbCallsWithHoldNotAssignedYet

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--Coleta dados da URA
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	--Eliminando chamadas (CallId) que entraram na URA mais de uma vez. Ex.: 200166784790180914
	IF OBJECT_ID('tempdb..#tbDuplicatedCallIdInIVR') IS NOT NULL
		DROP TABLE #tbDuplicatedCallIdInIVR

	SELECT InteractionKey AS CallId, COUNT(InteractionKey) AS N
	INTO #tbDuplicatedCallIdInIVR
	FROM dbo.IVRHistory
	WHERE cEventData1 LIKE 'URA%' AND cEventData1 NOT LIKE '% NOVA' 
	AND dEventTime BETWEEN @dataInicial AND @dataFinal 
	GROUP BY InteractionKey
	HAVING COUNT(InteractionKey) > 1


	SELECT
		InteractionKey
		,dEventTime
		,cEventData1
		,I3TimeStampGMT
		,SeqNo
		,CASE WHEN cEventData1 = 'Default Schedule'
			THEN 
				CASE WHEN LAG(LEFT(cEventData1, 4), 1, NULL) OVER (PARTITION BY InteractionKey ORDER BY dEventTime, SeqNo) = 'Fila'
					THEN 1
					ELSE 0
				END
			ELSE 
				1
		END AS IsValidData
	INTO #tbIVRDataToProcessor
	FROM dbo.IVRHistory
	WHERE (cEventData1 LIKE 'Fila%' OR cEventData1 = 'Default Schedule')
		AND dEventTime BETWEEN @dataInicial AND @dataFinal
		AND InteractionKey NOT IN (SELECT CallId FROM #tbDuplicatedCallIdInIVR)
	ORDER BY dEventTime, SeqNo

	

	SELECT 
		InteractionKey
		,dEventTime
		,cEventData1
		,I3TimeStampGMT
		,SeqNo
	INTO #tbIVRData
	FROM #tbIVRDataToProcessor
	WHERE IsValidData = 1

	

	SELECT 
		InteractionKey
		,dEventTime
		,CASE WHEN LEFT(cEventData1, 4) = 'Fila' 
			THEN SUBSTRING(cEventData1, 28, (LEN(cEventData1)-27))
			ELSE 'TRANSF_EVENT'
		END AS cEventData1
		,I3TimeStampGMT
		,SeqNo
	INTO #tbIVR
	FROM #tbIVRData
	ORDER BY InteractionKey, dEventTime, SeqNo

	

	SELECT 
		InteractionKey
		,dEventTime
		,CASE WHEN cEventData1 <> 'TRANSF_EVENT'
			THEN cEventData1
			ELSE NULL
		END AS Workgroup
		,CASE WHEN LEAD(cEventData1, 1, NULL) OVER(PARTITION BY InteractionKey ORDER BY dEventTime, SeqNo) = 'TRANSF_EVENT'
			THEN LEAD(dEventTime, 1, NULL) OVER(PARTITION BY InteractionKey ORDER BY dEventTime, SeqNo)
			ELSE NULL
		END AS HorarioTransferencia
		,I3TimeStampGMT
		,SeqNo
	INTO #tbWorkgroupDataFromIVRToProcess
	FROM #tbIVR
	ORDER BY InteractionKey, dEventTime, SeqNo

	

	SELECT
		ROW_NUMBER() OVER(ORDER BY U.dEventTime, U.SeqNo) AS Sequencia
		,RANK() OVER(PARTITION BY U.InteractionKey ORDER BY U.dEventTime, U.SeqNo) AS SequenciaCallId
		,U.InteractionKey AS CallId
		,I.RemoteNumberFmt
		,C.CustomString1
		,U.Workgroup
		,U.dEventTime AS HorarioEntradaFila
		,U.HorarioTransferencia
		,CAST(I.tHeld/1000. AS INT) AS TempoHold
		--,0 AS TempoHold
		,I.nHeld
		,I.Disposition
		,DATEADD(SECOND, StartDTOffset, TerminatedDateTimeUTC) AS DataTerminoChamada
		,U.dEventTime
		,U.I3TimeStampGMT
		,U.SeqNo
	INTO #tbWorkgroupDataFromIVR
	FROM #tbWorkgroupDataFromIVRToProcess AS U
		LEFT JOIN InteractionSummary AS I ON I.InteractionIDKey = U.InteractionKey
		LEFT JOIN InteractionCustomAttributes AS C ON C.InteractionIDKey = I.InteractionIDKey
	WHERE Workgroup IS NOT NULL 

	

	DELETE FROM #tbWorkgroupDataFromIVR WHERE CallId IN (SELECT InteractionIDKey FROM InteractionSummary WHERE ConnectionType = 2)

	

	--DELETE FROM #tbWorkgroupDataFromIVR where CallId IN('200107041420180824', '200170691050180830','3001161592D0181001')
	DELETE FROM #tbWorkgroupDataFromIVR where CallId IN('200107041420180824', '200170691050180830') --Descobri isto com Valdir


	--ajuste_inicio_25092019
	UPDATE W
		SET W.DataTerminoChamada = (SELECT TOP 1 dEventTime FROM IVRHistory AS I WHERE I.InteractionKey = W.CallId ORDER BY I.dEventTime DESC, I.SeqNo DESC)
	FROM
		#tbWorkgroupDataFromIVR AS W
	WHERE DataTerminoChamada IS NULL
	--ajuste_termino_25092019
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--Coleta dados da Chamada
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

--ajuste_inicio_27092019

	--SELECT
	--	ROW_NUMBER() OVER(ORDER BY P2.StartDateTime) AS Sequencia
	--	,ROW_NUMBER() OVER(PARTITION BY P1.CallIDKey ORDER BY P2.StartDateTime) AS SequenciaCallId
	--	,P1.CallIDKey AS CallId
	--	,S.AssignedWorkGroup AS Workgroup
	--	,A.ICUserID AS UserId
	--	,CASE WHEN MiddleName = ''
	--		THEN CONCAT(FirstName, ' ', LastName)
	--		ELSE CONCAT(FirstName, ' ', MiddleName, ' ', LastName)
	--	END AS FullName
	--	,DATEADD(SECOND, P2.IntxPartDateOffset, P2.StartDateTime) AS InicioConversacao
	--	,P2.Duration AS Duracao
	--	,S.PrevIntxID
	--	,S.NextIntxID
	--INTO #tbParticipant
	--FROM dbo.IntxSegment AS S
	--	INNER JOIN dbo.Intx_Participant AS P1 ON P1.IntxID = S.IntxID AND P1.[Role] = 1
	--	INNER JOIN dbo.Intx_Participant AS P2 ON P2.IntxID = S.IntxID AND P2.[Role] = 2
	--	INNER JOIN dbo.Individual AS A ON A.IndivID = P2.IndivID
	--WHERE 
	--	--P2.StartDateTime BETWEEN DATEADD(SECOND, -P2.IntxPartDateOffset, @dataInicial) AND DATEADD(SECOND, -P2.IntxPartDateOffset, @dataFinal)
	--	--AND P1.CallIDKey NOT IN(SELECT CallId FROM #tbDuplicatedCallIdInIVR)
	--	P1.CallIDKey IN (SELECT DISTINCT (CallId) FROM #tbWorkgroupDataFromIVR)
	--ORDER BY P2.StartDateTime

	--DELETE FROM #tbParticipant WHERE CallId IN('200107041420180824','200170691050180830','3001161592D0181001')


--	SELECT
--		ROW_NUMBER() OVER(ORDER BY P2.StartDateTime) AS Sequencia
--		,ROW_NUMBER() OVER(PARTITION BY P1.CallIDKey ORDER BY P2.StartDateTime) AS SequenciaCallId
--		,P1.CallIDKey AS CallId
--		,S.AssignedWorkGroup AS Workgroup
--		,A.ICUserID AS UserId
--		,CASE WHEN MiddleName = ''
--			THEN CONCAT(FirstName, ' ', LastName)
--			ELSE CONCAT(FirstName, ' ', MiddleName, ' ', LastName)
--		END AS FullName
--		,DATEADD(SECOND, P2.IntxPartDateOffset, P2.StartDateTime) AS InicioConversacao
--		,P2.Duration AS Duracao
--		,P2.IntxID
--		,S.PrevIntxID
--		,S.NextIntxID
--	INTO #tbParticipant
--	FROM dbo.IntxSegment AS S
--		INNER JOIN dbo.Intx_Participant AS P1 ON P1.IntxID = S.IntxID AND P1.[Role] = 1
--		INNER JOIN dbo.Intx_Participant AS P2 ON P2.IntxID = S.IntxID AND P2.[Role] = 2
--		INNER JOIN dbo.Individual AS A ON A.IndivID = P2.IndivID
--	WHERE 
--		--P2.StartDateTime BETWEEN DATEADD(SECOND, -P2.IntxPartDateOffset, @dataInicial) AND DATEADD(SECOND, -P2.IntxPartDateOffset, @dataFinal)
--		--AND P1.CallIDKey NOT IN(SELECT CallId FROM #tbDuplicatedCallIdInIVR)
--		P1.CallIDKey IN (SELECT DISTINCT (CallId) FROM #tbWorkgroupDataFromIVR)
--	ORDER BY P2.StartDateTime

--	DELETE FROM #tbParticipant WHERE CallId IN('200107041420180824','200170691050180830','3001161592D0181001')

--	IF OBJECT_ID('tempdb..#tbAnaliseDeChamadasRoteadas') IS NOT NULL
--		DROP TABLE #tbAnaliseDeChamadasRoteadas

--	IF OBJECT_ID('tempdb..#tbChamadasRoteadasAnalisadas') IS NOT NULL
--		DROP TABLE #tbChamadasRoteadasAnalisadas


--	SELECT
--		Sequencia
--		,SequenciaCallId
--		,CallId
--		,Workgroup
--		,UserId
--		,FullName
--		,InicioConversacao
--		,Duracao
--		,PrevIntxID
--		,NextIntxID
--		,IntxID
--		,CASE WHEN LEAD(InicioConversacao, 1, '1900-01-01 00:00:00') OVER(PARTITION BY CallId ORDER BY SequenciaCallId) = InicioConversacao
--			THEN 1
--			ELSE 
--				CASE WHEN LAG(InicioConversacao, 1, '1900-01-01 00:00:00') OVER(PARTITION BY CallId ORDER BY SequenciaCallId) = InicioConversacao
--					THEN 1
--					ELSE 0
--				END
--		END AS ChamadaDuplamenteRoteada
--	INTO #tbAnaliseDeChamadasRoteadas
--	FROM #tbParticipant



--	SELECT 
--		R.Sequencia
--		--,R.SequenciaCallId
--		,ROW_NUMBER() OVER(PARTITION BY R.CallId ORDER BY R.InicioConversacao, R.IntxID) AS SequenciaCallId
--		,R.CallId
--		,R.Workgroup
--		,R.UserId
--		,R.FullName
--		,R.InicioConversacao
--		,R.Duracao
--		,R.PrevIntxID
--		,R.NextIntxID
--		,R.IntxID
--		,R.ChamadaDuplamenteRoteada
--	INTO #tbChamadasRoteadasAnalisadas
--	FROM 
--		#tbAnaliseDeChamadasRoteadas AS R
--	WHERE ChamadaDuplamenteRoteada = 1


--	UPDATE P
--		SET P.SequenciaCallId = R.SequenciaCallId
--	FROM
--		#tbChamadasRoteadasAnalisadas AS R
--		INNER JOIN #tbParticipant AS P ON P.CallId = R.CallId AND P.UserId = R.UserId AND P.IntxID = R.IntxID
----ajuste_termino_27092019

--ajuste_inicio_07102019
	SELECT
		ROW_NUMBER() OVER(ORDER BY P2.StartDateTime, S.IntxID) AS Sequencia
		,ROW_NUMBER() OVER(PARTITION BY P1.CallIDKey ORDER BY P2.StartDateTime, S.IntxID) AS SequenciaCallId
		,P1.CallIDKey AS CallId
		,S.AssignedWorkGroup AS Workgroup
		,A.ICUserID AS UserId
		,CASE WHEN MiddleName = ''
			THEN CONCAT(FirstName, ' ', LastName)
			ELSE CONCAT(FirstName, ' ', MiddleName, ' ', LastName)
		END AS FullName
		,DATEADD(SECOND, P2.IntxPartDateOffset, P2.StartDateTime) AS InicioConversacao
		,P2.Duration AS Duracao
		,P2.IntxID
		,S.PrevIntxID
		,S.NextIntxID
	INTO #tbParticipant
	FROM dbo.IntxSegment AS S
		INNER JOIN dbo.Intx_Participant AS P1 ON P1.IntxID = S.IntxID AND P1.[Role] = 1
		INNER JOIN dbo.Intx_Participant AS P2 ON P2.IntxID = S.IntxID AND P2.[Role] = 2
		INNER JOIN dbo.Individual AS A ON A.IndivID = P2.IndivID
	WHERE 
		--P2.StartDateTime BETWEEN DATEADD(SECOND, -P2.IntxPartDateOffset, @dataInicial) AND DATEADD(SECOND, -P2.IntxPartDateOffset, @dataFinal)
		--AND P1.CallIDKey NOT IN(SELECT CallId FROM #tbDuplicatedCallIdInIVR)
		P1.CallIDKey IN (SELECT DISTINCT (CallId) FROM #tbWorkgroupDataFromIVR)

	

	ORDER BY P2.StartDateTime

	--DELETE FROM #tbParticipant WHERE CallId IN('200107041420180824','200170691050180830','3001161592D0181001')
	DELETE FROM #tbParticipant WHERE CallId IN('200107041420180824','200170691050180830')--Descobri isto com Valdir


--ajuste_termino_07102019



-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Junção dos dados da URA com os dados das chamadas dos participantes
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	-- Removendo as chamadas que entraram na tabela Participants, porém, não apresenta nem Workgroup e nem agente assinalados, que inclui 
	-- chamadas internas.
	-- Ex.: 100174932930180810

	IF OBJECT_ID('tempdb..#tbIrregularCalls') IS NOT NULL
		DROP TABLE #tbIrregularCalls

	SELECT DISTINCT(CallId) INTO #tbIrregularCalls FROM #tbParticipant WHERE Workgroup IS NULL AND UserId IS NULL

	DELETE FROM #tbWorkgroupDataFromIVR WHERE CallId IN (SELECT CallId FROM #tbIrregularCalls)
	DELETE FROM #tbParticipant WHERE CallId IN (SELECT CallId FROM #tbIrregularCalls)
	-------------------------------------------------------------------------------------------------------------------------------------------
--alteracao_inicio_24092019 
/*


--Tratamento de possíveis transferências entre os agentes. Verificando se a quantidade de chamadas por CallId é igual para as duas tabelas.
--Se Participantes for maior que a da URA, possível transferência entre agentes.

	IF OBJECT_ID('tempdb..#tbCallsDataToProcess') IS NOT NULL
		DROP TABLE #tbCallsDataToProcess

	SELECT CallId, COUNT(*) AS qtd INTO #tbWorkgroupDataToCompare FROM #tbWorkgroupDataFromIVR GROUP BY CallId

	SELECT CallId, COUNT(*) AS qtd INTO #tbParticipantDataToCompare FROM #tbParticipant GROUP BY CallId

	SELECT
		W1.CallId
		,W1.RemoteNumberFmt
		,W1.CustomString1
		,CASE WHEN LAG(P1.Workgroup, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY W1.dEventTime, W1.SeqNo) = W1.Workgroup
			THEN NULL
			ELSE W1.Workgroup
		END AS Workgroup
		--,P1.Workgroup
		,CASE WHEN LAG(P1.Workgroup, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY W1.dEventTime, W1.SeqNo) = W1.Workgroup
			THEN NULL
			ELSE W1.HorarioEntradaFila
		END AS HorarioEntradaFila
		,P1.UserId
		,P1.FullName
		,P1.InicioConversacao
		,P1.Duracao
		,W1.TempoHold
		,W1.nHeld
		,CASE WHEN LEAD(P1.Workgroup, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY W1.dEventTime, W1.SeqNo) = W1.Workgroup
			THEN LEAD(P1.InicioConversacao, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY W1.dEventTime, W1.SeqNo)
			ELSE W1.HorarioTransferencia
		END AS HorarioTransferencia
		--Trecho abaixo para identificar se a chamada passou pelo ponto da URA para marcar como DeTransferencia ou se caso a chamada foi transferida diretamente para o agente (DeTransferenciaDireta).
        ,CASE WHEN LEAD(W1.CallId, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY W1.dEventTime, W1.SeqNo) IS NOT NULL	--Alteracao_22072019_start_new
            THEN 
                CASE WHEN LEAD(P1.Workgroup, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY W1.dEventTime, W1.SeqNo) = W1.Workgroup
                        THEN 'I'
                        ELSE 'D'
                END
            ELSE ''
        END AS TipoTransferencia																						--Alteracao_22072019_end_new
		,W1.Disposition
		,W1.DataTerminoChamada
		,W1.dEventTime
		,W1.I3TimeStampGMT
		,W1.SeqNo
	INTO #tbCallDataToProcess
	FROM #tbWorkgroupDataToCompare AS W
		INNER JOIN #tbParticipantDataToCompare AS P ON P.CallId = W.CallId AND W.qtd < P.qtd
		INNER JOIN #tbWorkgroupDataFromIVR AS W1 ON W1.CallId = W.CallId
		INNER JOIN #tbParticipant AS P1 ON P1.CallId = P.CallId AND P1.Workgroup = W1.Workgroup



-------------------------------------------------------------------------------------------------------------------------------------------------
--R.M.: 22/08/2019:
--Inicio de tratamento das chamadas transferidas diretamente para Workgroups

    IF OBJECT_ID('tempdb..#tbCallsTransferredToWorkgroups') IS NOT NULL
		DROP TABLE #tbCallsTransferredToWorkgroups
	
	SELECT
		P1.CallId
		,P1.Sequencia
		,P1.SequenciaCallId
		--,W1.RemoteNumberFmt
		,CASE WHEN W1.RemoteNumberFmt IS NOT NULL
			THEN W1.RemoteNumberFmt
			ELSE LAG(W1.RemoteNumberFmt, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS RemoteNumberFmt
		--,W1.CustomString1
		,CASE WHEN W1.CustomString1 IS NOT NULL
			THEN W1.CustomString1
			ELSE LAG(W1.CustomString1, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS CustomString1
		,ISNULL(P1.Workgroup, W1.Workgroup) AS Workgroup
		--,W1.HorarioEntradaFila
		,CASE WHEN W1.HorarioEntradaFila IS NOT NULL
			THEN W1.HorarioEntradaFila
			ELSE
				CASE WHEN LAG(P1.CallId, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId) IS NOT NULL
					THEN DATEADD(SECOND, LAG(P1.Duracao, 1, 0) OVER(PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId) , LAG(P1.InicioConversacao, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId))
					ELSE NULL
				END
		END AS HorarioEntradaFila
		,P1.UserId
		,P1.FullName
		,P1.InicioConversacao
		,P1.Duracao
		--,W1.TempoHold
		,CASE WHEN W1.TempoHold IS NOT NULL
			THEN W1.TempoHold
			ELSE LAG(W1.TempoHold, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS TempoHold
		--,W1.nHeld
		,CASE WHEN W1.nHeld IS NOT NULL
			THEN W1.nHeld
			ELSE LAG(W1.nHeld, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS nHeld
		--,HorarioTransferencia
		,CASE WHEN LEAD(P1.CallId, 1, NULL) OVER(PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId) IS NOT NULL
			THEN DATEADD(SECOND, P1.Duracao, P1.InicioConversacao)
			ELSE NULL
		END AS HorarioTransferencia
		--,W1.Disposition
		,CASE WHEN W1.Disposition IS NOT NULL
			THEN W1.Disposition
			ELSE LAG(W1.Disposition, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS Disposition
		--,W1.DataTerminoChamada
		,CASE WHEN W1.DataTerminoChamada IS NOT NULL
			THEN W1.DataTerminoChamada
			ELSE LAG(W1.DataTerminoChamada, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS DataTerminoChamada
		--,W1.dEventTime
		,CASE WHEN W1.dEventTime IS NOT NULL
			THEN W1.dEventTime
			ELSE LAG(W1.dEventTime, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS dEventTime
		--,W1.I3TimeStampGMT
		,CASE WHEN W1.I3TimeStampGMT IS NOT NULL
			THEN W1.I3TimeStampGMT
			ELSE LAG(W1.I3TimeStampGMT, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS I3TimeStampGMT
		--,W1.SeqNo
		,CASE WHEN W1.SeqNo IS NOT NULL
			THEN W1.SeqNo
			ELSE LAG(W1.SeqNo + 1, 1, NULL) OVER (PARTITION BY P1.CallId ORDER BY P1.SequenciaCallId)
		END AS SeqNo
	INTO #tbCallsTransferredToWorkgroups
	FROM #tbWorkgroupDataToCompare AS W
		INNER JOIN #tbParticipantDataToCompare AS P ON P.CallId = W.CallId AND W.qtd < P.qtd
		INNER JOIN #tbWorkgroupDataFromIVR AS W1 ON W1.CallId = W.CallId
		RIGHT JOIN #tbParticipant AS P1 ON P1.CallId = P.CallId AND P1.Workgroup = W1.Workgroup

--Fim de tratamento das chamadas transferidas diretamente para Workgroups
-------------------------------------------------------------------------------------------------------------------------------------------------


	DELETE FROM #tbWorkgroupDataFromIVR WHERE CallId IN(SELECT DISTINCT(CallId) FROM #tbCallDataToProcess)
	DELETE FROM #tbParticipant WHERE CallId IN(SELECT DISTINCT(CallId) FROM #tbCallDataToProcess)


--Tratando o restante das chamadas
	INSERT INTO #tbCallDataToProcess
	SELECT 
		W.CallId
		,RemoteNumberFmt
		,CustomString1
		,W.Workgroup
		,W.HorarioEntradaFila
		,P.UserId
		,ISNULL(P.FullName, '') AS FullName
		,P.InicioConversacao
		,P.Duracao
		,ISNULL(W.TempoHold, 0) AS TempoHold
		,ISNULL(W.nHeld, 0) AS nHeld
		,W.HorarioTransferencia
		--Trecho abaixo para identificar se a chamada passou pelo ponto da URA para marcar como DeTransferencia ou se caso a chamada foi transferida diretamente para o workgroup (DeTransferenciaDireta).

        ,CASE WHEN LEAD(W.CallId, 1, NULL) OVER(PARTITION BY W.CallId ORDER BY W.dEventTime, W.SeqNo) IS NULL	--Alteracao_22072019_start_new
                THEN ''
                ELSE 'D'
        END AS TipoTransferencia																				--Alteracao_22072019_end_new
		,W.Disposition
		,W.DataTerminoChamada
		,W.dEventTime
		,W.I3TimeStampGMT
		,W.SeqNo
	FROM #tbWorkgroupDataFromIVR AS W
		LEFT JOIN #tbParticipant AS P ON P.CallId = W.CallId AND P.SequenciaCallId = W.SequenciaCallId  
 	ORDER BY W.CallId, W.HorarioEntradaFila


-------------------------------------------------------------------------------------------------------------------------------------------------
--R.M.: 22/08/2019:
--Inicio de tratamento das chamadas transferidas diretamente para Workgroups

	--Remove chamadas que foram identificadas no processo de tratamento de chamadas internas e que serão tratadas no processo de chamadas 
	--transferidas diretamente para workgroups.
	DELETE FROM #tbCallDataToProcess 
		WHERE CallId IN(
			SELECT DISTINCT(T.CallId) AS CallId
			FROM 
				#tbCallDataToProcess AS C
				RIGHT JOIN #tbCallsTransferredToWorkgroups AS T ON T.CallId = C.CallId AND T.Workgroup = C.Workgroup
			WHERE C.CallId IS NULL)

	--Agora, remove as chamadas que já foram identificadas como internas e remove da tabela que irá tratar as chamadas transferidas diretamente
	--para Workgroups.
	DELETE FROM #tbCallsTransferredToWorkgroups WHERE CallId IN(SELECT DISTINCT(CallId) FROM #tbCallDataToProcess)

	--Inicio de caso onde está faltando o termo Fila de grupo de trabalho: na IVRHistory. Ex.: 100147424330180801

	IF OBJECT_ID('tempdb..#tbCallsIdInconsistent') IS NOT NULL
		DROP TABLE #tbCallsIdInconsistent
	IF OBJECT_ID('tempdb..#tbCallsTransferredInconsistentTreated') IS NOT NULL
		DROP TABLE #tbCallsTransferredInconsistentTreated


	SELECT CallId INTO #tbCallsIdInconsistent FROM #tbCallsTransferredToWorkgroups WHERE SeqNo IS NULL

	SELECT
		CallId
		,Sequencia
		,SequenciaCallId
		--,RemoteNumberFmt
		,CASE WHEN RemoteNumberFmt IS NOT NULL
			THEN RemoteNumberFmt
			ELSE LEAD(RemoteNumberFmt, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS RemoteNumberFmt
		--,CustomString1
		,CASE WHEN CustomString1 IS NOT NULL
			THEN CustomString1
			ELSE LEAD(CustomString1, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS CustomString1
		,Workgroup
		--,HorarioEntradaFila
		,CASE WHEN HorarioEntradaFila IS NOT NULL	
			THEN HorarioEntradaFila
			ELSE InicioConversacao
		END AS HorarioEntradaFila
		,UserId
		,FullName
		,InicioConversacao
		,Duracao
		--,TempoHold
		,CASE WHEN TempoHold IS NOT NULL
			THEN TempoHold
			ELSE LEAD(TempoHold, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS TempoHold
		--,nHeld
		,CASE WHEN nHeld IS NOT NULL
			THEN nHeld
			ELSE LEAD(nHeld, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS nHeld
		,HorarioTransferencia
		--,Disposition
		,CASE WHEN Disposition IS NOT NULL
			THEN Disposition
			ELSE LEAD(Disposition, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS Disposition
		--,DataTerminoChamada
		,CASE WHEN DataTerminoChamada IS NOT NULL
			THEN DataTerminoChamada
			ELSE LEAD(DataTerminoChamada, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS DataTerminoChamada
		--,dEventTime
		,CASE WHEN dEventTime IS NOT NULL
			THEN dEventTime
			ELSE LEAD(dEventTime, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS dEventTime
		--,I3TimeStampGMT
		,CASE WHEN I3TimeStampGMT IS NOT NULL
			THEN I3TimeStampGMT
			ELSE LEAD(I3TimeStampGMT, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS I3TimeStampGMT
		--,SeqNo
		,CASE WHEN SeqNo IS NOT NULL
			THEN SeqNo
			ELSE LEAD(SeqNo - 1, 1, '') OVER(PARTITION BY CallId ORDER BY SequenciaCallId)
		END AS SeqNo
	INTO #tbCallsTransferredInconsistentTreated
	FROM #tbCallsTransferredToWorkgroups 
	WHERE CallId IN(SELECT CallId FROM #tbCallsIdInconsistent)

	DELETE FROM #tbCallsTransferredToWorkgroups WHERE CallId IN (SELECT CallId FROM #tbCallsIdInconsistent)

	INSERT INTO #tbCallsTransferredToWorkgroups
	SELECT
		CallId
		,Sequencia
		,SequenciaCallId
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,UserId
		,FullName
		,InicioConversacao
		,Duracao
		,TempoHold
		,nHeld
		,HorarioTransferencia
		,Disposition
		,DataTerminoChamada
		,dEventTime
		,I3TimeStampGMT
		,SeqNo
	FROM #tbCallsTransferredInconsistentTreated

	--Fim de caso onde está faltando o termo Fila de grupo de trabalho: na IVRHistory. Ex.: 100147424330180801
	INSERT INTO #tbCallDataToProcess
	SELECT 
		CallId
		--,Sequencia
		--,SequenciaCallId
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,UserId
		,FullName
		,InicioConversacao
		,Duracao
		,TempoHold
		,nHeld
		,HorarioTransferencia
        ,CASE WHEN LEAD(CallId, 1, NULL) OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo) IS NOT NULL	--Alteracao_22072019_start_new
            THEN 'I'
            ELSE ''
		END AS TipoTransferencia
		,Disposition
		,DataTerminoChamada
		--,dEventTime
		--,I3TimeStampGMT
		--,SeqNo
		,ISNULL(dEventTime, GETDATE()) AS dEventTime
		,ISNULL(I3TimeStampGMT, GETDATE()) AS I3TimeStampGMT
		,ISNULL(SeqNo, -1) AS SeqNo
	FROM #tbCallsTransferredToWorkgroups

--Fim de tratamento das chamadas transferidas diretamente para Workgroups
-------------------------------------------------------------------------------------------------------------------------------------------------



--Restante de cálculos, acerto de tempos, etc
	SELECT
		ROW_NUMBER() OVER(ORDER BY dEventTime, SeqNo) AS Sequencia
		,RANK() OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo) AS SeqCallId
		,CallId		
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,CASE WHEN HorarioEntradaFila IS NULL
			THEN 0
			--ELSE ISNULL(DATEDIFF(SECOND, HorarioEntradaFila, InicioConversacao), 0)
			ELSE 
				CASE WHEN UserId IS NOT NULL
					THEN ISNULL(DATEDIFF(SECOND, HorarioEntradaFila, InicioConversacao), 0)
					ELSE DATEDIFF(SECOND, HorarioEntradaFila, DataTerminoChamada)
				END
		END AS TempoFila
		,UserId
		,FullName
		,InicioConversacao
		,CASE WHEN HorarioTransferencia IS NULL
			THEN DATEADD(SECOND, Duracao, InicioConversacao)
			ELSE HorarioTransferencia
		END AS TerminoConversacao
		,CASE WHEN HorarioTransferencia IS NULL
			THEN ISNULL(Duracao, 0)
			ELSE ISNULL(DATEDIFF(SECOND, InicioConversacao, HorarioTransferencia), 0)
		END AS TempoConversacao
		,TempoHold
		,nHeld
		,HorarioTransferencia
		,CASE WHEN HorarioTransferencia IS NULL
			THEN 0
			ELSE 1
		END AS Transferida
		,CASE WHEN LAG(HorarioTransferencia, 1, NULL) OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo) IS NULL
			THEN 0
			--ELSE CASE WHEN HorarioEntradaFila IS NOT NULL																									--Alteracao_22072019_old
			ELSE CASE WHEN HorarioEntradaFila IS NOT NULL AND LAG(TipoTransferencia, 1, NULL) OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo) = 'D'	--Alteracao_22072019_new
					THEN 1
					ELSE 0
				END
		END AS DeTransferencia
		,CASE WHEN LAG(HorarioTransferencia, 1, NULL) OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo) IS NULL
			THEN 0
			--ELSE CASE WHEN HorarioEntradaFila IS NULL									--Alteracao_22072019_start_old
			--		THEN 1
			--		ELSE 0
			--	END																		--Alteracao_22072019_end_old
            ELSE CASE WHEN HorarioEntradaFila IS NULL									--Alteracao_22072019_start_new
                        THEN 1
                        ELSE CASE WHEN LAG(TipoTransferencia, 1, NULL) OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo) = 'I'
                                THEN 1
                                ELSE 0
                        END
            END																			--Alteracao_22072019_end_new		
		END AS DeTransferenciaDireta
		,DataTerminoChamada
		,CASE Disposition
			WHEN 0 THEN 'Desconhecida'
			WHEN 1 THEN 'Desconectada'
			WHEN 2 THEN 'Desconexao Remota em Fila'
			WHEN 3 THEN 'Desconexao Local em Fila'
			WHEN 4 THEN 'Desconexao Remota Tocando'
			WHEN 5 THEN 'Desconexao Local Tocando'
			WHEN 6 THEN 'Desconexao Remota'
			WHEN 7 THEN 'Desconexao Local'
			ELSE 'Desconhecida'
		END AS Desconexao		
		,dEventTime		
		,I3TimeStampGMT
		,SeqNo
	INTO #tbCallData
	FROM #tbCallDataToProcess
*/



	IF OBJECT_ID('tempdb..#tbCallsDataToProcess') IS NOT NULL
		DROP TABLE #tbCallsDataToProcess
	IF OBJECT_ID('tempdb..#tbWorkgroupDataFromIVRQty') IS NOT NULL
		DROP TABLE #tbWorkgroupDataFromIVRQty
	IF OBJECT_ID('tempdb..#tbParticipantQty') IS NOT NULL
		DROP TABLE #tbParticipantQty
	IF OBJECT_ID('tempdb..#tbCallStatusResult') IS NOT NULL
		DROP TABLE #tbCallStatusResult
	IF OBJECT_ID('tempdb..#tbParticipant_CallStatusDI') IS NOT NULL
		DROP TABLE #tbParticipant_CallStatusDI
	IF OBJECT_ID('tempdb..#tbWorkgroupDataFromIVR_CallStatusDI') IS NOT NULL
		DROP TABLE #tbWorkgroupDataFromIVR_CallStatusDI


	SELECT CallId, COUNT(*) AS Qtd INTO #tbWorkgroupDataFromIVRQty FROM #tbWorkgroupDataFromIVR GROUP BY CallId ORDER BY CallId
	SELECT CallId, COUNT(*) AS Qtd INTO #tbParticipantQty FROM #tbParticipant GROUP BY CallId ORDER BY CallId


	SELECT
		W.CallId
		--,P.CallId
		,W.Qtd AS W_Qtd
		,P.Qtd AS P_Qtd
		,CASE WHEN W.Qtd = P.Qtd
			THEN 'OK' --Chamada com a mesma quantidade de segmentos entre IVR e Participantes.
			ELSE
				CASE WHEN W.Qtd > P.Qtd
					THEN 'AB' --Chamada com quantidade maior de segmentos na IVR. Possível abandono na fila.
					ELSE 'DI' --Chamada com quantidade menor de segmentos na IVR. Possíveis transferências diretas para workgroups ou agentes
				END
		END AS CallStatus
	INTO #tbCallStatusResult
	FROM
		#tbWorkgroupDataFromIVRQty AS W
		INNER JOIN #tbParticipantQty AS P ON P.CallId = W.CallId

	--Tratando as chamadas com CallStatus OK
	SELECT 
		W.CallId
		,W.RemoteNumberFmt
		,W.CustomString1
		,CASE WHEN LAG(P.Workgroup, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY W.dEventTime, W.SeqNo) = W.Workgroup
			THEN NULL
			ELSE W.Workgroup
		END AS Workgroup
		,CASE WHEN LAG(P.Workgroup, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY W.dEventTime, W.SeqNo) = W.Workgroup
			THEN NULL
			ELSE W.HorarioEntradaFila
		END AS HorarioEntradaFila
		,P.UserId
		,P.FullName
		,P.InicioConversacao
		,P.Duracao
		,W.TempoHold
		,W.nHeld
		,CASE WHEN LEAD(P.Workgroup, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY W.dEventTime, W.SeqNo) = W.Workgroup
			THEN LEAD(P.InicioConversacao, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY W.dEventTime, W.SeqNo)
			ELSE W.HorarioTransferencia
		END AS HorarioTransferencia
		,CASE WHEN LEAD(W.CallId, 1, NULL) OVER(PARTITION BY W.CallId ORDER BY W.dEventTime, W.SeqNo) IS NOT NULL
			THEN 'I' 
			ELSE ''
		END AS TipoTransferencia
		,W.Disposition
		,W.DataTerminoChamada
		,W.dEventTime
		,W.I3TimeStampGMT
		,W.SeqNo
	INTO #tbCallDataToProcess
	FROM
		#tbWorkgroupDataFromIVR AS W
		INNER JOIN #tbParticipant AS P ON P.CallId = W.CallId AND P.SequenciaCallId = W.SequenciaCallId AND P.Workgroup = W.Workgroup
	WHERE
		W.CallId IN(SELECT CallId FROM #tbCallStatusResult AS R WHERE R.CallStatus = 'OK')
	ORDER BY W.CallId, W.SequenciaCallId


	--Tratando as chamadas com CallStatus AB
	INSERT INTO #tbCallDataToProcess
	SELECT 
		W.CallId
		,W.RemoteNumberFmt
		,W.CustomString1
		,CASE WHEN LAG(P.Workgroup, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY W.dEventTime, W.SeqNo) = W.Workgroup
			THEN NULL
			ELSE W.Workgroup
		END AS Workgroup
		,CASE WHEN LAG(P.Workgroup, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY W.dEventTime, W.SeqNo) = W.Workgroup
			THEN NULL
			ELSE W.HorarioEntradaFila
		END AS HorarioEntradaFila
		,P.UserId
		,ISNULL(P.FullName, '') AS FullName
		,P.InicioConversacao
		,P.Duracao
		,W.TempoHold
		,W.nHeld
		,CASE WHEN LEAD(P.Workgroup, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY W.dEventTime, W.SeqNo) = W.Workgroup
			THEN LEAD(P.InicioConversacao, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY W.dEventTime, W.SeqNo)
			ELSE W.HorarioTransferencia
		END AS HorarioTransferencia
		,CASE WHEN LEAD(W.CallId, 1, NULL) OVER(PARTITION BY W.CallId ORDER BY W.dEventTime, W.SeqNo) IS NOT NULL
			THEN 'I' 
			ELSE ''
		END AS TipoTransferencia
		,W.Disposition
		,W.DataTerminoChamada
		,W.dEventTime
		,W.I3TimeStampGMT
		,W.SeqNo
	FROM
		#tbWorkgroupDataFromIVR AS W
		LEFT JOIN #tbParticipant AS P ON P.CallId = W.CallId AND P.Workgroup = W.Workgroup AND P.SequenciaCallId = W.SequenciaCallId
	WHERE
		W.CallId IN(SELECT CallId FROM #tbCallStatusResult AS R WHERE R.CallStatus = 'AB')
	ORDER BY W.CallId, W.SequenciaCallId

	--Tratando as chamadas com CallStatus DI

--ajuste_inicio_26092019
/*
	SELECT 
		*
		,RANK() OVER(PARTITION BY CallId, Workgroup ORDER BY SequenciaCallId) AS SequenciaWG 
	INTO #tbWorkgroupDataFromIVR_CallStatusDI
	FROM #tbWorkgroupDataFromIVR
	ORDER BY CallId, SequenciaCallId 

	SELECT 
		*
		,RANK() OVER(PARTITION BY CallId, Workgroup ORDER BY SequenciaCallId) AS SequenciaWG 
	INTO #tbParticipant_CallStatusDI
	FROM #tbParticipant 
	ORDER BY CallId, SequenciaCallId
*/
	--Aqui realiza a união da tabela de workgroups (IVRHistory) e da tabela de participantes, onde há diferença de quantidade de segmentos entre elas, tendo como HorarioDoSegmento, a união dos dados HorarioEntradaFila e InicioConversacao,
	--para identificar a sequencia dos segmentos da chamada

	IF OBJECT_ID('tempdb..#tbCallSegmentSequence') IS NOT NULL
		DROP TABLE #tbCallSegmentSequence
	IF OBJECT_ID('tempdb..#tbCallSegmentSequenceProcessed') IS NOT NULL
		DROP TABLE #tbCallSegmentSequenceProcessed

	SELECT 
		CallId
		,SequenciaCallId
		,Workgroup
		,HorarioEntradaFila AS HorarioDoSegmento
		,RANK() OVER(PARTITION BY CallId, Workgroup ORDER BY SequenciaCallId) AS SequenciaWG 
		,1 AS Origem --origem da tabela de WorkgroupDataFromIVR (IVRHistory)
	INTO #tbCallSegmentSequence
	FROM #tbWorkgroupDataFromIVR

	UNION ALL

	SELECT 
		CallId
		,SequenciaCallId
		,Workgroup
		,InicioConversacao AS HorarioDoSegmento
		,0 AS SequenciaWG 
		,2 AS Origem --origem da tabela de #tbParticipant (IntxSegment x Intx_Participant)
	FROM #tbParticipant 
	ORDER BY CallId, SequenciaCallId

	SELECT
		CallId
		,SequenciaCallId
		,Workgroup
		,HorarioDoSegmento
		,SequenciaWG
		,Origem
		,CASE WHEN Origem = 2 --Quando a chamada é da participante (com um agente associado) verifica se o segmento anterior é de origem da IVRHistory, para realizar o join entre as tabelas.
			THEN 
				CASE WHEN LAG(Origem, 1, NULL) OVER(PARTITION BY CallId ORDER BY  HorarioDoSegmento, SequenciaCallId, Origem) = 1	--Verifica se anterior é de origem da IVRHistory
					THEN LAG(SequenciaWG, 1, NULL) OVER(PARTITION BY CallId ORDER BY  HorarioDoSegmento, SequenciaCallId, Origem)	--Caso for, iguala o dado SequenciaWG da tabela origem de participante com a da origem da IVRHistory
					ELSE SequenciaWG																								--Desta forma, consegue realizar o join entre os dados da tabela cronologicamente.
				END
			ELSE SequenciaWG
		END AS SequenciaWGNew
	INTO #tbCallSegmentSequenceProcessed
	FROM #tbCallSegmentSequence ORDER BY HorarioDoSegmento, CallId, SequenciaCallId

	SELECT 
		*
		,RANK() OVER(PARTITION BY CallId, Workgroup ORDER BY SequenciaCallId) AS SequenciaWG 
	INTO #tbWorkgroupDataFromIVR_CallStatusDI
	FROM #tbWorkgroupDataFromIVR
	ORDER BY CallId, SequenciaCallId 

	SELECT 
		*
		,RANK() OVER(PARTITION BY CallId, Workgroup ORDER BY SequenciaCallId) AS SequenciaWG 
	INTO #tbParticipant_CallStatusDI
	FROM #tbParticipant 
	ORDER BY CallId, SequenciaCallId

	--Como a tabela temporária #tbCallSegmentSequenceProcessed, tem o dado SegmentoWGNew, atualiza esse dado na tabela originada da Participante para realizar o join com a tabela originada da IVRHistory
	UPDATE P
		SET P.SequenciaWG = C.SequenciaWGNew 
	FROM #tbParticipant_CallStatusDI AS P
		INNER JOIN #tbCallSegmentSequenceProcessed AS C ON C.CallId = P.CallId AND C.SequenciaCallId = P.SequenciaCallId AND C.Origem = 2
	--ORDER BY P.CallId, P.SequenciaCallId






--ajuste_termino_26092019

	INSERT INTO #tbCallDataToProcess
	SELECT 
		P.CallId
		,(SELECT TOP 1 I.RemoteNumberFmt FROM #tbWorkgroupDataFromIVR AS I WHERE I.CallId = P.CallId ORDER BY I.SequenciaCallId) AS RemoteNumberFmt
		,(SELECT TOP 1 I.CustomString1 FROM #tbWorkgroupDataFromIVR AS I WHERE I.CallId = P.CallId ORDER BY I.SequenciaCallId) AS CustomString1
		,P.Workgroup
		,COALESCE(W.HorarioEntradaFila, P.InicioConversacao, NULL) AS HorarioEntradaFila
		,P.UserId
		,P.FullName
		,P.InicioConversacao
		,P.Duracao
		,(SELECT TOP 1 I.TempoHold FROM #tbWorkgroupDataFromIVR AS I WHERE I.CallId = P.CallId ORDER BY I.SequenciaCallId) AS TempoHold
		,(SELECT TOP 1 I.nHeld FROM #tbWorkgroupDataFromIVR AS I WHERE I.CallId = P.CallId ORDER BY I.SequenciaCallId) AS nHeld
		,CASE WHEN LEAD(W.HorarioEntradaFila, 1, '1900-01-01 00:00:00') OVER(PARTITION BY P.CallId ORDER BY P.SequenciaCallId) IS NULL
			THEN LEAD(P.InicioConversacao, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY P.SequenciaCallId)
			ELSE CASE WHEN LEAD(W.HorarioEntradaFila, 1, '1900-01-01 00:00:00') OVER(PARTITION BY P.CallId ORDER BY P.SequenciaCallId) = '1900-01-01 00:00:00'
				THEN NULL
				ELSE LEAD(W.HorarioEntradaFila, 1, NULL) OVER(PARTITION BY P.CallId ORDER BY P.SequenciaCallId)
			END
		END AS HorarioTransferencia
		,CASE WHEN LEAD(W.HorarioEntradaFila, 1, '1900-01-01 00:00:00') OVER(PARTITION BY P.CallId ORDER BY P.SequenciaCallId) IS NULL
			THEN 'D'
			ELSE CASE WHEN LEAD(W.HorarioEntradaFila, 1, '1900-01-01 00:00:00') OVER(PARTITION BY P.CallId ORDER BY P.SequenciaCallId) = '1900-01-01 00:00:00'
				THEN ''
				ELSE 'I'
			END
		END AS TipoTransferencia
		,(SELECT TOP 1 I.Disposition FROM #tbWorkgroupDataFromIVR AS I WHERE I.CallId = P.CallId ORDER BY I.SequenciaCallId) AS Disposition
		,(SELECT TOP 1 I.DataTerminoChamada FROM #tbWorkgroupDataFromIVR AS I WHERE I.CallId = P.CallId ORDER BY I.SequenciaCallId) AS DataTerminoChamada
		,COALESCE(W.dEventTime, P.InicioConversacao, NULL) AS dEventTime
		,COALESCE(W.I3TimeStampGMT, W.HorarioEntradaFila, P.InicioConversacao, NULL) AS I3TimeStampGMT
		,CASE WHEN W.SeqNo IS NULL
			THEN ISNULL(LAG(W.SeqNo, 1, 0) OVER(PARTITION BY P.CallId ORDER BY P.SequenciaCallId), 1) + 1
			ELSE W.SeqNo
		END AS SeqNo
	FROM
		#tbWorkgroupDataFromIVR_CallStatusDI AS W
		RIGHT JOIN #tbParticipant_CallStatusDI AS P ON P.CallId = W.CallId AND P.Workgroup = W.Workgroup AND P.SequenciaWG = W.SequenciaWG 
	WHERE
		P.CallId IN(SELECT CallId FROM #tbCallStatusResult AS R WHERE R.CallStatus = 'DI')
	ORDER BY P.CallId, P.SequenciaCallId

	--Tratando chamadas que aparecem na IVR, mas não aparece na participantes. Possíveis abandonos sem atendimentos.
	INSERT INTO #tbCallDataToProcess
	SELECT 
		CallId
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,NULL AS UserId
		,'' AS FullName
		,NULL AS InicioConversacao
		,0 AS Duracao
		,0 AS TempoHold
		,0 AS nHeld
		,HorarioTransferencia
		,'' AS TipoTransferencia 
		,Disposition
		,DataTerminoChamada
		,dEventTime
		,I3TimeStampGMT
		,SeqNo	 
	FROM #tbWorkgroupDataFromIVR WHERE CallId NOT IN(SELECT CallId FROM #tbParticipant)

--Restante de cálculos, acerto de tempos, etc
	SELECT
		ROW_NUMBER() OVER(ORDER BY dEventTime, SeqNo) AS Sequencia
		,RANK() OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo) AS SeqCallId
		,CallId		
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,CASE WHEN HorarioEntradaFila IS NULL
			THEN 0
			--ELSE ISNULL(DATEDIFF(SECOND, HorarioEntradaFila, InicioConversacao), 0)
			ELSE 
				CASE WHEN UserId IS NOT NULL
					THEN ISNULL(DATEDIFF(SECOND, HorarioEntradaFila, InicioConversacao), 0)
					ELSE DATEDIFF(SECOND, HorarioEntradaFila, DataTerminoChamada)
				END
		END AS TempoFila
		,UserId
		,FullName
		,InicioConversacao
		,CASE WHEN HorarioTransferencia IS NULL
			THEN DATEADD(SECOND, Duracao, InicioConversacao)
			ELSE HorarioTransferencia
		END AS TerminoConversacao
		,CASE WHEN HorarioTransferencia IS NULL
			THEN ISNULL(Duracao, 0)
			ELSE ISNULL(DATEDIFF(SECOND, InicioConversacao, HorarioTransferencia), 0)
		END AS TempoConversacao
		,TempoHold
		,nHeld
		,HorarioTransferencia


		-----------------------------------------------------------------------------
		,CASE WHEN HorarioTransferencia IS NULL
			THEN 0
			ELSE 1
		END AS Transferida --Exite um acerto deste campo no final da Procedure

		,CASE WHEN LAG(HorarioTransferencia, 1, NULL) OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo) IS NULL
			THEN 0
			ELSE CASE WHEN HorarioEntradaFila IS NOT NULL
					THEN 1
					ELSE 0
				END
		END AS DeTransferencia

		,CASE WHEN HorarioTransferencia IS NULL
			THEN 0
			ELSE CASE WHEN TipoTransferencia = 'D'
				THEN 1
				ELSE 0
			END
		END AS DeTransferenciaDireta

		-----------------------------------------------------------------------------


		,DataTerminoChamada
		,CASE Disposition
			WHEN 0 THEN 'Desconhecida'
			WHEN 1 THEN 'Desconectada'
			WHEN 2 THEN 'Desconexao Remota em Fila'
			WHEN 3 THEN 'Desconexao Local em Fila'
			WHEN 4 THEN 'Desconexao Remota Tocando'
			WHEN 5 THEN 'Desconexao Local Tocando'
			WHEN 6 THEN 'Desconexao Remota'
			WHEN 7 THEN 'Desconexao Local'
			ELSE 'Desconhecida'
		END AS Desconexao		
		,dEventTime		
		,I3TimeStampGMT
		,SeqNo
	INTO #tbCallData
	FROM #tbCallDataToProcess

--alteracao_termino_24092019



----Existem algumas chamadas, que ocorreu algum problema no sistema, onde o tempo de hold é maior que o tempo de conversação
----Sendo assim, essas chamadas são removidas
--	IF OBJECT_ID('tempdb..#tbCallsWithHoldTimeException') IS NOT NULL
--		DROP TABLE #tbCallsWithHoldTimeException

--	SELECT 
--		CallId, SUM(TempoConversacao) AS TempoConversacao, MAX(TempoHold) AS TempoHold 
--	INTO #tbCallsWithHoldTimeException
--	FROM #tbCallData
--	GROUP BY CallId
--	HAVING SUM(TempoConversacao) < MAX(TempoHold)

--	DELETE FROM #tbCallData WHERE CallId IN(SELECT CallId FROM #tbCallsWithHoldTimeException)

--Remove chamadas que não se conseguiu calcular o tempo de conversação por falta de informação.
	SELECT CallId INTO #tbCallsToRemove FROM #tbCallData WHERE TempoConversacao < 0
	DELETE FROM #tbCallData WHERE CallId IN (SELECT CallId FROM #tbCallsToRemove)
	DROP TABLE #tbCallsToRemove


--Tratamento do tempo Hold: para chamadas que houve transferências e tempo hold, primeiramente, atribui o tempo de hold para a primeira perna da chamada.
--Caso o tempo de conversação for menor do que o tempo de hold, verifica se o tempo de conversação da segunda é maior que o tempo hold e 
--assim sucessivamente.
	SELECT
		Sequencia
		,SeqCallId
		,CallId
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,TempoFila
		,UserId
		,FullName
		,InicioConversacao
		,TerminoConversacao
		,TempoConversacao
		,TempoHold
		,nHeld
		,HorarioTransferencia
		,Transferida
		,DeTransferencia
		,DeTransferenciaDireta
		,DataTerminoChamada
		,Desconexao
		,CASE WHEN TempoConversacao >= TempoHold
			THEN 1
			ELSE 0
		END AS UsarTempoHold
		,dEventTime
		,I3TimeStampGMT
		,SeqNo
	INTO #tbCallsToProcess
	FROM #tbCallData
	ORDER BY CallId, HorarioEntradaFila

	SELECT 
		Sequencia
		,SeqCallId
		,CallId
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,TempoFila
		,UserId
		,FullName
		,InicioConversacao
		,TerminoConversacao
		,TempoConversacao AS TempoConversacaoComHold
		,CASE WHEN UsarTempoHold = 1
			THEN 
				CASE WHEN ISNULL(LAG(UsarTempoHold, 1, NULL) OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo), 0) = 0
					THEN TempoConversacao - TempoHold
					ELSE TempoConversacao
				END
			ELSE TempoConversacao
		END AS TempoConversacao
		--,TempoHold
		,CASE WHEN UsarTempoHold = 1
			THEN 
				CASE WHEN ISNULL(LAG(UsarTempoHold, 1, NULL) OVER(PARTITION BY CallId ORDER BY dEventTime, SeqNo), 0) = 0
					THEN TempoHold
					ELSE 0
				END
			ELSE 0
		END AS TempoHold
		,nHeld
		,HorarioTransferencia
		,Transferida
		,DeTransferencia
		,DeTransferenciaDireta
		,DataTerminoChamada
		,Desconexao
		,UsarTempoHold
		,dEventTime
		,I3TimeStampGMT
		,SeqNo
	INTO #tbCallsDataToProcess
	FROM #tbCallsToProcess
	WHERE UsarTempoHold = 1

	INSERT INTO #tbCallsDataToProcess
	SELECT
		Sequencia
		,SeqCallId
		,CallId
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,TempoFila
		,UserId
		,FullName
		,InicioConversacao
		,TerminoConversacao
		,TempoConversacao AS TempoConversacaoComHold
		,TempoConversacao
		,0 AS TempoHold
		,nHeld
		,HorarioTransferencia
		,Transferida
		,DeTransferencia
		,DeTransferenciaDireta
		,DataTerminoChamada
		,Desconexao
		,UsarTempoHold
		,dEventTime
		,I3TimeStampGMT
		,SeqNo
	FROM #tbCallsToProcess
	WHERE UsarTempoHold = 0
	ORDER BY CallId, HorarioEntradaFila

----Remove chamadas que não se conseguiu calcular o tempo de conversação por falta de informação.
--	SELECT CallId INTO #tbCallsToRemove FROM #tbCallsDataToProcess WHERE TempoConversacao < 0
--	DELETE FROM #tbCallsDataToProcess WHERE CallId IN (SELECT CallId FROM #tbCallsToRemove)
--	DROP TABLE #tbCallsToRemove

	SELECT 
		IDENTITY(int, 1, 1) AS Sequencia
		,SeqCallId
		,CallId
		,RemoteNumberFmt
		,CustomString1
		,Workgroup
		,HorarioEntradaFila
		,TempoFila
		,UserId
		,FullName
		,InicioConversacao
		,TerminoConversacao
		,TempoConversacaoComHold
		,TempoConversacao
		,TempoHold
		,nHeld
		,HorarioTransferencia
		,Transferida
		,DeTransferencia
		,DeTransferenciaDireta
		,DataTerminoChamada
		,Desconexao
		,UsarTempoHold
		,dEventTime
		,I3TimeStampGMT
		,SeqNo
	INTO #tbCalls
	FROM #tbCallsDataToProcess
	ORDER BY CallId, SeqCallId

	-- Removendo as chamadas com fila negativa. CallId: 100174932930180810
	IF OBJECT_ID('tempdb..#tbRemoveCallsWihNegativeQueueTime') IS NOT NULL
		DROP TABLE #tbRemoveCallsWihNegativeQueueTime

	SELECT CallId INTO #tbRemoveCallsWihNegativeQueueTime FROM #tbCalls WHERE TempoFila < 0

	DELETE FROM #tbCalls WHERE CallId IN(SELECT CallId FROM #tbRemoveCallsWihNegativeQueueTime)



	--Acerto do Campo: Transferida
	UPDATE #tbCalls SET Transferida = 0 WHERE DeTransferenciaDireta = 1




	--Mostrando o resultado final, filtrado ou não pelo workgroup
	IF @workgroup = '' OR @workgroup IS NULL
	BEGIN
		SELECT
			Sequencia
			,SeqCallId
			,CallId
			,RemoteNumberFmt
			,CustomString1
			,Workgroup
			,HorarioEntradaFila
			,TempoFila
			,UserId
			,FullName
			,InicioConversacao
			,TerminoConversacao
			,TempoConversacao
			,TempoHold
			,nHeld
			,HorarioTransferencia
			,Transferida
			,DeTransferencia
			,DeTransferenciaDireta
			--,DataTerminoChamada
			,Desconexao
			,dEventTime
			,I3TimeStampGMT
			,SeqNo
		FROM #tbCalls 
		WHERE (UserID NOT IN (SELECT [Data] FROM [dbo].[FN_Split] (@ExcludeLoginID, ',')) OR UserID IS NULL)

		ORDER BY CallId, SeqCallId
	END
	ELSE
	BEGIN
		SELECT
			Sequencia
			,SeqCallId
			,CallId
			,RemoteNumberFmt
			,CustomString1
			,Workgroup
			,HorarioEntradaFila
			,TempoFila
			,UserId
			,FullName
			,InicioConversacao
			,TerminoConversacao
			,TempoConversacao
			,TempoHold
			,nHeld
			,HorarioTransferencia
			,Transferida
			,DeTransferencia
			,DeTransferenciaDireta
			--,DataTerminoChamada
			,Desconexao
			,dEventTime
			,I3TimeStampGMT
			,SeqNo
		FROM #tbCalls
		WHERE Workgroup = @workgroup
		AND (UserID NOT IN (SELECT [Data] FROM [dbo].[FN_Split] (@ExcludeLoginID, ',')) OR UserID IS NULL)
		--ORDER BY Sequencia
		ORDER BY CallId, SeqCallId
	END


	IF OBJECT_ID('tempdb..#tbIVRDataToProcessor') IS NOT NULL
		DROP TABLE #tbIVRDataToProcessor

	IF OBJECT_ID('tempdb..#tbIVRData') IS NOT NULL
		DROP TABLE #tbIVRData

	IF OBJECT_ID('tempdb..#tbIVR') IS NOT NULL
		DROP TABLE #tbIVR

	IF OBJECT_ID('tempdb..#tbWorkgroupDataFromIVRToProcess') IS NOT NULL
		DROP TABLE #tbWorkgroupDataFromIVRToProcess

	IF OBJECT_ID('tempdb..#tbWorkgroupDataFromIVR') IS NOT NULL
		DROP TABLE #tbWorkgroupDataFromIVR

	IF OBJECT_ID('tempdb..#tbParticipant') IS NOT NULL
		DROP TABLE #tbParticipant

	IF OBJECT_ID('tempdb..#tbWorkgroupDataToCompare') IS NOT NULL
		DROP TABLE #tbWorkgroupDataToCompare

	IF OBJECT_ID('tempdb..#tbParticipantDataToCompare') IS NOT NULL
		DROP TABLE #tbParticipantDataToCompare

	IF OBJECT_ID('tempdb..#tbCallDataToProcess') IS NOT NULL
		DROP TABLE #tbCallDataToProcess

	IF OBJECT_ID('tempdb..#tbCallData') IS NOT NULL
		DROP TABLE #tbCallData

	IF OBJECT_ID('tempdb..#tbCallsToProcess') IS NOT NULL
		DROP TABLE #tbCallsToProcess

	IF OBJECT_ID('tempdb..#tbCalls') IS NOT NULL
		DROP TABLE #tbCalls

	IF OBJECT_ID('tempdb..#tbCallsWithHoldNotAssignedYet') IS NOT NULL
		DROP TABLE #tbCallsWithHoldNotAssignedYet

	IF OBJECT_ID('tempdb..#tbDuplicatedCallIdInIVR') IS NOT NULL
		DROP TABLE #tbDuplicatedCallIdInIVR

	IF OBJECT_ID('tempdb..#tbIrregularCalls') IS NOT NULL
		DROP TABLE #tbIrregularCalls

	IF OBJECT_ID('tempdb..#tbCallsDataToProcess') IS NOT NULL
		DROP TABLE #tbCallsDataToProcess
	IF OBJECT_ID('tempdb..#tbWorkgroupDataFromIVRQty') IS NOT NULL
		DROP TABLE #tbWorkgroupDataFromIVRQty
	IF OBJECT_ID('tempdb..#tbParticipantQty') IS NOT NULL
		DROP TABLE #tbParticipantQty
	IF OBJECT_ID('tempdb..#tbCallStatusResult') IS NOT NULL
		DROP TABLE #tbCallStatusResult
	IF OBJECT_ID('tempdb..#tbParticipant_CallStatusDI') IS NOT NULL
		DROP TABLE #tbParticipant_CallStatusDI
	IF OBJECT_ID('tempdb..#tbWorkgroupDataFromIVR_CallStatusDI') IS NOT NULL
		DROP TABLE #tbWorkgroupDataFromIVR_CallStatusDI

	IF OBJECT_ID('tempdb..#tbCallSegmentSequence') IS NOT NULL
		DROP TABLE #tbCallSegmentSequence
	IF OBJECT_ID('tempdb..#tbCallSegmentSequenceProcessed') IS NOT NULL
		DROP TABLE #tbCallSegmentSequenceProcessed

	IF OBJECT_ID('tempdb..#tbRemoveCallsWihNegativeQueueTime') IS NOT NULL
		DROP TABLE #tbRemoveCallsWihNegativeQueueTime


END



----
GO


