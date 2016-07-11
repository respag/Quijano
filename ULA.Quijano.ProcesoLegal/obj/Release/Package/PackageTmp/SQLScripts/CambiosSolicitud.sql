USE [SOLICITUD]
GO

/****** Object:  Schema [ClienteCentrico]    Script Date: 2/10/2015 8:14:58 PM ******/
CREATE SCHEMA [ClienteCentrico]
GO

/****** Object:  Table [ClienteCentrico].[DetSolicitudDatosGenerales]    Script Date: 2/11/2015 10:40:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ClienteCentrico].[DetSolicitudDatosGenerales](
	[sdg_codigo] [int] NOT NULL,
	[sdg_co_solicitud] [int] NOT NULL,
	[sdg_co_cliente] [int] NOT NULL,
	[sdg_fecha_solicitud] [datetime] NOT NULL,
	[sdg_fecha_calculoHP] [datetime] NULL,
	[sdg_fecha_calculoPP] [datetime] NULL,
	[sdg_fecha_calculoPA] [datetime] NULL,
 CONSTRAINT [PK_DetSolicitudDatosGenerales] PRIMARY KEY CLUSTERED 
(
	[sdg_codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ClienteCentrico].[DetSolicitudDatosGenerales]  WITH CHECK ADD  CONSTRAINT [FK_DetSolicitudDatosGenerales_DetSolicitud] FOREIGN KEY([sdg_co_solicitud])
REFERENCES [dbo].[DetSolicitud] ([sol_co_solicitud])
GO

ALTER TABLE [ClienteCentrico].[DetSolicitudDatosGenerales] CHECK CONSTRAINT [FK_DetSolicitudDatosGenerales_DetSolicitud]
GO

/****** Object:  Table [ClienteCentrico].[DetObligacionesDetalle]    Script Date: 2/11/2015 10:40:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ClienteCentrico].[DetObligacionesDetalle](
	[od_codigo] [int] NOT NULL,
	[od_ob_codigo] [int] NOT NULL,
	[od_co_producto] [int] NOT NULL,
	[od_endeudamiento] [bit] NULL,
	[od_cancelacion] [bit] NULL,
 CONSTRAINT [PK_DetObligacionesDetalle] PRIMARY KEY CLUSTERED 
(
	[od_codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ClienteCentrico].[DetObligacionesDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DetObligacionesDetalle_DetObligaciones] FOREIGN KEY([od_ob_codigo])
REFERENCES [dbo].[DetObligaciones] ([ob_codigo])
GO

ALTER TABLE [ClienteCentrico].[DetObligacionesDetalle] CHECK CONSTRAINT [FK_DetObligacionesDetalle_DetObligaciones]
GO

/****** Object:  Table [ClienteCentrico].[DetCalculoPersonal]    Script Date: 2/11/2015 10:41:41 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ClienteCentrico].[DetCalculoPersonal](
	[calp_codigo] [int] NOT NULL,
	[calp_co_solicitud] [int] NOT NULL,
	[calp_co_producto] [int] NOT NULL,
	[calp_co_feria] [int] NOT NULL,
	[calp_co_segmento] [int] NOT NULL,
	[calp_co_tipo_prestamo] [varchar](50) NOT NULL,
	[calp_monto_prestamo] [decimal](18, 2) NULL,
	[calp_pagos] [int] NULL,
	[calp_plazos] [int] NULL,
	[calp_fecha_primer_descuento] [datetime] NULL,
	[calp_monto_bruto_recibir] [decimal](18, 2) NULL,
	[calp_monto_total] [int] NULL,
	[calp_letra_maxima_permitida] [decimal](18, 2) NULL,
	[calp_monto_maximo] [decimal](18, 2) NULL,
	[calp_tasa] [decimal](18, 2) NULL,
	[calp_tasa_efectiva] [decimal](18, 2) NULL,
	[calp_plazo_maximo] [int] NULL,
	[calp_periodo_gracia] [int] NULL,
	[calp_mes_no_pago] [int] NULL,
	[calp_con_seguro_vida] [decimal](18, 2) NULL,
	[calp_recargo_seguro_vida] [decimal](18, 2) NULL,
	[calp_fecha_efectiva_pago] [datetime] NULL,
	[calp_capital_intereses] [decimal](18, 2) NULL,
	[calp_seguro_vida] [decimal](18, 2) NULL,
	[calp_seguro_fraude] [decimal](18, 2) NULL,
	[calp_feci] [decimal](18, 2) NULL,
	[calp_mensualidad_total] [decimal](18, 2) NULL,
	[calp_capacidad_pago] [decimal](18, 2) NULL,
	[calp_endeudamiento_total] [decimal](18, 2) NULL,
	[calp_endeudamiento_PP] [decimal](18, 2) NULL,
	[calp_costo_vida] [decimal](18, 2) NULL,
	[calp_comision_manejo] [decimal](18, 2) NULL,
	[calp_comision] [decimal](18, 2) NULL,
	[calp_servicio_descuento] [decimal](18, 2) NULL,
	[calp_provicion_seguro_vida] [decimal](18, 2) NULL,
	[calp_provicion_seguro_fraude] [decimal](18, 2) NULL,
	[calp_timbres_fiscales] [decimal](18, 2) NULL,
	[calp_notaria] [decimal](18, 2) NULL,
	[calp_total_gastos] [decimal](18, 2) NULL,
	[calp_salario_mensual] [decimal](18, 2) NULL,
 CONSTRAINT [PK_DetCalculoPersonal] PRIMARY KEY CLUSTERED 
(
	[calp_codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ClienteCentrico].[DetCalculoPersonal]  WITH CHECK ADD  CONSTRAINT [FK_DetCalculoPersonal_DetSolicitud] FOREIGN KEY([calp_co_solicitud])
REFERENCES [dbo].[DetSolicitud] ([sol_co_solicitud])
GO

ALTER TABLE [ClienteCentrico].[DetCalculoPersonal] CHECK CONSTRAINT [FK_DetCalculoPersonal_DetSolicitud]
GO

INSERT INTO [SOLICITUD].[dbo].[CatProceso] ([pro_descripcion]) VALUES ('Solicitud Prestamo de Auto')
GO
INSERT INTO [SOLICITUD].[dbo].[CatProceso] ([pro_descripcion]) VALUES ('Solicitud Tarjetas de Credito')
GO




GO
ALTER TABLE [dbo].[DetSolicitud] ADD sol_fecha_creacion datetime NULL ;


