using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace XChange.Api.Models
{
    public partial class XChangeDatabaseContext : DbContext
    {
        public XChangeDatabaseContext()
        {
        }

        public XChangeDatabaseContext(DbContextOptions<XChangeDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<Buyers> Buyers { get; set; }
        public virtual DbSet<CreditCards> CreditCards { get; set; }
        public virtual DbSet<Discounts> Discounts { get; set; }
        public virtual DbSet<GiftCards> GiftCards { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<Offers> Offers { get; set; }
        public virtual DbSet<OrderHasProducts> OrderHasProducts { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OtpLog> OtpLog { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<RegistrationLog> RegistrationLog { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<Sellers> Sellers { get; set; }
        public virtual DbSet<Shippers> Shippers { get; set; }
        public virtual DbSet<ShoppingCarts> ShoppingCarts { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserTimeLog> UserTimeLog { get; set; }
        public virtual DbSet<WishLists> WishLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=XChangeDatabase;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.AdressType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(e => e.AuditLogId).HasColumnName("AuditLogID");

                entity.Property(e => e.Activity).HasColumnType("text");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TimeLogged)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Buyers>(entity =>
            {
                entity.HasKey(e => e.BuyerId);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Buyers__A9D10534E21914AA")
                    .IsUnique();

                entity.Property(e => e.BuyerId).HasColumnName("BuyerID");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.MembershipId).HasColumnName("MembershipID");

                entity.Property(e => e.Phone).HasColumnType("text");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<CreditCards>(entity =>
            {
                entity.HasKey(e => e.CreditCardId);

                entity.Property(e => e.CreditCardId).HasColumnName("CreditCardID");

                entity.Property(e => e.CardAddress)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.CardCity)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.CardCvv).HasColumnName("CardCVV");

                entity.Property(e => e.CardPostalCode)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.CreditCardNumber)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            });

            modelBuilder.Entity<Discounts>(entity =>
            {
                entity.HasKey(e => e.DiscountId);

                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");

                entity.Property(e => e.DiscountPercent)
                    .HasColumnName("DIscountPercent")
                    .HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<GiftCards>(entity =>
            {
                entity.HasKey(e => e.GiftCardId);

                entity.Property(e => e.GiftCardId).HasColumnName("GiftCardID");

                entity.Property(e => e.GiftCardExpiryMonth)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.GiftCardExpiryYear)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.GiftCardNumber)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.Property(e => e.MembershipId).HasColumnName("MembershipID");

                entity.Property(e => e.MembershipType)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Offers>(entity =>
            {
                entity.HasKey(e => e.OfferId);

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
            });

            modelBuilder.Entity<OrderHasProducts>(entity =>
            {
                entity.HasKey(e => e.OrderProductId);

                entity.Property(e => e.OrderProductId).HasColumnName("OrderProductID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Freight).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.SalesTax).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.Property(e => e.TimeStamp)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.TransactStatus)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<OtpLog>(entity =>
            {
                entity.Property(e => e.OtpLogId).HasColumnName("OtpLogID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsSent)
                    .HasColumnName("Is_Sent")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsValidated).HasColumnName("Is_Validated");

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.Otp)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeSent)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Category)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductDescription).HasColumnType("text");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<RegistrationLog>(entity =>
            {
                entity.Property(e => e.RegistrationLogId).HasColumnName("RegistrationLogID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Error).HasColumnType("text");

                entity.Property(e => e.IsSuccessful).HasColumnName("isSuccessful");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TimeLogged)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserType)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.HasKey(e => e.ReviewId);

                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.CustomerReview)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Rating)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sellers>(entity =>
            {
                entity.HasKey(e => e.SellerId);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Sellers__A9D10534E8D5F22D")
                    .IsUnique();

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ContactFirstName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ContactLastName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPosition)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Logo).HasColumnType("text");

                entity.Property(e => e.MembershipId).HasColumnName("MembershipID");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Shippers>(entity =>
            {
                entity.HasKey(e => e.ShipperId);

                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.ShipperName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ShoppingCarts>(entity =>
            {
                entity.HasKey(e => e.ShoppingCartId);

                entity.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartID");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Users__A9D1053416BE8642")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.DateRegistered)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Photo).HasColumnType("text");

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserTimeLog>(entity =>
            {
                entity.Property(e => e.UserTimeLogId).HasColumnName("UserTimeLogID");

                entity.Property(e => e.Error).HasColumnType("text");

                entity.Property(e => e.IsSuccessful).HasColumnName("isSuccessful");

                entity.Property(e => e.TimeLogged)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<WishLists>(entity =>
            {
                entity.HasKey(e => e.WishListId);

                entity.Property(e => e.WishListId).HasColumnName("WishListID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });
        }
    }
}
