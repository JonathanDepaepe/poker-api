using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace DataAccessLayer.Models
{
    public partial class HpcmContext : DbContext
    {
        public HpcmContext()
        {
        }

        public HpcmContext(DbContextOptions<HpcmContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Announcement> Announcements { get; set; } = null!;
        public virtual DbSet<Club> Clubs { get; set; } = null!;
        public virtual DbSet<ClubMember> ClubMembers { get; set; } = null!;
        public virtual DbSet<Invitation> Invitations { get; set; } = null!;
        public virtual DbSet<Jwt> Jwts { get; set; } = null!;
        public virtual DbSet<League> Leagues { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<MemberType> MemberTypes { get; set; } = null!;
        public virtual DbSet<Tournament> Tournaments { get; set; } = null!;
        public virtual DbSet<TournamentEntry> TournamentEntries { get; set; } = null!;
        public virtual DbSet<TournamentLink> TournamentLinks { get; set; } = null!;
        public virtual DbSet<TournamentReservation> TournamentReservations { get; set; } = null!;
        public virtual DbSet<LoginModel> LoginModel { get; set; } = null!;
        public virtual DbSet<RegisterModel> RegisterModel { get; set; } = null!;
        public virtual DbSet<Response> Response { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:hpcm.database.windows.net,1433;Initial Catalog=Hpcm;Persist Security Info=False;User ID=DaanVanderHaegen;Password=Hpcm2022!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasKey(e => e.PostId)
                    .HasName("PK__Announce__AA12601834128234");

                entity.HasIndex(e => e.ClubId, "ClubPostRelation");

                entity.HasIndex(e => e.CreatorId, "CreatorRelation");

                entity.Property(e => e.CreationDateTime).HasPrecision(0);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Announcements)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ClubPostRelation");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Announcements)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CreatorRelation");
            });

            modelBuilder.Entity<Club>(entity =>
            {
                entity.ToTable("Club");

                entity.HasIndex(e => e.OwnerId, "OwnerRelation");

                entity.Property(e => e.CreationDateTime).HasPrecision(0);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.PictureUrl).HasMaxLength(500);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Clubs )
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OwnerRelation");
            });

            modelBuilder.Entity<ClubMember>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.ClubId, "ClubMembersRelation");

                entity.HasIndex(e => e.MemberId, "UserMembersRelation");

                entity.Property(e => e.Role).HasMaxLength(255);

                entity.HasOne(d => d.Club)
                    .WithMany()
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ClubMembersRelation");

                entity.HasOne(d => d.Member)
                    .WithMany()
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserMembersRelation");
            });

            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Invitation");

                entity.HasIndex(e => e.ClubId, "ClubRelation");

                entity.HasIndex(e => e.MemberId, "UserRelation");

                entity.Property(e => e.ExpirationDate).HasPrecision(0);

                entity.Property(e => e.InvitationHash).HasMaxLength(500);
                entity.Property(e => e.Role).HasMaxLength(500);

                entity.HasOne(d => d.Club)
                    .WithMany()
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ClubRelation");

                entity.HasOne(d => d.Member)
                    .WithMany()
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserRelation");
            });

            modelBuilder.Entity<Jwt>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Jwt");

                entity.HasIndex(e => e.MemberId, "UserJWTConnection");

                entity.Property(e => e.Token).HasMaxLength(500);

                entity.Property(e => e.TokenExpirationDate).HasPrecision(0);

                entity.HasOne(d => d.Member)
                    .WithMany()
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserJWTConnection");
            });
            
            modelBuilder.Entity<LoginModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LoginModel");
                
                entity.Property(e => e.Username).HasMaxLength(500);

                entity.Property(e => e.Password);
                
            });
            
            modelBuilder.Entity<RegisterModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RegisterModel");
                
                entity.Property(e => e.Username).HasMaxLength(500);
                entity.Property(e => e.Email).HasMaxLength(500);
                entity.Property(e => e.Password);
                
            });
            
            modelBuilder.Entity<Response>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Response");
                
                entity.Property(e => e.Status);

                entity.Property(e => e.Message);
                
            });

            
            modelBuilder.Entity<League>(entity =>
            {
                entity.HasIndex(e => e.ClubId, "ClubLeagueRelation");

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Leagues)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ClubLeagueRelation");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.HasIndex(e => e.TypeId, "UserTypeRelation");

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Nickname).HasMaxLength(255);

                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserTypeRelation");
            });

            modelBuilder.Entity<MemberType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__MemberTy__516F03B5D856EE9F");

                entity.ToTable("MemberType");

                entity.Property(e => e.Type).HasMaxLength(300);
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.Location).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Status).HasMaxLength(255);

                entity.Property(e => e.StartDateTime).HasPrecision(0);
            });

            modelBuilder.Entity<TournamentEntry>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.TournamentId, "TournamentEntriesRelation");

                entity.HasIndex(e => e.MemberId, "TournamentMembersRelation");

                entity.Property(e => e.RegistrationDateTime).HasPrecision(0);

                entity.Property(e => e.TournamentId).HasColumnName("TournamentID");

                entity.HasOne(d => d.Member)
                    .WithMany()
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TournamentMembersRelation");

                entity.HasOne(d => d.Tournament)
                    .WithMany()
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TournamentEntriesRelation");
            });

            modelBuilder.Entity<TournamentLink>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TournamentLink");

                entity.HasIndex(e => e.TournamentId, "TournamentRelation");

                entity.HasOne(d => d.Tournament)
                    .WithMany()
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TournamentRelation");
            });

            modelBuilder.Entity<TournamentReservation>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.MemberId, "MemberReservationRelation");

                entity.HasIndex(e => e.TournamentId, "TournamentReservationRelation");

                entity.HasOne(d => d.Member)
                    .WithMany()
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserReservationRelation");

                entity.HasOne(d => d.Tournament)
                    .WithMany()
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TournamentReservationRelation");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
