using BookTradeAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookTradeAPI.Data;

public partial class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookExchange> BookExchanges { get; set; }

    public virtual DbSet<BookExchangeDetail> BookExchangeDetails { get; set; }

    public virtual DbSet<BookExchangePost> BookExchangePosts { get; set; }

    public virtual DbSet<BookExchangeRequest> BookExchangeRequests { get; set; }

    public virtual DbSet<BookReview> BookReviews { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderBookExchange> OrderBookExchanges { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3213E83F1D02F890");

            entity.ToTable("Book");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .HasColumnName("author");
            entity.Property(e => e.AverageRating)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("average_rating");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("base_price");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Condition).HasColumnName("condition");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DiscountPrice)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("discount_price");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.ShopId).HasColumnName("shop_id");
            entity.Property(e => e.SoldQuantity).HasColumnName("sold_quantity");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.TotalRating).HasColumnName("total_rating");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Book__category_i__5535A963");

            entity.HasOne(d => d.Shop).WithMany(p => p.Books)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK__Book__shop_id__5629CD9C");
        });

        modelBuilder.Entity<BookExchange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookExch__3213E83FC9142806");

            entity.ToTable("BookExchange");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .HasColumnName("author");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Condition).HasColumnName("condition");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedQuantity).HasColumnName("created_quantity");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.ExchangeableQuantity).HasColumnName("exchangeable_quantity");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.BookExchanges)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__BookExcha__categ__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.BookExchanges)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BookExcha__user___5812160E");
        });

        modelBuilder.Entity<BookExchangeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookExch__3213E83F23468FF4");

            entity.ToTable("BookExchangeDetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookExchangeId).HasColumnName("book_exchange_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.BookExchange).WithMany(p => p.BookExchangeDetails)
                .HasForeignKey(d => d.BookExchangeId)
                .HasConstraintName("FK__BookExcha__book___59063A47");
        });

        modelBuilder.Entity<BookExchangePost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookExch__3213E83FA0D15C35");

            entity.ToTable("BookExchangePost");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookExchangeId).HasColumnName("book_exchange_id");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExchangeableQuantity).HasColumnName("exchangeable_quantity");

            entity.HasOne(d => d.BookExchange).WithMany(p => p.BookExchangePosts)
                .HasForeignKey(d => d.BookExchangeId)
                .HasConstraintName("FK__BookExcha__book___59FA5E80");
        });

        modelBuilder.Entity<BookExchangeRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookExch__3213E83F2C859E9C");

            entity.ToTable("BookExchangeRequest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CancelReason)
                .HasMaxLength(100)
                .HasColumnName("cancel_reason");
            entity.Property(e => e.ChooseBookExchangeDetailId).HasColumnName("choose_book_exchange_detail_id");
            entity.Property(e => e.ChooseBookExchangeDetailStatusOld).HasColumnName("choose_book_exchange_detail_status_old");
            entity.Property(e => e.PostBookExchangeDetailId).HasColumnName("post_book_exchange_detail_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.ChooseBookExchangeDetail).WithMany(p => p.BookExchangeRequestChooseBookExchangeDetails)
                .HasForeignKey(d => d.ChooseBookExchangeDetailId)
                .HasConstraintName("FK__BookExcha__choos__5BE2A6F2");

            entity.HasOne(d => d.PostBookExchangeDetail).WithMany(p => p.BookExchangeRequestPostBookExchangeDetails)
                .HasForeignKey(d => d.PostBookExchangeDetailId)
                .HasConstraintName("FK__BookExcha__post___5AEE82B9");
        });

        modelBuilder.Entity<BookReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookRevi__3213E83FA03D6107");

            entity.ToTable("BookReview");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .HasColumnName("content");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReviewDate).HasColumnName("review_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.BookReviews)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookRevie__book___6B24EA82");

            entity.HasOne(d => d.User).WithMany(p => p.BookReviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BookRevie__user___6A30C649");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3213E83F62A094CE");

            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cart__user_id__6754599E");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.BookId }).HasName("PK__CartItem__2A65FB89E5619962");

            entity.ToTable("CartItem");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Book).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__book_i__693CA210");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__cart_i__68487DD7");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F178CB119");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83FE1E63353");

            entity.ToTable("Order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.BuyerId).HasColumnName("buyer_id");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.ShopId).HasColumnName("shop_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("total");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK__Order__buyer_id__628FA481");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__Order__payment_i__6477ECF3");

            entity.HasOne(d => d.Shop).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK__Order__shop_id__6383C8BA");
        });

        modelBuilder.Entity<OrderBookExchange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderBoo__3213E83FE1EE4DD2");

            entity.ToTable("OrderBookExchange");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChooseBookExchangeId).HasColumnName("choose_book_exchange_id");
            entity.Property(e => e.ChooseOrderStatus).HasColumnName("choose_order_status");
            entity.Property(e => e.ChooseUserId).HasColumnName("choose_user_id");
            entity.Property(e => e.ExchangeDate).HasColumnName("exchange_date");
            entity.Property(e => e.PostBookExchangeId).HasColumnName("post_book_exchange_id");
            entity.Property(e => e.PostOrderStatus).HasColumnName("post_order_status");
            entity.Property(e => e.PostUserId).HasColumnName("post_user_id");

            entity.HasOne(d => d.ChooseBookExchange).WithMany(p => p.OrderBookExchangeChooseBookExchanges)
                .HasForeignKey(d => d.ChooseBookExchangeId)
                .HasConstraintName("FK__OrderBook__choos__5FB337D6");

            entity.HasOne(d => d.ChooseUser).WithMany(p => p.OrderBookExchangeChooseUsers)
                .HasForeignKey(d => d.ChooseUserId)
                .HasConstraintName("FK__OrderBook__choos__5EBF139D");

            entity.HasOne(d => d.PostBookExchange).WithMany(p => p.OrderBookExchangePostBookExchanges)
                .HasForeignKey(d => d.PostBookExchangeId)
                .HasConstraintName("FK__OrderBook__post___5DCAEF64");

            entity.HasOne(d => d.PostUser).WithMany(p => p.OrderBookExchangePostUsers)
                .HasForeignKey(d => d.PostUserId)
                .HasConstraintName("FK__OrderBook__post___5CD6CB2B");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.BookId }).HasName("PK__OrderIte__42C9B3875A8121CA");

            entity.ToTable("OrderItem");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__book___66603565");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__order__656C112C");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3213E83F9D9FCA61");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("total");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shop__3213E83FC53AF6FC");

            entity.ToTable("Shop");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BannerUrl)
                .HasMaxLength(255)
                .HasColumnName("banner_url");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Shops)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Shop__user_id__5441852A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FBA4D6AC2");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatar_url");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasColumnName("full_name");

            entity.HasMany(d => d.Books).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__FavoriteB__book___619B8048"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__FavoriteB__user___60A75C0F"),
                    j =>
                    {
                        j.HasKey("UserId", "BookId").HasName("PK__Favorite__BD2EE6A189BC3702");
                        j.ToTable("FavoriteBook");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("BookId").HasColumnName("book_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
