using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AnunciaPicos.Backend.Infrastructure.Data
{
    public class AnunciaPicosDbContext : DbContext
    {
        public AnunciaPicosDbContext(DbContextOptions<AnunciaPicosDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ConversationModel> Conversation { get; set; }
        public DbSet<EvaluationModel> Evaluation { get; set; }
        public DbSet<MessageModel> Message { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<FavoriteModel> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do relacionamento entre UserModel e ProductModel
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração do relacionamento de muitos-para-um entre Conversation e UserModel
            modelBuilder.Entity<ConversationModel>()
                .HasOne(c => c.userModel1)
                .WithMany()
                .HasForeignKey(c => c.UserId1)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConversationModel>()
                .HasOne(c => c.userModel2)
                .WithMany()
                .HasForeignKey(c => c.UserId2)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento entre Message e Conversation
            modelBuilder.Entity<MessageModel>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento entre Message e User (Sender)
            modelBuilder.Entity<MessageModel>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamentos para EvaluationModel
            modelBuilder.Entity<EvaluationModel>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<EvaluationModel>()
                .HasOne(e => e.UserEvaluated)
                .WithMany()
                .HasForeignKey(e => e.UserIdEvaluated);

            // NOVO: Configuração para Pagamentos (substituindo Subscriptions)
            modelBuilder.Entity<PaymentModel>()
                .HasOne(p => p.User) // Propriedade de navegação para UserModel
                .WithMany(u => u.Payments) // Coleção de pagamentos no UserModel
                .HasForeignKey(p => p.UserId) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PaymentModel>()
                .Property(p => p.SessionId)
                .IsRequired();

            modelBuilder.Entity<PaymentModel>()
                .HasIndex(p => p.SessionId)
                .IsUnique();

            modelBuilder.Entity<PaymentModel>()
                .HasIndex(p => p.PaymentIntentId)
                .IsUnique();

            modelBuilder.Entity<PaymentModel>()
                .Property(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<PaymentModel>()
                .Property(p => p.PlanType)
                .IsRequired();

            modelBuilder.Entity<PaymentModel>()
                .Property(p => p.PurchaseDate)
                .IsRequired();

            modelBuilder.Entity<PaymentModel>()
                .Property(p => p.ExpirationDate)
                .IsRequired();

            modelBuilder.Entity<FavoriteModel>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<FavoriteModel>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<FavoriteModel>()
                .HasOne(f => f.Product)
                .WithMany(p => p.FavoritedBy)
                .HasForeignKey(f => f.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}