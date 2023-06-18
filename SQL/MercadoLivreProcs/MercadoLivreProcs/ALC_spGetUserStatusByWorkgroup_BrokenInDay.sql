USE [I3_IC_40]
GO

/****** Object:  StoredProcedure [Alc_rel].[ALC_spGetUserStatusByWorkgroup_BrokenInDay]    Script Date: 18/06/2023 10:56:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [Alc_rel].[ALC_spGetUserStatusByWorkgroup_BrokenInDay]
	@dataInicio DATETIME
	,@dataTermino DATETIME
	,@workgroup VARCHAR(100)
	,@ExcludeLoginID varchar(MAX)
AS
BEGIN	
	-- VXX (Direto em Produção)    19.06.2021

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
	    
	--DECLARE @dataInicio DATETIME = '2018-07-01 00:00:00'
	--DECLARE @dataTermino DATETIME = '2018-07-31 23:59:59'	
	--DECLARE @workgroup VARCHAR(100) = ''

	DECLARE @userId VARCHAR(100) = ''	
	DECLARE @nome VARCHAR(255) = ''
	DECLARE @dataHoraInicio DATETIME = @dataInicio				
	DECLARE @dataHoraTermino DATETIME = @dataTermino		


	IF OBJECT_ID('tempdb..#tbAgentesTodosStatus') IS NOT NULL
		DROP TABLE #tbAgentesTodosStatus
	CREATE TABLE #tbAgentesTodosStatus
		(UserId NVARCHAR(100), StatusKey NVARCHAR(100), StatusDateTime DATETIME, EndDateTime DATETIME, LoggedIn SMALLINT, SeqNo SMALLINT, I3TimeStampGMT DATETIME)
	--CREATE CLUSTERED INDEX idx_SeqStDTEnDT ON #tbAgentesTodosStatus(StatusDateTime, I3TimeStampGMT, SeqNo)
	CREATE CLUSTERED INDEX idx_SeqStDTEnDT ON #tbAgentesTodosStatus(StatusDateTime, I3TimeStampGMT, SeqNo)

	IF OBJECT_ID('tempdb..#tbAgentesTodosStatusParaProcessar') IS NOT NULL
		   DROP TABLE #tbAgentesTodosStatusParaProcessar

	IF OBJECT_ID('tempdb..#tbAgentesTodosStatusLoginParaProcessar') IS NOT NULL
		   DROP TABLE #tbAgentesTodosStatusLoginParaProcessar

	IF OBJECT_ID('tempdb..#tbAgentesTodosStatusLogin') IS NOT NULL
		   DROP TABLE #tbAgentesTodosStatusLogin

	IF OBJECT_ID('tempdb..#tbAgentesTodosStatusLoginResumido') IS NOT NULL
		   DROP TABLE #tbAgentesTodosStatusLoginResumido

	IF OBJECT_ID('tempdb..#tbAgentesResumo') IS NOT NULL
		   DROP TABLE #tbAgentesResumo

	IF OBJECT_ID('tempdb..#tbLoginsIncompletos') IS NOT NULL
		   DROP TABLE #tbLoginsIncompletos

	IF OBJECT_ID('tempdb..#tbLoginsCompletados') IS NOT NULL
		   DROP TABLE #tbLoginsCompletados

	IF OBJECT_ID('tempdb..#tbLogins') IS NOT NULL
		   DROP TABLE #tbLogins

	IF OBJECT_ID('tempdb..#tbAgentsStatusWorkgroup') IS NOT NULL
		DROP TABLE #tbAgentsStatusWorkgroup

	IF OBJECT_ID('tempdb..#tbStatusToProcess') IS NOT NULL
		DROP TABLE #tbStatusToProcess

	IF (@userId = '' OR @userId IS NULL)
		   BEGIN
				  INSERT INTO #tbAgentesTodosStatus
				  SELECT 
						 UserId
						 ,StatusKey
						 ,StatusDateTime
						 ,EndDateTime
						 ,LoggedIn
						 ,SeqNo
						 ,I3TimeStampGMT
				  FROM dbo.AgentActivityLog
				  WHERE StatusDateTime BETWEEN @dataHoraInicio AND @dataHoraTermino 
				  ORDER BY StatusDateTime, I3TimeStampGMT, SeqNo
		   END
	ELSE
		   BEGIN
				  INSERT INTO #tbAgentesTodosStatus
				  SELECT 
						 UserId
						 ,StatusKey
						 ,StatusDateTime
						 ,EndDateTime
						 ,LoggedIn
						 ,SeqNo
						 ,I3TimeStampGMT
				  FROM dbo.AgentActivityLog
				  WHERE StatusDateTime BETWEEN @dataHoraInicio AND @dataHoraTermino 
						 AND UserId = @userId
				  ORDER BY StatusDateTime, I3TimeStampGMT, SeqNo
		   END
		  
	   
	SELECT 
		   UserId
		   ,StatusKey
		   ,StatusDateTime
		   ,EndDateTime
		   ,LoggedIn
		   ,SeqNo
		   ,CASE WHEN ISNULL(LAG(LoggedIn, 1, NULL) OVER(PARTITION BY UserId ORDER BY StatusDateTime, I3TimeStampGMT, SeqNo), 0) = 0       
				  THEN 1
				  ELSE
						 CASE WHEN LoggedIn = 0
								THEN 0
								ELSE NULL
						 END 
		   END AS EventoDeLogin
		   ,I3TimeStampGMT
	INTO #tbAgentesTodosStatusParaProcessar
	FROM #tbAgentesTodosStatus
	ORDER BY UserId, StatusDateTime


	SELECT 
		   UserId
		   ,StatusKey
		   ,CASE WHEN LoggedIn = 1
				  THEN StatusDateTime
				  ELSE NULL
		   END AS StatusDateTime
		   ,CASE WHEN LoggedIn = 1 AND LEAD(LoggedIn, 1, NULL) OVER(PARTITION BY UserId ORDER BY StatusDateTime, I3TimeStampGMT, SeqNo) = 0
				  THEN LEAD(EndDateTime, 1, NULL) OVER(PARTITION BY UserId ORDER BY StatusDateTime, I3TimeStampGMT, SeqNo)
				  ELSE NULL
		   END AS EndDateTime
		   ,LoggedIn
		   ,SeqNo
		   ,I3TimeStampGMT
	INTO #tbAgentesTodosStatusLoginParaProcessar
	FROM #tbAgentesTodosStatusParaProcessar
	WHERE EventoDeLogin IS NOT NULL

	--Inserido trecho para realizar o tratamento de logins no dia (online)
	DECLARE @currentDateTime datetime = GETDATE()

	IF CAST(@currentDateTime AS date) = CAST(@dataHoraTermino AS date)
	BEGIN
		UPDATE S SET S.EndDateTime = @currentDateTime
		FROM #tbAgentesTodosStatusLoginParaProcessar AS S
		WHERE 
			CAST(S.StatusDateTime AS date) = CAST(@currentDateTime AS date)
			AND EndDateTime IS NULL
	END


	SELECT
		   UserId
		   ,StatusKey
		   ,StatusDateTime
		   ,EndDateTime
		   ,DATEDIFF(SECOND, StatusDateTime, EndDateTime) AS LoginDuration
			,CASE WHEN DATEDIFF(SECOND, StatusDateTime, EndDateTime)/3600 < 10
				THEN '0' + CAST(DATEDIFF(SECOND, StatusDateTime, EndDateTime)/3600  AS VARCHAR(1))
				ELSE CAST(DATEDIFF(SECOND, StatusDateTime, EndDateTime)/3600 AS VARCHAR(10))
			END + ':'
			+ CASE WHEN (DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)/60 < 10
				THEN '0' + CAST((DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)/60 AS VARCHAR(1))
				ELSE CAST((DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)/60 AS VARCHAR(2))
			END + ':'
			+ CASE WHEN (DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)%60 < 10
				THEN '0' + CAST((DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)%60 AS VARCHAR(1))
				ELSE CAST((DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)%60 AS VARCHAR(2))
			END AS LoginDuration_HHMMSS
			,SeqNo
		   ,I3TimeStampGMT
	INTO #tbAgentesTodosStatusLogin
	FROM #tbAgentesTodosStatusLoginParaProcessar
	WHERE StatusDateTime IS NOT NULL

	SELECT
		 UserId
		 ,StatusKey
		 ,StatusDateTime
		 ,EndDateTime
		 ,LoginDuration
		 ,LoginDuration_HHMMSS
		 ,SeqNo
		 ,I3TimeStampGMT
	INTO #tbLogins
	FROM #tbAgentesTodosStatusLogin
	WHERE LoginDuration > 0
	ORDER BY UserId, StatusDateTime

	SELECT
		 UserId
		 ,StatusKey
		 ,StatusDateTime
		 ,EndDateTime
		 ,LoginDuration
		 ,LoginDuration_HHMMSS
		 ,SeqNo
		 ,I3TimeStampGMT
	INTO #tbLoginsIncompletos
	FROM #tbAgentesTodosStatusLogin
	WHERE LoginDuration IS NULL
	ORDER BY UserId, StatusDateTime

	SELECT
		I.UserId
		,I.StatusKey
		,I.StatusDateTime
		,A.StatusDateTime AS EndDateTime
		,A.SeqNo
		,A.I3TimeStampGMT
		,RANK() OVER(PARTITION BY I.UserId ORDER BY A.I3TimeStampGMT, A.SeqNo) AS ordem
	INTO #tbLoginsCompletados
	FROM #tbLoginsIncompletos AS I
		INNER JOIN dbo.AgentActivityLog AS A ON A.UserId = I.UserId AND A.StatusDateTime > I.StatusDateTime AND A.LoggedIn = 0
	ORDER BY A.I3TimeStampGMT

	INSERT #tbLogins
	SELECT
		UserId
		,StatusKey
		,StatusDateTime
		,EndDateTime
		,DATEDIFF(SECOND, StatusDateTime, EndDateTime) AS LoginDuration
		,CASE WHEN DATEDIFF(SECOND, StatusDateTime, EndDateTime)/3600 < 10
			THEN '0' + CAST(DATEDIFF(SECOND, StatusDateTime, EndDateTime)/3600  AS VARCHAR(1))
			ELSE CAST(DATEDIFF(SECOND, StatusDateTime, EndDateTime)/3600 AS VARCHAR(10))
		END + ':'
		+ CASE WHEN (DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)/60 < 10
			THEN '0' + CAST((DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)/60 AS VARCHAR(1))
			ELSE CAST((DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)/60 AS VARCHAR(2))
		END + ':'
		+ CASE WHEN (DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)%60 < 10
			THEN '0' + CAST((DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)%60 AS VARCHAR(1))
			ELSE CAST((DATEDIFF(SECOND, StatusDateTime, EndDateTime)%3600)%60 AS VARCHAR(2))
		END AS LoginDuration_HHMMSS
		,SeqNo
		,I3TimeStampGMT
	FROM #tbLoginsCompletados
	WHERE ordem = 1

	--SELECT
	--	UserId
	--	,StatusDateTime
	--	,EndDateTime
	--	,LoginDuration
	--	,LoginDuration_HHMMSS
	--	,SeqNo
	--	,I3TimeStampGMT
	--FROM #tbLogins
	--ORDER BY UserId, I3TimeStampGMT, SeqNo

	-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	--Workgroup Activation

	IF OBJECT_ID('tempdb..#tbWorkgroupActivation') IS NOT NULL
		DROP TABLE #tbWorkgroupActivation

	IF OBJECT_ID('tempdb..#tbWorkgroupActivatedOnLogin') IS NOT NULL
		DROP TABLE #tbWorkgroupActivatedOnLogin

	IF OBJECT_ID('tempdb..#tbLoginWorkgroupActivation') IS NOT NULL
		DROP TABLE #tbLoginWorkgroupActivation

	IF OBJECT_ID('tempdb..#tbLogoutWorkgroupActivation') IS NOT NULL
		DROP TABLE #tbLogoutWorkgroupActivation

	IF OBJECT_ID('tempdb..#tbLoginLogoutWorkgroupToProcess') IS NOT NULL
		DROP TABLE #tbLoginLogoutWorkgroupToProcess

	IF OBJECT_ID('tempdb..#tbLoginLogoutWorkgroup') IS NOT NULL
		DROP TABLE #tbLoginLogoutWorkgroup



	SELECT 
		UserId
		,Workgroup
		,ActivationDateTime
		,ActivationFlag
		,I3TimeStampGMT
	INTO #tbWorkgroupActivation
	FROM dbo.AgentQueueActivationHist


	--- A Pedido de Ramon e Ronaldo em 24/06/2022
	DELETE #tbWorkgroupActivation
	WHERE
	UserId = 'cnoah'
	AND Workgroup = 'OUV_MELI'
	AND ActivationDateTime > '2022-01-11 16:26:23.000'
	---

	DELETE 
	FROM #tbWorkgroupActivation 
	WHERE
		UserId = 'rlais' 
		AND Workgroup = 'OUV_MELI'
		AND ActivationDateTime >= '2022-03-04 11:00:00.000'


	SELECT
		L.UserId
		,L.StatusDateTime
		,L.EndDateTime
		,L.SeqNo
		,L.I3TimeStampGMT AS I3TimeStampGMT_A
		,W.Workgroup
		,W.ActivationDateTime
		,W.ActivationFlag
		,W.I3TimeStampGMT AS I3TimeStampGMT_W
		,RANK() OVER(PARTITION BY L.UserId, L.StatusDateTime, W.Workgroup ORDER BY L.I3TimeStampGMT, L.SeqNo, W.I3TimeStampGMT DESC) AS Ordem
	INTO #tbWorkgroupActivatedOnLogin
	FROM #tbLogins AS L
		LEFT JOIN #tbWorkgroupActivation AS W ON W.UserId = L.UserId AND W.ActivationDateTime <= L.StatusDateTime
	ORDER BY L.UserId, L.I3TimeStampGMT, L.SeqNo, W.I3TimeStampGMT DESC

	DELETE FROM #tbWorkgroupActivatedOnLogin WHERE Ordem <> 1 OR ActivationFlag = 0

	SELECT 
		UserId
		,StatusDateTime
		,EndDateTime
		,SeqNo
		,I3TimeStampGMT_A
		,Workgroup
		--,ActivationDateTime
		,StatusDateTime AS ActivationDateTime 
		,ActivationFlag
		,I3TimeStampGMT_W 
	INTO #tbLoginWorkgroupActivation
	FROM #tbWorkgroupActivatedOnLogin

	INSERT INTO #tbLoginWorkgroupActivation
	SELECT
		L.UserId
		,L.StatusDateTime
		,L.EndDateTime
		,L.SeqNo
		,L.I3TimeStampGMT AS I3TimeStampGMT_A
		,W.Workgroup
		--,W.ActivationDateTime
		,L.StatusDateTime AS ActivationDateTime 
		,W.ActivationFlag
		,W.I3TimeStampGMT_W
	FROM #tbLogins AS L
		LEFT JOIN #tbWorkgroupActivatedOnLogin AS W ON L.UserId = W.UserId AND L.StatusDateTime = W.StatusDateTime
	WHERE W.Workgroup IS NULL


	INSERT INTO #tbLoginWorkgroupActivation
	SELECT
		L.UserId
		,L.StatusDateTime
		,L.EndDateTime
		,L.SeqNo
		,L.I3TimeStampGMT AS I3TimeStampGMT_A
		,W.Workgroup
		,W.ActivationDateTime
		,W.ActivationFlag
		,W.I3TimeStampGMT AS I3TimeStampGMT_W
	FROM #tbLogins AS L
		LEFT JOIN #tbWorkgroupActivation AS W ON W.UserId = L.UserId AND W.ActivationDateTime BETWEEN L.StatusDateTime AND L.EndDateTime
	WHERE W.Workgroup IS NOT NULL AND W.ActivationFlag = 1
	ORDER BY L.UserId, L.I3TimeStampGMT, L.SeqNo, W.I3TimeStampGMT

	SELECT
		L.UserId
		,L.StatusDateTime
		,L.EndDateTime
		,L.SeqNo
		,L.I3TimeStampGMT AS I3TimeStampGMT_A
		,W.Workgroup
		,W.ActivationDateTime
		,W.ActivationFlag
		,W.I3TimeStampGMT AS I3TimeStampGMT_W
	INTO #tbLogoutWorkgroupActivation
	FROM #tbLogins AS L
		LEFT JOIN #tbWorkgroupActivation AS W ON W.UserId = L.UserId AND W.ActivationDateTime BETWEEN L.StatusDateTime AND L.EndDateTime
	WHERE W.Workgroup IS NOT NULL AND W.ActivationFlag = 0
	ORDER BY L.UserId, L.I3TimeStampGMT, L.SeqNo, W.I3TimeStampGMT

	SELECT 
		I.UserId
		,I.StatusDateTime
		,I.EndDateTime
		,I.SeqNo
		,I.I3TimeStampGMT_A
		,I.Workgroup
		,CASE WHEN I.Workgroup IS NOT NULL 
			THEN I.ActivationDateTime
			ELSE NULL
		END AS LoginDateTimeInWorkgroup
		,W.ActivationFlag
		,W.I3TimeStampGMT_W
		,CASE WHEN I.Workgroup IS NOT NULL
			THEN 
				CASE WHEN I.ActivationDateTime < W.ActivationDateTime
					THEN W.ActivationDateTime
					ELSE I.EndDateTime 
				END
			ELSE NULL
		END AS LogoutDateTimeInWorkgroup
	INTO #tbLoginLogoutWorkgroupToProcess
	FROM #tbLoginWorkgroupActivation AS I
		LEFT JOIN #tbLogoutWorkgroupActivation AS W ON W.UserId = I.UserId AND W.StatusDateTime = I.StatusDateTime AND W.Workgroup = I.Workgroup
	ORDER BY I.UserId, I.I3TimeStampGMT_A, I.SeqNo, W.I3TimeStampGMT_W

	SELECT
		UserId
		,StatusDateTime AS LoginDateTime
		,EndDateTime AS LogoutDateTime
		,DATEDIFF(SECOND, StatusDateTime, EndDateTime) AS LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,CASE WHEN Workgroup IS NOT NULL 
			THEN DATEDIFF(SECOND, LoginDateTimeInWorkgroup, LogoutDateTimeInWorkgroup) 
			ELSE 0
		END AS LoginDurationInWorkgroup
		,SeqNo
		,I3TimeStampGMT_A
		,I3TimeStampGMT_W
		,CASE WHEN LAG(Workgroup, 1, '') OVER(PARTITION BY UserId, Workgroup ORDER BY LoginDateTimeInWorkgroup, LogoutDateTimeInWorkgroup) = Workgroup
			THEN 
				CASE WHEN LAG(LoginDateTimeInWorkgroup, 1, '') OVER(PARTITION BY UserId, Workgroup ORDER BY LoginDateTimeInWorkgroup, LogoutDateTimeInWorkgroup) = LoginDateTimeInWorkgroup
					THEN 1
					ELSE 0
				END 
			ELSE 0
		END AS DataToRemove
	INTO #tbLoginLogoutWorkgroup
	FROM #tbLoginLogoutWorkgroupToProcess

	DELETE FROM #tbLoginLogoutWorkgroup WHERE DataToRemove = 1


	--IF @workgroup = '' OR @workgroup IS NULL
	--BEGIN
	--	SELECT
	--		UserId
	--		,LoginDateTime
	--		,LogoutDateTime
	--		,LoginDuration
	--		,Workgroup
	--		,LoginDateTimeInWorkgroup
	--		,LogoutDateTimeInWorkgroup
	--		,LoginDurationInWorkgroup
	--	FROM #tbLoginLogoutWorkgroup
	--	ORDER BY UserId, I3TimeStampGMT_A, SeqNo, LoginDateTimeInWorkgroup
	--END
	--ELSE
	--BEGIN
	--	SELECT
	--		UserId
	--		,LoginDateTime
	--		,LogoutDateTime
	--		,LoginDuration
	--		,Workgroup
	--		,LoginDateTimeInWorkgroup
	--		,LogoutDateTimeInWorkgroup
	--		,LoginDurationInWorkgroup
	--	FROM #tbLoginLogoutWorkgroup
	--	WHERE Workgroup = @workgroup
	--	ORDER BY UserId, I3TimeStampGMT_A, SeqNo, LoginDateTimeInWorkgroup
	--END
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
--Status by Workgroup


	DECLARE @startStatusDateTime DATETIME = (SELECT TOP 1 LoginDateTime FROM #tbLoginLogoutWorkgroup ORDER BY LoginDateTime)
	DECLARE @endStatusDateTime DATETIME = (SELECT TOP 1 LogoutDateTime FROM #tbLoginLogoutWorkgroup ORDER BY LogoutDateTime DESC)
	
	IF OBJECT_ID('tempdb..#tbUserStatusKeyLoggedIn') IS NOT NULL
		DROP TABLE #tbUserStatusKeyLoggedIn
	CREATE TABLE #tbUserStatusKeyLoggedIn
		(
			SequenceId INT, UserId_Seq INT, UserId NVARCHAR(100), StatusKey NVARCHAR(100), StatusDateTime DATETIME, EndDateTime DATETIME, StateDuration INT
			,I3TimeStampGMT DATETIME, SeqNo INT
		)
	CREATE CLUSTERED INDEX idx_sequenceId ON #tbUserStatusKeyLoggedIn(UserId, I3TimeStampGMT, SeqNo)
	
	IF OBJECT_ID('tempdb..#tbUserStatusKeyLoggedInByWorkgroup') IS NOT NULL
		DROP TABLE #tbUserStatusKeyLoggedInByWorkgroup
	CREATE TABLE #tbUserStatusKeyLoggedInByWorkgroup
		(
			SequenceId INT, UserId_Seq INT, UserId NVARCHAR(100), LoginDateTime DATETIME, LogoutDateTime DATETIME, LoginDuration INT, Workgroup NVARCHAR(100)
			,LoginDateTimeInWorkgroup DATETIME, LogoutDateTimeInWorkgroup DATETIME, LoginDurationInWorkgroup INT, StatusKey NVARCHAR(100)
			,StatusDateTime DATETIME, EndDateTime DATETIME, StateDuration INT, I3TimeStampGMT DATETIME, I3TimeStampGMT_A DATETIME, SeqNo INT
		)
	CREATE CLUSTERED INDEX idx_sequenceId ON #tbUserStatusKeyLoggedInByWorkgroup(UserId, I3TimeStampGMT, SeqNo, StatusDateTime)

	IF OBJECT_ID('tempdb..#tbUserStatusKeyLoggedInByWorkgroupToProcess') IS NOT NULL
		DROP TABLE #tbUserStatusKeyLoggedInByWorkgroupToProcess

	IF OBJECT_ID('tempdb..#tbUserStatusKeyLoggedInByWorkgroupProcessed') IS NOT NULL
		DROP TABLE #tbUserStatusKeyLoggedInByWorkgroupProcessed

	IF OBJECT_ID('tempdb..#tbUserStatusKeyByWorkgroupRemoveDuplicatedData') IS NOT NULL
		DROP TABLE #tbUserStatusKeyByWorkgroupRemoveDuplicatedData

	IF OBJECT_ID('tempdb..#tbUserStatusKeyByWorkgroup') IS NOT NULL
		DROP TABLE #tbUserStatusKeyByWorkgroup
	CREATE TABLE #tbUserStatusKeyByWorkgroup
		(
			SequenceId INT, UserId_Seq INT, UserId NVARCHAR(100), LoginDateTime DATETIME, LogoutDateTime DATETIME, LoginDuration INT, Workgroup NVARCHAR(100)
			,LoginDateTimeInWorkgroup DATETIME, LogoutDateTimeInWorkgroup DATETIME, LoginDurationInWorkgroup INT, StatusKey NVARCHAR(100)
			,StatusDateTime DATETIME, EndDateTime DATETIME, StateDuration INT, I3TimeStampGMT DATETIME, SeqNo INT
		)
	CREATE CLUSTERED INDEX idx_sequenceId ON #tbUserStatusKeyByWorkgroup(UserId, I3TimeStampGMT, SeqNo, StatusDateTime)

	IF @userId = '' OR @userId IS NULL
	BEGIN
		INSERT INTO #tbUserStatusKeyLoggedIn
		SELECT
			ROW_NUMBER() OVER(ORDER BY UserId, I3TimeStampGMT, SeqNo) AS SequenceId
			,RANK() OVER(PARTITION BY UserId ORDER BY I3TimeStampGMT, SeqNo) AS UserId_Seq
			,UserId
			,StatusKey
			,StatusDateTime
			,EndDateTime
			,StateDuration
			,I3TimeStampGMT
			,SeqNo
		FROM dbo.AgentActivityLog
		WHERE StatusDateTime BETWEEN @startStatusDateTime AND @endStatusDateTime AND LoggedIn = 1

		ORDER BY I3TimeStampGMT, SeqNo
	END
	ELSE
	BEGIN
		INSERT INTO #tbUserStatusKeyLoggedIn
		SELECT
			ROW_NUMBER() OVER(ORDER BY UserId, I3TimeStampGMT, SeqNo) AS SequenceId
			,RANK() OVER(PARTITION BY UserId ORDER BY I3TimeStampGMT, SeqNo) AS UserId_Seq
			,UserId
			,StatusKey
			,StatusDateTime
			,EndDateTime
			,StateDuration
			,I3TimeStampGMT
			,SeqNo
		FROM dbo.AgentActivityLog
		WHERE StatusDateTime BETWEEN @startStatusDateTime AND  @endStatusDateTime AND LoggedIn = 1 AND UserId = @userId

		ORDER BY I3TimeStampGMT, SeqNo
	END



	INSERT INTO #tbUserStatusKeyLoggedInByWorkgroup
	SELECT
		S.SequenceId
		,S.UserId_Seq
		,L.UserId
		,L.LoginDateTime
		,L.LogoutDateTime
		,L.LoginDuration
		,L.Workgroup
		,L.LoginDateTimeInWorkgroup
		,L.LogoutDateTimeInWorkgroup
		,L.LoginDurationInWorkgroup
		,S.StatusKey
		,S.StatusDateTime
		,S.EndDateTime
		,S.StateDuration
		,S.I3TimeStampGMT
		,L.I3TimeStampGMT_A
		,S.SeqNo
	FROM #tbLoginLogoutWorkgroup AS L
		INNER JOIN #tbUserStatusKeyLoggedIn AS S ON S.UserId = L.UserId
			AND S.StatusDateTime >= L.LoginDateTimeInWorkgroup AND S.EndDateTime <= L.LogoutDateTimeInWorkgroup
	ORDER BY SequenceId	

	SELECT 
		S.SequenceId
		,S.UserId_Seq
		,S.UserId
		,L.LoginDateTime
		,L.LogoutDateTime
		,L.LoginDuration
		,L.Workgroup
		,L.LoginDateTimeInWorkgroup
		,L.LogoutDateTimeInWorkgroup
		,L.LoginDurationInWorkgroup
		,S.StatusKey
		,S.StatusDateTime
		,S.EndDateTime
		,S.StateDuration
		,S.I3TimeStampGMT
		,L.I3TimeStampGMT_A
		,S.SeqNo
	INTO #tbUserStatusKeyLoggedInByWorkgroupToProcess
	FROM #tbLoginLogoutWorkgroup AS L
		INNER JOIN #tbUserStatusKeyLoggedIn AS S ON S.UserId = L.UserId AND L.LoginDateTime <> L.LoginDateTimeInWorkgroup
			AND L.LoginDateTimeInWorkgroup BETWEEN S.StatusDateTime AND S.EndDateTime
	ORDER BY LoginDateTime

	--SELECT 
	--	S.SequenceId
	--	,S.UserId_Seq
	--	--,L.UserId
	--	,S.UserId
	--	,L.LoginDateTime
	--	,L.LogoutDateTime
	--	,L.LoginDuration
	--	,L.Workgroup
	--	,L.LoginDateTimeInWorkgroup
	--	,L.LogoutDateTimeInWorkgroup
	--	,L.LoginDurationInWorkgroup
	--	,S.StatusKey
	--	,S.StatusDateTime
	--	,S.EndDateTime
	--	,S.StateDuration
	--	,S.I3TimeStampGMT
	--	,S.SeqNo
	--INTO #tbUserStatusKeyLoggedInByWorkgroupToProcess
	--FROM #tbUserStatusKeyLoggedIn AS S
	--	LEFT JOIN #tbUserStatusKeyLoggedInByWorkgroup AS L ON L.SequenceId = S.SequenceId AND L.UserId_Seq = S.UserId_Seq
	--WHERE L.UserId IS NULL
	--ORDER BY S.SequenceId, S.UserId_Seq

	SELECT 
		S.SequenceId
		,S.UserId_Seq
		--,L.UserId
		,S.UserId
		,L.LoginDateTime
		,L.LogoutDateTime
		,L.LoginDuration
		,L.Workgroup
		,L.LoginDateTimeInWorkgroup
		,L.LogoutDateTimeInWorkgroup
		,L.LoginDurationInWorkgroup
		,S.StatusKey
		,S.StatusDateTime
		,S.EndDateTime
		,S.StateDuration
		,S.I3TimeStampGMT
		,L.I3TimeStampGMT_A
		,S.SeqNo
		,RANK() OVER(PARTITION BY CAST(L.LoginDateTime AS DATE), S.UserId ORDER BY L.LoginDateTime DESC) AS SequenceDesc ---AQUI CAST(L.LoginDateTime AS DATE)
	INTO #tbUserStatusKeyLoggedInByWorkgroupProcessed
	FROM #tbLoginLogoutWorkgroup AS L
		INNER JOIN #tbUserStatusKeyLoggedInByWorkgroupToProcess AS S ON S.UserId = L.UserId AND CAST(L.LoginDateTime AS DATE) = CAST(S.StatusDateTime AS DATE)
			AND (S.StatusDateTime >= L.LoginDateTimeInWorkgroup OR S.EndDateTime > L.LoginDateTimeInWorkgroup)

	INSERT INTO #tbUserStatusKeyLoggedInByWorkgroup
	SELECT
		SequenceId
		,UserId_Seq
		,UserId
		,LoginDateTime
		,LogoutDateTime
		,LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,LoginDurationInWorkgroup
		,StatusKey
		--,StatusDateTime
		,CASE WHEN LoginDateTimeInWorkgroup > StatusDateTime AND LogoutDateTimeInWorkgroup >= EndDateTime
			THEN LoginDateTimeInWorkgroup
			ELSE StatusDateTime
		END AS StatusDateTime
		--,EndDateTime
		,CASE WHEN LoginDateTimeInWorkgroup <= StatusDateTime AND LogoutDateTimeInWorkgroup < EndDateTime 
			THEN LogoutDateTimeInWorkgroup
			ELSE EndDateTime
		END AS EndDateTime	
		--,StateDuration
		,CASE WHEN LoginDateTimeInWorkgroup > StatusDateTime AND LogoutDateTimeInWorkgroup >= EndDateTime
			THEN DATEDIFF(SECOND, LoginDateTimeInWorkgroup, EndDateTime)
			ELSE
				CASE WHEN LoginDateTimeInWorkgroup <= StatusDateTime AND LogoutDateTimeInWorkgroup < EndDateTime 
					THEN DATEDIFF(SECOND, StatusDateTime, LogoutDateTimeInWorkgroup)
					ELSE StateDuration
				END
		END AS StateDuration
		,I3TimeStampGMT
		,I3TimeStampGMT_A
		,SeqNo
	FROM #tbUserStatusKeyLoggedInByWorkgroupProcessed 
	WHERE SequenceDesc = 1
	ORDER BY I3TimeStampGMT, SeqNo

	DELETE FROM #tbUserStatusKeyLoggedInByWorkgroup WHERE StateDuration < 0

	--SELECT
	--	SequenceId
	--	,UserId_Seq
	--	,UserId
	--	,LoginDateTime
	--	,LogoutDateTime
	--	,LoginDuration
	--	,Workgroup
	--	,LoginDateTimeInWorkgroup
	--	,LogoutDateTimeInWorkgroup
	--	,LoginDurationInWorkgroup
	--	,StatusKey
	--	,StatusDateTime
	--	,EndDateTime
	--	,StateDuration
	--	,I3TimeStampGMT
	--	,SeqNo
	--	,CASE WHEN 
	--		(
	--			LAG(UserId, 1, NULL) OVER(PARTITION BY LoginDateTime, UserId, Workgroup ORDER BY I3TimeStampGMT, SeqNo) = UserId
	--			AND LAG(StatusKey, 1, NULL) OVER(PARTITION BY CAST (LoginDateTime AS DATE), UserId, Workgroup ORDER BY I3TimeStampGMT, SeqNo) = StatusKey
	--			AND LAG(StateDuration, 1, NULL) OVER(PARTITION BY CAST (LoginDateTime AS DATE), UserId, Workgroup ORDER BY I3TimeStampGMT, SeqNo) = StateDuration
	--		)
	--		THEN 1
	--		ELSE 0
	--	END AS LineDuplicated
	--INTO #tbUserStatusKeyByWorkgroupRemoveDuplicatedData
	--FROM #tbUserStatusKeyLoggedInByWorkgroup

	SELECT *, RANK() OVER(PARTITION BY UserId, StatusKey, StatusDateTime, StateDuration ORDER BY I3TimeStampGMT, SeqNo) AS LineDuplicated
	INTO #tbUserStatusKeyByWorkgroupRemoveDuplicatedData
	FROM #tbUserStatusKeyLoggedInByWorkgroup
	ORDER BY UserId, I3TimeStampGMT, SeqNo
	
	--select count(*) as Duplicated from #tbUserStatusKeyByWorkgroupRemoveDuplicatedData where LineDuplicated = 1

	--select *, rank() over(partition by UserId, StatusKey, StatusDateTime, StateDuration order by I3TimeStampGMT, SeqNo) as ordem
	--into #tbRemove
	--from #tbUserStatusKeyByWorkgroupRemoveDuplicatedData --where LineDuplicated = 1
	--order by UserId, I3TimeStampGMT, SeqNo

	--select count(*) as ToRemove from #tbRemove where ordem = 1
	--select count(*) as ToRemove from #tbRemove where ordem > 1

	--drop table #tbRemove

	INSERT INTO #tbUserStatusKeyByWorkgroup
	SELECT 
		SequenceId
		,UserId_Seq
		,UserId
		,LoginDateTime
		,LogoutDateTime
		,LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,LoginDurationInWorkgroup
		,StatusKey
		,StatusDateTime
		,EndDateTime
		,StateDuration
		,I3TimeStampGMT
		,SeqNo			
	FROM #tbUserStatusKeyByWorkgroupRemoveDuplicatedData 
	WHERE LineDuplicated = 1

	--Coleta os dados de agentes que não estão ativados em workgroups
	INSERT INTO #tbUserStatusKeyByWorkgroup
	SELECT
		S.SequenceId
		,S.UserId_Seq
		,S.UserId
		,L.LoginDateTime
		,L.LogoutDateTime
		,L.LoginDuration
		,L.Workgroup
		,L.LoginDateTimeInWorkgroup
		,L.LogoutDateTimeInWorkgroup
		,L.LoginDurationInWorkgroup
		,S.StatusKey
		,S.StatusDateTime
		,S.EndDateTime
		,S.StateDuration
		,S.I3TimeStampGMT
		,S.SeqNo			
	FROM #tbLoginLogoutWorkgroup AS L 
		INNER JOIN #tbUserStatusKeyLoggedIn AS S ON S.UserId = L.UserId AND L.Workgroup IS NULL 
			AND CAST(L.LoginDateTime AS DATE) = CAST(S.StatusDateTime AS DATE)
			AND (S.StatusDateTime >= L.LoginDateTime AND S.EndDateTime <= L.LogoutDateTime) 

	--UPDATE #tbUserStatusKeyByWorkgroup SET StateDuration = 5 WHERE StatusKey = 'acw'

	--IF @workgroup = '' OR @workgroup IS NULL
	--BEGIN
	--	SELECT
	--		SequenceId
	--		,UserId_Seq
	--		,UserId
	--		,LoginDateTime
	--		,LogoutDateTime
	--		,LoginDuration
	--		,Workgroup
	--		,LoginDateTimeInWorkgroup
	--		,LogoutDateTimeInWorkgroup
	--		,LoginDurationInWorkgroup
	--		,StatusKey
	--		,StatusDateTime
	--		,EndDateTime
	--		,StateDuration
	--		,I3TimeStampGMT
	--		,SeqNo		
	--	FROM #tbUserStatusKeyByWorkgroup 
	--	ORDER BY UserId, I3TimeStampGMT, SeqNo, StatusDateTime
	--END
	--ELSE
	--BEGIN
	--	SELECT
	--		SequenceId
	--		,UserId_Seq
	--		,UserId
	--		,LoginDateTime
	--		,LogoutDateTime
	--		,LoginDuration
	--		,Workgroup
	--		,LoginDateTimeInWorkgroup
	--		,LogoutDateTimeInWorkgroup
	--		,LoginDurationInWorkgroup
	--		,StatusKey
	--		,StatusDateTime
	--		,EndDateTime
	--		,StateDuration
	--		,I3TimeStampGMT
	--		,SeqNo		
	--	FROM #tbUserStatusKeyByWorkgroup 
	--	WHERE Workgroup = @workgroup
	--	ORDER BY UserId, I3TimeStampGMT, SeqNo, StatusDateTime
	--END
-----------------------------------------------------------------------------------------------------------------------------------------------
-- Quebrando resultado dos status diariamente, ou seja, status inciados no dia, porém, não finalizados no mesmo.
-- Para esses casos o status é quebrado na virada dos dias.
-----------------------------------------------------------------------------------------------------------------------------------------------
	IF OBJECT_ID('tempdb..#tbUserStatusLevelDays') IS NOT NULL
		DROP TABLE #tbUserStatusLevelDays
	
	IF OBJECT_ID('tempdb..#tbUserStatusToBreakInDays') IS NOT NULL
		DROP TABLE #tbUserStatusToBreakInDays

	IF OBJECT_ID('tempdb..#tbUserStatusToBreakInDays') IS NOT NULL
		DROP TABLE #tbUserStatusToBreakInDays

	IF OBJECT_ID('tempdb..#tbUserStatusDoNotNeedToBreakInDays') IS NOT NULL
		DROP TABLE #tbUserStatusDoNotNeedToBreakInDays

	IF OBJECT_ID('tempdb..#tbUserStatusBrokenInDays') IS NOT NULL
		DROP TABLE #tbUserStatusBrokenInDays

	IF OBJECT_ID('tempdb..#tbUserStatusByWorkgroupBrokenInDays') IS NOT NULL
		DROP TABLE #tbUserStatusByWorkgroupBrokenInDays

	SELECT DISTINCT
		SequenceId
		,UserId_Seq
		,UserId
		,LoginDateTime
		,LogoutDateTime
		,LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,LoginDurationInWorkgroup
		,StatusKey
		,StatusDateTime
		,EndDateTime
		,StateDuration
		,I3TimeStampGMT
		,SeqNo		
		,DATEDIFF(DAY, StatusDateTime, EndDateTime) AS Nivel
	INTO #tbUserStatusLevelDays
	FROM #tbUserStatusKeyByWorkgroup

	SELECT
		SequenceId
		,UserId_Seq
		,UserId
		,LoginDateTime
		,LogoutDateTime
		,LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,LoginDurationInWorkgroup
		,StatusKey
		,StatusDateTime
		,EndDateTime
		,StateDuration
		,StatusDateTime AS StatusDateTimeOnDay
		,EndDateTime AS EndDateTimeOnDay
		,StateDuration AS StateDurationOnDay
		,I3TimeStampGMT
		,SeqNo		
	INTO #tbUserStatusDoNotNeedToBreakInDays
	FROM #tbUserStatusLevelDays
	WHERE Nivel = 0
	--AND UserId NOT IN( 'ronaldo.morais', 'khalil.mohmari', 'Concentrix_Supervisor_2', 'Concentrix_Supervisor_3')

	
	SELECT
		SequenceId
		,UserId_Seq
		,UserId
		,LoginDateTime
		,LogoutDateTime
		,LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,LoginDurationInWorkgroup
		,StatusKey
		,StatusDateTime
		,EndDateTime
		,StateDuration
		,I3TimeStampGMT
		,SeqNo
		,Nivel		
	INTO #tbUserStatusToBreakInDays
	FROM #tbUserStatusLevelDays
	WHERE Nivel > 0
		--AND UserId NOT IN( 'ronaldo.morais', 'khalil.mohmari', 'Concentrix_Supervisor_2', 'Concentrix_Supervisor_3')




	;WITH cte --CTE1
	(	
		SequenceId, UserId_Seq, UserId, LoginDateTime, LogoutDateTime, LoginDuration, Workgroup, LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup, LoginDurationInWorkgroup, StatusKey, StatusDateTime, EndDateTime, StateDuration
		,StatusDateTimeOnDay, EndDateTimeOnDay, I3TimeStampGMT, SeqNo, Nivel
	)
	AS
	(
		SELECT
			SequenceId
			,UserId_Seq
			,UserId
			,LoginDateTime
			,LogoutDateTime
			,LoginDuration
			,Workgroup
			,LoginDateTimeInWorkgroup
			,LogoutDateTimeInWorkgroup
			,LoginDurationInWorkgroup
			,StatusKey
			,StatusDateTime
			,EndDateTime
			,StateDuration
			,DATEADD(DAY, Nivel, CAST(CAST(StatusDateTime AS DATE) AS DATETIME)) AS StatusDateTimeOnDay
			,EndDateTime AS EndDateTimeOnDay
			,I3TimeStampGMT
			,SeqNo
			,Nivel		
		FROM #tbUserStatusToBreakInDays		

		UNION ALL

		SELECT
			S.SequenceId
			,S.UserId_Seq
			,S.UserId
			,S.LoginDateTime
			,S.LogoutDateTime
			,S.LoginDuration
			,S.Workgroup
			,S.LoginDateTimeInWorkgroup
			,S.LogoutDateTimeInWorkgroup
			,S.LoginDurationInWorkgroup
			,S.StatusKey
			,S.StatusDateTime
			,S.EndDateTime
			,S.StateDuration
			,CASE WHEN (C.Nivel - 1) = 0
				THEN S.StatusDateTime
				ELSE DATEADD(DAY, (C.Nivel - 1), CAST(CAST(S.StatusDateTime AS DATE) AS DATETIME))
			END AS StatusDateTimeOnDay
			,DATEADD(DAY, (C.Nivel), CAST(CAST(S.StatusDateTime AS DATE) AS DATETIME)) AS EndDateTimeOnDay
			,S.I3TimeStampGMT
			,S.SeqNo
			,(S.Nivel - 1) AS Nivel
		FROM #tbUserStatusToBreakInDays AS S
			--INNER JOIN cte AS C ON C.UserId = S.UserId AND C.StatusKey = S.StatusKey AND C.StatusDateTime = S.StatusDateTime
			--	AND C.Workgroup = S.Workgroup
		INNER JOIN cte AS C ON
			C.SequenceId = S.SequenceId
		AND C.UserId_Seq = S.UserId_Seq
		AND C.UserId = S.UserId
		AND C.LoginDateTime = S.LoginDateTime
		AND C.LogoutDateTime = S.LogoutDateTime
		AND C.LoginDuration = S.LoginDuration
		AND C.Workgroup = S.Workgroup
		AND C.LoginDateTimeInWorkgroup = S.LoginDateTimeInWorkgroup
		AND C.LogoutDateTimeInWorkgroup = S.LogoutDateTimeInWorkgroup
		AND C.LoginDurationInWorkgroup = S.LoginDurationInWorkgroup
		AND C.StatusKey = S.StatusKey
		AND C.StatusDateTime = S.StatusDateTime
		AND C.EndDateTime = S.EndDateTime
		AND C.StateDuration = S.StateDuration
		AND C.SeqNo = S.SeqNo
		AND C.Nivel = S.Nivel
	
		WHERE C.Nivel > 0
	)




	SELECT
		SequenceId
		,UserId_Seq
		,UserId
		,LoginDateTime
		,LogoutDateTime
		,LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,LoginDurationInWorkgroup
		,StatusKey
		,StatusDateTime
		,EndDateTime
		,StateDuration
		,StatusDateTimeOnDay
		,EndDateTimeOnDay
		,DATEDIFF(SECOND, StatusDateTimeOnDay, EndDateTimeOnDay) AS StateDurationOnDay
		,I3TimeStampGMT
		,SeqNo
		--,Nivel	
	INTO #tbUserStatusBrokenInDays	
	FROM cte		
	ORDER BY UserId, StatusDateTime, StatusDateTimeOnDay
	OPTION (MAXRECURSION 0);

	INSERT INTO #tbUserStatusBrokenInDays
	SELECT
		SequenceId
		,UserId_Seq
		,UserId
		,LoginDateTime
		,LogoutDateTime
		,LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,LoginDurationInWorkgroup
		,StatusKey
		,StatusDateTime
		,EndDateTime
		,StateDuration
		,StatusDateTimeOnDay
		,EndDateTimeOnDay
		,StateDurationOnDay
		,I3TimeStampGMT
		,SeqNo	
	FROM #tbUserStatusDoNotNeedToBreakInDays


	SELECT
		ROW_NUMBER() OVER(ORDER BY UserId, LoginDateTimeInWorkgroup, StatusDateTime, StatusDateTimeOnDay) AS SequenceId
		,UserId_Seq
		,UserId
		,LoginDateTime
		,LogoutDateTime
		,LoginDuration
		,Workgroup
		,LoginDateTimeInWorkgroup
		,LogoutDateTimeInWorkgroup
		,LoginDurationInWorkgroup
		,StatusKey
		,StatusDateTime
		,EndDateTime
		,StateDuration
		,StatusDateTimeOnDay
		,EndDateTimeOnDay
		,StateDurationOnDay
		,I3TimeStampGMT
		,SeqNo	
	INTO #tbUserStatusByWorkgroupBrokenInDays
	FROM #tbUserStatusBrokenInDays
	ORDER BY UserId, LoginDateTimeInWorkgroup, StatusDateTime, StatusDateTimeOnDay

	IF @workgroup = '' OR @workgroup IS NULL
	BEGIN
		SELECT *
		FROM #tbUserStatusByWorkgroupBrokenInDays
		WHERE (UserID NOT IN (SELECT [Data] FROM [dbo].[FN_Split] (@ExcludeLoginID, ',')) OR UserID IS NULL)
		ORDER BY UserId, LoginDateTimeInWorkgroup, StatusDateTime, StatusDateTimeOnDay
	END
	ELSE
	BEGIN
		SELECT *
		FROM #tbUserStatusByWorkgroupBrokenInDays
		WHERE Workgroup = @workgroup
		AND (UserID NOT IN (SELECT [Data] FROM [dbo].[FN_Split] (@ExcludeLoginID, ',')) OR UserID IS NULL)
		ORDER BY UserId, LoginDateTimeInWorkgroup, StatusDateTime, StatusDateTimeOnDay
	END


	IF OBJECT_ID('tempdb..#tbAgentesTodosStatus') IS NOT NULL
		DROP TABLE #tbAgentesTodosStatus
	IF OBJECT_ID('tempdb..#tbAgentesTodosStatusParaProcessar') IS NOT NULL
		   DROP TABLE #tbAgentesTodosStatusParaProcessar

	IF OBJECT_ID('tempdb..#tbAgentesTodosStatusLoginParaProcessar') IS NOT NULL
		   DROP TABLE #tbAgentesTodosStatusLoginParaProcessar

	IF OBJECT_ID('tempdb..#tbAgentesTodosStatusLogin') IS NOT NULL
		   DROP TABLE #tbAgentesTodosStatusLogin

	IF OBJECT_ID('tempdb..#tbAgentesTodosStatusLoginResumido') IS NOT NULL
		   DROP TABLE #tbAgentesTodosStatusLoginResumido

	IF OBJECT_ID('tempdb..#tbAgentesResumo') IS NOT NULL
		   DROP TABLE #tbAgentesResumo

	IF OBJECT_ID('tempdb..#tbLoginsIncompletos') IS NOT NULL
		   DROP TABLE #tbLoginsIncompletos

	IF OBJECT_ID('tempdb..#tbLoginsCompletados') IS NOT NULL
		   DROP TABLE #tbLoginsCompletados

	IF OBJECT_ID('tempdb..#tbLogins') IS NOT NULL
		   DROP TABLE #tbLogins

	IF OBJECT_ID('tempdb..#tbAgentsStatusWorkgroup') IS NOT NULL
		DROP TABLE #tbAgentsStatusWorkgroup

	IF OBJECT_ID('tempdb..#tbStatusToProcess') IS NOT NULL
		DROP TABLE #tbStatusToProcess
	IF OBJECT_ID('tempdb..#tbWorkgroupActivation') IS NOT NULL
		DROP TABLE #tbWorkgroupActivation

	IF OBJECT_ID('tempdb..#tbWorkgroupActivatedOnLogin') IS NOT NULL
		DROP TABLE #tbWorkgroupActivatedOnLogin

	IF OBJECT_ID('tempdb..#tbLoginWorkgroupActivation') IS NOT NULL
		DROP TABLE #tbLoginWorkgroupActivation

	IF OBJECT_ID('tempdb..#tbLogoutWorkgroupActivation') IS NOT NULL
		DROP TABLE #tbLogoutWorkgroupActivation

	IF OBJECT_ID('tempdb..#tbLoginLogoutWorkgroupToProcess') IS NOT NULL
		DROP TABLE #tbLoginLogoutWorkgroupToProcess

	IF OBJECT_ID('tempdb..#tbLoginLogoutWorkgroup') IS NOT NULL
		DROP TABLE #tbLoginLogoutWorkgroup
	IF OBJECT_ID('tempdb..#tbUserStatusKeyLoggedIn') IS NOT NULL
		DROP TABLE #tbUserStatusKeyLoggedIn
	IF OBJECT_ID('tempdb..#tbUserStatusKeyLoggedInByWorkgroup') IS NOT NULL
		DROP TABLE #tbUserStatusKeyLoggedInByWorkgroup
	IF OBJECT_ID('tempdb..#tbUserStatusKeyLoggedInByWorkgroupToProcess') IS NOT NULL
		DROP TABLE #tbUserStatusKeyLoggedInByWorkgroupToProcess

	IF OBJECT_ID('tempdb..#tbUserStatusKeyLoggedInByWorkgroupProcessed') IS NOT NULL
		DROP TABLE #tbUserStatusKeyLoggedInByWorkgroupProcessed

	IF OBJECT_ID('tempdb..#tbUserStatusKeyByWorkgroupRemoveDuplicatedData') IS NOT NULL
		DROP TABLE #tbUserStatusKeyByWorkgroupRemoveDuplicatedData

	IF OBJECT_ID('tempdb..#tbUserStatusKeyByWorkgroup') IS NOT NULL
		DROP TABLE #tbUserStatusKeyByWorkgroup
	IF OBJECT_ID('tempdb..#tbUserStatusLevelDays') IS NOT NULL
		DROP TABLE #tbUserStatusLevelDays
	
	IF OBJECT_ID('tempdb..#tbUserStatusToBreakInDays') IS NOT NULL
		DROP TABLE #tbUserStatusToBreakInDays

	IF OBJECT_ID('tempdb..#tbUserStatusToBreakInDays') IS NOT NULL
		DROP TABLE #tbUserStatusToBreakInDays

	IF OBJECT_ID('tempdb..#tbUserStatusDoNotNeedToBreakInDays') IS NOT NULL
		DROP TABLE #tbUserStatusDoNotNeedToBreakInDays

	IF OBJECT_ID('tempdb..#tbUserStatusBrokenInDays') IS NOT NULL
		DROP TABLE #tbUserStatusBrokenInDays

	IF OBJECT_ID('tempdb..#tbUserStatusByWorkgroupBrokenInDays') IS NOT NULL
		DROP TABLE #tbUserStatusByWorkgroupBrokenInDays





END


GO


