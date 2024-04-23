

GO
USE WatchShop2

--CREATE TABLE--
--Product
GO
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(255) NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
)

GO
CREATE TABLE Colors (
	ColorId INT IDENTITY(1,1) PRIMARY KEY,
	ColorName NVARCHAR(255) NOT NULL,
	ColorValue VARCHAR(255) NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
	CONSTRAINT UC_Color_ColorName UNIQUE (ColorName),
	CONSTRAINT UC_Color_ColorValue UNIQUE (ColorValue),
)

GO
CREATE TABLE Sizes (
	SizeId INT IDENTITY(1,1) PRIMARY KEY,
	SizeName NVARCHAR(255) NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
	CONSTRAINT UC_Size_SizeName UNIQUE (SizeName)
)

GO
CREATE TABLE Products (
	ProductId INT IDENTITY(1,1) PRIMARY KEY,
	ColorId INT NOT NULL,
	SizeId INT NOT NULL,
	ProductName NVARCHAR(255) NOT NULL,
	ProductSlug VARCHAR(255) NOT NULL,
	ProductDesc NVARCHAR(MAX) NOT NULL,
	Price FLOAT NOT NULL,
	Quantity INT NOT NULL DEFAULT 0,
	Discount INT NOT NULL DEFAULT 0,
	ProductImages NVARCHAR(MAX) NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
	CONSTRAINT UC_Product_ProductSlug UNIQUE (ProductSlug),
	CONSTRAINT CHK_Product_Price CHECK (Price > 0),
	CONSTRAINT CHK_Product_Quantity CHECK (Quantity >= 0),
	CONSTRAINT CHK_Product_Discount CHECK (Discount BETWEEN 0 AND 100),
	CONSTRAINT FK_Product_Color FOREIGN KEY(ColorId)
	REFERENCES dbo.Colors(ColorId),
	CONSTRAINT FK_Product_Size FOREIGN KEY(SizeId)
	REFERENCES dbo.Sizes(SizeId)
)

GO
CREATE TABLE ProductCategory (
    ProductId INT NOT NULL,
    CategoryId INT NOT NULL,
	CONSTRAINT PK_ProductCategory PRIMARY KEY(ProductId, CategoryId),
	CONSTRAINT FK_ProductCategory_Product FOREIGN KEY(ProductId)
	REFERENCES dbo.Products(ProductId),
	CONSTRAINT FK_ProductCategory_Category FOREIGN KEY(CategoryId)
	REFERENCES dbo.Categories(CategoryId)
)

--User
GO 
--Role: Admin, Support(Tu van, phan hoi khach hang), Customer
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(255) NOT NULL,
	CONSTRAINT UC_Role_RoleName UNIQUE (RoleName),
)

GO
CREATE TABLE Users (
	UserId INT IDENTITY(1,1) PRIMARY KEY,
	RoleId INT NOT NULL,
	Email VARCHAR(255) NOT NULL,
	Password VARCHAR(255) NOT NULL,
	FirstName NVARCHAR(255),
	LastName NVARCHAR(255) NOT NULL,
	Gender BIT NOT NULL, --1: Nam, 0: Nu
	Birthdate DATE,
	Active BIT DEFAULT 1,
	CreateAt DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_User_Role FOREIGN KEY(RoleId)
	REFERENCES dbo.Roles(RoleId),
	CONSTRAINT UC_User_Email UNIQUE (Email),
)

GO
CREATE TABLE UserContacts (
	UserContactId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL,
	Address NVARCHAR(255) NOT NULL,
	PhoneNumber VARCHAR(15) NOT NULL,
	IsDefault BIT DEFAULT 0,
	CONSTRAINT FK_UserContact_User FOREIGN KEY(UserId)
	REFERENCES dbo.Users(UserId)
)

--Rating
--GO
--CREATE TABLE Ratings (
--	RatingId INT IDENTITY(1,1) PRIMARY KEY,
--	UserId INT NOT NULL,
--	ProductId INT NOT NULL,
--	Rate FLOAT NOT NULL,
--	Comment NVARCHAR(255) DEFAULT N'',
--	CONSTRAINT CHK_Rating_Rate CHECK (Rate BETWEEN 0 AND 5),
--	CONSTRAINT FK_Rating_User FOREIGN KEY(UserId)
--	REFERENCES dbo.Users(UserId),
--	CONSTRAINT FK_Rating_Product FOREIGN KEY(ProductId)
--	REFERENCES dbo.Products(ProductId)
--)

--GO
--CREATE TABLE Replies (
--    ReplyId INT IDENTITY(1,1) PRIMARY KEY,
--	RatingId INT NOT NULL,
--	UserId INT NOT NULL,
--	Comment NVARCHAR(MAX) DEFAULT N'',
--	CONSTRAINT FK_Reply_Rate FOREIGN KEY(RatingId)
--	REFERENCES dbo.Ratings(RatingId),
--	CONSTRAINT FK_Reply_User FOREIGN KEY(UserId)
--	REFERENCES dbo.Users(UserId)
--)

--Cart
GO
CREATE TABLE Carts (
	CartId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL,
	ProductId INT NOT NULL,
	Quantity INT NOT NULL,
	CONSTRAINT CHK_Cart_Quantity CHECK (Quantity > 0),
	CONSTRAINT FK_Cart_User FOREIGN KEY(UserId)
	REFERENCES dbo.Users(UserId),
	CONSTRAINT FK_Cart_Product FOREIGN KEY(ProductId)
	REFERENCES dbo.Products(ProductId)
)

--Order - Payment
GO
CREATE TABLE Orders (
	OrderId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL,
	OrderDate DATETIME NOT NULL,
	Address NVARCHAR(255) NOT NULL,
	PhoneNumber VARCHAR(15) NOT NULL,
	Status NVARCHAR(255) DEFAULT 'Pending',
	CONSTRAINT FK_Order_User FOREIGN KEY(UserId)
	REFERENCES dbo.Users(UserId)
)

