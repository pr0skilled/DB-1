using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DatabaseLab3.Models.DataBaseModels
{
    public partial class DatabaseContext : DbContext 
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Anime> Animes { get; set; }
        public virtual DbSet<Girl> Girls { get; set; }
        public virtual DbSet<LinksAnimeProducer> LinksAnimeProducers { get; set; }
        public virtual DbSet<LinksGirlsAnime> LinksGirlsAnimes { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=anime_girls;Username=postgres;Password=asd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anime>(entity =>
            {
                entity.ToTable("anime");

                entity.Property(e => e.AnimeId)
                    .HasColumnName("anime_id")
                    .HasDefaultValueSql("nextval('anime_id_anime_seq'::regclass)");

                entity.Property(e => e.Rating)
                    .HasPrecision(4, 2)
                    .HasColumnName("rating");

                entity.Property(e => e.Series).HasColumnName("series");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<Girl>(entity =>
            {
                entity.ToTable("girls");

                entity.Property(e => e.GirlId)
                    .HasColumnName("girl_id")
                    .HasDefaultValueSql("nextval('girls_id_girl_seq'::regclass)");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Eyes)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("eyes");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fullname");

                entity.Property(e => e.Hair)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("hair");
            });

            modelBuilder.Entity<LinksAnimeProducer>(entity =>
            {
                entity.ToTable("links_anime_producers");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('links2_id_link1_seq'::regclass)");

                entity.Property(e => e.AnimeId).HasColumnName("anime_id");

                entity.Property(e => e.ProducerId).HasColumnName("producer_id");

                entity.HasOne(d => d.Anime)
                    .WithMany(p => p.LinksAnimeProducers)
                    .HasForeignKey(d => d.AnimeId)
                    .HasConstraintName("anime_fkey");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.LinksAnimeProducers)
                    .HasForeignKey(d => d.ProducerId)
                    .HasConstraintName("producer_fkey");
            });

            modelBuilder.Entity<LinksGirlsAnime>(entity =>
            {
                entity.ToTable("links_girls_anime");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('links1_id_link1_seq'::regclass)");

                entity.Property(e => e.AnimeId).HasColumnName("anime_id");

                entity.Property(e => e.GirlId).HasColumnName("girl_id");

                entity.HasOne(d => d.Anime)
                    .WithMany(p => p.LinksGirlsAnimes)
                    .HasForeignKey(d => d.AnimeId)
                    .HasConstraintName("anime_fkey");

                entity.HasOne(d => d.Girl)
                    .WithMany(p => p.LinksGirlsAnimes)
                    .HasForeignKey(d => d.GirlId)
                    .HasConstraintName("girl_fkey");
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.ToTable("producers");

                entity.Property(e => e.ProducerId)
                    .HasColumnName("producer_id")
                    .HasDefaultValueSql("nextval('producers_id_producer_seq'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NumberOfWorks).HasColumnName("number_of_works");

                entity.Property(e => e.Studio)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("studio");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
