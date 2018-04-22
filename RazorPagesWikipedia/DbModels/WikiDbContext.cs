using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RazorPagesWikipedia.DbModels
{
    public partial class WikiDbContext : DbContext
    {
        public virtual DbSet<KpFirstlinks> KpFirstlinks { get; set; }
        public virtual DbSet<Page> Page { get; set; }

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
            modelBuilder.Entity<KpFirstlinks>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.ToTable("kp_firstlinks");

                entity.HasIndex(e => e.DestinationTitle)
                    .HasName("destination_title");

                entity.HasIndex(e => e.PageIsRedirect)
                    .HasName("redirect");

                entity.Property(e => e.PageId).HasColumnName("page_id");

                entity.Property(e => e.DestinationTitle)
                    .IsRequired()
                    .HasColumnName("destination_title")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PageIsRedirect)
                    .HasColumnName("page_is_redirect")
                    .HasDefaultValueSql("'0'");
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
