using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using IPGeo.Data.Models;
using IPGeo.Data.Strategies;

namespace IPGeo.Data
{
    public class IPGeoContext : DbContext, ISetDataStrategy
    {
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<IP> IPs { get; set; }
        public virtual DbSet<History> History { get; set; }

        public ISetDataStrategy SetDataStrategy { get; set; }

        public IPGeoContext(DbContextOptions<IPGeoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack");

            modelBuilder.Entity<History>(b =>
            {
                b.ToTable("history");

                b.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseNpgsqlIdentityAlwaysColumn();

                b.Property(e => e.UpdatedAt)
                    .HasColumnName("update_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Country>(b =>
            {
                b.ToTable("countries");

                b.HasIndex(e => e.Code)
                    .HasName("countries_code_key")
                    .IsUnique();

                b.HasIndex(e => e.Name)
                    .HasName("countries_name_key")
                    .IsUnique();

                b.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseNpgsqlIdentityAlwaysColumn();

                b.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("character(2)");

                b.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<IP>(b =>
            {
                b.ToTable("ips").HasKey(e => new { e.IpFrom, e.IpTo });

                b.Property(e => e.IpFrom).HasColumnName("ip_from");

                b.Property(e => e.IpTo).HasColumnName("ip_to");

                b.Property(e => e.CountryId).HasColumnName("country_id");

                b.HasOne(d => d.Country)
                    .WithMany(p => p.IPs)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("ips_country_id_fkey");
            });
        }

        public void SetData(string filePath)
        {
            SetDataStrategy.SetData(filePath);
        }
    }
}
