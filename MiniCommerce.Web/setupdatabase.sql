/*
    MiniCommerce database setup.
    Creates a single database (MiniCommerce) containing the ASP.NET Core Identity
    tables, the Products table and the Orders table, then seeds sample data.

    Run with: sqlcmd -S localhost -U sa -P Cvbnm123@ -i setupdatabase.sql
*/

IF (DB_ID(N'MiniCommerce') IS NOT NULL)
    BEGIN
        USE Master
        ALTER DATABASE [MiniCommerce] SET single_user WITH ROLLBACK IMMEDIATE
        DROP DATABASE [MiniCommerce]
    END
GO

CREATE DATABASE [MiniCommerce]
GO

USE [MiniCommerce]
GO

-- Required so the Identity filtered unique indexes can be created.
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

-- =========================================================
-- ASP.NET Core Identity schema
-- =========================================================
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

-- =========================================================
-- Catalog schema
-- =========================================================
CREATE TABLE [Products] (
    [ProductId] int NOT NULL IDENTITY(1,1),
    [ProductGuid] uniqueidentifier NOT NULL,
    [ProductName] nvarchar(100) NOT NULL,
    [Description] nvarchar(1000) NOT NULL,
    [ImageUrl] nvarchar(500) NOT NULL,
    [Price] decimal(30, 5) NOT NULL,
    [DiscountRate] decimal(5, 4) NOT NULL,
    [CategoryName] nvarchar(100) NOT NULL,
    [Stock] int NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId])
);
GO

-- =========================================================
-- Orders schema
-- =========================================================
CREATE TABLE [Orders] (
    [OrderId] int NOT NULL IDENTITY(1,1),
    [OrderGuid] uniqueidentifier NOT NULL,
    [UserGuid] uniqueidentifier NOT NULL,
    [ProductGuid] uniqueidentifier NOT NULL,
    [PurchasedPrice] decimal(30, 5) NOT NULL,
    [Quantity] int NOT NULL,
    [TotalPrice] decimal(30, 5) NOT NULL,
    [CreatedAt] datetime2 NOT NULL CONSTRAINT [DF_Orders_CreatedAt] DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId])
);
GO

-- =========================================================
-- Seed: roles
-- =========================================================
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES
('11111111-1111-1111-1111-111111111111', 'Admin', 'ADMIN', CONVERT(nvarchar(36), NEWID())),
('22222222-2222-2222-2222-222222222222', 'Customer', 'CUSTOMER', CONVERT(nvarchar(36), NEWID()))
GO

-- =========================================================
-- Seed: users (password for every user is "Cvbnm123@")
-- =========================================================
DECLARE @PasswordHash nvarchar(max) = 'AQAAAAIAAYagAAAAEBd34mNVNoRygtYBecR+qyPFpCJfoA5HnPEkh3CImN9BbKm1bzScxieBJD/A+bhOfg==';

INSERT INTO [AspNetUsers] (
    [Id], [Name], [UserName], [NormalizedUserName], [Email], [NormalizedEmail],
    [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp],
    [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
VALUES
('a1bdee07-44fd-48b8-9842-cddea53af7b3', 'MiniCommerce Admin', 'admin', 'ADMIN', 'admin@minicommerce.local', 'ADMIN@MINICOMMERCE.LOCAL',
    1, @PasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), 0, 0, 1, 0),
('40e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'Cristiano Ronaldo', 'ronaldo', 'RONALDO', 'ronaldo@minicommerce.local', 'RONALDO@MINICOMMERCE.LOCAL',
    1, @PasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), 0, 0, 1, 0),
('50e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'Wayne Rooney', 'rooney', 'ROONEY', 'rooney@minicommerce.local', 'ROONEY@MINICOMMERCE.LOCAL',
    1, @PasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), 0, 0, 1, 0),
('60e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'Paul Scholes', 'scholes', 'SCHOLES', 'scholes@minicommerce.local', 'SCHOLES@MINICOMMERCE.LOCAL',
    1, @PasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), 0, 0, 1, 0)
