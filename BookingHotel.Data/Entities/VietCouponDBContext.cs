using System;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class VietCouponDBContext : DbContext
    {
        public VietCouponDBContext()
        {
        }

        public VietCouponDBContext(DbContextOptions<VietCouponDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountFunction> AccountFunctions { get; set; }
        public virtual DbSet<AccountGroupAccount> AccountGroupAccounts { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<Assess> Assesses { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<ConfigSystem> ConfigSystems { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerPartner> CustomerPartners { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
        public virtual DbSet<FunctionGroupFunction> FunctionGroupFunctions { get; set; }
        public virtual DbSet<GroupAccount> GroupAccounts { get; set; }
        public virtual DbSet<GroupFunction> GroupFunctions { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<NotificationAccount> NotificationAccounts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<PageType> PageTypes { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }
        public virtual DbSet<PartnerLocal> PartnerLocals { get; set; }
        public virtual DbSet<PartnerProduct> PartnerProducts { get; set; }
        public virtual DbSet<PartnerType> PartnerTypes { get; set; }
        public virtual DbSet<PayType> PayTypes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductWallet> ProductWallets { get; set; }
        public virtual DbSet<ProductTab> ProductTabs { get; set; }
        public virtual DbSet<ProductWishlist> ProductWishlists { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Recruitment> Recruitments { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
        public virtual DbSet<Tab> Tabs { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<MethodUsed> MethodUseds { get; set; }
        //public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=DESKTOP-93PR43M\\MSSQLSERVER2017;Database=vnsosoft_vietcoupon;user id=sa;password=123456;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<About>(entity =>
            {
                entity.ToTable("About");

                entity.HasIndex(e => e.CreateBy).HasName("IX_About_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Blog)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Facebook)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Hotline)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Instagram)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Linkedin)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Pinterest)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShortContent).HasColumnType("ntext");

                entity.Property(e => e.Tel)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.Property(e => e.Twitter).HasMaxLength(1024);

                entity.Property(e => e.Website)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Youtube)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Zalo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Abouts)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_About_Account");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.AccountTypeId).HasName("IX_Account_AccountTypeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountTypeId).HasColumnName("AccountTypeID");

                entity.Property(e => e.VerificationTokenExpiresTime).HasColumnType("datetime");

                entity.Property(e => e.VerifiedTime).HasColumnType("datetime");

                entity.Property(e => e.ResetTokenExpiresTime).HasColumnType("datetime");

                entity.Property(e => e.PasswordResetTime).HasColumnType("datetime");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_AccountType");

            
            });

            modelBuilder.Entity<AccountFunction>(entity =>
            {
                entity.HasKey(e => new { e.FunctionId, e.AccountId });

                entity.ToTable("AccountFunction");

                entity.HasIndex(e => e.AccountId).HasName("IX_AccountFunction_AccountID");

                entity.Property(e => e.FunctionId).HasColumnName("FunctionID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountFunctions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountFunction_Account");

                entity.HasOne(d => d.Function)
                    .WithMany(p => p.AccountFunctions)
                    .HasForeignKey(d => d.FunctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountFunction_Function");
            });

            modelBuilder.Entity<AccountGroupAccount>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.GroupAccountId });

                entity.ToTable("AccountGroupAccount");

                entity.HasIndex(e => e.GroupAccountId).HasName("IX_AccountGroupAccount_GroupAccountID");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("AccountID");

                entity.Property(e => e.GroupAccountId).HasColumnName("GroupAccountID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountGroupAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountGroupAccount_Account");

                entity.HasOne(d => d.GroupAccount)
                    .WithMany(p => p.AccountGroupAccounts)
                    .HasForeignKey(d => d.GroupAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountGroupAccount_GroupAccount");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.ToTable("AccountType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Key)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("Article");

                entity.HasIndex(e => e.ArticleCategoryId).HasName("IX_Article_ArticleCategoryID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArticleCategoryId).HasColumnName("ArticleCategoryID");

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.SourceLink).HasMaxLength(1024);

                entity.Property(e => e.SourcePage).HasMaxLength(1024);

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.Property(e => e.ValueAssess).HasColumnType("decimal(5, 1)");

                entity.HasOne(d => d.ArticleCategory)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.ArticleCategoryId)
                    .HasConstraintName("FK_Article_ArticleCategory");
            });

            modelBuilder.Entity<ArticleCategory>(entity =>
            {
                entity.ToTable("ArticleCategory");

                entity.HasIndex(e => e.CreateBy).HasName("IX_ArticleCategory_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.ArticleCategories)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_ArticleCategory_Account");
            });

            modelBuilder.Entity<Assess>(entity =>
            {
                entity.ToTable("Assess");

                entity.HasIndex(e => e.AccountId).HasName("IX_Assess_AccountID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.NumberStar).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.FullName).HasMaxLength(300);

                entity.Property(e => e.KeyId).HasColumnName("KeyID");

                entity.Property(e => e.KeyName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Assesses)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assess_Account");
               
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.ToTable("Banner");

                entity.HasIndex(e => e.CreateBy).HasName("IX_Banner_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.Property(e => e.Url)
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Banners)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Banner_Account");
            });

            modelBuilder.Entity<ConfigSystem>(entity =>
            {
                entity.ToTable("ConfigSystem");

                entity.HasIndex(e => e.CreateBy).HasName("IX_ConfigSystem_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Key)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Values)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.ConfigSystems)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_ConfigSystem_Account");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.HasIndex(e => e.ApproveBy).HasName("IX_Contact_ApproveBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.Content).HasMaxLength(4000);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(300);

                entity.Property(e => e.Mobi).HasMaxLength(13);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Subject).HasMaxLength(250);

                entity.HasOne(d => d.ApproveByNavigation)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.ApproveBy)
                    .HasConstraintName("FK_Contact_Account");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasIndex(e => e.AccountId).HasName("IX_Customer_AccountID");

                entity.HasIndex(e => e.CustomerTypeId).HasName("IX_Customer_CustomerTypeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Birthday)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.CompanyName).HasMaxLength(1000);

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");

                entity.Property(e => e.Email)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(300);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ReferralCode).IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Account");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .HasConstraintName("FK_Customer_CustomerType");
            });

            modelBuilder.Entity<CustomerPartner>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.PartnerId });

                entity.ToTable("CustomerPartner");

                entity.HasIndex(e => e.PartnerId).HasName("IX_CustomerPartner_PartnerID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.PartnerId).HasColumnName("PartnerID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerPartners)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPartner_Customer");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.CustomerPartners)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPartner_Partner");
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.ToTable("CustomerType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");

                entity.HasIndex(e => e.ProvinceId).HasName("IX_District_ProvinceID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");

                entity.Property(e => e.Name).HasMaxLength(1024);

                entity.Property(e => e.Type).HasMaxLength(1024);

                entity.Property(e => e.Location).HasMaxLength(1024);

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_District_Province");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("Faq");

                entity.HasIndex(e => e.CreateBy).HasName("IX_Faq_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Alias)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Faqs)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Faq_Account");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.HasIndex(e => e.CreateBy).HasName("IX_Feedback_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Content).HasMaxLength(1024);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(300);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Regency).HasMaxLength(1024);

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Feedback_Account");
            });

            modelBuilder.Entity<Function>(entity =>
            {
                entity.ToTable("Function");

                entity.HasIndex(e => e.ModuleId).HasName("IX_Function_ModuleID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Action)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Controller)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.Note).HasColumnType("ntext");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Title).HasMaxLength(1024);
                entity.Property(e => e.Icon).HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Functions)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_Function_Module");
            });

            modelBuilder.Entity<FunctionGroupFunction>(entity =>
            {
                entity.HasKey(e => new { e.GroupFunctionId, e.FunctionId });

                entity.ToTable("FunctionGroupFunction");

                entity.HasIndex(e => e.FunctionId).HasName("IX_FunctionGroupFunction_FunctionID");

                entity.Property(e => e.GroupFunctionId).HasColumnName("GroupFunctionID");

                entity.Property(e => e.FunctionId).HasColumnName("FunctionID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.HasOne(d => d.Function)
                    .WithMany(p => p.FunctionGroupFunctions)
                    .HasForeignKey(d => d.FunctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FunctionGroupFunction_Function");

                entity.HasOne(d => d.GroupFunction)
                    .WithMany(p => p.FunctionGroupFunctions)
                    .HasForeignKey(d => d.GroupFunctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FunctionGroupFunction_GroupFunction");
            });

            modelBuilder.Entity<GroupAccount>(entity =>
            {
                entity.ToTable("GroupAccount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Key)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<GroupFunction>(entity =>
            {
                entity.ToTable("GroupFunction");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.ToTable("Module");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
                entity.Property(e => e.Icon).HasMaxLength(100);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.HasIndex(e => e.CreateBy).HasName("IX_Notification_CreateBy");

                entity.HasIndex(e => e.NotificationTypeId).HasName("IX_Notification_NotificationTypeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.NotificationTypeId).HasColumnName("NotificationTypeID");

                entity.Property(e => e.Receiver).HasColumnType("ntext");

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Notification_Account");

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .HasConstraintName("FK_Notification_NotificationType");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("NotificationType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<NotificationAccount>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.NotificationId });

                entity.ToTable("NotificationAccount");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.NotificationAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationAccount_Account");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.NotificationAccounts)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationCustomer_Notification");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.ProductId });

                entity.ToTable("Cart");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cart_Account");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cart_Product");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasIndex(e => e.CustomerId).HasName("IX_Order_CustomerID");

                entity.HasIndex(e => e.OrderStatusId).HasName("IX_Order_OrderStatusID");

                entity.HasIndex(e => e.PayTypeId).HasName("IX_Order_PayTypeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.Code)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Email)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(300);

                entity.Property(e => e.Mobi).HasMaxLength(13);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.NoteFeeShip).HasMaxLength(200);

                entity.Property(e => e.NoteSale).HasMaxLength(200);

                entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");

                entity.Property(e => e.PayTypeId).HasColumnName("PayTypeID");

                entity.Property(e => e.Remark).HasMaxLength(1024);

                entity.Property(e => e.SaleOff).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("FK_Order_OrderStatus");

                entity.HasOne(d => d.PayType)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PayTypeId)
                    .HasConstraintName("FK_Order_PayType");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.HasIndex(e => e.OrderId).HasName("IX_OrderDetail_OrderID");

                entity.HasIndex(e => e.ProductId).HasName("IX_OrderDetail_ProductID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Option).HasColumnType("ntext");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.ToTable("Page");

                entity.HasIndex(e => e.CreateBy).HasName("IX_Page_CreateBy");

                entity.HasIndex(e => e.PageTypeId).HasName("IX_Page_PageTypeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Alias)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.PageTypeId).HasColumnName("PageTypeID");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.Property(e => e.Url)
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Pages)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Page_Account");

                entity.HasOne(d => d.PageType)
                    .WithMany(p => p.Pages)
                    .HasForeignKey(d => d.PageTypeId)
                    .HasConstraintName("FK_Page_PageType");
            });

            modelBuilder.Entity<PageType>(entity =>
            {
                entity.ToTable("PageType");

                entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partner");

                entity.HasIndex(e => e.CreateBy).HasName("IX_Partner_CreateBy");

                entity.HasIndex(e => e.PartnerTypeId).HasName("IX_Partner_PartnerTypeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Banner).HasMaxLength(1024);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.PartnerTypeId).HasColumnName("PartnerTypeID");

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.Title).HasMaxLength(1024);
                entity.Property(e => e.Representative).HasMaxLength(1024);

                entity.Property(e => e.Url)
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.ValueAssess).HasColumnType("decimal(5, 1)");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Partner_Account");

                entity.HasOne(d => d.PartnerType)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.PartnerTypeId)
                    .HasConstraintName("FK_Partner_PartnerType");
            });

            modelBuilder.Entity<PartnerLocal>(entity =>
            {
                entity.ToTable("PartnerLocal");

                entity.HasIndex(e => e.PartnerId).HasName("IX_PartnerLocal_PartnerID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.PartnerId).HasColumnName("PartnerID");

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerLocals)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerLocal_Partner");
            });

            modelBuilder.Entity<PartnerProduct>(entity =>
            {
                entity.HasKey(e => new { e.PartnerId, e.ProductId });

                entity.ToTable("PartnerProduct");

                entity.HasIndex(e => e.ProductId).HasName("IX_PartnerProduct_ProductID");

                entity.Property(e => e.PartnerId).HasColumnName("PartnerID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerProducts)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerProduct_Partner");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PartnerProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerProduct_Product");
            });

            modelBuilder.Entity<PartnerType>(entity =>
            {
                entity.ToTable("PartnerType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<PayType>(entity =>
            {
                entity.ToTable("PayType");

                entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.ProductCategoryId).HasName("IX_Product_ProductCategoryID");

                entity.HasIndex(e => e.PartnerId).HasName("IX_Product_PartnerID");

                entity.HasIndex(e => e.SaleId).HasName("IX_Product_SaleID");

                entity.HasIndex(e => e.UnitId).HasName("IX_Product_UnitID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Alias)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Code)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.ImageListProduct).HasMaxLength(4000);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                //entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OldPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");

                entity.Property(e => e.PartnerId).HasColumnName("PartnerID");

                entity.Property(e => e.SaleStart).HasColumnType("datetime");

                entity.Property(e => e.SaleDeadLine).HasColumnType("datetime");

                entity.Property(e => e.SaleId).HasColumnName("SaleID");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.MethodUsedId).HasColumnName("MethodUsedID");

                entity.Property(e => e.ValueAssess).HasColumnType("decimal(5, 1)");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_Product_ProductCategory");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_Product_Partner");

                entity.HasOne(d => d.SaleNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SaleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Sale");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Product_Unit");

                entity.HasOne(d => d.MethodUsed)
                   .WithMany(p => p.Products)
                   .HasForeignKey(d => d.MethodUsedId)
                   .HasConstraintName("FK_Product_MethodUsed");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Alias)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<ProductWallet>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.OrderId, e.ProductId });

                entity.ToTable("ProductWallet");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.MethodUsedId).HasColumnName("MethodUsedID");

                entity.Property(e => e.Code).HasMaxLength(4000);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.OriginKey)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UseTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ProductWallets)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductWallet_Account");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductWallets)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductWallet_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductWallets)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductWallet_Product");

                entity.HasOne(d => d.MethodUsed)
                   .WithMany(p => p.ProductWallets)
                   .HasForeignKey(d => d.MethodUsedId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_ProductWallet_MethodUsed");
            });

            modelBuilder.Entity<ProductTab>(entity =>
            {
                entity.HasKey(e => new { e.TabId, e.ProductId });

                entity.ToTable("ProductTab");

                entity.HasIndex(e => e.ProductId).HasName("IX_ProductTab_ProductID");

                entity.Property(e => e.TabId).HasColumnName("TabID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTabs)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTab_Product");

                entity.HasOne(d => d.Tab)
                    .WithMany(p => p.ProductTabs)
                    .HasForeignKey(d => d.TabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTab_Tab");
            });

            modelBuilder.Entity<ProductWishlist>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.ProductId });

                entity.ToTable("ProductWishlist");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ProductWishlists)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductWishlist_Account");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductWishlists)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductWishlist_Product");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.ToTable("Province");

                entity.HasIndex(e => e.CreateBy).HasName("IX_Province_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.FeeShip).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(1024);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Provinces)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Province_Account");
            });

            modelBuilder.Entity<Recruitment>(entity =>
            {
                entity.ToTable("Recruitment");

                entity.HasIndex(e => e.CreateBy).HasName("IX_Recruitment_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Recruitments)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Recruitment_Account");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sale");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Alias)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.KeySale)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.PriceSale).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.ShortTitle).HasMaxLength(300);

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.HasIndex(e => e.ServiceCategoryId).HasName("IX_Service_ServiceCategoryID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Alias)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.ServiceCategoryId).HasColumnName("ServiceCategoryID");

                entity.Property(e => e.Thumb).HasMaxLength(1024);

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.Property(e => e.ValueAssess).HasColumnType("decimal(5, 1)");

                entity.HasOne(d => d.ServiceCategory)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ServiceCategoryId)
                    .HasConstraintName("FK_Service_ServiceCategory");
            });

            modelBuilder.Entity<ServiceCategory>(entity =>
            {
                entity.ToTable("ServiceCategory");

                entity.HasIndex(e => e.CreateBy).HasName("IX_ServiceCategory_CreateBy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Alias)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Schemas).HasColumnType("ntext");

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.ServiceCategories)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_ServiceCategory_Account");
            });

            modelBuilder.Entity<Tab>(entity =>
            {
                entity.ToTable("Tab");

                entity.HasIndex(e => e.TabTypeId).HasName("IX_Tab_TabTypeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TabTypeId).HasColumnName("TabTypeID");

                entity.Property(e => e.Code)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);

                entity.HasOne(d => d.TabType)
                    .WithMany(p => p.Tabs)
                    .HasForeignKey(d => d.TabTypeId)
                    .HasConstraintName("FK_Tab_TabType");
            });

            modelBuilder.Entity<TabType>(entity =>
            {
                entity.ToTable("TabType");

                entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit");

                entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<MethodUsed>(entity =>
            {
                entity.ToTable("MethodUsed");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(1024);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("RefreshToken");

                entity.HasOne(d => d.Account)
                  .WithMany(p => p.RefreshTokens)
                  .HasForeignKey(d => d.AccountId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_RefreshToken_Account");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
