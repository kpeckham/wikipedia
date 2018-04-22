using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RazorPagesWikipedia.DbModels
{
    public partial class WikiDbContext : DbContext
    {
        public virtual DbSet<Categorylinks> Categorylinks { get; set; }
        public virtual DbSet<Page> Page { get; set; }

        // Unable to generate entity type for table 'kp_firstlinks'. Please see the warning messages.
        // Unable to generate entity type for table 'pagelinks'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=enwiki_20180401;uid=razor_ro;password=nNXGsnqo731UaVoB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorylinks>(entity =>
            {
                entity.HasKey(e => new { e.ClFrom, e.ClTo });

                entity.ToTable("categorylinks");

                entity.HasIndex(e => new { e.ClTo, e.ClTimestamp })
                    .HasName("cl_timestamp");

                entity.Property(e => e.ClFrom)
                    .HasColumnName("cl_from")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ClTo)
                    .HasColumnName("cl_to")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ClCollation)
                    .IsRequired()
                    .HasColumnName("cl_collation")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ClSortkey)
                    .IsRequired()
                    .HasColumnName("cl_sortkey")
                    .HasMaxLength(230)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ClSortkeyPrefix)
                    .IsRequired()
                    .HasColumnName("cl_sortkey_prefix")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ClTimestamp)
                    .HasColumnName("cl_timestamp")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.ToTable("page");

                entity.HasIndex(e => e.PageLen)
                    .HasName("page_len");

                entity.HasIndex(e => new { e.PageNamespace, e.PageTitle })
                    .HasName("name_title")
                    .IsUnique();

                entity.HasIndex(e => new { e.PageIsRedirect, e.PageNamespace, e.PageLen })
                    .HasName("page_redirect_namespace_len");

                entity.Property(e => e.PageId).HasColumnName("page_id");

                entity.Property(e => e.PageContentModel)
                    .HasColumnName("page_content_model")
                    .HasMaxLength(32);

                entity.Property(e => e.PageCounter)
                    .HasColumnName("page_counter")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageIsNew)
                    .HasColumnName("page_is_new")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageIsRedirect)
                    .HasColumnName("page_is_redirect")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageLang)
                    .HasColumnName("page_lang")
                    .HasMaxLength(35);

                entity.Property(e => e.PageLatest)
                    .HasColumnName("page_latest")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageLen)
                    .HasColumnName("page_len")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageLinksUpdated)
                    .HasColumnName("page_links_updated")
                    .HasMaxLength(14);

                entity.Property(e => e.PageNamespace)
                    .HasColumnName("page_namespace")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageRestrictions)
                    .IsRequired()
                    .HasColumnName("page_restrictions")
                    .HasColumnType("tinyblob");

                entity.Property(e => e.PageTitle)
                    .IsRequired()
                    .HasColumnName("page_title")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PageTouched)
                    .IsRequired()
                    .HasColumnName("page_touched")
                    .HasMaxLength(14)
                    .HasDefaultValueSql("''");
            });
        }
    }
}
