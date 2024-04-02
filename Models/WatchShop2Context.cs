using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

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

    public User? GetUserById(int UserId)
    {
        return this.Users.FromSqlRaw("EXECUTE pr_GetUserById @UserId",
            new SqlParameter("@UserId", UserId)
        ).ToList().SingleOrDefault();
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

    // Account
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

    public int UpdateUserPassword(int UserId, string Password)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_UpdateUserPassword @UserId, @Password",
            new SqlParameter("@UserId", UserId),
            new SqlParameter("@Password", Password)
        );
    }

    //User
    public User? GetUserInfo(int UserId)
    {
        return this.Users.FromSqlRaw("EXECUTE pr_GetUserInfo @UserId",
            new SqlParameter("@UserId", UserId)
        ).ToList().SingleOrDefault();
    }

    public List<UserContact> GetUserContact(int UserId)
    {
        return this.UserContacts.FromSqlRaw("EXECUTE pr_GetUserContact @UserId",
            new SqlParameter("@UserId", UserId)
        ).ToList();
    }

    public int UpdateUserInfo(User user)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_UpdateUserInfo @UserId, @Email, @FirstName, @LastName, @Gender, @Birthdate",
            new SqlParameter("@UserId", user.UserId),
            new SqlParameter("@Email", user.Email),
            new SqlParameter("@FirstName", user.FirstName),
            new SqlParameter("@LastName", user.LastName),
            new SqlParameter("@Gender", user.Gender),
            new SqlParameter("@Birthdate", user.Birthdate)
        );
    }

    public int AddUserContact(UserContact userContact)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_AddUserContact @UserId, @Address, @PhoneNumber",
            new SqlParameter("@UserId", userContact.UserId),
            new SqlParameter("@Address", userContact.Address),
            new SqlParameter("@PhoneNumber", userContact.PhoneNumber)
        );
    }

    public int UpdateUserContact(UserContact userContact)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_UpdateUserContact @UserId, @UserContactId, @Address, @PhoneNumber",
            new SqlParameter("@UserId", userContact.UserId),
            new SqlParameter("@UserContactId", userContact.UserContactId),
            new SqlParameter("@Address", userContact.Address),
            new SqlParameter("@PhoneNumber", userContact.PhoneNumber)
        );
    }

    public int SetDefaultUserContact(UserContact userContact)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_SetDefaultUserContact @UserId, @UserContactId",
            new SqlParameter("@UserId", userContact.UserId),
            new SqlParameter("@UserContactId", userContact.UserContactId)
        );
    }

    public int DeleteUserContact(UserContact userContact)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_DeleteUserContact @UserId, @UserContactId",
            new SqlParameter("@UserId", userContact.UserId),
            new SqlParameter("@UserContactId", userContact.UserContactId)
        );
    }

    // Cart
    public List<CartModel> GetCart(int UserId)
    {
        return this.Database.SqlQueryRaw<CartModel>("EXECUTE pr_GetUserCart @UserId",
            new SqlParameter("@UserId", UserId)
        ).ToList();
    }

    public int AddCart(int UserId, int ProductId, int Quantity)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_AddToCart @UserId, @ProductId, @Quantity",
            new SqlParameter("@UserId", UserId),
            new SqlParameter("@ProductId", ProductId),
            new SqlParameter("@Quantity", Quantity)
        );
    }

    public int UpdateCart(int CartId, int Quantity)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_UpdateCart @CartId, @Quantity",
            new SqlParameter("@CartId", CartId),
            new SqlParameter("@Quantity", Quantity)
        );
    }

    public int DeleteCart(int CartId)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_RemoveFromCart @CartId",
            new SqlParameter("@CartId", CartId)
        );
    }

    // Order
    public int AddOrder(AddOrderModel addOrderModel)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_AddOrder @UserId, @Carts, @PhoneNumber, @Address",
            new SqlParameter("@UserId", addOrderModel.UserId),
            new SqlParameter("@Carts", addOrderModel.Carts),
            new SqlParameter("@PhoneNumber", addOrderModel.PhoneNumber),
            new SqlParameter("@Address", addOrderModel.Address)
        );
    }

    public List<Order> GetOrder(int UserId, string ProductName)
    {
        return this.Orders.FromSqlRaw("EXECUTE pr_GetOrder @UserId, @ProductName",
            new SqlParameter("@UserId", UserId),
            new SqlParameter("@ProductName", ProductName)
        ).ToList();
    }

    public int CancelOrder(int OrderId)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_CancelOrder @OrderId",
            new SqlParameter("@OrderId", OrderId)
        );
    }

    //Admin/ Administrators
    public List<UserWithRoleName> GetAdministrators()
    {
        return this.Database.SqlQueryRaw<UserWithRoleName>("EXECUTE pr_GetAdministrators").ToList();
    }

    //Admin/ Customers
    public List<UserWithRoleName> GetCustomers()
    {
        return this.Database.SqlQueryRaw<UserWithRoleName>("EXECUTE pr_GetCustomers").ToList();
    }

    //Admin/ Products
    public List<Product> GetProducts(int CategoryId = 0, int SizeId = 0, int ColorId = 0)
    {
        return this.Products.FromSqlRaw("EXECUTE pr_GetProducts @CategoryId, @SizeId, @ColorId",
            new SqlParameter("@CategoryId", CategoryId),
            new SqlParameter("@SizeId", SizeId),
            new SqlParameter("@ColorId", ColorId)
        ).ToList();
    }

    public List<Product> GetProductsWithOut(int CategoryId = 0, int SizeId = 0, int ColorId = 0)
    {
        return this.Products.FromSqlRaw("EXECUTE pr_GetProductsWithOut @CategoryId, @SizeId, @ColorId",
            new SqlParameter("@CategoryId", CategoryId),
            new SqlParameter("@SizeId", SizeId),
            new SqlParameter("@ColorId", ColorId)
        ).ToList();
    }

    public Product? GetProductById(int ProductId)
    {
        return this.Products.FromSqlRaw("EXECUTE pr_GetProductById @ProductId",
            new SqlParameter("@ProductId", ProductId)
        ).ToList().SingleOrDefault();
    }

    public List<ProductCategories> GetProductCategories(int ProductId)
    {
        return this.Database.SqlQueryRaw<ProductCategories>("EXECUTE pr_GetProductCategories @ProductId",
            new SqlParameter("@ProductId", ProductId)
        ).ToList();
    }

    public int UpdateProduct(AddProductModel product)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_UpdateProduct @ProductId, @Categories, @ColorId, @SizeId, @ProductName, @ProductSlug, @ProductDesc, @Price, @Quantity, @Discount, @ProductImages",
            new SqlParameter("@ProductId", product.ProductId),
            new SqlParameter("@Categories", product.Categories),
            new SqlParameter("@ColorId", product.ColorId),
            new SqlParameter("@SizeId", product.SizeId),
            new SqlParameter("@ProductName", product.ProductName),
            new SqlParameter("@ProductSlug", product.ProductSlug),
            new SqlParameter("@ProductDesc", product.ProductDesc),
            new SqlParameter("@Price", product.Price),
            new SqlParameter("@Quantity", product.Quantity),
            new SqlParameter("@Discount", product.Discount),
            new SqlParameter("@ProductImages", product.ProductImages)
        );
    }

    //Admin/ Categories
    public List<CategoryWithProductCountModel> GetCategories()
    {
        return this.Database.SqlQueryRaw<CategoryWithProductCountModel>("EXECUTE pr_GetCategories").ToList();
    }

    public Category? GetCategoryById(int CategoryId)
    {
        return this.Categories.FromSqlRaw("EXECUTE pr_GetCategoryById @CategoryId",
            new SqlParameter("@CategoryId", CategoryId)
        ).ToList().SingleOrDefault();
    }

    public int UpdateCategory(Category category)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_UpdateCategory @CategoryId, @CategoryName",
            new SqlParameter("@CategoryId", category.CategoryId),
            new SqlParameter("@CategoryName", category.CategoryName)
        );
    }

    public int AddProductCategory(ProductCategory productCategory)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_AddProductCategory @ProductId, @CategoryId",
            new SqlParameter("@ProductId", productCategory.ProductId),
            new SqlParameter("@CategoryId", productCategory.CategoryId)
        );
    }

    public int DeleteProductCategory(ProductCategory productCategory)
    {
        return this.Database.ExecuteSqlRaw("EXECUTE pr_DeleteProductCategory @ProductId, @CategoryId",
            new SqlParameter("@ProductId", productCategory.ProductId),
            new SqlParameter("@CategoryId", productCategory.CategoryId)
        );
    }

    // Admin/ Sizes
    public List<Size> GetSizes(string ProductName = "", string Categories = "")
    {
        var sizes = this.Sizes.FromSqlRaw("EXECUTE pr_GetSizes @ProductName, @Categories",
            new SqlParameter("@ProductName", ProductName),
            new SqlParameter("@Categories", Categories)
        ).ToList();
        return sizes;
    }

    public Size? GetSizeById(int SizeId)
    {
        return this.Sizes.FromSqlRaw("EXECUTE pr_GetSizeById @SizeId",
            new SqlParameter("@SizeId", SizeId)
        ).ToList().SingleOrDefault();
    }

    // Admin/ Colors
    public List<Color> GetColors(string ProductName = "", string Categories = "")
    {
        var colors = this.Colors.FromSqlRaw("EXECUTE pr_GetColors @ProductName, @Categories",
            new SqlParameter("@ProductName", ProductName),
            new SqlParameter("@Categories", Categories)
        ).ToList();
        return colors;
    }

    public Color? GetColorById(int ColorId)
    {
        return this.Colors.FromSqlRaw("EXECUTE pr_GetColorById @ColorId",
            new SqlParameter("@ColorId", ColorId)
        ).ToList().SingleOrDefault();
    }

    //Admin/ Roles
    public List<Role> GetRoles()
    {
        return this.Roles.FromSqlRaw("EXECUTE pr_GetRoles").ToList();
    }

    public Role? GetRoleById(int RoleId)
    {
        return this.Roles.FromSqlRaw("EXECUTE pr_GetRoleById @RoleId",
            new SqlParameter("@RoleId", RoleId)
        ).ToList().SingleOrDefault();
    }

    public List<UserWithRoleName> GetUserByRole(int RoleId)
    {
        return this.Database.SqlQueryRaw<UserWithRoleName>("EXECUTE pr_GetUserByRole @RoleId",
            new SqlParameter("@RoleId", RoleId)
        ).ToList();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__51BCD7B7FD16B235");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Product");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B002DAFF9");

            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Colors__8DA7674D32B442E3");

            entity.HasIndex(e => e.ColorName, "UC_Color_ColorName").IsUnique();

            entity.HasIndex(e => e.ColorValue, "UC_Color_ColorValue").IsUnique();

            entity.Property(e => e.ColorName).HasMaxLength(255);
            entity.Property(e => e.ColorValue)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF8396E71F");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36CEB0F9DC1");

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
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38BD04BDC0");

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
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CDDD4B3C2A");

            entity.HasIndex(e => e.ProductSlug, "UC_Product_ProductSlug").IsUnique();

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
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF87CC9006BF0");

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
            entity.HasKey(e => e.ReplyId).HasName("PK__Replies__C25E4609086C57BE");

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
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AB92DEEBE");

            entity.HasIndex(e => e.RoleName, "UC_Role_RoleName").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Sizes__83BD097A7329952C");

            entity.HasIndex(e => e.SizeName, "UC_Size_SizeName").IsUnique();

            entity.Property(e => e.SizeName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C14C96383");

            entity.HasIndex(e => e.Email, "UC_User_Email").IsUnique();

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
            entity.HasKey(e => e.UserContactId).HasName("PK__UserCont__3911BAA54AB838D5");

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
