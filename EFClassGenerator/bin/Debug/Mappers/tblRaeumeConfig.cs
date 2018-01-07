using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblRaeumeConfig : EntityTypeConfiguration<tblRaeume>
    {
        public tblRaeumeConfig()
        {
            ToTable("tblRaeume");

            #region Primary Key

            HasKey(x => x.Raumnummer).Property(x => x.Raumnummer).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Raumnummer).HasColumnName("raumnummer").IsRequired().IsUnicode(false);

                 Property(x => x.BezeichnungKurz).HasColumnName("bezeichnungKurz").IsRequired().IsUnicode(false);

                 Property(x => x.BezeichnungLang).HasColumnName("bezeichnungLang").IsUnicode(false);

                 Property(x => x.Kustos1).HasColumnName("kustos1").IsUnicode(false);

                 Property(x => x.Kustos2).HasColumnName("kustos2").IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblLehrer)
                            .WithMany(x => x.tblRaeume)
                            .HasForeignKey(d => d.Kustos1);
            HasRequired(x => x.TblLehrer)
                            .WithMany(x => x.tblRaeume)
                            .HasForeignKey(d => d.Kustos2);
            HasMany(x => x.tblLehrfaecher)
                            .WithMany(x => x.tblRaeume)
                            .Map(m =>
                            {
                                m.ToTable("tblRaumLehrfaecher");
                                m.MapLeftKey("lehrfachKurzbezeichnung");
                                m.MapRightKey("raumnummer");
                            });

            #endregion
        }
    }
}