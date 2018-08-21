//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

//namespace OldOneWinDB.Models
//{
//    public partial class baseContext : DbContext
//    {
//        public baseContext()
//        {
//        }

//        public baseContext(DbContextOptions<baseContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<RvcBdom> RvcBdom { get; set; }
//        public virtual DbSet<RvcSulic> RvcSulic { get; set; }
//        public virtual DbSet<RvcSulictip> RvcSulictip { get; set; }
//        public virtual DbSet<TblBrtimsg> TblBrtimsg { get; set; }
//        public virtual DbSet<TblDocRegistry> TblDocRegistry { get; set; }
//        public virtual DbSet<TblFamily> TblFamily { get; set; }
//        public virtual DbSet<TblOrganization> TblOrganization { get; set; }
//        public virtual DbSet<TblRegistration> TblRegistration { get; set; }

//        // Unable to generate entity type for table 'OneWin.DBO_1_~1'. Please see the warning messages.
//        // Unable to generate entity type for table 'dbo.tblExport'. Please see the warning messages.

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=.;Database=base_par;Trusted_Connection=True;");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<RvcBdom>(entity =>
//            {
//                entity.HasKey(e => e.Koddom);

//                entity.ToTable("RVC_BDOM");

//                entity.Property(e => e.Koddom)
//                    .HasColumnName("KODDOM")
//                    .HasMaxLength(6)
//                    .ValueGeneratedNever();

//                entity.Property(e => e.AdresDop)
//                    .HasColumnName("ADRES_DOP")
//                    .HasMaxLength(20);

//                entity.Property(e => e.Datekor)
//                    .HasColumnName("DATEKOR")
//                    .HasMaxLength(30);

//                entity.Property(e => e.Etag)
//                    .HasColumnName("ETAG")
//                    .HasMaxLength(11);

//                entity.Property(e => e.Fond)
//                    .HasColumnName("FOND")
//                    .HasMaxLength(1);

//                entity.Property(e => e.Godpost).HasColumnName("GODPOST");

//                entity.Property(e => e.Ind)
//                    .HasColumnName("IND")
//                    .HasMaxLength(2);

//                entity.Property(e => e.Jes)
//                    .HasColumnName("JES")
//                    .HasMaxLength(3);

//                entity.Property(e => e.Jreo)
//                    .HasColumnName("JREO")
//                    .HasMaxLength(2);

//                entity.Property(e => e.Kmst)
//                    .HasColumnName("KMST")
//                    .HasMaxLength(3);

//                entity.Property(e => e.Kodul)
//                    .HasColumnName("KODUL")
//                    .HasMaxLength(5);

//                entity.Property(e => e.Kolkv).HasColumnName("KOLKV");

//                entity.Property(e => e.Kopl).HasColumnName("KOPL");

//                entity.Property(e => e.Korp)
//                    .HasColumnName("KORP")
//                    .HasMaxLength(2);

//                entity.Property(e => e.Krkm)
//                    .HasColumnName("KRKM")
//                    .HasMaxLength(3);

//                entity.Property(e => e.Ndom).HasColumnName("NDOM");

//                entity.Property(e => e.Plgl).HasColumnName("PLGL");

//                entity.Property(e => e.Plob).HasColumnName("PLOB");

//                entity.Property(e => e.Plpl).HasColumnName("PLPL");

//                entity.Property(e => e.Plzem).HasColumnName("PLZEM");

//                entity.Property(e => e.Postav)
//                    .HasColumnName("POSTAV")
//                    .HasMaxLength(3);

//                entity.Property(e => e.Username)
//                    .HasColumnName("USERNAME")
//                    .HasMaxLength(10);

//                entity.HasOne(d => d.KodulNavigation)
//                    .WithMany(p => p.RvcBdom)
//                    .HasForeignKey(d => d.Kodul)
//                    .HasConstraintName("FK_RVC_BDOM_RVC_SULIC");
//            });

//            modelBuilder.Entity<RvcSulic>(entity =>
//            {
//                entity.HasKey(e => e.Kodul);

//                entity.ToTable("RVC_SULIC");

//                entity.HasIndex(e => e.Name)
//                    .HasName("IX_RVC_SULIC");

//                entity.Property(e => e.Kodul)
//                    .HasColumnName("KODUL")
//                    .HasMaxLength(5)
//                    .ValueGeneratedNever();

//                entity.Property(e => e.Name)
//                    .HasColumnName("NAME")
//                    .HasMaxLength(25);

//                entity.Property(e => e.Ultip)
//                    .HasColumnName("ULTIP")
//                    .HasMaxLength(2);

//                entity.HasOne(d => d.UltipNavigation)
//                    .WithMany(p => p.RvcSulic)
//                    .HasForeignKey(d => d.Ultip)
//                    .HasConstraintName("FK_RVC_SULIC_RVC_SULICTIP");
//            });

//            modelBuilder.Entity<RvcSulictip>(entity =>
//            {
//                entity.HasKey(e => e.Ultip);

//                entity.ToTable("RVC_SULICTIP");

//                entity.Property(e => e.Ultip)
//                    .HasColumnName("ULTIP")
//                    .HasMaxLength(2)
//                    .ValueGeneratedNever();

//                entity.Property(e => e.Fname)
//                    .HasColumnName("FNAME")
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.Name)
//                    .HasColumnName("NAME")
//                    .HasMaxLength(10)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<TblBrtimsg>(entity =>
//            {
//                entity.HasKey(e => e.BrtimsgId);

//                entity.ToTable("tblBRTIMsg");

