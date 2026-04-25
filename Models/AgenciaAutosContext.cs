using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AgenciaAutosMVC.Models;

public partial class AgenciaAutosContext : DbContext
{
    public AgenciaAutosContext()
    {
    }

    public AgenciaAutosContext(DbContextOptions<AgenciaAutosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<ProximoServicio> ProximoServicios { get; set; }

    public virtual DbSet<Refaccion> Refaccions { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServicioRefaccion> ServicioRefaccions { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.IdAdmin).HasName("PRIMARY");

            entity.ToTable("administrador");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.Usuario, "Usuario").IsUnique();

            entity.Property(e => e.IdAdmin)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Admin");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Usuario).HasMaxLength(50);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PRIMARY");

            entity.ToTable("cliente");

            entity.Property(e => e.IdCliente)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Cliente");
            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PRIMARY");

            entity.ToTable("marca");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.IdMarca)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Marca");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.IdModelo).HasName("PRIMARY");

            entity.ToTable("modelo");

            entity.HasIndex(e => e.IdMarca, "FK_Mod_Marca");

            entity.Property(e => e.IdModelo)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Modelo");
            entity.Property(e => e.Anio).HasColumnType("year(4)");
            entity.Property(e => e.IdMarca)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Marca");
            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Modelos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mod_Marca");
        });

        modelBuilder.Entity<ProximoServicio>(entity =>
        {
            entity.HasKey(e => e.IdProxServ).HasName("PRIMARY");

            entity.ToTable("proximo_servicio");

            entity.HasIndex(e => e.Folio, "Folio").IsUnique();

            entity.Property(e => e.IdProxServ)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Prox_Serv");
            entity.Property(e => e.FechaProg).HasColumnName("Fecha_Prog");
            entity.Property(e => e.Folio).HasColumnType("int(11)");
            entity.Property(e => e.KmProximo)
                .HasColumnType("int(11)")
                .HasColumnName("Km_Proximo");
            entity.Property(e => e.Notas).HasMaxLength(255);

            entity.HasOne(d => d.FolioNavigation).WithOne(p => p.ProximoServicio)
                .HasForeignKey<ProximoServicio>(d => d.Folio)
                .HasConstraintName("FK_Prox_Servicio");
        });

        modelBuilder.Entity<Refaccion>(entity =>
        {
            entity.HasKey(e => e.IdRefaccion).HasName("PRIMARY");

            entity.ToTable("refaccion");

            entity.HasIndex(e => e.Codigo, "Codigo").IsUnique();

            entity.Property(e => e.IdRefaccion)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Refaccion");
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasPrecision(10, 2);
            entity.Property(e => e.Stock).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Folio).HasName("PRIMARY");

            entity.ToTable("servicio");

            entity.HasIndex(e => e.IdAdmin, "FK_Ser_Admin");

            entity.HasIndex(e => e.IdTipoServ, "FK_Ser_TipoServ");

            entity.HasIndex(e => e.IdVehiculo, "FK_Ser_Vehiculo");

            entity.Property(e => e.Folio).HasColumnType("int(11)");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Estatus)
                .HasDefaultValueSql("'En espera'")
                .HasColumnType("enum('En espera','En proceso','Finalizado')");
            entity.Property(e => e.FechaIngreso).HasColumnName("Fecha_Ingreso");
            entity.Property(e => e.FechaSalida).HasColumnName("Fecha_Salida");
            entity.Property(e => e.IdAdmin)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Admin");
            entity.Property(e => e.IdTipoServ)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Tipo_Serv");
            entity.Property(e => e.IdVehiculo)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Vehiculo");
            entity.Property(e => e.QuienEntrego)
                .HasMaxLength(100)
                .HasColumnName("Quien_Entrego");

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ser_Admin");

            entity.HasOne(d => d.IdTipoServNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdTipoServ)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ser_TipoServ");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ser_Vehiculo");
        });

        modelBuilder.Entity<ServicioRefaccion>(entity =>
        {
            entity.HasKey(e => new { e.Folio, e.IdRefaccion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("servicio_refaccion");

            entity.HasIndex(e => e.IdRefaccion, "FK_SR_Refaccion");

            entity.Property(e => e.Folio).HasColumnType("int(11)");
            entity.Property(e => e.IdRefaccion)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Refaccion");
            entity.Property(e => e.Cantidad)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");
            entity.Property(e => e.PrecioAplicado)
                .HasPrecision(10, 2)
                .HasColumnName("Precio_Aplicado");

            entity.HasOne(d => d.FolioNavigation).WithMany(p => p.ServicioRefaccions)
                .HasForeignKey(d => d.Folio)
                .HasConstraintName("FK_SR_Servicio");

            entity.HasOne(d => d.IdRefaccionNavigation).WithMany(p => p.ServicioRefaccions)
                .HasForeignKey(d => d.IdRefaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SR_Refaccion");
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.IdTipoServ).HasName("PRIMARY");

            entity.ToTable("tipo_servicio");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.IdTipoServ)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Tipo_Serv");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PRIMARY");

            entity.ToTable("vehiculo");

            entity.HasIndex(e => e.IdCliente, "FK_Veh_Cliente");

            entity.HasIndex(e => e.IdModelo, "FK_Veh_Modelo");

            entity.HasIndex(e => e.NumSerie, "Num_Serie").IsUnique();

            entity.HasIndex(e => e.Placa, "Placa").IsUnique();

            entity.Property(e => e.IdVehiculo)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Vehiculo");
            entity.Property(e => e.Color).HasMaxLength(30);
            entity.Property(e => e.IdCliente)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Cliente");
            entity.Property(e => e.IdModelo)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Modelo");
            entity.Property(e => e.KmActual)
                .HasColumnType("int(11)")
                .HasColumnName("Km_Actual");
            entity.Property(e => e.NumSerie)
                .HasMaxLength(50)
                .HasColumnName("Num_Serie");
            entity.Property(e => e.Placa).HasMaxLength(20);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Veh_Cliente");

            entity.HasOne(d => d.IdModeloNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdModelo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Veh_Modelo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}