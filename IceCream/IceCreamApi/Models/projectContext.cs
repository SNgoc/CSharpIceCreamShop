using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IceCreamApi.Models
{
    public partial class projectContext : DbContext
    {
        public projectContext()
        {
        }

        public projectContext(DbContextOptions<projectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<BookIceCream> BookIceCreams { get; set; }
        public virtual DbSet<BookOrder> BookOrders { get; set; }
        public virtual DbSet<BookOrderDetail> BookOrderDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberOption> MemberOptions { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=project;User=sa;Password=1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BookIceCream>(entity =>
            {
                entity.HasKey(e => e.BookId)
                    .HasName("PK__BookIceC__3DE0C227AF42F483");

                entity.ToTable("BookIceCream");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.Author)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Image)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.BookIceCreams)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK_CatCode");
            });

            modelBuilder.Entity<BookOrder>(entity =>
            {
                entity.ToTable("BookOrder");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BookOrderDetail>(entity =>
            {
                entity.ToTable("BookOrderDetail");

                entity.HasOne(d => d.BookOrder)
                    .WithMany(p => p.BookOrderDetails)
                    .HasForeignKey(d => d.BookOrderId)
                    .HasConstraintName("FK_BookOrderDetail_BookOrder");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatId)
                    .HasName("PK__Category__6A1C8ADA94B8DC7F");

                entity.ToTable("Category");

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailUser)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NameUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.Reply).HasColumnType("text");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK_RecipeID");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.IdMemberOption).HasColumnName("Id_MemberOption");

                entity.Property(e => e.Password)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterDay).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdMemberOptionNavigation)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.IdMemberOption)
                    .HasConstraintName("FK_Member_MemberOption");
            });

            modelBuilder.Entity<MemberOption>(entity =>
            {
                entity.ToTable("MemberOption");

                entity.Property(e => e.Day)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.MemberOption1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MemberOption");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.ToTable("Page");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipe");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.CategoryID).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Ingredents).HasColumnType("text");

                entity.Property(e => e.Preparation).HasColumnType("text");

                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.CategoryID)
                    .HasConstraintName("FK_Recipe_Category");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
