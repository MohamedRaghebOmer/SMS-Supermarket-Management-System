USE [SMS]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[CategoryDescription] [nvarchar](250) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__Categori__19093A0B82EEAD87] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Categori__8517B2E072118DEA] UNIQUE NONCLUSTERED 
(
	[CategoryName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Countrie__10D160BFDBD6933F] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerLedger]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerLedger](
	[LedgerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[EntryDate] [datetime2](7) NOT NULL,
	[EntryType] [nvarchar](20) NOT NULL,
	[ReferenceType] [nvarchar](20) NOT NULL,
	[ReferenceId] [int] NULL,
	[DebitAmount] [decimal](18, 2) NOT NULL,
	[CreditAmount] [decimal](18, 2) NOT NULL,
	[BalanceBefore] [decimal](18, 2) NOT NULL,
	[BalanceAfter] [decimal](18, 2) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Notes] [nvarchar](250) NULL,
 CONSTRAINT [PK__Customer__AE70E0CF5CC28466] PRIMARY KEY CLUSTERED 
(
	[LedgerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[JoinDate] [date] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsBlocked] [bit] NOT NULL,
	[PaymentDay] [tinyint] NOT NULL,
	[CurrentBalance] [decimal](18, 2) NOT NULL,
	[LastPaymentDate] [datetime2](7) NULL,
	[NextDueDate] [date] NULL,
	[Notes] [nvarchar](250) NULL,
 CONSTRAINT [PK__Customer__A4AE64D8D207FEBA] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Customer__AA2FFBE4E18CF3AF] UNIQUE NONCLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[People]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[PersonId] [int] IDENTITY(1,1) NOT NULL,
	[NationalNo] [nvarchar](20) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[SecondName] [nvarchar](50) NOT NULL,
	[ThirdName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Gender] [tinyint] NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[NationalityCountryId] [int] NOT NULL,
	[ImageGuid] [uniqueidentifier] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Email] [nvarchar](50) NULL,
 CONSTRAINT [PK__Persons__AA2FFBE548C638A6] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [CK_People_Phone] UNIQUE NONCLUSTERED 
(
	[Phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Persons__E9AA1A65CCB688FB] UNIQUE NONCLUSTERED 
(
	[NationalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ProductName] [nvarchar](150) NOT NULL,
	[SKU] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[UnitId] [int] NOT NULL,
	[CostPrice] [decimal](18, 2) NOT NULL,
	[SellPrice] [decimal](18, 2) NOT NULL,
	[DiscountPercent] [decimal](5, 2) NOT NULL,
	[ImageGuid] [uniqueidentifier] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK__Products__B40CC6CDCE19801B] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Products__CA1ECF0DAAD1731E] UNIQUE NONCLUSTERED 
(
	[SKU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Products__DD5A978A8089B9E2] UNIQUE NONCLUSTERED 
(
	[ProductName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductStock]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductStock](
	[ProductId] [int] NOT NULL,
	[QuantityOnHand] [decimal](18, 3) NOT NULL,
	[ReorderLevel] [decimal](18, 3) NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__ProductS__B40CC6CDEB224828] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnItems]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnItems](
	[ReturnItemId] [int] IDENTITY(1,1) NOT NULL,
	[ReturnId] [int] NOT NULL,
	[SaleItemId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [decimal](18, 3) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[LineTotal] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK__ReturnIt__8D87CD3A5B6181BB] PRIMARY KEY CLUSTERED 
(
	[ReturnItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Returns]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Returns](
	[ReturnId] [int] IDENTITY(1,1) NOT NULL,
	[SaleId] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[ReturnDate] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ReturnReason] [nvarchar](250) NULL,
	[ReturnTotal] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__Returns__F445E9A8C253D0A1] PRIMARY KEY CLUSTERED 