GO
CREATE TABLE OrderDetails (
	OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
	OrderId INT NOT NULL,
	ProductId INT NOT NULL,
	Price FLOAT NOT NULL,
	Quantity INT NOT NULL ,
	Discount INT NOT NULL DEFAULT 0,
	CONSTRAINT CHK_OrderDetail_Price CHECK (Price > 0),
	CONSTRAINT CHK_OrderDetail_Quantity CHECK (Quantity > 0),
	CONSTRAINT CHK_OrderDetail_Discount CHECK (Discount >= 0 AND Discount <= 100),
	CONSTRAINT FK_OrderDetail_Order FOREIGN KEY(OrderId)
	REFERENCES dbo.Orders(OrderId),
	CONSTRAINT FK_OrderDetail_Product FOREIGN KEY(ProductId)
	REFERENCES dbo.Products(ProductId)
)

GO
CREATE TABLE Payments (
	PaymentId INT IDENTITY(1,1) PRIMARY KEY,
	OrderId INT NOT NULL,
	PaymentMethod NVARCHAR(255) NOT NULL,
	BankName NVARCHAR(255),
	CardNumber VARCHAR(255),
	CONSTRAINT FK_Payment_Order FOREIGN KEY(OrderId)
	REFERENCES dbo.Orders(OrderId)
)

--=== INSERT ===
INSERT INTO dbo.Categories(CategoryName)
VALUES 
(N'Men Watches'),
(N'Women Watches'),
(N'Sport Watches'),
(N'Luxury Watches'),
(N'New Watches'),
(N'Special Watches'),
(N'Sale Watches')

GO
INSERT INTO dbo.Sizes(SizeName)
VALUES
(N'35mm'),(N'36mm'),(N'37mm'),(N'38mm'),(N'39mm'),
(N'40mm'),(N'41mm'),(N'42mm'),(N'43mm'),(N'44mm'),(N'45mm')

GO
INSERT INTO dbo.Colors(ColorName, ColorValue)
VALUES
(N'Black', '#1f2024'),
(N'White', '#ffffff'),
(N'Sliver', '#d0d0d0'),
(N'Gold', '#e9d677'),
(N'Blue', '#2ea8d5'),
(N'Red', '#ec4351'),
(N'Brown', '#bf8656'),
(N'Green', '#76ff76'),
(N'Pink', '#ffb1dd'),
(N'Purple', '#c469ff')



--=== PROCEDURE ===
--ADMIN
--ADMIN/ Product
GO
CREATE OR ALTER PROCEDURE pr_AddProductCategories(
	@ProductId INT,
	@Categories NVARCHAR(MAX)
)
AS
BEGIN
	DELETE FROM dbo.ProductCategory
	WHERE ProductId = @ProductId

    DECLARE @CategoryId NVARCHAR(MAX)
	DECLARE cursor1 CURSOR
		FOR SELECT DISTINCT TRIM(value) FROM STRING_SPLIT(@Categories, ',')
	OPEN cursor1
	FETCH NEXT FROM cursor1
		INTO @CategoryId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @CategoryId = TRIM(@CategoryId)
		PRINT @CategoryId
		IF EXISTS (SELECT CategoryId FROM dbo.Categories WHERE CategoryId = @CategoryId)
		BEGIN
			INSERT INTO dbo.ProductCategory(ProductId, CategoryId)
			VALUES(@ProductId, CONVERT(INT, @CategoryId))
		END

		FETCH NEXT FROM cursor1
			INTO @CategoryId
	END
	CLOSE cursor1
	DEALLOCATE cursor1
END

