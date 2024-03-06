using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WatchShop2.Models;

public partial class WatchShop2Context : DbContext
{
    public WatchShop2Context()
    {
    }

    public WatchShop2Context(DbContextOptions<WatchShop2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserContact> UserContacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DuyDat");

    public List<Size> GetSizes(string ProductName = "", string Categories = "")
    {
        var sizes = this.Sizes.FromSqlRaw("EXECUTE pr_GetSizes @ProductName, @Categories",
            new SqlParameter("@ProductName", ProductName),
            new SqlParameter("@Categories", Categories)
        ).ToList();
        return sizes;
    }

    public List<Color> GetColors(string ProductName = "", string Categories = "")
    {
        var colors = this.Colors.FromSqlRaw("EXECUTE pr_GetColors @ProductName, @Categories",
            new SqlParameter("@ProductName", ProductName),
            new SqlParameter("@Categories", Categories)
        ).ToList();
        return colors;
    }

    public List<Product> FilterProducts(string ProductName = "", string Categories = "", string Colors = "", string Sizes = "", float PriceStart = 0, float PriceEnd = float.MaxValue, int PageNumber = 1, int PageSize = 10, string sort = "auto")
    {
        var products = this.Products.FromSqlRaw("EXECUTE pr_FilterProducts @ProductName, @Categories, @Colors, @Sizes, @PriceStart, @PriceEnd, @PageNumber, @PageSize, @SortType",
            new SqlParameter("@ProductName", ProductName),
            new SqlParameter("@Categories", Categories),
            new SqlParameter("@Colors", Colors),
            new SqlParameter("@Sizes", Sizes),
            new SqlParameter("@PriceStart", PriceStart),
            new SqlParameter("@PriceEnd", PriceEnd),
            new SqlParameter("@PageNumber", PageNumber),
            new SqlParameter("@PageSize", PageSize),
            new SqlParameter("@SortType", sort)
        )
        .ToList();

        return products;
    }

    public Product? GetProductBySlug(string ProductSlug)
    {
        return this.Products.FromSqlRaw("EXECUTE pr_GetProductBySlug @ProductSlug",
            new SqlParameter("@ProductSlug", ProductSlug)
        ).ToList().SingleOrDefault();
    }

    public int AddProduct(string Categories, int ColorId, int SizeId, string ProductName, string ProductSlug, string ProductDesc, double Price, int Quantity, int Discount, string ProductImages)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_AddProduct @Categories, @ColorId, @SizeId, @ProductName, @ProductSlug, @ProductDesc, @Price, @Quantity, @Discount, @ProductImages",
            new SqlParameter("@Categories", Categories),
            new SqlParameter("@ColorId", ColorId),
            new SqlParameter("@SizeId", SizeId),
            new SqlParameter("@ProductName", ProductName),
            new SqlParameter("@ProductSlug", ProductSlug),
            new SqlParameter("@ProductDesc", ProductDesc),
            new SqlParameter("@Price", Price),
            new SqlParameter("@Quantity", Quantity),
            new SqlParameter("@Discount", Discount),
            new SqlParameter("@ProductImages", ProductImages)
        );
    }

    public int SignUp(string Email, string Password, string FirstName, string LastName, bool Gender, DateOnly? Birthdate = null)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_SignUp @Email, @Password, @FirstName, @LastName, @Gender, @Birthdate",
            new SqlParameter("@Email", Email),
            new SqlParameter("@Password", Password),
            new SqlParameter("@FirstName", FirstName),
            new SqlParameter("@LastName", LastName),
            new SqlParameter("@Gender", Gender),
            new SqlParameter("@Birthdate", Birthdate == null ? DBNull.Value : Birthdate)
        );
    }

    public User? SignIn(string Email)
    {
        return this.Users.FromSqlRaw("EXECUTE pr_SignIn @Email",
            new SqlParameter("@Email", Email)
        ).ToList().SingleOrDefault();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__51BCD7B79624B2D7");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_User");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.HasKey(e => e.CartDetailId).HasName("PK__CartDeta__01B6A6B4A21CF3D0");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartDetail_Cart");

            entity.HasOne(d => d.Product).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartDetail_Product");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B9F895855");

            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Colors__8DA7674D0D67560B");

            entity.HasIndex(e => e.ColorValue, "UQ__Colors__2E5AF42661A1BEC0").IsUnique();

            entity.HasIndex(e => e.ColorName, "UQ__Colors__C71A5A7B674EBF8D").IsUnique();

            entity.Property(e => e.ColorName).HasMaxLength(255);
            entity.Property(e => e.ColorValue)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFBC20A744");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36CC979560E");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A388F5BCA9C");

            entity.Property(e => e.BankName).HasMaxLength(255);
            entity.Property(e => e.CardNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethod).HasMaxLength(255);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Order");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD840D5E5C");

            entity.HasIndex(e => e.ProductSlug, "UQ__Products__A8918E70882D5F5D").IsUnique();

            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.ProductSlug)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Color).WithMany(p => p.Products)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Color");

            entity.HasOne(d => d.Size).WithMany(p => p.Products)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Size");

            entity.HasMany(d => d.Categories).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductCategory_Category"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductCategory_Product"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId");
                        j.ToTable("ProductCategory");
                    });
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF87C10AB8081");

            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasDefaultValue("");

            entity.HasOne(d => d.Product).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Product");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_User");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Replies__C25E46096091F731");

            entity.Property(e => e.Comment).HasDefaultValue("");

            entity.HasOne(d => d.Rating).WithMany(p => p.Replies)
                .HasForeignKey(d => d.RatingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reply_Rate");

            entity.HasOne(d => d.User).WithMany(p => p.Replies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reply_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A272A914C");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61603765B3B4").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Sizes__83BD097A0622FED4");

            entity.HasIndex(e => e.SizeName, "UQ__Sizes__619EFC3E9410370E").IsUnique();

            entity.Property(e => e.SizeName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CBAA3DFDC");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C4F2E4BD").IsUnique();

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<UserContact>(entity =>
        {
            entity.HasKey(e => e.UserContactId).HasName("PK__UserCont__3911BAA5F6E8085D");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserContacts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserContact_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
