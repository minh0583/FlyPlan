using Microsoft.EntityFrameworkCore;

namespace FlyPlan.Data.Models
{
    public partial class FlyplanContext : DbContext
    {
        private static string IN_MEMORY = "Microsoft.EntityFrameworkCore.InMemory";

        public FlyplanContext()
        {
        }

        public FlyplanContext(DbContextOptions<FlyplanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConfirmationInfo> ConfirmationInfo { get; set; }
        public virtual DbSet<Flight> Flight { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Traveller> Traveller { get; set; }
        public virtual DbSet<TravellerOrder> TravellerOrder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfirmationInfo>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(250);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ClassType).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Depart).HasMaxLength(250);

                entity.Property(e => e.DepartAirlineName).HasMaxLength(250);

                entity.Property(e => e.DepartAirlinePicture).HasMaxLength(250);

                entity.Property(e => e.DepartAirlinePlane).HasMaxLength(250);

                entity.Property(e => e.DepartAirport).HasMaxLength(250);

                entity.Property(e => e.DepartTime).HasMaxLength(250);

                entity.Property(e => e.Return).HasMaxLength(250);

                entity.Property(e => e.ReturnAirlineName).HasMaxLength(250);

                entity.Property(e => e.ReturnAirlinePicture).HasMaxLength(250);

                entity.Property(e => e.ReturnAirlinePlane).HasMaxLength(250);

                entity.Property(e => e.ReturnAirport).HasMaxLength(250);

                entity.Property(e => e.ReturnTime).HasMaxLength(250);

                entity.Property(e => e.TotalTime).HasMaxLength(250);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BillingAddress).HasMaxLength(250);

                entity.Property(e => e.CardNumber).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(250);

                entity.Property(e => e.CountryId).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreditCardType).HasMaxLength(250);

                entity.Property(e => e.Cvvcode)
                    .HasColumnName("CVVCode")
                    .HasMaxLength(250);

                entity.Property(e => e.ExpiryDateInMonth).HasMaxLength(250);

                entity.Property(e => e.ExpiryDateInYear).HasMaxLength(250);

                entity.Property(e => e.NameOnTheCard).HasMaxLength(250);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Zipcode)
                    .HasColumnName("ZIPCode")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Traveller>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CheckedBaggae).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(250);

                entity.Property(e => e.LastName).HasMaxLength(250);

                entity.Property(e => e.PasportExpiryDateMonth).HasMaxLength(250);

                entity.Property(e => e.PasportExpiryDateYear).HasMaxLength(250);

                entity.Property(e => e.PasportId).HasMaxLength(250);

                entity.Property(e => e.PersonType).HasMaxLength(250);

                entity.Property(e => e.TravelInsurance).HasMaxLength(250);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            if (Database.ProviderName == IN_MEMORY)
            {
                modelBuilder.Entity<Order>(entity =>
                {
                    entity.Property(e => e.Id).ValueGeneratedNever();

                    entity.Property(e => e.Code).HasMaxLength(250);

                    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
                });

                modelBuilder.Entity<TravellerOrder>(entity =>
                {
                    entity.HasKey(e => new { e.TravellerId, e.OrderId });
                });
            }
            else
            {
                modelBuilder.Entity<Order>(entity =>
                {
                    entity.Property(e => e.Id).ValueGeneratedNever();

                    entity.Property(e => e.Code).HasMaxLength(250);

                    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                    entity.HasOne(d => d.Confirmation)
                        .WithMany(p => p.Order)
                        .HasForeignKey(d => d.ConfirmationId)
                        .HasConstraintName("FK_Order_ConfirmationInfo");

                    entity.HasOne(d => d.Flight)
                        .WithMany(p => p.Order)
                        .HasForeignKey(d => d.FlightId)
                        .HasConstraintName("FK_Order_Flight");

                    entity.HasOne(d => d.Payment)
                        .WithMany(p => p.Order)
                        .HasForeignKey(d => d.PaymentId)
                        .HasConstraintName("FK_Order_Payment");
                });

                modelBuilder.Entity<TravellerOrder>(entity =>
                {
                    entity.HasKey(e => new { e.TravellerId, e.OrderId });

                    if (Database.ProviderName != IN_MEMORY)
                    {
                        entity.HasOne(d => d.Order)
                            .WithMany(p => p.TravellerOrder)
                            .HasForeignKey(d => d.OrderId)
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_TravellerOrder_Order");

                        entity.HasOne(d => d.Traveller)
                            .WithMany(p => p.TravellerOrder)
                            .HasForeignKey(d => d.TravellerId)
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_TravellerOrder_Traveller");
                    }
                });
            }
        }
    }
}
