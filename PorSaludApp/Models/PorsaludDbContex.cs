using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace PorSaludApp.Models
{
        public class PorsaludDbContext : DbContext
        {
            public PorsaludDbContext() : base("name=PorsaludDbContext")
            {
                Configuration.LazyLoadingEnabled = false;
                Configuration.ProxyCreationEnabled = false;
            }

            public DbSet<Cliente> Clientes { get; set; }
            public DbSet<Documento> Documentos { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                // Configuraciones del modelo
                modelBuilder.Entity<Cliente>()
                    .Property(c => c.FechaCreacion)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

                modelBuilder.Entity<Documento>()
                    .Property(d => d.FechaCarga)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

                base.OnModelCreating(modelBuilder);
            }
        }
}