GO

-- =========================================================
-- Seed: user roles
-- =========================================================
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES
('a1bdee07-44fd-48b8-9842-cddea53af7b3', '11111111-1111-1111-1111-111111111111'),
('40e6ce14-f560-40b1-82ae-c1d9ec1015ff', '22222222-2222-2222-2222-222222222222'),
('50e6ce14-f560-40b1-82ae-c1d9ec1015ff', '22222222-2222-2222-2222-222222222222'),
('60e6ce14-f560-40b1-82ae-c1d9ec1015ff', '22222222-2222-2222-2222-222222222222')
GO

-- =========================================================
-- Seed: user claims (name + email)
-- =========================================================
INSERT INTO [AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES
('a1bdee07-44fd-48b8-9842-cddea53af7b3', 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', 'admin'),
('a1bdee07-44fd-48b8-9842-cddea53af7b3', 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', 'admin@minicommerce.local'),
('40e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', 'ronaldo'),
('40e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', 'ronaldo@minicommerce.local'),
('50e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', 'rooney'),
('50e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', 'rooney@minicommerce.local'),
('60e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', 'scholes'),
('60e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', 'scholes@minicommerce.local')
GO

-- =========================================================
-- Seed: orders
-- =========================================================
INSERT INTO [Orders] ([OrderGuid], [UserGuid], [ProductGuid], [PurchasedPrice], [Quantity], [TotalPrice], [CreatedAt]) VALUES
(NEWID(), '40e6ce14-f560-40b1-82ae-c1d9ec1015ff', '28d65b5d-f522-448c-8ce5-203e4efe2c17', 15.95, 1, 15.95, '2026-09-01T10:15:00'),
(NEWID(), '50e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'a8814752-c866-4944-8020-61279d380087', 10.95, 2, 21.90, '2026-09-05T14:30:00'),
(NEWID(), '60e6ce14-f560-40b1-82ae-c1d9ec1015ff', '51149bd0-c9e0-4068-b8f2-3368caa5c85c', 5.685, 1, 5.685, '2026-09-10T09:45:00'),
(NEWID(), '40e6ce14-f560-40b1-82ae-c1d9ec1015ff', '4ae642de-3ccf-4bca-8c24-14ae508a759f', 3.465, 3, 10.395, '2026-09-15T16:20:00'),
(NEWID(), '50e6ce14-f560-40b1-82ae-c1d9ec1015ff', '5df4c8d9-77ba-469a-8a23-5b87f9f7d9c5', 14.2125, 1, 14.2125, '2026-09-18T11:10:00'),
(NEWID(), '60e6ce14-f560-40b1-82ae-c1d9ec1015ff', '293d91cd-0de8-4b18-b2e9-a1a26678dada', 8.975, 2, 17.95, '2026-09-20T13:05:00'),
(NEWID(), '40e6ce14-f560-40b1-82ae-c1d9ec1015ff', 'ad237f2f-94ce-46f5-91e5-950d328922a3', 18.95, 1, 18.95, '2026-09-22T08:40:00'),
(NEWID(), '50e6ce14-f560-40b1-82ae-c1d9ec1015ff', '03eb5b7c-3e42-4361-8210-3cb6ce0cf88f', 10.95, 3, 32.85, '2026-09-24T15:25:00'),
(NEWID(), '60e6ce14-f560-40b1-82ae-c1d9ec1015ff', '90e3af6d-e79a-45b6-b2c8-8bc25f60708a', 2.9625, 2, 5.925, '2026-09-26T10:00:00'),
(NEWID(), '40e6ce14-f560-40b1-82ae-c1d9ec1015ff', '324a1f34-97f9-4636-be93-ec456b19f90e', 1.975, 4, 7.90, '2026-09-28T17:45:00')
GO

-- =========================================================
-- Seed: products
-- =========================================================
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '28d65b5d-f522-448c-8ce5-203e4efe2c17',
    'Kudu Purple Sweatshirt',
    'The Kudu Purple Sweatshirt blends style and comfort in a unique, eye-catching design. Crafted from a soft, high-quality cotton blend, this sweatshirt provides a relaxed fit thatΓÇÖs perfect for everything from coding marathons to weekend hangouts. The rich purple hue is both bold and subtle, making it an easy yet standout piece in any wardrobe. Featuring the Kudu logo, a nod to the powerful Azure App Service, itΓÇÖs a must-have for developers, tech enthusiasts, or anyone with a passion for cloud computing. With ribbed cuffs and waistband for a snug, cozy fit, this sweatshirt keeps you warm and stylish no matter where you go.',
    '/images/products/Kudu Purple Sweatshirt.png',
    15.95,
    0,
    'Shirt',
    245
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '51149bd0-c9e0-4068-b8f2-3368caa5c85c',
    '.NET Blue Sweatshirt',
    'The .NET Blue Sweatshirt is a stylish and comfortable addition to any developer''s wardrobe. Made from a soft, premium cotton blend, this sweatshirt offers a cozy fit perfect for casual outings or cozying up during coding sessions. The bold blue color is complemented by the subtle yet distinctive .NET logo, representing your passion for technology and development. Featuring ribbed cuffs and a ribbed waistband for a snug fit, it ensures warmth without sacrificing mobility. Whether you''re working late or relaxing on the weekend, this sweatshirt is the perfect way to showcase your love for the .NET ecosystem while staying comfortable.',
    '/images/products/NET Blue Sweatshirt.png',
    18.95,
    0.7,
    'Shirt',
    412
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '5df4c8d9-77ba-469a-8a23-5b87f9f7d9c5',
    '.NET Bot Black Sweatshirt',
    'The .NET Bot Black Sweatshirt is the perfect blend of sleek design and comfort, ideal for developers and tech enthusiasts alike. Featuring a bold, minimalist graphic of the .NET Bot, this sweatshirt brings a fun and modern twist to your casual wardrobe. Made from a soft, premium cotton blend, it offers warmth and comfort for long coding sessions, weekend relaxation, or casual meetups. The timeless black color pairs easily with any outfit, while ribbed cuffs and waistband provide a snug fit to keep you cozy all day long.',
    '/images/products/NET Bot Black Sweatshirt.png',
    18.95,
    0.25,
    'Shirt',
    178
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '293d91cd-0de8-4b18-b2e9-a1a26678dada',
    '.NET Foundation Sweatshirt',
    'Stay cozy and show your support for the .NET ecosystem with this stylish .NET Foundation Sweatshirt. Crafted from a soft, high-quality blend of cotton and polyester, it offers the perfect balance of comfort and durability. The sleek, modern design features the iconic .NET Foundation logo, making it a great way to represent the open-source community while staying warm. Whether you''re coding late into the night or just relaxing with friends, this sweatshirt is a must-have for any .NET enthusiast.',
    '/images/products/NET Foundation Sweatshirt.png',
    17.95,
    0.5,
    'Shirt',
    330
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    'ad237f2f-94ce-46f5-91e5-950d328922a3',
    'Prism White T-Shirt',
    'The Prism White T-Shirt combines simplicity and style for an effortless, everyday look. Made from premium soft cotton, this classic white tee offers a comfortable, breathable fit thatΓÇÖs perfect for any occasion. Its clean, minimalist design features a subtle, yet striking logo or graphic, making it a versatile wardrobe staple. Whether you''re layering it under a jacket, pairing it with jeans, or wearing it on its own, the Prism White T-Shirt brings a fresh, crisp look to any outfit.',
    '/images/products/Prism White T-Shirt.png',
    18.95,
    0,
    'Shirt',
    289
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '035cea53-40e4-4100-8a92-3d478342090c',
    'Roslyn Red T-Shirt',
    'The Roslyn Red T-Shirt is a bold and vibrant addition to your wardrobe, inspired by the energy of the Roslyn compiler platform. Made from a soft, high-quality cotton blend, this t-shirt offers a comfortable, breathable fit that feels great all day long. The rich, deep red hue gives it a distinctive, eye-catching appeal, while the minimalist design ensures it pairs effortlessly with jeans, shorts, or casual jackets.',
    '/images/products/Roslyn Red T-Shirt.png',
    17.95,
    0.1,
    'Shirt',
    156
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '722baa3a-5cf7-4952-9cda-dfaa1ad1367f',
    'Cup<T> T-Shirt',
    'The Cup<T> T-Shirt is the perfect choice for developers who love to blend their passion for code with their personal style. Inspired by the powerful world of generics in C#, this shirt features a clever graphic showcasing the "Cup<T>" template, making it a must-have for any programmer. Made from a soft, high-quality cotton blend, it offers all-day comfort while providing a modern, tailored fit.',
    '/images/products/CupT T-Shirt.png',
    19.95,
    0.25,
    'Shirt',
    467
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    'a8814752-c866-4944-8020-61279d380087',
    'Cup<T> White Mug',
    'The Cup<T> White Mug is a must-have for any developer who appreciates the finer details of codingΓÇöand coffee. With a sleek, minimalist design, this 11oz ceramic mug features the iconic Cup<T> graphic, a nod to the versatility of generics in C# programming. Its smooth, glossy white finish makes it a stylish addition to your desk, while the comfortable handle and durable construction ensure itΓÇÖs perfect for sipping your favorite hot beverages.',
    '/images/products/CupT White Mug.png',
    10.95,
    0.5,
    'Cup',
    203
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '03eb5b7c-3e42-4361-8210-3cb6ce0cf88f',
    '.NET Black White Mug',
    'The .NET Black & White Mug is a sleek and stylish way to enjoy your favorite drinks while representing the world of .NET development. Featuring a bold black exterior with a contrasting white .NET logo, this 11oz ceramic mug offers a modern, minimalist design that stands out on any desk. The smooth finish and comfortable handle make it perfect for sipping coffee, tea, or any hot beverage during those long coding sessions.',
    '/images/products/NET Black White Mug.png',
    10.95,
    0,
    'Cup',
    391
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '4ae642de-3ccf-4bca-8c24-14ae508a759f',
    'Cup<T> Badge',
    'The Cup<T> Badge is the perfect accessory for developers who appreciate both clever coding concepts and stylish flair. Featuring the iconic Cup<T> graphic, this badge is a playful nod to the power of generics in C#. Whether youΓÇÖre adding it to your laptop bag, jacket, or lanyard, this small but impactful pin is a great way to show off your love for coding and your .NET expertise.',
    '/images/products/CupT Badge.png',
    4.95,
    0.3,
    'Badge',
    128
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '90e3af6d-e79a-45b6-b2c8-8bc25f60708a',
    '.NET Foundation Badge',
    'The .NET Foundation Badge is a stylish and meaningful way to show your support for the .NET open-source community. Featuring the iconic .NET Foundation logo, this sleek, high-quality badge is the perfect accessory for any developer passionate about the .NET ecosystem. Whether you pin it to your backpack, jacket, or conference lanyard, this badge serves as a subtle yet powerful symbol of your commitment to collaboration, innovation, and the open-source movement.',
    '/images/products/NET Foundation Badge.png',
    3.95,
    0.25,
    'Badge',
    355
)
INSERT INTO [Products] (ProductGuid, ProductName, Description, ImageUrl, Price, DiscountRate, CategoryName, Stock) VALUES (
    '324a1f34-97f9-4636-be93-ec456b19f90e',
    'Roslyn Red Badge',
    'The Roslyn Red Badge is a bold and vibrant way to showcase your passion for C# and the Roslyn compiler platform. Featuring a striking red design and a sleek, minimalist graphic, this badge is a perfect representation of your love for modern software development. Whether youΓÇÖre attending a tech meetup, coding event, or simply want to add some personality to your gear, the Roslyn Red Badge makes a stylish statement.',
    '/images/products/Roslyn Red Badge.png',
    3.95,
    0.5,
    'Badge',
    480
)
GO

