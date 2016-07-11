namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModeloQuijano : DbContext
    {
        public ModeloQuijano()
            : base("name=ModeloQuijano")
        {
        }

        public virtual DbSet<ASUNTOS_ORIGEN> ASUNTOS_ORIGEN { get; set; }
        public virtual DbSet<BITACORA_COMENTARIOS> BITACORA_COMENTARIOS { get; set; }
        public virtual DbSet<BPM_CATFORMS> BPM_CATFORMS { get; set; }
        public virtual DbSet<BPM_CATPROCESSES> BPM_CATPROCESSES { get; set; }
        public virtual DbSet<BPM_CATSTEPS> BPM_CATSTEPS { get; set; }
        public virtual DbSet<BPM_CATWEBAPLICATIONS> BPM_CATWEBAPLICATIONS { get; set; }
        public virtual DbSet<BPM_FORMSPROCESS> BPM_FORMSPROCESS { get; set; }
        public virtual DbSet<CATALOGO_ACCIONES> CATALOGO_ACCIONES { get; set; }
        public virtual DbSet<CATALOGO_AREA> CATALOGO_AREA { get; set; }
        public virtual DbSet<CATALOGO_ENTIDADES> CATALOGO_ENTIDADES { get; set; }
        public virtual DbSet<CATALOGO_IDIOMA> CATALOGO_IDIOMA { get; set; }
        public virtual DbSet<CATALOGO_PROCEDENCIA> CATALOGO_PROCEDENCIA { get; set; }
        public virtual DbSet<CATALOGO_TRAMITE> CATALOGO_TRAMITE { get; set; }
        public virtual DbSet<ESTADO_SOLICITUD> ESTADO_SOLICITUD { get; set; }
        public virtual DbSet<PERSONAL_SERVICIO> PERSONAL_SERVICIO { get; set; }
        public virtual DbSet<SOLICITAR_SERVICIO> SOLICITAR_SERVICIO { get; set; }
        public virtual DbSet<TRAMITES_POR_AREA> TRAMITES_POR_AREA { get; set; }
        public virtual DbSet<ABOGADO_AREAS> ABOGADO_AREAS { get; set; }
        public virtual DbSet<ABOGADO_JURISDICCION> ABOGADO_JURISDICCION { get; set; }
        public virtual DbSet<CATALOGO_TIPO_SOLICITUD> CATALOGO_TIPO_SOLICITUD { get; set; }
        public virtual DbSet<COMUNICACION> COMUNICACION { get; set; }
        public virtual DbSet<DATOS_CLIENTE> DATOS_CLIENTE { get; set; }
        public virtual DbSet<LIBRETA_DIRECCIONES> LIBRETA_DIRECCIONES { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ASUNTOS_ORIGEN>()
                .Property(e => e.ID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<ASUNTOS_ORIGEN>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<BITACORA_COMENTARIOS>()
                .Property(e => e.ID_COMENTARIO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BITACORA_COMENTARIOS>()
                .Property(e => e.PROCESO)
                .IsUnicode(false);

            modelBuilder.Entity<BITACORA_COMENTARIOS>()
                .Property(e => e.INCIDENTE)
                .IsUnicode(false);

            modelBuilder.Entity<BITACORA_COMENTARIOS>()
                .Property(e => e.USUARIO)
                .IsUnicode(false);

            modelBuilder.Entity<BITACORA_COMENTARIOS>()
                .Property(e => e.COMENTARIO)
                .IsUnicode(false);

            modelBuilder.Entity<BPM_CATFORMS>()
                .Property(e => e.IDFORM)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_CATFORMS>()
                .Property(e => e.IDWEBAPLICATION)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_CATFORMS>()
                .HasMany(e => e.BPM_FORMSPROCESS)
                .WithRequired(e => e.BPM_CATFORMS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BPM_CATPROCESSES>()
                .Property(e => e.IDPROCESS)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_CATPROCESSES>()
                .Property(e => e.PROCESSVERSION)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_CATPROCESSES>()
                .HasMany(e => e.BPM_CATSTEPS)
                .WithRequired(e => e.BPM_CATPROCESSES)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BPM_CATSTEPS>()
                .Property(e => e.IDSTEP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_CATSTEPS>()
                .Property(e => e.STEPNAME)
                .IsUnicode(false);

            modelBuilder.Entity<BPM_CATSTEPS>()
                .Property(e => e.IDPROCESS)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_CATSTEPS>()
                .HasMany(e => e.BPM_FORMSPROCESS)
                .WithRequired(e => e.BPM_CATSTEPS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BPM_CATWEBAPLICATIONS>()
                .Property(e => e.IDWEBAPLICATION)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_CATWEBAPLICATIONS>()
                .HasMany(e => e.BPM_CATFORMS)
                .WithRequired(e => e.BPM_CATWEBAPLICATIONS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BPM_FORMSPROCESS>()
                .Property(e => e.IDFORMPROCESS)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_FORMSPROCESS>()
                .Property(e => e.IDSTEP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_FORMSPROCESS>()
                .Property(e => e.FORMORDER)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BPM_FORMSPROCESS>()
                .Property(e => e.IDFORM)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CATALOGO_ACCIONES>()
                .Property(e => e.CODIGOACCION)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CATALOGO_ACCIONES>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<CATALOGO_ACCIONES>()
                .HasMany(e => e.SOLICITAR_SERVICIO)
                .WithOptional(e => e.CATALOGO_ACCIONES)
                .HasForeignKey(e => e.ID_ACCION);

            modelBuilder.Entity<CATALOGO_AREA>()
                .Property(e => e.CODAREA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CATALOGO_AREA>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<CATALOGO_ENTIDADES>()
                .Property(e => e.CODIGOENTIDAD)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CATALOGO_ENTIDADES>()
                .Property(e => e.ENTIDAD)
                .IsUnicode(false);

            modelBuilder.Entity<CATALOGO_ENTIDADES>()
                .HasMany(e => e.SOLICITAR_SERVICIO)
                .WithOptional(e => e.CATALOGO_ENTIDADES)
                .HasForeignKey(e => e.ID_ENTIDAD);

            modelBuilder.Entity<CATALOGO_IDIOMA>()
                .Property(e => e.CODIDIOMA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CATALOGO_IDIOMA>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<CATALOGO_PROCEDENCIA>()
                .Property(e => e.CODPROCEDENCIA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CATALOGO_PROCEDENCIA>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<CATALOGO_TRAMITE>()
                .Property(e => e.CODTRAMITE)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CATALOGO_TRAMITE>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<ESTADO_SOLICITUD>()
                .Property(e => e.CODIGO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<ESTADO_SOLICITUD>()
                .Property(e => e.NOMBRE_ESTADO)
                .IsUnicode(false);

            modelBuilder.Entity<ESTADO_SOLICITUD>()
                .Property(e => e.COMPLETAR_TAREA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PERSONAL_SERVICIO>()
                .Property(e => e.CODIGO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PERSONAL_SERVICIO>()
                .Property(e => e.NOMBRE_USUARIO)
                .IsUnicode(false);

            modelBuilder.Entity<PERSONAL_SERVICIO>()
                .Property(e => e.NOMBRE_COMPLETO)
                .IsUnicode(false);

            modelBuilder.Entity<PERSONAL_SERVICIO>()
                .Property(e => e.CORREO_ELECTRONICO)
                .IsUnicode(false);

            modelBuilder.Entity<PERSONAL_SERVICIO>()
                .Property(e => e.TIPO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.SOLICITUD)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.ID_ENTIDAD)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.SOCIEDAD_ENTIDAD_EMPRESA)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.NOMBRE_PARA)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.NOMBRE_DE)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.TRAMITE)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.DETALLE)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.DIRECCION)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.PISO)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.CALLE)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.REFERENCIAS)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.HORARIO)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.TELEFONO)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.ASUNTO)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.ID_ACCION)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.ID_RESPONSABLE)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.ESTADO)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.COMENTARIOS)
                .IsUnicode(false);

            modelBuilder.Entity<SOLICITAR_SERVICIO>()
                .Property(e => e.INCIDENTE)
                .HasPrecision(38, 0);

            modelBuilder.Entity<TRAMITES_POR_AREA>()
                .Property(e => e.IDTRAMITE)
                .HasPrecision(38, 0);

            modelBuilder.Entity<TRAMITES_POR_AREA>()
                .Property(e => e.IDAREA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<ABOGADO_AREAS>()
                .Property(e => e.CODIGO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<ABOGADO_AREAS>()
                .Property(e => e.NOMBRE_USUARIO)
                .IsUnicode(false);

            modelBuilder.Entity<ABOGADO_AREAS>()
                .Property(e => e.NOMBRE_COMPLETO)
                .IsUnicode(false);

            modelBuilder.Entity<ABOGADO_JURISDICCION>()
                .Property(e => e.CODIGO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<ABOGADO_JURISDICCION>()
                .Property(e => e.NOMBRE_USUARIO)
                .IsUnicode(false);

            modelBuilder.Entity<ABOGADO_JURISDICCION>()
                .Property(e => e.NOMBRE_COMPLETO)
                .IsUnicode(false);

            modelBuilder.Entity<CATALOGO_TIPO_SOLICITUD>()
                .Property(e => e.CODIGO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CATALOGO_TIPO_SOLICITUD>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.INCIDENTE)
                .HasPrecision(38, 0);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.IDCLIENTE)
                .HasPrecision(38, 0);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.INCIDENTE_RELACIONADO)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CODIDIOMA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CODPROCEDENCIA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CODAREA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CODJURISDICCION)
                .HasPrecision(38, 0);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CODTRAMITE)
                .HasPrecision(38, 0);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.ABOGADO_ASIGNADO)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CORREO_ORIGEN)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CORREO_PARA)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.ASUNTO)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.ASUNTO_ORIGEN)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CONTENIDO)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.CORREO_INFRACTOR)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.RESPUESTA)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.ABOGADO_RESPONDE)
                .IsUnicode(false);

            modelBuilder.Entity<COMUNICACION>()
                .Property(e => e.GENERA_INSTRUCCION)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.ID_DATOS_CLIENTE)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.COD)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.CLIENTE_EXISTENTE)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.ESTADO_CUMPLIMIENTO)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.NIVEL_RIESGO)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.PUNTAJE_RIESGO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.PASAPORTE)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.IDIOMA)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.TELEFONO_CASA)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.PAIS)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.MONEDA)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.OCUPACION)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.CELULAR)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.SECTOR_ECONOMICO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.PEP)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.TIPO_PERSONA)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.CLASIFICACION_CLIENTE)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.REQ_FORMULARIO)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.REQ_PASAPORTE)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.REQ_REFERENCIA)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.REQ_PROFESIONAL)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.REQ_DOMICILIO)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.REQ_INCORP)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.REQ_PROMOCIONALES)
                .IsUnicode(false);

            modelBuilder.Entity<DATOS_CLIENTE>()
                .Property(e => e.REQ_MEMBRESIA)
                .IsUnicode(false);

            modelBuilder.Entity<LIBRETA_DIRECCIONES>()
                .Property(e => e.ID_DIRECCION)
                .HasPrecision(38, 0);

            modelBuilder.Entity<LIBRETA_DIRECCIONES>()
                .Property(e => e.NOMBRE_FACTURA)
                .IsUnicode(false);

            modelBuilder.Entity<LIBRETA_DIRECCIONES>()
                .Property(e => e.CODIGO_DIRECCION)
                .IsUnicode(false);

            modelBuilder.Entity<LIBRETA_DIRECCIONES>()
                .Property(e => e.DIRECCION)
                .IsUnicode(false);

            modelBuilder.Entity<LIBRETA_DIRECCIONES>()
                .Property(e => e.CODIGO_CLIENTE)
                .HasPrecision(38, 0);
        }
    }
}
