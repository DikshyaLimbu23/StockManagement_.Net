using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StockManagementSystem.Models;

public partial class InventoryManagementContext : DbContext
{
    public InventoryManagementContext()
    {
    }

    public InventoryManagementContext(DbContextOptions<InventoryManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<BillPrint> BillPrints { get; set; }

    public virtual DbSet<BillRecord> BillRecords { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CompleteBillView> CompleteBillViews { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Plant> Plants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DbConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasKey(e => e.BillDetId).HasName("PK__BillDeta__820910BF2A5C4275");

            entity.Property(e => e.BillRid).HasColumnName("BillRId");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("rate");

            entity.HasOne(d => d.BillR).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.BillRid)
                .HasConstraintName("FK__BillDetai__BillR__3B40CD36");

            entity.HasOne(d => d.Product).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillDetai__produ__3587F3E0");
        });

        modelBuilder.Entity<BillPrint>(entity =>
        {
            entity.HasKey(e => e.Bpid).HasName("PK__BillPrin__3876B68CB67D61D2");

            entity.ToTable("BillPrint");

            entity.Property(e => e.Bpid).HasColumnName("BPID");
            entity.Property(e => e.Brid).HasColumnName("BRID");
            entity.Property(e => e.PrintedBy)
                .HasMaxLength(225)
                .IsUnicode(false);

            entity.HasOne(d => d.Br).WithMany(p => p.BillPrints)
                .HasForeignKey(d => d.Brid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillPrint__BRID__3864608B");
        });

        modelBuilder.Entity<BillRecord>(entity =>
        {
            entity.HasKey(e => e.Bid).HasName("PK__BillReco__C6D111C9B583764C");

            entity.Property(e => e.Canceleduserid).HasColumnName("canceleduserid");
            entity.Property(e => e.ReasonforCancel).IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.TotalAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Canceleduser).WithMany(p => p.BillRecordCanceledusers)
                .HasForeignKey(d => d.Canceleduserid)
                .HasConstraintName("FK__BillRecor__cance__32AB8735");

            entity.HasOne(d => d.User).WithMany(p => p.BillRecordUsers)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillRecor__useri__31B762FC");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__Cart__5D9A6C6EFCE98BF9");

            entity.ToTable("Cart");

            entity.Property(e => e.CartItemId).HasColumnName("cart_item_id");
            entity.Property(e => e.CateId).HasColumnName("cateID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Cate).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__cateID__5BE2A6F2");

            entity.HasOne(d => d.Plant).WithMany(p => p.Carts)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__plant_id__5AEE82B9");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId).HasName("PK__category__A88B4DC46FBCA669");

            entity.ToTable("category");

            entity.Property(e => e.CateId).HasColumnName("cateID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Category_name");
        });

        modelBuilder.Entity<CompleteBillView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CompleteBillView");

            entity.Property(e => e.BillRid).HasColumnName("BillRId");
            entity.Property(e => e.Bpid).HasColumnName("BPID");
            entity.Property(e => e.Brid).HasColumnName("BRID");
            entity.Property(e => e.Canceleduserid).HasColumnName("canceleduserid");
            entity.Property(e => e.PlantName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PrintedBy)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("rate");
            entity.Property(e => e.ReasonforCancel).IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("PK__Orders__080E3775B68D2234");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.OrderDate).HasColumnName("orderDate");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailsId).HasName("PK__Order_De__9DD74DBD797B7A39");

            entity.ToTable("Order_Details");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_Det__quant__02084FDA");

            entity.HasOne(d => d.Plant).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Plantid)
                .HasConstraintName("FK__Order_Det__Plant__02FC7413");
        });

        modelBuilder.Entity<Plant>(entity =>
        {
            entity.HasKey(e => e.PlantId).HasName("PK__Plant__A576B3B4DCE535BD");

            entity.ToTable("Plant");

            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.CateId).HasColumnName("cateID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.PlantName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("plant_name");
            entity.Property(e => e.Plantfile)
                .IsUnicode(false)
                .HasColumnName("plantfile");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SaleQuantity).HasColumnName("sale_quantity");
            entity.Property(e => e.TotalQuantity).HasColumnName("total_quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Cate).WithMany(p => p.Plants)
                .HasForeignKey(d => d.CateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Plant__cateID__5629CD9C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PK__users__CBA1B25716C7ED1B");

            entity.ToTable("users");

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.DeleteDate).HasColumnName("deleteDate");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Isactive).HasDefaultValue(false);
            entity.Property(e => e.Locations)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("locations");
            entity.Property(e => e.RegisterDate).HasColumnName("registerDate");
            entity.Property(e => e.ResonforDelete)
                .IsUnicode(false)
                .HasColumnName("resonforDelete");
            entity.Property(e => e.UserPassword).IsUnicode(false);
            entity.Property(e => e.UserRole)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("userRole");
            entity.Property(e => e.Userfile)
                .IsUnicode(false)
                .HasColumnName("userfile");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
