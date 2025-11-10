CREATE DATABASE BookTradeDB;
GO
USE BookTradeDB;

CREATE TABLE [User] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [full_name] nvarchar(50) NOT NULL,
  [address] nvarchar(255) NOT NULL,
  [avatar_url] nvarchar(255)
)
GO

CREATE TABLE [Shop] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [name] nvarchar(100) NOT NULL,
  [description] text,
  [banner_url] nvarchar(255),
  [status] tinyint NOT NULL,
  [created_at] datetime2 NOT NULL,
  [user_id] int
)
GO

CREATE TABLE [Book] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [title] nvarchar(255) NOT NULL,
  [author] nvarchar(100),
  [description] text,
  [condition] tinyint NOT NULL,
  [status] tinyint NOT NULL,
  [stock_quantity] int NOT NULL,
  [sold_quantity] int NOT NULL,
  [base_price] decimal(19,4) NOT NULL,
  [discount_price] decimal(19,4) NOT NULL,
  [average_rating] decimal(3,2),
  [total_rating] smallint,
  [image_url] nvarchar(255),
  [created_at] datetime2 NOT NULL,
  [category_id] int,
  [shop_id] int
)
GO

CREATE TABLE [Category] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [name] nvarchar(50) NOT NULL
)
GO

CREATE TABLE [FavoriteBook] (
  [user_id] int,
  [book_id] int,
  PRIMARY KEY ([user_id], [book_id])
)
GO

CREATE TABLE [Order] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [order_date] datetime2 NOT NULL,
  [status] tinyint NOT NULL,
  [total] decimal(19,4) NOT NULL,
  [address] nvarchar(255) NOT NULL,
  [buyer_id] int,
  [shop_id] int,
  [payment_id] int
)
GO

CREATE TABLE [OrderItem] (
  [order_id] int,
  [book_id] int,
  [quantity] int NOT NULL,
  PRIMARY KEY ([order_id], [book_id])
)
GO

CREATE TABLE [Payment] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [payment_date] datetime2 NOT NULL,
  [total] decimal(19,4) NOT NULL,
  [payment_method] tinyint NOT NULL,
  [status] tinyint NOT NULL
)
GO

CREATE TABLE [Cart] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int
)
GO

CREATE TABLE [CartItem] (
  [cart_id] int,
  [book_id] int,
  [quantity] int NOT NULL,
  PRIMARY KEY ([cart_id], [book_id])
)
GO

CREATE TABLE [BookReview] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [rating] tinyint NOT NULL,
  [content] nvarchar(100) NOT NULL,
  [review_date] datetime2 NOT NULL,
  [user_id] int,
  [book_id] int
)
GO

CREATE TABLE [Report] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [title] nvarchar(100) NOT NULL,
  [description] text,
  [status] tinyint NOT NULL,
  [created_at] datetime2 NOT NULL,
  [type] tinyint NOT NULL,
  [target_id] int NOT NULL,
  [reporter_id] int
)
GO

CREATE TABLE [Notification] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [title] nvarchar(255) NOT NULL,
  [description] text,
  [is_read] bit NOT NULL,
  [created_at] datetime2 NOT NULL,
  [redirect_url] nvarchar(255),
  [image_url] nvarchar(255),
  [user_id] int,
  [order_id] int
)
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '0 - Pending - Chờ xác nhận
1 - Canceled - Đã hủy
2 - Active - Đang hoạt động
3 - Locked - Bị khóa
',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Shop',
@level2type = N'Column', @level2name = 'status';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '0 - New - Mới
1 - Old - Cũ
',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Book',
@level2type = N'Column', @level2name = 'condition';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '0 - Available - Có sẵn
1 - Not available - Không có sẵn
2 - Sold out - Hết hàng
',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Book',
@level2type = N'Column', @level2name = 'status';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '0 - Pending - Chờ xác nhận
1 - Processing - Đang xử lý
2 - Shipped - Đang giao
3 - Delivered - Đã giao
4 - Received - Đã nhận
5 - Canceled - Đã hủy
',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Order',
@level2type = N'Column', @level2name = 'status';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '0 - COD
1 - Online
',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Payment',
@level2type = N'Column', @level2name = 'payment_method';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '0 - Pending - Chờ thanh toán
1 - Paid - Đã thanh toán
2 - Failed - Thất bại
',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Payment',
@level2type = N'Column', @level2name = 'status';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '0 - Pending - Đang chờ
1 - Processing - Đang xử lý
2 - Resolved - Đã giải quyết
3 - Rejected - Đã từ chối
',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Report',
@level2type = N'Column', @level2name = 'status';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '0 - Book
1 - Shop
',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Report',
@level2type = N'Column', @level2name = 'type';
GO

ALTER TABLE [Shop] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Book] ADD FOREIGN KEY ([category_id]) REFERENCES [Category] ([id])
GO

ALTER TABLE [Book] ADD FOREIGN KEY ([shop_id]) REFERENCES [Shop] ([id])
GO

ALTER TABLE [FavoriteBook] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [FavoriteBook] ADD FOREIGN KEY ([book_id]) REFERENCES [Book] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([buyer_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([shop_id]) REFERENCES [Shop] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([payment_id]) REFERENCES [Payment] ([id])
GO

ALTER TABLE [OrderItem] ADD FOREIGN KEY ([order_id]) REFERENCES [Order] ([id])
GO

ALTER TABLE [OrderItem] ADD FOREIGN KEY ([book_id]) REFERENCES [Book] ([id])
GO

ALTER TABLE [Cart] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [CartItem] ADD FOREIGN KEY ([cart_id]) REFERENCES [Cart] ([id])
GO

ALTER TABLE [CartItem] ADD FOREIGN KEY ([book_id]) REFERENCES [Book] ([id])
GO

ALTER TABLE [BookReview] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [BookReview] ADD FOREIGN KEY ([book_id]) REFERENCES [Book] ([id])
GO

ALTER TABLE [Report] ADD FOREIGN KEY ([reporter_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Notification] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Notification] ADD FOREIGN KEY ([order_id]) REFERENCES [Order] ([id])
GO
