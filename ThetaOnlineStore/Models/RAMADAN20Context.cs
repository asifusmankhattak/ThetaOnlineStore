using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ThetaOnlineStore.Models
{
    public partial class RAMADAN20Context : DbContext
    {
        public RAMADAN20Context()
        {
        }

        public RAMADAN20Context(DbContextOptions<RAMADAN20Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-7ME2D46;Database=RAMADAN20;User ID=sa; Password=123;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(50);

                entity.Property(e => e.LongDescription).HasColumnName("long_description");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Picture)
                    .HasColumnName("picture")
                    .HasMaxLength(50);

                entity.Property(e => e.ShortDescription)
                    .HasColumnName("short_description")
                    .HasMaxLength(10);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CostPrice).HasColumnName("cost_price");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(50);

                entity.Property(e => e.CurrentStock).HasColumnName("current_stock");

                entity.Property(e => e.LongDescription)
                    .HasColumnName("long_description")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.OpeningDate)
                    .HasColumnName("opening_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OpeningStock)
                    .HasColumnName("opening_stock")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductCode)
                    .HasColumnName("product_code")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductFeatures)
                    .HasColumnName("product_features")
                    .HasMaxLength(50);

                entity.Property(e => e.SalePrice).HasColumnName("sale_price");

                entity.Property(e => e.ShortDescription)
                    .HasColumnName("short_description")
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.ToTable("system_user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(50);

                entity.Property(e => e.DisplayName).HasColumnName("display_name");

                entity.Property(e => e.CV)
                 .HasColumnName("cv")
                 .HasMaxLength(200);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