//                entity.Property(e => e.BrtimsgId)
//                    .HasColumnName("BRTIMsgID")
//                    .ValueGeneratedNever();

//                entity.Property(e => e.Address)
//                    .IsRequired()
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.AddressDate).HasColumnType("datetime");

//                entity.Property(e => e.Bank)
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.Dob)
//                    .HasColumnName("DOB")
//                    .HasColumnType("datetime");

//                entity.Property(e => e.DocIssueDate).HasColumnType("datetime");

//                entity.Property(e => e.DocIssuer)
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.DocNo)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.DocType)
//                    .IsRequired()
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.DogDate).HasColumnType("datetime");

//                entity.Property(e => e.DogNo)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.Fname)
//                    .IsRequired()
//                    .HasColumnName("FName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Lname)
//                    .IsRequired()
//                    .HasColumnName("LName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Mname)
//                    .IsRequired()
//                    .HasColumnName("MName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.PayNo)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.PersonalNo)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
//            });

//            modelBuilder.Entity<TblDocRegistry>(entity =>
//            {
//                entity.HasKey(e => e.RegId);

//                entity.ToTable("tblDocRegistry");

//                entity.Property(e => e.RegId)
//                    .HasColumnName("RegID")
//                    .ValueGeneratedNever();

//                entity.Property(e => e.AdditionalDoc).HasColumnType("text");

//                entity.Property(e => e.DeptId).HasColumnName("DeptID");

//                entity.Property(e => e.GetDataFromRvc).HasColumnName("GetDataFromRVC");

//                entity.Property(e => e.ParrentId).HasColumnName("ParrentID");

//                entity.Property(e => e.RegName)
//                    .IsRequired()
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.StatementForm)
//                    .HasMaxLength(15)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<TblFamily>(entity =>
//            {
//                entity.HasKey(e => e.FamilyId);

//                entity.ToTable("tblFamily");

//                entity.Property(e => e.FamilyId)
//                    .HasColumnName("FamilyID")
//                    .ValueGeneratedNever();

//                entity.Property(e => e.Address)
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.AddressDate).HasColumnType("datetime");

//                entity.Property(e => e.Dob)
//                    .HasColumnName("DOB")
//                    .HasColumnType("datetime");

//                entity.Property(e => e.Fname)
//                    .IsRequired()
//                    .HasColumnName("FName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Lname)
//                    .IsRequired()
//                    .HasColumnName("LName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Mname)
//                    .IsRequired()
//                    .HasColumnName("MName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.NrotN)
//                    .IsRequired()
//                    .HasColumnName("NRotN")
//                    .HasMaxLength(40)
//                    .IsUnicode(false);

//                entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
//            });

//            modelBuilder.Entity<TblOrganization>(entity =>
//            {
//                entity.HasKey(e => e.DeptId);

//                entity.ToTable("tblOrganization");

//                entity.Property(e => e.DeptId)
//                    .HasColumnName("DeptID")
//                    .ValueGeneratedNever();

//                entity.Property(e => e.Address)
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.Cabinet)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.DeptName)
//                    .IsRequired()
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.Notes)
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.PhoneNo)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<TblRegistration>(entity =>
//            {
//                entity.HasKey(e => e.RegistrationId);

//                entity.ToTable("tblRegistration");

//                entity.Property(e => e.RegistrationId)
//                    .HasColumnName("RegistrationID")
//                    .ValueGeneratedNever();

//                entity.Property(e => e.Address)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.CaseNumber)
//                    .HasMaxLength(11)
//                    .IsUnicode(false);

//                entity.Property(e => e.DateSsolutions).HasColumnType("datetime");

//                entity.Property(e => e.EvaluationControl)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.EvaluationNotification)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.Flat)
//                    .HasMaxLength(10)
//                    .IsUnicode(false);

//                entity.Property(e => e.Fname)
//                    .IsRequired()
//                    .HasColumnName("FName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.GettingDate).HasColumnType("datetime");

//                entity.Property(e => e.IssueDate).HasColumnType("datetime");

//                entity.Property(e => e.Lname)
//                    .IsRequired()
//                    .HasColumnName("LName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Mname)
//                    .HasColumnName("MName")
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.MustBeReadyDate).HasColumnType("datetime");

//                entity.Property(e => e.Notes)
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.Nprav)
//                    .HasColumnName("NPrav")
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.NumberSolutions)
//                    .HasMaxLength(7)
//                    .IsUnicode(false);

//                entity.Property(e => e.OrderNo).ValueGeneratedOnAdd();

//                entity.Property(e => e.Organiz)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.OutDeptDate).HasColumnType("datetime");

//                entity.Property(e => e.PassIssueDate).HasColumnType("datetime");

//                entity.Property(e => e.PassIssuer)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.PassportNo)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.PersonalNo)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.PhoneNo)
//                    .HasMaxLength(30)
//                    .IsUnicode(false);

//                entity.Property(e => e.PlG).HasColumnType("numeric(5, 2)");

//                entity.Property(e => e.PlO).HasColumnType("numeric(5, 2)");

//                entity.Property(e => e.Proceedings)
//                    .HasMaxLength(500)
//                    .IsUnicode(false);

//                entity.Property(e => e.RegId).HasColumnName("RegID");

//                entity.Property(e => e.ReturnInDeptDate).HasColumnType("datetime");

//                entity.Property(e => e.Rvccontent)
//                    .HasColumnName("RVCContent")
//                    .HasColumnType("image");

//                entity.Property(e => e.StatementForm)
//                    .HasMaxLength(15)
//                    .IsUnicode(false);

// //  // (e => e.TblDocRegistry).
//            });
//        }
//    }
//}
