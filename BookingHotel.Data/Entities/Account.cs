using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Account
    {
        public Account()
        {
            Abouts = new HashSet<About>();
            AccountFunctions = new HashSet<AccountFunction>();
            AccountGroupAccounts = new HashSet<AccountGroupAccount>();
            ArticleCategories = new HashSet<ArticleCategory>();
            Assesses = new HashSet<Assess>();
            Banners = new HashSet<Banner>();
            ConfigSystems = new HashSet<ConfigSystem>();
            Contacts = new HashSet<Contact>();
            Customers = new HashSet<Customer>();
            Faqs = new HashSet<Faq>();
            Feedbacks = new HashSet<Feedback>();
            Notifications = new HashSet<Notification>();
            Pages = new HashSet<Page>();
            Partners = new HashSet<Partner>();
            ProductWishlists = new HashSet<ProductWishlist>();
            Provinces = new HashSet<Province>();
            Recruitments = new HashSet<Recruitment>();
            ServiceCategories = new HashSet<ServiceCategory>();
            //RefreshTokens = new HashSet<RefreshToken>();
            NotificationAccounts = new HashSet<NotificationAccount>();
            Carts = new HashSet<Cart>();
            ProductWallets = new HashSet<ProductWallet>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }
        public bool? AcceptTerms { get; set; }
        //public Role Role { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? VerificationTokenExpiresTime { get; set; }
        public DateTime? VerifiedTime { get; set; }
        public bool IsVerified => VerifiedTime.HasValue || PasswordResetTime.HasValue;
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpiresTime { get; set; }
        public DateTime? PasswordResetTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int AccountTypeId { get; set; }

        //public bool OwnsToken(string token)
        //{
        //    return this.RefreshTokens?.FirstOrDefault(x => x.Token == token) != null;
        //}

        public virtual AccountType AccountType { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<About> Abouts { get; set; }
        public virtual ICollection<AccountFunction> AccountFunctions { get; set; }
        public virtual ICollection<AccountGroupAccount> AccountGroupAccounts { get; set; }
        public virtual ICollection<ArticleCategory> ArticleCategories { get; set; }
        public virtual ICollection<Assess> Assesses { get; set; }
        public virtual ICollection<Banner> Banners { get; set; }
        public virtual ICollection<ConfigSystem> ConfigSystems { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Faq> Faqs { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Partner> Partners { get; set; }
        public virtual ICollection<ProductWishlist> ProductWishlists { get; set; }
        public virtual ICollection<Province> Provinces { get; set; }
        public virtual ICollection<Recruitment> Recruitments { get; set; }
        public virtual ICollection<ServiceCategory> ServiceCategories { get; set; }
        public virtual ICollection<NotificationAccount> NotificationAccounts { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ProductWallet> ProductWallets { get; set; }
    }
}
