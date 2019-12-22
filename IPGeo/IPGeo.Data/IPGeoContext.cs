using IPGeo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IPGeo.Data
{
    public class IPGeoContext : DbContext
    {
        public virtual DbSet<IPGeoDataPair> IPGeoDataCollection { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your 
#warning connection string, you should move it out of source code. 
# warning See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.

                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=Password1!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("admin");

            modelBuilder.Entity<IPGeoDataPair>(b =>
            {
                b.ToTable("ip2location_db1")
                    .HasKey(e => new { e.IpFrom, e.IpTo });

                b.Property(e => e.IpFrom).HasColumnName("ip_from");
                b.Property(e => e.IpTo).HasColumnName("ip_to");
                b.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("country_code")
                    .HasColumnType("character(2)");
                b.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnName("country_name")
                    .HasMaxLength(64);
            });
        }
    }
}
