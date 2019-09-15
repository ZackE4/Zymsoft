using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CapstoneTest.Models
{
    public partial class CapstoneContext : DbContext
    {
        public CapstoneContext()
        {
        }

        public CapstoneContext(DbContextOptions<CapstoneContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FoulLogs> FoulLogs { get; set; }
        public virtual DbSet<Games> Games { get; set; }
        public virtual DbSet<Leagues> Leagues { get; set; }
        public virtual DbSet<Logins> Logins { get; set; }
        public virtual DbSet<MediaLogs> MediaLogs { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<ScoringLogs> ScoringLogs { get; set; }
        public virtual DbSet<Seasons> Seasons { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=CS20\\VYMSOFT;Database=Capstone;User Id=sa;Password=Zymsoft2019!;ConnectRetryCount=0");
                optionsBuilder.UseSqlServer("Server=DESKTOP-12MJJEK\\SQLEXPRESS;Database=Capstone;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoulLogs>(entity =>
            {
                entity.HasKey(e => e.FouldLogId);

                entity.HasIndex(e => e.GameGameId)
                    .HasName("IX_FK_GameFoulLog");

                entity.HasIndex(e => e.PlayerPlayerId)
                    .HasName("IX_FK_PlayerFoulLog");

                entity.Property(e => e.GameGameId).HasColumnName("Game_GameId");

                entity.Property(e => e.PlayerPlayerId).HasColumnName("Player_PlayerId");

                entity.HasOne(d => d.GameGame)
                    .WithMany(p => p.FoulLogs)
                    .HasForeignKey(d => d.GameGameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameFoulLog");

                entity.HasOne(d => d.PlayerPlayer)
                    .WithMany(p => p.FoulLogs)
                    .HasForeignKey(d => d.PlayerPlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayerFoulLog");
            });

            modelBuilder.Entity<Games>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.HasIndex(e => e.SeasonSeasonId)
                    .HasName("IX_FK_SeasonGame");

                entity.Property(e => e.AwayTeamId).IsRequired();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.HomeTeamId).IsRequired();

                entity.Property(e => e.SeasonSeasonId).HasColumnName("Season_SeasonId");

                entity.HasOne(d => d.SeasonSeason)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.SeasonSeasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeasonGame");
            });

            modelBuilder.Entity<Leagues>(entity =>
            {
                entity.HasKey(e => e.LeagueId);

                entity.Property(e => e.HashPassword).IsRequired();

                entity.Property(e => e.LeagueKey).IsRequired();

                entity.Property(e => e.LeagueName).IsRequired();

                entity.Property(e => e.Logo).IsRequired();
            });

            modelBuilder.Entity<Logins>(entity =>
            {
                entity.HasKey(e => e.LoginId);

                entity.HasIndex(e => e.LeagueLeagueId)
                    .HasName("IX_FK_LoginLeague");

                entity.Property(e => e.Expiry).HasColumnType("datetime");

                entity.Property(e => e.LeagueLeagueId).HasColumnName("League_LeagueId");

                entity.Property(e => e.LoginKey).IsRequired();

                entity.Property(e => e.LoginTimestamp).HasColumnType("datetime");

                entity.HasOne(d => d.LeagueLeague)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.LeagueLeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LoginLeague");
            });

            modelBuilder.Entity<MediaLogs>(entity =>
            {
                entity.HasKey(e => e.MediaLogId);

                entity.HasIndex(e => e.GameGameId)
                    .HasName("IX_FK_GameMediaLog");

                entity.Property(e => e.GameGameId).HasColumnName("Game_GameId");

                entity.Property(e => e.MediaName).IsRequired();

                entity.HasOne(d => d.GameGame)
                    .WithMany(p => p.MediaLogs)
                    .HasForeignKey(d => d.GameGameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameMediaLog");
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.HasIndex(e => e.TeamTeamId)
                    .HasName("IX_FK_TeamPlayer");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Picture).IsRequired();

                entity.Property(e => e.PlayerNum).IsRequired();

                entity.Property(e => e.Position).IsRequired();

                entity.Property(e => e.TeamTeamId).HasColumnName("Team_TeamId");

                entity.HasOne(d => d.TeamTeam)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.TeamTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamPlayer");
            });

            modelBuilder.Entity<ScoringLogs>(entity =>
            {
                entity.HasKey(e => e.ScoringLogId);

                entity.HasIndex(e => e.GameGameId)
                    .HasName("IX_FK_GameScoringLog");

                entity.HasIndex(e => e.PlayerPlayerId)
                    .HasName("IX_FK_PlayerScoringLog");

                entity.Property(e => e.GameGameId).HasColumnName("Game_GameId");

                entity.Property(e => e.PlayerPlayerId).HasColumnName("Player_PlayerId");

                entity.HasOne(d => d.GameGame)
                    .WithMany(p => p.ScoringLogs)
                    .HasForeignKey(d => d.GameGameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameScoringLog");

                entity.HasOne(d => d.PlayerPlayer)
                    .WithMany(p => p.ScoringLogs)
                    .HasForeignKey(d => d.PlayerPlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayerScoringLog");
            });

            modelBuilder.Entity<Seasons>(entity =>
            {
                entity.HasKey(e => e.SeasonId);

                entity.HasIndex(e => e.LeagueLeagueId)
                    .HasName("IX_FK_LeagueSeason");

                entity.Property(e => e.LeagueLeagueId).HasColumnName("League_LeagueId");

                entity.Property(e => e.SeasonEnd).HasColumnType("datetime");

                entity.Property(e => e.SeasonStart).HasColumnType("datetime");

                entity.HasOne(d => d.LeagueLeague)
                    .WithMany(p => p.Seasons)
                    .HasForeignKey(d => d.LeagueLeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeagueSeason");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.HasIndex(e => e.LeagueLeagueId)
                    .HasName("IX_FK_LeagueTeam");

                entity.Property(e => e.CoachName).IsRequired();

                entity.Property(e => e.LeagueLeagueId).HasColumnName("League_LeagueId");

                entity.Property(e => e.Logo).IsRequired();

                entity.Property(e => e.TeamName).IsRequired();

                entity.HasOne(d => d.LeagueLeague)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.LeagueLeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeagueTeam");
            });
        }
    }
}
