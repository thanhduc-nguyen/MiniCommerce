IF (DB_ID(N'CatalogDb') IS NOT NULL)
    BEGIN
        USE Master
        ALTER DATABASE CatalogDb SET single_user WITH ROLLBACK IMMEDIATE
        DROP DATABASE CatalogDb
    END
GO

CREATE DATABASE CatalogDb
GO

USE CatalogDb
GO

CREATE TABLE dbo.Category (
    CategoryId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    CategoryGuid UNIQUEIDENTIFIER NOT NULL,
    CategoryName NVARCHAR(50) NOT NULL
)

CREATE TABLE dbo.Product (
    ProductId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    ProductGuid UNIQUEIDENTIFIER NOT NULL,
    ProductName NVARCHAR(100) NOT NULL,
    ShortDescription NVARCHAR(500),
    LongDescription NVARCHAR(MAX),
    ImageUrls NVARCHAR(MAX),
    ImageThumbnailUrl NVARCHAR(1000),
    Price DECIMAL(30, 5),
    CategoryId INT NOT NULL,
    CONSTRAINT Fk_CategoryProduct FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
)
GO

INSERT INTO dbo.Category (CategoryGuid, CategoryName) VALUES ('791a4219-b883-4c7b-93bf-bf977a25fdb6', 'Shirt')
INSERT INTO dbo.Category (CategoryGuid, CategoryName) VALUES ('1d097e4a-6950-4eb9-b4ba-1ff8c3315c98', 'Cup')
INSERT INTO dbo.Category (CategoryGuid, CategoryName) VALUES ('18b4cb49-bdee-4387-9935-df3215e0a0bf', 'Badge')

INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '28d65b5d-f522-448c-8ce5-203e4efe2c17',
    'Kudu Purple Sweatshirt',
    'Our famous shirt!',
    'The Kudu Purple Sweatshirt blends style and comfort in a unique, eye-catching design. Crafted from a soft, high-quality cotton blend, this sweatshirt provides a relaxed fit that’s perfect for everything from coding marathons to weekend hangouts. The rich purple hue is both bold and subtle, making it an easy yet standout piece in any wardrobe. Featuring the Kudu logo, a nod to the powerful Azure App Service, it’s a must-have for developers, tech enthusiasts, or anyone with a passion for cloud computing. With ribbed cuffs and waistband for a snug, cozy fit, this sweatshirt keeps you warm and stylish no matter where you go. Show off your tech pride in ultimate comfort with the Kudu Purple Sweatshirt.',
    '["/images/products/Kudu Purple Sweatshirt HD.png"]',
    '/images/products/Kudu Purple Sweatshirt.png',
    15.95,
    1
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '51149bd0-c9e0-4068-b8f2-3368caa5c85c',
    '.NET Blue Sweatshirt',
    'You''ll love it!',
    'The .NET Blue Sweatshirt is a stylish and comfortable addition to any developer''s wardrobe. Made from a soft, premium cotton blend, this sweatshirt offers a cozy fit perfect for casual outings or cozying up during coding sessions. The bold blue color is complemented by the subtle yet distinctive .NET logo, representing your passion for technology and development. Featuring ribbed cuffs and a ribbed waistband for a snug fit, it ensures warmth without sacrificing mobility. Whether you''re working late or relaxing on the weekend, this sweatshirt is the perfect way to showcase your love for the .NET ecosystem while staying comfortable.',
    '["/images/products/NET Blue Sweatshirt HD.png"]',
    '/images/products/NET Blue Sweatshirt.png',
    18.95,
    1
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '5df4c8d9-77ba-469a-8a23-5b87f9f7d9c5',
    '.NET Bot Black Sweatshirt',
    'Best seller in year.',
    'The .NET Bot Black Sweatshirt is the perfect blend of sleek design and comfort, ideal for developers and tech enthusiasts alike. Featuring a bold, minimalist graphic of the .NET Bot, this sweatshirt brings a fun and modern twist to your casual wardrobe. Made from a soft, premium cotton blend, it offers warmth and comfort for long coding sessions, weekend relaxation, or casual meetups. The timeless black color pairs easily with any outfit, while ribbed cuffs and waistband provide a snug fit to keep you cozy all day long. Whether you''re showcasing your .NET pride or simply enjoying a laid-back day, the .NET Bot Black Sweatshirt is a stylish, tech-inspired essential.',
    '["/images/products/NET Bot Black Sweatshirt HD.png"]',
    '/images/products/NET Bot Black Sweatshirt.png',
    18.95,
    1
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '293d91cd-0de8-4b18-b2e9-a1a26678dada',
    '.NET Foundation Sweatshirt',
    'A summer classic!',
    'Stay cozy and show your support for the .NET ecosystem with this stylish .NET Foundation Sweatshirt. Crafted from a soft, high-quality blend of cotton and polyester, it offers the perfect balance of comfort and durability. The sleek, modern design features the iconic .NET Foundation logo, making it a great way to represent the open-source community while staying warm. Whether you''re coding late into the night or just relaxing with friends, this sweatshirt is a must-have for any .NET enthusiast. Available in a variety of sizes and colors, it’s a versatile piece that’s perfect for everyday wear.',
    '["/images/products/NET Foundation Sweatshirt HD.png"]',
    '/images/products/NET Foundation Sweatshirt.png',
    17.95,
    1
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    'ad237f2f-94ce-46f5-91e5-950d328922a3',
    'Prism White T-Shirt',
    'Welcome summer!',
    'The Prism White T-Shirt combines simplicity and style for an effortless, everyday look. Made from premium soft cotton, this classic white tee offers a comfortable, breathable fit that’s perfect for any occasion. Its clean, minimalist design features a subtle, yet striking logo or graphic (depending on the style), making it a versatile wardrobe staple. Whether you''re layering it under a jacket, pairing it with jeans, or wearing it on its own, the Prism White T-Shirt brings a fresh, crisp look to any outfit. Ideal for casual outings, tech meetups, or just relaxing at home, this t-shirt is designed to offer both comfort and a touch of modern flair.',
    '["/images/products/Prism White T-Shirt HD.png"]',
    '/images/products/Prism White T-Shirt.png',
    18.95,
    1
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '035cea53-40e4-4100-8a92-3d478342090c',
    'Roslyn Red T-Shirt',
    'A summer favorite.',
    'The Roslyn Red T-Shirt is a bold and vibrant addition to your wardrobe, inspired by the energy of the Roslyn compiler platform. Made from a soft, high-quality cotton blend, this t-shirt offers a comfortable, breathable fit that feels great all day long. The rich, deep red hue gives it a distinctive, eye-catching appeal, while the minimalist design ensures it pairs effortlessly with jeans, shorts, or casual jackets. Whether you’re coding, hanging out with friends, or attending a tech event, the Roslyn Red T-Shirt is a versatile and stylish way to show off your passion for software development. With its premium fabric and modern fit, it’s as durable as it is stylish, making it a go-to piece for both work and play.',
    '["/images/products/Roslyn Red T-Shirt HD.png"]',
    '/images/products/Roslyn Red T-Shirt.png',
    17.95,
    1
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '722baa3a-5cf7-4952-9cda-dfaa1ad1367f',
    'Cup<T> T-Shirt',
    'Simple the best!',
    'The Cup<T> T-Shirt is the perfect choice for developers who love to blend their passion for code with their personal style. Inspired by the powerful world of generics in C#, this shirt features a clever graphic showcasing the \"Cup<T>\" template, making it a must-have for any programmer. Made from a soft, high-quality cotton blend, it offers all-day comfort while providing a modern, tailored fit. Whether you''re at a hackathon, a coding meetup, or just relaxing at home, this t-shirt is a great conversation starter and a subtle nod to the power of generic types in programming. Available in a variety of sizes, the Cup<T> T-Shirt combines tech-savvy appeal with laid-back style, making it an ideal choice for every .NET enthusiast and software developer.',
    '["/images/products/CupT T-Shirt HD.png"]',
    '/images/products/CupT T-Shirt.png',
    19.95,
    1
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    'a8814752-c866-4944-8020-61279d380087',
    'Cup<T> White Mug',
    'A new designed cup.',
    'The Cup<T> White Mug is a must-have for any developer who appreciates the finer details of coding—and coffee. With a sleek, minimalist design, this 11oz ceramic mug features the iconic Cup<T> graphic, a nod to the versatility of generics in C# programming. Its smooth, glossy white finish makes it a stylish addition to your desk, while the comfortable handle and durable construction ensure it’s perfect for sipping your favorite hot beverages. Whether you’re fueling a late-night coding session or enjoying a morning break, this mug is the perfect way to add a touch of techie flair to your routine. A great gift for developers and .NET enthusiasts, the Cup<T> White Mug combines functionality with geeky charm—because every coder deserves a mug that’s as clever as their code.',
    '["/images/products/CupT White Mug HD.png"]',
    '/images/products/CupT White Mug.png',
    10.95,
    2
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '03eb5b7c-3e42-4361-8210-3cb6ce0cf88f',
    '.NET Black White Mug',
    'A classic designed cup.',
    'The .NET Black & White Mug is a sleek and stylish way to enjoy your favorite drinks while representing the world of .NET development. Featuring a bold black exterior with a contrasting white .NET logo, this 11oz ceramic mug offers a modern, minimalist design that stands out on any desk. The smooth finish and comfortable handle make it perfect for sipping coffee, tea, or any hot beverage during those long coding sessions. Whether you''re a .NET enthusiast or simply love clean, sophisticated designs, this mug adds a touch of tech-inspired elegance to your daily routine. A great gift for developers, it''s both functional and a subtle nod to the .NET ecosystem.',
    '["/images/products/NET Black White Mug HD.png"]',
    '/images/products/NET Black White Mug.png',
    10.95,
    2
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '4ae642de-3ccf-4bca-8c24-14ae508a759f',
    'Cup<T> Badge',
    'A geeky badge!',
    'The Cup<T> Badge is the perfect accessory for developers who appreciate both clever coding concepts and stylish flair. Featuring the iconic **Cup<T>** graphic, this badge is a playful nod to the power of generics in C#. Whether you’re adding it to your laptop bag, jacket, or lanyard, this small but impactful pin is a great way to show off your love for coding and your .NET expertise. Crafted from durable materials with a secure backing, the **Cup<T> Badge** is designed to last through every project and tech event. A must-have for every developer''s collection, it''s a fun and subtle way to wear your coding passion on your sleeve—literally!',
    '["/images/products/CupT Badge HD.png"]',
    '/images/products/CupT Badge.png',
    4.95,
    3
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '90e3af6d-e79a-45b6-b2c8-8bc25f60708a',
    '.NET Foundation Badge',
    'You''ll love it!',
    'The .NET Foundation Badge is a stylish and meaningful way to show your support for the .NET open-source community. Featuring the iconic .NET Foundation logo, this sleek, high-quality badge is the perfect accessory for any developer passionate about the .NET ecosystem. Whether you pin it to your backpack, jacket, or conference lanyard, this badge serves as a subtle yet powerful symbol of your commitment to collaboration, innovation, and the open-source movement. Crafted from durable materials with a secure backing, the **.NET Foundation Badge** is both lightweight and long-lasting, making it an ideal way to proudly represent your involvement in the .NET community wherever you go. Perfect for meetups, hackathons, and casual wear, this badge is a must-have for any .NET enthusiast.',
    '["/images/products/NET Foundation Badge HD.png"]',
    '/images/products/NET Foundation Badge.png',
    3.95,
    3
)
INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId) VALUES (
    '324a1f34-97f9-4636-be93-ec456b19f90e',
    'Roslyn Red Badge',
    'This is a nice badge!',
    'The Roslyn Red Badge is a bold and vibrant way to showcase your passion for C# and the Roslyn compiler platform. Featuring a striking red design and a sleek, minimalist graphic, this badge is a perfect representation of your love for modern software development. Whether you’re attending a tech meetup, coding event, or simply want to add some personality to your gear, the **Roslyn Red Badge** makes a stylish statement. Crafted from high-quality materials with a secure pin backing, it’s built to last and can easily be attached to your bag, jacket, or lanyard. Show off your Roslyn pride and stand out in the developer community with this eye-catching, tech-inspired accessory.',
    '["/images/products/Roslyn Red Badge HD.png"]',
    '/images/products/Roslyn Red Badge.png',
    3.95,
    3
)
GO