(
	[ReturnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePagePermissions]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePagePermissions](
	[RoleId] [int] NOT NULL,
	[PageId] [int] NOT NULL,
	[PermissionMask] [int] NOT NULL,
 CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[PageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__Roles__8AFACE1AB24626B7] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Roles__8A2B61604B2480A8] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SaleItems]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleItems](
	[SaleItemId] [int] IDENTITY(1,1) NOT NULL,
	[SaleId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [decimal](18, 3) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[LineTotal] [decimal](18, 2) NOT NULL,
	[CostPriceAtSale] [decimal](18, 2) NULL,
 CONSTRAINT [PK__SaleItem__C6059401298A5F99] PRIMARY KEY CLUSTERED 
(
	[SaleItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[SaleId] [int] IDENTITY(1,1) NOT NULL,
	[SaleDate] [datetime2](7) NOT NULL,
	[CustomerId] [int] NULL,
	[CashierId] [int] NOT NULL,
	[PaymentMethod] [tinyint] NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[NetTotal] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[ChangeAmount] [decimal](18, 2) NOT NULL,
	[IsCredit] [bit] NOT NULL,
	[Notes] [nvarchar](250) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK__Sales__1EE3C3FFA51D1F72] PRIMARY KEY CLUSTERED 
(
	[SaleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemPages]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemPages](
	[PageId] [int] IDENTITY(1,1) NOT NULL,
	[PageTitle] [nvarchar](100) NOT NULL,
	[ModuleName] [nvarchar](50) NULL,
	[Description] [nvarchar](250) NULL,
 CONSTRAINT [PK__Permissi__EFA6FB2F14BB0FB2] PRIMARY KEY CLUSTERED 
(
	[PageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Permissi__0FFDA357949D7C60] UNIQUE NONCLUSTERED 
(
	[PageTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemSettings]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemSettings](
	[MaxCreditLimit] [decimal](18, 2) NOT NULL,
	[MinimumPaymentPercent] [decimal](5, 2) NOT NULL,
	[GraceDays] [int] NOT NULL,
	[FeesFrequencyDays] [int] NOT NULL,
	[FeesPercent] [decimal](5, 2) NOT NULL,
	[CapPercent] [decimal](5, 2) NOT NULL,
	[AllowCreditSales] [bit] NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Units]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Units](
	[UnitId] [int] IDENTITY(1,1) NOT NULL,
	[UnitName] [nvarchar](20) NOT NULL,
	[Symbol] [nvarchar](10) NOT NULL,
	[IsDecimal] [bit] NOT NULL,
 CONSTRAINT [PK__Units__44F5ECB59EF95DF0] PRIMARY KEY CLUSTERED 
(
	[UnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Units__B5EE667816E7127C] UNIQUE NONCLUSTERED 
(
	[UnitName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserActivityLogs]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserActivityLogs](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ActionType] [nvarchar](50) NOT NULL,
	[EntityName] [nvarchar](50) NULL,
	[EntityId] [int] NULL,
	[ActionDate] [datetime2](7) NOT NULL,
	[Details] [nvarchar](400) NULL,
	[IpAddress] [nvarchar](45) NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedAt] [datetime2](7) NULL,
 CONSTRAINT [PK__UserActi__5E548648A54F4152] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/4/2026 3:04:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[TokenHash] [nvarchar](255) NULL,
	[RoleId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[LastLoginAt] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK__Users__1788CC4CBEF4E97B] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Users__536C85E4D3A1526A] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Users__AA2FFBE476671A34] UNIQUE NONCLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF__Categorie__IsAct__778AC167]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CustomerLedger] ADD  CONSTRAINT [DF__CustomerL__Entry__6A30C649]  DEFAULT (sysdatetime()) FOR [EntryDate]
GO
ALTER TABLE [dbo].[CustomerLedger] ADD  CONSTRAINT [DF__CustomerL__Debit__6B24EA82]  DEFAULT ((0)) FOR [DebitAmount]
GO
ALTER TABLE [dbo].[CustomerLedger] ADD  CONSTRAINT [DF__CustomerL__Credi__6C190EBB]  DEFAULT ((0)) FOR [CreditAmount]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF__Customers__JoinD__628FA481]  DEFAULT (CONVERT([date],sysdatetime())) FOR [JoinDate]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF__Customers__IsAct__6383C8BA]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF__Customers__IsBlo__6477ECF3]  DEFAULT ((0)) FOR [IsBlocked]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF__Customers__Curre__656C112C]  DEFAULT ((0)) FOR [CurrentBalance]
GO
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF__Persons__Created__4BAC3F29]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__Products__Discou__7F2BE32F]  DEFAULT ((0)) FOR [DiscountPercent]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__Products__IsActi__00200768]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__Products__Create__01142BA1]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProductStock] ADD  CONSTRAINT [DF__ProductSt__Quant__05D8E0BE]  DEFAULT ((0)) FOR [QuantityOnHand]
GO
ALTER TABLE [dbo].[ProductStock] ADD  CONSTRAINT [DF__ProductSt__Reord__06CD04F7]  DEFAULT ((0)) FOR [ReorderLevel]
GO
ALTER TABLE [dbo].[ProductStock] ADD  CONSTRAINT [DF__ProductSt__LastU__07C12930]  DEFAULT (sysdatetime()) FOR [LastUpdated]
GO
ALTER TABLE [dbo].[Returns] ADD  CONSTRAINT [DF__Returns__ReturnD__1AD3FDA4]  DEFAULT (sysdatetime()) FOR [ReturnDate]
GO
ALTER TABLE [dbo].[Returns] ADD  CONSTRAINT [DF__Returns__Created__1BC821DD]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF__Roles__IsActive__4F7CD00D]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF__Roles__CreatedAt__5070F446]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SaleItems] ADD  CONSTRAINT [DF__SaleItems__Disco__160F4887]  DEFAULT ((0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF__Sales__SaleDate__0B91BA14]  DEFAULT (sysdatetime()) FOR [SaleDate]
GO
ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF__Sales__DiscountA__0C85DE4D]  DEFAULT ((0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF__Sales__PaidAmoun__0D7A0286]  DEFAULT ((0)) FOR [PaidAmount]
GO
ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF__Sales__ChangeAmo__0E6E26BF]  DEFAULT ((0)) FOR [ChangeAmount]
GO
ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF__Sales__IsCredit__0F624AF8]  DEFAULT ((0)) FOR [IsCredit]
GO
ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF__Sales__CreatedAt__10566F31]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemSettings] ADD  CONSTRAINT [DF__SystemSet__Allow__72C60C4A]  DEFAULT ((1)) FOR [AllowCreditSales]
GO
ALTER TABLE [dbo].[SystemSettings] ADD  CONSTRAINT [DF__SystemSet__Updat__73BA3083]  DEFAULT (sysdatetime()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[UserActivityLogs] ADD  CONSTRAINT [DF__UserActiv__Actio__2645B050]  DEFAULT (sysdatetime()) FOR [ActionDate]
GO
ALTER TABLE [dbo].[UserActivityLogs] ADD  CONSTRAINT [DF_UserActivityLogs_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__IsActive__5BE2A6F2]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__CreatedAt__5CD6CB2B]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[CustomerLedger]  WITH CHECK ADD  CONSTRAINT [FK_CustomerLedger_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
GO
ALTER TABLE [dbo].[CustomerLedger] CHECK CONSTRAINT [FK_CustomerLedger_Customers]
GO
ALTER TABLE [dbo].[CustomerLedger]  WITH CHECK ADD  CONSTRAINT [FK_CustomerLedger_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[CustomerLedger] CHECK CONSTRAINT [FK_CustomerLedger_Users]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Persons]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_People_Countries] FOREIGN KEY([NationalityCountryId])
REFERENCES [dbo].[Countries] ([CountryId])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_People_Countries]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Units] FOREIGN KEY([UnitId])
REFERENCES [dbo].[Units] ([UnitId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Units]
GO
ALTER TABLE [dbo].[ProductStock]  WITH CHECK ADD  CONSTRAINT [FK_ProductStock_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[ProductStock] CHECK CONSTRAINT [FK_ProductStock_Products]
GO
ALTER TABLE [dbo].[ReturnItems]  WITH CHECK ADD  CONSTRAINT [FK_ReturnItems_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[ReturnItems] CHECK CONSTRAINT [FK_ReturnItems_Products]
GO
ALTER TABLE [dbo].[ReturnItems]  WITH CHECK ADD  CONSTRAINT [FK_ReturnItems_Returns] FOREIGN KEY([ReturnId])
REFERENCES [dbo].[Returns] ([ReturnId])
GO
ALTER TABLE [dbo].[ReturnItems] CHECK CONSTRAINT [FK_ReturnItems_Returns]
GO
ALTER TABLE [dbo].[ReturnItems]  WITH CHECK ADD  CONSTRAINT [FK_ReturnItems_SaleItems] FOREIGN KEY([SaleItemId])
REFERENCES [dbo].[SaleItems] ([SaleItemId])
GO
ALTER TABLE [dbo].[ReturnItems] CHECK CONSTRAINT [FK_ReturnItems_SaleItems]
GO
ALTER TABLE [dbo].[Returns]  WITH CHECK ADD  CONSTRAINT [FK_Returns_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
GO
ALTER TABLE [dbo].[Returns] CHECK CONSTRAINT [FK_Returns_Customers]
GO
ALTER TABLE [dbo].[Returns]  WITH CHECK ADD  CONSTRAINT [FK_Returns_Sales] FOREIGN KEY([SaleId])
REFERENCES [dbo].[Sales] ([SaleId])
GO
ALTER TABLE [dbo].[Returns] CHECK CONSTRAINT [FK_Returns_Sales]
GO
ALTER TABLE [dbo].[Returns]  WITH CHECK ADD  CONSTRAINT [FK_Returns_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Returns] CHECK CONSTRAINT [FK_Returns_Users]
GO
ALTER TABLE [dbo].[RolePagePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY([PageId])
REFERENCES [dbo].[SystemPages] ([PageId])
GO
ALTER TABLE [dbo].[RolePagePermissions] CHECK CONSTRAINT [FK_RolePermissions_Permissions]
GO
ALTER TABLE [dbo].[RolePagePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[RolePagePermissions] CHECK CONSTRAINT [FK_RolePermissions_Roles]
GO
ALTER TABLE [dbo].[SaleItems]  WITH CHECK ADD  CONSTRAINT [FK_SaleItems_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[SaleItems] CHECK CONSTRAINT [FK_SaleItems_Products]
GO
ALTER TABLE [dbo].[SaleItems]  WITH CHECK ADD  CONSTRAINT [FK_SaleItems_Sales] FOREIGN KEY([SaleId])
REFERENCES [dbo].[Sales] ([SaleId])
GO
ALTER TABLE [dbo].[SaleItems] CHECK CONSTRAINT [FK_SaleItems_Sales]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Customers]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Users] FOREIGN KEY([CashierId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Users]
GO
ALTER TABLE [dbo].[UserActivityLogs]  WITH CHECK ADD  CONSTRAINT [FK_UserActivityLogs_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserActivityLogs] CHECK CONSTRAINT [FK_UserActivityLogs_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Persons]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[CustomerLedger]  WITH CHECK ADD  CONSTRAINT [CK_CustomerLedger_CreditAmount] CHECK  (([CreditAmount]>=(0)))
GO
ALTER TABLE [dbo].[CustomerLedger] CHECK CONSTRAINT [CK_CustomerLedger_CreditAmount]
GO
ALTER TABLE [dbo].[CustomerLedger]  WITH CHECK ADD  CONSTRAINT [CK_CustomerLedger_DebitAmount] CHECK  (([DebitAmount]>=(0)))
GO
ALTER TABLE [dbo].[CustomerLedger] CHECK CONSTRAINT [CK_CustomerLedger_DebitAmount]
GO
ALTER TABLE [dbo].[CustomerLedger]  WITH CHECK ADD  CONSTRAINT [CK_CustomerLedger_EntryType] CHECK  (([EntryType]='Adjustment' OR [EntryType]='Fee' OR [EntryType]='Return' OR [EntryType]='Payment' OR [EntryType]='Sale'))
GO
ALTER TABLE [dbo].[CustomerLedger] CHECK CONSTRAINT [CK_CustomerLedger_EntryType]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [CK_Customers_PaymentDay] CHECK  (([PaymentDay]>=(1) AND [PaymentDay]<=(31)))
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [CK_Customers_PaymentDay]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [CK_People_Gender] CHECK  (([Gender]=(2) OR [Gender]=(1)))
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [CK_People_Gender]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [CK_Sales_PaymentMethod] CHECK  (([PaymentMethod]=(1) OR [PaymentMethod]=(0) OR [PaymentMethod] IS NULL))
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [CK_Sales_PaymentMethod]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Male=1, Female=2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'People', @level2type=N'COLUMN',@level2name=N'Gender'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Read=1 Write=2 Delete=4 Update=8' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePagePermissions', @level2type=N'COLUMN',@level2name=N'PermissionMask'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Kg, L, Pcs, ...etc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Units', @level2type=N'COLUMN',@level2name=N'Symbol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the system admin wants to delete a User activity, will not be actually deleted from the databasee.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserActivityLogs', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