GO
CREATE OR ALTER PROCEDURE pr_AddProduct(
	@Categories NVARCHAR(MAX),
	@ColorId INT,
	@SizeId INT,
	@ProductName NVARCHAR(255),
	@ProductSlug VARCHAR(255),
	@ProductDesc NVARCHAR(MAX),
	@Price FLOAT,
	@Quantity INT,
	@Discount INT,
	@ProductImages NVARCHAR(MAX),
	@Active BIT
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;

		INSERT INTO dbo.Products(ColorId, SizeId, ProductName, ProductSlug, ProductDesc, Price, Quantity, Discount, ProductImages, Active)
		VALUES(@ColorId, @SizeId, @ProductName, @ProductSlug, @ProductDesc, @Price, @Quantity, @Discount, @ProductImages, @Active)
		DECLARE @ProductId INT
		SELECT @ProductId = SCOPE_IDENTITY()

		EXEC dbo.pr_AddProductCategories 
			@ProductId=@ProductId, -- int
			@Categories=@Categories -- nvarchar(max)

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW
	END CATCH
END

GO
EXEC dbo.pr_AddProduct 
	@Categories=N'1,2', -- int
    @ColorId=1, -- int
    @SizeId=1, -- int
    @ProductName=N'', -- nvarchar(255)
    @ProductSlug='12', -- varchar(255)
    @ProductDesc=N'', -- nvarchar(max)
    @Price=1, -- float
    @Quantity=0, -- int
    @Discount=0, -- int
    @ProductImages=N'' -- nvarchar(max)

GO
CREATE OR ALTER PROCEDURE pr_UpdateProduct(
	@ProductId INT,
	@Categories NVARCHAR(MAX),
	@ColorId INT,
	@SizeId INT,
	@ProductName NVARCHAR(255),
	@ProductSlug VARCHAR(255),
	@ProductDesc NVARCHAR(MAX),
	@Price FLOAT,
	@Quantity INT,
	@Discount INT,
	@ProductImages NVARCHAR(MAX),
	@Active BIT
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;

		UPDATE dbo.Products
		SET ColorId = @ColorId,
			SizeId = @SizeId,
			ProductName = @ProductName,
			ProductSlug = @ProductSlug,
			ProductDesc = @ProductDesc,
			Price = @Price,
			Quantity = @Quantity,
			Discount = @Discount,
			ProductImages = @ProductImages,
			Active = @Active
		WHERE ProductId = @ProductId

		EXEC dbo.pr_AddProductCategories 
			@ProductId=@ProductId, -- int
			@Categories=@Categories -- nvarchar(max)

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW
	END CATCH
END

EXEC dbo.pr_UpdateProduct 
	@ProductId=3, -- int
    @Categories=N'7', -- nvarchar(max)
    @ColorId=1, -- int
    @SizeId=1, -- int
    @ProductName=N'1', -- nvarchar(255)
    @ProductSlug='', -- varchar(255)
    @ProductDesc=N'', -- nvarchar(max)
    @Price=5000, -- float
    @Quantity=0, -- int
    @Discount=0, -- int
    @ProductImages=N'' -- nvarchar(max)


GO
CREATE OR ALTER PROCEDURE pr_DeleteProduct(@ProductId INT)
AS
BEGIN
	UPDATE dbo.Products
	SET Active = 0
	WHERE ProductId = @ProductId
END

EXEC dbo.pr_DeleteProduct @ProductId=20 -- int

--ADMIN/ Role
GO
CREATE OR ALTER PROCEDURE pr_GetRoles
AS
BEGIN
    SELECT Roles.*, COUNT(DISTINCT UserId) AS UserCount
	FROM Roles
	LEFT JOIN dbo.Users ON Users.RoleId = Roles.RoleId
	GROUP BY Roles.RoleId, RoleName
END

GO
CREATE OR ALTER PROCEDURE pr_GetRoleById(@RoleId INT)
AS
BEGIN
    SELECT Roles.*, COUNT(DISTINCT UserId) AS UserCount
	FROM Roles
	LEFT JOIN dbo.Users ON Users.RoleId = Roles.RoleId
	WHERE Roles.RoleId = @RoleId
	GROUP BY Roles.RoleId, RoleName
END

GO
CREATE OR ALTER PROCEDURE pr_GetUserByRole(@RoleId INT)
AS
BEGIN
    SELECT Users.*, RoleName FROM dbo.Users 
	INNER JOIN dbo.Roles ON Roles.RoleId = Users.RoleId
	WHERE Roles.RoleId = @RoleId
END

EXEC dbo.pr_GetUserByRole @RoleId=1 -- int

GO
CREATE OR ALTER PROCEDURE pr_GetUserByRoleWithOut(@RoleId INT)
AS
BEGIN
    SELECT Users.*, RoleName FROM dbo.Users 
	INNER JOIN dbo.Roles ON Roles.RoleId <> Users.RoleId
	WHERE Roles.RoleId = @RoleId
END

EXEC dbo.pr_GetUserByRoleWithOut @RoleId=1 -- int

GO
CREATE OR ALTER PROCEDURE pr_ChangeUserRole(@UserId INT, @RoleId INT)
AS
BEGIN
    UPDATE dbo.Users
    SET RoleId = @RoleId
    WHERE UserId = @UserId
END

--ADMIN/ GetAdministrators
GO
CREATE OR ALTER PROCEDURE pr_GetAdministrators
AS
BEGIN
    SELECT Users.*, RoleName FROM dbo.Users 
	INNER JOIN dbo.Roles ON Roles.RoleId = Users.RoleId
	WHERE RoleName <> N'Customer'
END

GO
CREATE OR ALTER PROCEDURE pr_AddAdmin(
	@Email VARCHAR(255),
	@Password VARCHAR(255),
	@FirstName NVARCHAR(255),
	@LastName NVARCHAR(255),
	@Gender BIT, --1: Nam, 0: Nu
	@Birthdate DATE = NULL
)
AS
BEGIN
    INSERT INTO dbo.Users(RoleId, Email, Password, FirstName, LastName, Gender, Birthdate, Active)
    VALUES(1, @Email, @Password, @FirstName, @LastName, @Gender, @Birthdate, 1)
END

--ADMIN/ Customers
GO
CREATE OR ALTER PROCEDURE pr_GetCustomers
AS
BEGIN
    SELECT Users.*, RoleName FROM dbo.Users 
	INNER JOIN dbo.Roles ON Roles.RoleId = Users.RoleId
	WHERE RoleName = N'Customer'
END

GO
CREATE OR ALTER PROCEDURE pr_BanOrUnbanUser(@UserId INT, @Active BIT)
AS
BEGIN
    UPDATE dbo.Users
    SET Active = @Active
    WHERE UserId = @UserId
END

--ADMIN/ Products
GO
CREATE OR ALTER PROCEDURE pr_GetProducts(
	@CategoryId INT = 0,
	@SizeId INT = 0, 
	@ColorId INT = 0)
AS
BEGIN
    SELECT Products.*, SizeName, ColorName, ColorValue
	FROM dbo.Products
	LEFT JOIN dbo.Sizes ON Sizes.SizeId = Products.SizeId
	LEFT JOIN dbo.Colors ON Colors.ColorId = Products.ColorId
	INNER JOIN (
		SELECT DISTINCT ProductId 
		FROM dbo.ProductCategory
		WHERE (@CategoryId = 0 OR CategoryId = @CategoryId)
	) c ON c.ProductId = Products.ProductId
	WHERE (@SizeId = 0 OR Products.SizeId = @SizeId) AND
		  (@ColorId = 0 OR Products.ColorId = @ColorId)
END

EXEC dbo.pr_GetProducts @CategoryId=1, @SizeId=6, @ColorId=1

GO
CREATE OR ALTER PROCEDURE pr_GetProductsWithOut(
	@CategoryId INT = 0,
	@SizeId INT = 0, 
	@ColorId INT = 0
)
AS
BEGIN
    SELECT Products.*, SizeName, ColorName, ColorValue
	FROM dbo.Products
	LEFT JOIN dbo.Sizes ON Sizes.SizeId = Products.SizeId
	LEFT JOIN dbo.Colors ON Colors.ColorId = Products.ColorId
	INNER JOIN (
		SELECT ProductId
		FROM dbo.ProductCategory
		EXCEPT
		SELECT ProductId
		FROM dbo.ProductCategory
		WHERE CategoryId = @CategoryId
	) c ON c.ProductId = Products.ProductId
	WHERE (@SizeId = 0 OR Products.SizeId <> @SizeId) AND
		  (@ColorId = 0 OR Products.ColorId <> @ColorId) 
END

EXEC dbo.pr_GetProductsWithOut @CategoryId=1, @SizeId=6, @ColorId=1

--ADMIN/ Category
GO
CREATE OR ALTER PROCEDURE pr_GetCategories
AS
BEGIN
    SELECT Categories.*, COUNT(DISTINCT ProductId) AS ProductCount
	FROM dbo.Categories
	LEFT JOIN dbo.ProductCategory ON ProductCategory.CategoryId = Categories.CategoryId
	GROUP BY Categories.CategoryId, CategoryName, Categories.Active
END

GO
CREATE OR ALTER PROCEDURE pr_GetCategoryById(@CategoryId INT)
AS
BEGIN
    SELECT * FROM dbo.Categories
	WHERE CategoryId = @CategoryId
END

GO
CREATE OR ALTER PROCEDURE pr_AddCategory(@CategoryName NVARCHAR(255))
AS
BEGIN
	IF NOT EXISTS (SELECT CategoryId FROM dbo.Categories WHERE LOWER(CategoryName) LIKE LOWER(TRIM(@CategoryName)))
	BEGIN
		INSERT INTO dbo.Categories(CategoryName) 
		VALUES(TRIM(@CategoryName))

		SELECT CONVERT(INT, SCOPE_IDENTITY())
	END
	ELSE
	BEGIN
	    RAISERROR(N'This Category already exists!',16,10);
	    RETURN
	END
END

EXEC dbo.pr_AddCategory @CategoryName=N'ehe' -- nvarchar(255)


GO
CREATE OR ALTER PROCEDURE pr_UpdateCategory(
	@CategoryId INT,
	@CategoryName NVARCHAR(255)
)
AS
BEGIN
    UPDATE dbo.Categories
    SET CategoryName = @CategoryName
    WHERE CategoryId = @CategoryId
END

GO
CREATE OR ALTER PROCEDURE pr_DeleteCategory(@CategoryId INT)
AS
BEGIN
    DELETE FROM dbo.Categories
    WHERE CategoryId = @CategoryId
END

GO
CREATE OR ALTER PROCEDURE pr_AddProductCategory(@ProductId INT, @CategoryId INT)
AS
BEGIN
    INSERT INTO dbo.ProductCategory(ProductId, CategoryId)
	VALUES(@ProductId, @CategoryId)
END

GO
CREATE OR ALTER PROCEDURE pr_DeleteProductCategory(@ProductId INT, @CategoryId INT)
AS
BEGIN
    DELETE FROM dbo.ProductCategory
    WHERE ProductId = @ProductId AND CategoryId = @CategoryId
END

--ADMIN/ Color
GO
CREATE OR ALTER PROCEDURE pr_GetColorById(@ColorId INT)
AS
BEGIN
    SELECT Colors.*, COUNT(DISTINCT ProductId) AS Quantity
	FROM dbo.Colors
	LEFT JOIN dbo.Products ON Products.ColorId = Colors.ColorId
	WHERE Colors.ColorId = @ColorId
	GROUP BY Colors.ColorId, ColorName, ColorValue, Colors.Active
END

--ADMIN/ Size
GO
CREATE OR ALTER PROCEDURE pr_GetSizeById(@SizeId INT)
AS
BEGIN
    SELECT Sizes.*, COUNT(DISTINCT ProductId) AS Quantity
	FROM dbo.Sizes
	LEFT JOIN dbo.Products ON Products.SizeId = Sizes.SizeId
	WHERE Sizes.SizeId = @SizeId
	GROUP BY Sizes.SizeId, SizeName, Sizes.Active
END



--DATA
--DATA/ GetUserById
GO
CREATE OR ALTER PROCEDURE pr_GetUserById(@UserId INT)
AS
BEGIN
    SELECT * FROM dbo.Users WHERE UserId = @UserId
END

--DATA/ Get colors
GO
CREATE OR ALTER PROCEDURE pr_GetColors(
	@ProductName NVARCHAR(255) = '',
	@Categories NVARCHAR(MAX) = '')
AS
BEGIN
    SELECT Colors.*, ISNULL(q.Quantity, 0) AS Quantity FROM dbo.Colors
	LEFT JOIN (
		SELECT ColorId, COUNT(DISTINCT Products.ProductId) AS Quantity FROM dbo.Products
		INNER JOIN dbo.ProductCategory ON ProductCategory.ProductId = Products.ProductId
		INNER JOIN dbo.Categories ON Categories.CategoryId = ProductCategory.CategoryId
		WHERE LOWER(ProductName) LIKE N'%' + LOWER(@ProductName) + '%' AND
			  (@Categories = '' OR CategoryName IN (SELECT value FROM STRING_SPLIT(@Categories, ',')))
		GROUP BY ColorId
	) q ON q.ColorId = Colors.ColorId
END

EXEC dbo.pr_GetColors 
	@ProductName=N'', -- nvarchar(255)
    @Categories=N'Sport Watches' -- varchar(255)

--DATA/ Get sizes
GO
CREATE OR ALTER PROCEDURE pr_GetSizes(
	@ProductName NVARCHAR(255) = '',
	@Categories NVARCHAR(MAX) = '')
AS
BEGIN
	SELECT Sizes.*, ISNULL(q.Quantity, 0) AS Quantity FROM dbo.Sizes
	LEFT JOIN (
		SELECT SizeId, COUNT(DISTINCT Products.ProductId) AS Quantity FROM dbo.Products
		INNER JOIN dbo.ProductCategory ON ProductCategory.ProductId = Products.ProductId
		INNER JOIN dbo.Categories ON Categories.CategoryId = ProductCategory.CategoryId
		WHERE LOWER(ProductName) LIKE N'%' + LOWER(@ProductName) + '%' AND
			  (@Categories = '' OR CategoryName IN (SELECT value FROM STRING_SPLIT(@Categories, ',')))
		GROUP BY SizeId
	) q ON q.SizeId = Sizes.SizeId
END

EXEC dbo.pr_GetSizes 
	@ProductName=N'', -- nvarchar(255)
    @Categories=N'' -- nvarchar(max)


--DATA/ Filter Products
GO
CREATE OR ALTER PROCEDURE pr_FilterProducts(
	@ProductName NVARCHAR(255) = '',
	@Categories VARCHAR(MAX) = '',
	@Colors VARCHAR(255) = '',
	@Sizes VARCHAR(255) = '',
	@PriceStart FLOAT = 0,
	@PriceEnd FLOAT = 1.79E+308,
	@PageNumber INT = 1,
	@PageSize INT = 10,
	@SortType VARCHAR(255) = 'auto'
)
AS
BEGIN
	SELECT p.*
	FROM (
		SELECT DISTINCT Products.*, SizeName, ColorName, ColorValue, (Price * (100-Discount)/100) AS PriceSale
		FROM dbo.Products
		LEFT JOIN dbo.ProductCategory ON ProductCategory.ProductId = Products.ProductId
		LEFT JOIN dbo.Categories ON Categories.CategoryId = ProductCategory.CategoryId
		LEFT JOIN dbo.Sizes ON Sizes.SizeId = Products.SizeId
		LEFT JOIN dbo.Colors ON Colors.ColorId = Products.ColorId
		WHERE LOWER(ProductName) LIKE N'%' + LOWER(@ProductName) + '%' AND
			(@Sizes = '' OR SizeName IN (SELECT value FROM STRING_SPLIT(@Sizes, ','))) AND
			(@Colors = '' OR ColorName IN (SELECT value FROM STRING_SPLIT(@Colors, ','))) AND
			(@Categories = '' OR CategoryName IN (SELECT value FROM STRING_SPLIT(@Categories, ','))) AND
			((Price * (100-Discount)/100) BETWEEN @PriceStart AND @PriceEnd) AND
		  Products.Active = 1
	) p
	ORDER BY 
		CASE WHEN @SortType = 'atoz' THEN ProductName END,
		CASE WHEN @SortType = 'ltoh' THEN CONVERT(FLOAT,(PriceSale)) END,
		CASE WHEN @SortType = 'ztoa' THEN ProductName END DESC,
		CASE WHEN @SortType = 'htol' THEN CONVERT(FLOAT,(PriceSale)) END DESC,
		CASE WHEN @SortType = 'auto' THEN ProductId END
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END

EXEC dbo.pr_FilterProducts 
	@ProductName=N'', -- nvarchar(255)
    @Categories='', -- varchar(max)
    @Colors='', -- varchar(255)
    @Sizes='', -- varchar(255)
    @SortType='auto', -- varchar(255)
	@PageNumber=1

GO
CREATE OR ALTER PROCEDURE pr_GetProductBySlug(@ProductSlug VARCHAR(255))
AS
BEGIN
    SELECT Products.*, SizeName, ColorName, ColorValue FROM dbo.Products
	INNER JOIN dbo.Colors ON Colors.ColorId = Products.ColorId
	INNER JOIN dbo.Sizes ON Sizes.SizeId = Products.SizeId
	WHERE ProductSlug = @ProductSlug
END

EXEC dbo.pr_GetProductBySlug @ProductSlug='devon-tread-1e' -- varchar(255)

GO
CREATE OR ALTER PROCEDURE pr_GetProductById(@ProductId INT)
AS
BEGIN
    SELECT Products.*, SizeName, ColorName, ColorValue
	FROM dbo.Products
	LEFT JOIN dbo.Sizes ON Sizes.SizeId = Products.SizeId
	LEFT JOIN dbo.Colors ON Colors.ColorId = Products.ColorId
	WHERE ProductId = @ProductId
END

GO
CREATE OR ALTER PROCEDURE pr_GetProductCategories(@ProductId INT)
AS
BEGIN
    SELECT CategoryId FROM dbo.ProductCategory
	WHERE ProductId = @ProductId
END

EXEC pr_GetProductCategories @ProductId=1

--ACCOUNT
--ACCOUNT/ Signup
GO
CREATE OR ALTER PROCEDURE pr_SignUp(
	@Email VARCHAR(255),
	@Password VARCHAR(255),
	@FirstName NVARCHAR(255),
	@LastName NVARCHAR(255),
	@Gender BIT, --1: Nam, 0: Nu
	@Birthdate DATE = NULL
)
AS
BEGIN
	INSERT INTO dbo.Users(RoleId, Email, Password, FirstName, LastName, Gender, Birthdate, Active)
    VALUES(2, @Email, @Password, @FirstName, @LastName, @Gender, @Birthdate, 1)
END

--EXEC dbo.pr_SignUp 
--	@Email='test1@gmail.com', -- varchar(255)
--    @Password='123456', -- varchar(255)
--    @FirstName=N'Cac', -- nvarchar(255)
--    @LastName=N'Lon', -- nvarchar(255)
--    @Gender=1, -- bit
--    @Birthdate=Null -- date


--ACCOUNT/ Signup
GO
CREATE OR ALTER PROCEDURE pr_SignIn(@Email VARCHAR(255))
AS
BEGIN
    SELECT * FROM dbo.Users WHERE Email = @Email
END

EXEC dbo.pr_SignIn @Email='phamduydat2002@gmail.com'

--ACCOUNT/ UpdateUserPassword
GO
CREATE OR ALTER PROCEDURE pr_UpdateUserPassword(@UserId INT, @Password VARCHAR(255))
AS
BEGIN
	UPDATE dbo.Users
	SET Password = @Password
	WHERE UserId = @UserId
END

--ACCOUNT/ GetUserInfo
GO
CREATE OR ALTER PROCEDURE pr_GetUserInfo(@UserId INT)
AS
BEGIN
    SELECT UserId, RoleId, Email, '' AS Password , FirstName, LastName, Gender, Birthdate, Active, CreateAt 
	FROM dbo.Users 
	WHERE UserId = @UserId
END

EXEC dbo.pr_GetUserInfo @UserId=2 -- int


--ACCOUNT/ GetUserContact
GO
CREATE OR ALTER PROCEDURE pr_GetUserContact(@UserId INT)
AS
BEGIN
    SELECT * FROM dbo.UserContacts WHERE UserId = @UserId
END

EXEC dbo.pr_GetUserContact @UserId=2 -- int

--ACCOUNT/ UpdateUserInfo
GO
CREATE OR ALTER PROCEDURE pr_UpdateUserInfo(
	@UserId INT,
	@Email VARCHAR(255),
	@FirstName NVARCHAR(255),
	@LastName NVARCHAR(255),
	@Gender BIT,
	@Birthdate DATE
)
AS
BEGIN
	UPDATE dbo.Users
	SET FirstName = @FirstName,
		LastName = @LastName,
		Email = @Email,
		Birthdate = @Birthdate,
		Gender = @Gender
	WHERE UserId = @UserId
END

--ACCOUNT/ AddUserContact
GO
CREATE OR ALTER PROCEDURE pr_AddUserContact(
	@UserId INT,
	@Address NVARCHAR(255),
	@PhoneNumber VARCHAR(15)
)
AS
BEGIN
	IF EXISTS (SELECT UserContactId FROM dbo.UserContacts WHERE UserId = @UserId AND Address = @Address AND PhoneNumber = @PhoneNumber)
	BEGIN
	    RAISERROR(N'This User Contact already exists!',16,10);
	    RETURN
	END
	IF NOT EXISTS (SELECT UserContactId FROM dbo.UserContacts WHERE UserId = @UserId)
	BEGIN
		INSERT INTO dbo.UserContacts(UserId, Address, PhoneNumber, IsDefault)
		VALUES(@UserId, @Address, @PhoneNumber, 1)
	END
	ELSE
	BEGIN
	    INSERT INTO dbo.UserContacts(UserId, Address, PhoneNumber, IsDefault)
		VALUES(@UserId, @Address, @PhoneNumber, 0)
	END
END

--ACCOUNT/ UpdateUserContact
GO
CREATE OR ALTER PROCEDURE pr_UpdateUserContact(
	@UserId INT,
	@UserContactId INT,
	@Address NVARCHAR(255),
	@PhoneNumber VARCHAR(15)
)
AS
BEGIN
    UPDATE dbo.UserContacts
    SET Address = @Address,
		PhoneNumber = @PhoneNumber
    WHERE UserContactId = @UserContactId AND UserId = @UserId
END

--ACCOUNT/ SetDefaultUserContact
GO
CREATE OR ALTER PROCEDURE pr_SetDefaultUserContact(@UserId INT, @UserContactId INT)
AS
BEGIN
    UPDATE dbo.UserContacts
    SET IsDefault = 0
    WHERE UserId = @UserId

	UPDATE dbo.UserContacts
    SET IsDefault = 1
    WHERE UserContactId = @UserContactId
END

--ACCOUNT/ DeleteUserContact
GO
CREATE OR ALTER PROCEDURE pr_DeleteUserContact(@UserId INT, @UserContactId INT)
AS
BEGIN
    DECLARE @Count INT = 0
	SELECT @Count = COUNT(*) FROM dbo.UserContacts WHERE UserId = @UserId

	IF (@Count > 1)
	BEGIN
	    DELETE FROM dbo.UserContacts
	    WHERE UserId = @UserId AND UserContactId = @UserContactId
	END
END

--CART
--CART/ Add to cart
GO
CREATE OR ALTER PROCEDURE pr_AddToCart(
	@UserId INT,
	@ProductId INT,
	@Quantity INT
)
AS
BEGIN
	DECLARE @CartId INT
	SELECT @CartId = CartId FROM dbo.Carts WHERE UserId = @UserId AND ProductId = @ProductId

	IF (@CartId IS NULL)
	BEGIN
		INSERT INTO dbo.Carts(UserId, ProductId, Quantity)
		VALUES(@UserId, @ProductId, @Quantity)
	END
	ELSE
	BEGIN
		UPDATE dbo.Carts
		SET Quantity += @Quantity
		WHERE CartId = @CartId
	END
END

EXEC dbo.pr_AddToCart 
	@UserId=2, -- int
    @ProductId=3, -- int
    @Quantity=1 -- int

--CART/ Update cart
GO
CREATE OR ALTER PROCEDURE pr_UpdateCart(
	@CartId INT,
	@Quantity INT
)
AS
BEGIN
    UPDATE dbo.Carts
	SET Quantity = @Quantity
	WHERE CartId = @CartId
END

EXEC dbo.pr_UpdateCart 
	@CartId=4, -- int
	@Quantity=5

--CART/ Delete product from cart
GO
CREATE OR ALTER PROCEDURE pr_RemoveFromCart(@CartId INT)
AS
BEGIN
    DELETE FROM dbo.Carts
	WHERE CartId = @CartId
END

EXEC dbo.pr_RemoveFromCart @CartId=4 -- int



--CART/ Get Cart
GO
CREATE OR ALTER PROCEDURE pr_GetUserCart(@UserId INT)
AS
BEGIN
    SELECT CartId, Products.ProductId, ProductName, ProductSlug, ProductDesc, ProductImages, Price, Discount, 
		SizeName, ColorName, ColorValue, Carts.Quantity AS CartQuantity, Products.Quantity AS ProductQuantity 
	FROM dbo.Carts
	INNER JOIN dbo.Products ON Products.ProductId = Carts.ProductId
	INNER JOIN dbo.Colors ON Colors.ColorId = Products.ColorId
	INNER JOIN dbo.Sizes ON Sizes.SizeId = Products.SizeId
	WHERE UserId = @UserId
END

EXEC dbo.pr_GetUserCart @UserId=2 -- int


SELECT * FROM dbo.Users
SELECT * FROM dbo.Products
SELECT * FROM dbo.Carts

--ORDER/ AddOrder
GO
CREATE OR ALTER PROCEDURE pr_AddOrder(
	@UserId INT,
	@Carts NVARCHAR(MAX),
	@PhoneNumber VARCHAR(15),
	@Address NVARCHAR(255)
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;

		DECLARE @Count INT = 0
		
		IF EXISTS (SELECT CartId FROM dbo.Carts WHERE UserId = @UserId)
		BEGIN
			INSERT INTO dbo.Orders(UserId, OrderDate, Address, PhoneNumber, Status)
			VALUES (@UserId, GETDATE(), @Address, @PhoneNumber, DEFAULT)
			DECLARE @OrderId INT
			SELECT @OrderId = SCOPE_IDENTITY()

			DECLARE @CartId INT
			DECLARE @ProductId INT
			DECLARE @Price FLOAT
			DECLARE @Quantity INT
			DECLARE @Discount INT
			DECLARE cursor1 CURSOR
				FOR SELECT DISTINCT CONVERT(INT, TRIM(value)) FROM STRING_SPLIT(@Carts, ',')
			OPEN cursor1
			FETCH NEXT FROM cursor1
				INTO @CartId
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @CartId = @CartId

				SELECT @ProductId = Carts.ProductId, @Price = Price, @Quantity = Carts.Quantity, @Discount = Discount FROM dbo.Carts 
				INNER JOIN dbo.Products ON Products.ProductId = Carts.ProductId
				WHERE CartId = @CartId AND UserId = @UserId
			
				IF ISNULL(@ProductId, 0) <> 0
				BEGIN
					SET @Count = @Count + 1

					INSERT INTO dbo.OrderDetails(OrderId, ProductId, Price, Quantity, Discount)
					VALUES (@OrderId, @ProductId, @Price, @Quantity, @Discount)

					EXEC dbo.pr_RemoveFromCart @CartId=@CartId -- int
				END

				FETCH NEXT FROM cursor1
					INTO @CartId
			END
			CLOSE cursor1
			DEALLOCATE cursor1

		END
	
		IF @Count = 0
			ROLLBACK TRANSACTION;
		ELSE
			COMMIT TRANSACTION;

		RETURN @Count
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW
	END CATCH
END

EXEC dbo.pr_AddOrder @UserId=0, -- int
    @Carts=N'', -- nvarchar(max)
    @PhoneNumber='', -- varchar(15)
    @Address=N'' -- nvarchar(255)

--ORDER/ GetOrders
GO
CREATE OR ALTER PROCEDURE pr_GetOrders
AS
BEGIN
    SELECT Orders.*, FirstName, LastName, Email,
		(
			SELECT SUM(Price * (100 - Discount)/100 * Quantity) 
			FROM dbo.OrderDetails
			WHERE OrderId = Orders.OrderId
			GROUP BY OrderId
		) AS Total
	FROM dbo.Orders
	INNER JOIN dbo.Users ON Users.UserId = Orders.UserId
	ORDER BY OrderDate DESC
END

--ORDER/ GetOrderById
GO
CREATE OR ALTER PROCEDURE pr_GetOrderById(@OrderId INT)
AS
BEGIN
    SELECT Orders.*, FirstName, LastName, Email
	FROM dbo.Orders
	INNER JOIN dbo.Users ON Users.UserId = Orders.UserId
	WHERE OrderId = @OrderId
END

--ORDER/ GetOrderDetail
GO
CREATE OR ALTER PROCEDURE pr_GetOrderDetail(@OrderId INT)
AS
BEGIN
    SELECT OrderDetails.*, ProductName, ProductSlug, ProductDesc, ProductImages, SizeName, ColorName, ColorValue 
	FROM dbo.OrderDetails 
	INNER JOIN dbo.Products ON Products.ProductId = OrderDetails.ProductId
	INNER JOIN dbo.Colors ON Colors.ColorId = Products.ColorId
	INNER JOIN dbo.Sizes ON Sizes.SizeId = Products.SizeId
	WHERE OrderId = @OrderId
END

--ORDER/ GetOrder
GO
CREATE OR ALTER PROCEDURE pr_GetOrder(@UserId INT, @ProductName NVARCHAR(255) = '')
AS
BEGIN
	SELECT Orders.*,
		(
			SELECT *
			FROM 
			(
				SELECT OrderDetails.*, ProductName, ProductSlug, ProductDesc, ProductImages,
					SizeName, ColorName, ColorValue 
				FROM 
				dbo.OrderDetails
				INNER JOIN dbo.Products ON Products.ProductId = OrderDetails.ProductId
				INNER JOIN dbo.Colors ON Colors.ColorId = Products.ColorId
				INNER JOIN dbo.Sizes ON Sizes.SizeId = Products.SizeId
				WHERE OrderId = Orders.OrderId
			) a
			FOR JSON AUTO
		) AS OrderDetailJson
	FROM dbo.Orders
	INNER JOIN (
		SELECT DISTINCT OrderId FROM dbo.OrderDetails
		INNER JOIN dbo.Products ON Products.ProductId = OrderDetails.ProductId
		WHERE LOWER(ProductName) LIKE N'%' + LOWER(@ProductName) + '%'
	) s ON s.OrderId = Orders.OrderId
	WHERE UserId = @UserId  
	ORDER BY OrderDate DESC
END

EXEC dbo.pr_GetOrder @UserId=2, @ProductName=N'suu'
EXEC dbo.pr_GetOrder @UserId=2, @ProductName=N'ti'


--ADMIN/ORDER/ Confirm Order
GO
CREATE OR ALTER PROCEDURE pr_ConfirmOrder(@OrderId INT)
AS
BEGIN
    BEGIN TRY
		BEGIN TRANSACTION;

		IF EXISTS (SELECT OrderId FROM dbo.Orders WHERE OrderId = @OrderId AND Status = 'Pending')
		BEGIN
		    UPDATE dbo.Products
			SET Quantity -= OrderDetails.Quantity
			FROM dbo.Products
			INNER JOIN dbo.OrderDetails ON OrderDetails.ProductId = Products.ProductId
			WHERE OrderId = @OrderId

			UPDATE dbo.Orders
			SET Status = 'Completed'
			FROM dbo.Orders
	
			COMMIT TRANSACTION;
		END
		ELSE
		BEGIN
			ROLLBACK TRANSACTION;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW
	END CATCH
END

EXEC dbo.pr_ConfirmOrder @OrderId=1 -- int

--ADMIN/ORDER/ Cancel Order
GO
CREATE OR ALTER PROCEDURE pr_CancelOrder(@OrderId INT)
AS
BEGIN
    UPDATE dbo.Orders
    SET Status = N'Cancelled'
    WHERE OrderId = @OrderId AND Status = N'Pending'
END

EXEC dbo.pr_CancelOrder @OrderId=1 -- int


SELECT * FROM dbo.Orders WHERE OrderId = 1
SELECT * FROM dbo.OrderDetails WHERE OrderId = 1

SELECT Products.ProductId, Products.Quantity FROM dbo.Products
INNER JOIN dbo.OrderDetails ON OrderDetails.ProductId = Products.ProductId
WHERE OrderId = 1

--ADMIN/COLOR
GO
CREATE OR ALTER PROCEDURE pr_CreateColor(@ColorName NVARCHAR(255), @ColorValue VARCHAR(255))
AS
BEGIN
    INSERT INTO dbo.Colors(ColorName, ColorValue, Active)
    VALUES(@ColorName, @ColorValue, 1)

	SELECT CONVERT(INT, SCOPE_IDENTITY())
END

GO
CREATE OR ALTER PROCEDURE pr_UpdateColor(@ColorId INT, @ColorName NVARCHAR(255), @ColorValue VARCHAR(255))
AS
BEGIN
    UPDATE dbo.Colors
    SET ColorName = @ColorName,
		ColorValue = @ColorValue
    WHERE ColorId = @ColorId
END

GO
CREATE OR ALTER PROCEDURE pr_ChangeProductColorSize(@ProductId INT, @ColorId INT = 0, @SizeId INT = 0)
AS
BEGIN
    UPDATE dbo.Products
    SET ColorId = @ColorId,
		SizeId = @SizeId
    WHERE ProductId = @ProductId
END

--ADMIN/ DASHBOARD
GO
CREATE OR ALTER PROCEDURE pr_GetDashboardInfo(@Month INT, @Year INT)
AS
BEGIN
	DECLARE @NewCustomers INT = 0
	DECLARE @Orders INT = 0
	DECLARE @Sales FLOAT = 0

    SELECT @Orders = COUNT(DISTINCT Orders.OrderId), @Sales = SUM(Price * (100 - Discount)/100 * Quantity)  
	FROM dbo.Orders
	LEFT JOIN dbo.OrderDetails ON OrderDetails.OrderId = Orders.OrderId
	WHERE MONTH(OrderDate) = @Month AND YEAR(OrderDate) = @Year

	SELECT @NewCustomers = COUNT(UserId)  
	FROM dbo.Users 
	WHERE MONTH(CreateAt) = @Month AND YEAR(CreateAt) = @Year AND RoleId = 2

	SELECT @NewCustomers AS NewCustomers, @Orders AS Orders, @Sales AS Sales
END

EXEC dbo.pr_GetDashboardInfo @Month=4, @Year=2024 

GO
CREATE OR ALTER PROCEDURE pr_SalesStatitic(@Month INT , @Year INT)
AS
BEGIN
    SELECT DAY(OrderDate) AS OrderDay,
		SUM(CASE WHEN Status = N'Completed' THEN (Price * (100 - Discount)/100 * Quantity) ELSE 0 END) AS CompletedSales,
		SUM(CASE WHEN Status = N'Pending' THEN (Price * (100 - Discount)/100 * Quantity) ELSE 0 END) AS PendingSales
	FROM dbo.Orders
	INNER JOIN dbo.OrderDetails ON OrderDetails.OrderId = Orders.OrderId
	WHERE MONTH(OrderDate) = @Month AND YEAR(OrderDate) = @Year
	GROUP BY DAY(OrderDate)
END

EXEC dbo.pr_SalesStatitic @Month=4, -- int
    @Year=2024 -- int


GO
CREATE OR ALTER PROCEDURE pr_GetOrderStatus(@Month INT = NULL, @Year INT)
AS
BEGIN
    SELECT SUM(CASE WHEN status = 'Cancelled' THEN 1 ELSE 0 END) AS CancelledOrders,
			SUM(CASE WHEN status = 'Completed' THEN 1 ELSE 0 END) AS CompletedOrders,
			SUM(CASE WHEN status = 'Pending' THEN 1 ELSE 0 END) AS PendingOrders
	FROM dbo.Orders
	WHERE MONTH(OrderDate) = @Month AND YEAR(OrderDate) = @Year
END

EXEC sp_describe_first_result_set N'pr_GetOrderStatus';

GO
CREATE OR ALTER PROCEDURE pr_GetRecentOrders
AS
BEGIN
    SELECT TOP 5 Orders.*, FirstName, LastName, Email,
		(
			SELECT SUM(Price * (100 - Discount)/100 * Quantity) 
			FROM dbo.OrderDetails
			WHERE OrderId = Orders.OrderId
			GROUP BY OrderId
		) AS Total
	FROM dbo.Orders
	INNER JOIN dbo.Users ON Users.UserId = Orders.UserId
	ORDER BY OrderDate DESC
END