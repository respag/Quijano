/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [dcp_codigo]
      ,[dcp_co_documento]
      ,[dcp_co_proceso]
      ,[dcp_co_etapa]
  FROM [SOLICITUD].[dbo].[PrmDocumentoProceso]

  USE [SOLICITUD]
GO

INSERT INTO [dbo].[PrmDocumentoProceso] ([dcp_codigo],[dcp_co_documento],[dcp_co_proceso] ,[dcp_co_etapa])
VALUES (139 ,1,1  , NULL)
GO

INSERT INTO [dbo].[PrmDocumentoProceso] ([dcp_codigo],[dcp_co_documento],[dcp_co_proceso] ,[dcp_co_etapa])
VALUES (140 ,2,1  , NULL)
GO
INSERT INTO [dbo].[PrmDocumentoProceso] ([dcp_codigo],[dcp_co_documento],[dcp_co_proceso] ,[dcp_co_etapa])
VALUES (141 ,3,1  , NULL)
GO

INSERT INTO [dbo].[PrmDocumentoProceso] ([dcp_codigo],[dcp_co_documento],[dcp_co_proceso] ,[dcp_co_etapa])
VALUES (142 ,4,1  , NULL)
GO

INSERT INTO [dbo].[PrmDocumentoProceso] ([dcp_codigo],[dcp_co_documento],[dcp_co_proceso] ,[dcp_co_etapa])
VALUES (143 ,5,1  , NULL)
GO

INSERT INTO [dbo].[PrmDocumentoProceso] ([dcp_codigo],[dcp_co_documento],[dcp_co_proceso] ,[dcp_co_etapa])
VALUES (144 ,6,1  , NULL)
GO

INSERT INTO [dbo].[PrmDocumentoProceso] ([dcp_codigo],[dcp_co_documento],[dcp_co_proceso] ,[dcp_co_etapa])
VALUES (145 ,7,1  , NULL)
GO
INSERT INTO [dbo].[PrmDocumentoProceso] ([dcp_codigo],[dcp_co_documento],[dcp_co_proceso] ,[dcp_co_etapa])
VALUES (146 ,8,1  , NULL)
GO