CREATE PROCEDURE dbo.Product_Get
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    DECLARE @SkipNumber INT;

    IF @PageNumber = 1
        SET @SkipNumber = 0
    ELSE
        SET @SkipNumber = @PageSize * (@PageNumber - 1)
    SELECT * FROM dbo.Product ORDER BY CURRENT_TIMESTAMP OFFSET @SkipNumber ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO

CREATE PROCEDURE dbo.Product_Create
    @ProductId INT OUT,
    @ProductGuid UNIQUEIDENTIFIER,
    @ProductName NVARCHAR(100),
    @ShortDescription NVARCHAR(500),
    @LongDescription NVARCHAR(MAX),
    @ImageUrls NVARCHAR(MAX),
    @ImageThumbnailUrl NVARCHAR(1000),
    @Price DECIMAL(30, 5),
    @CategoryId INT
AS
BEGIN
    INSERT INTO dbo.Product (ProductGuid, ProductName, ShortDescription, LongDescription, ImageUrls, ImageThumbnailUrl, Price, CategoryId)
    VALUES (
         @ProductGuid,
         @ProductName,
         @ShortDescription, 
         @LongDescription,
         @ImageUrls,
         @ImageThumbnailUrl,
         @Price,
         @CategoryId
    )
    SELECT @ProductId = SCOPE_IDENTITY()
END
GO

CREATE PROCEDURE dbo.Product_Update
    @ProductId INT,
    @ProductGuid UNIQUEIDENTIFIER,
    @ProductName NVARCHAR(100),
    @ShortDescription NVARCHAR(500),
    @LongDescription NVARCHAR(MAX),
    @ImageUrls NVARCHAR(MAX),
    @ImageThumbnailUrl NVARCHAR(1000),
    @Price DECIMAL(30, 5),
    @CategoryId INT
AS
BEGIN
    UPDATE dbo.Product
    SET
        ProductName = @ProductName,
        ProductGuid = @ProductGuid,
        ShortDescription = @ShortDescription, 
        LongDescription = @LongDescription,
        ImageUrls = @ImageUrls,
        ImageThumbnailUrl = @ImageThumbnailUrl,
        Price = @Price,
        CategoryId = @CategoryId
    WHERE ProductId = @ProductId
END
GO

CREATE PROCEDURE dbo.Product_Delete
    @ProductId INT
AS
BEGIN
    DELETE FROM dbo.Product WHERE ProductId = @ProductId
END