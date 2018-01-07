using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblLehrerConfig : EntityTypeConfiguration<tblLehrer>
    {
        public tblLehrerConfig()
        {
            ToTable("tblLehrer");

            #region Primary Key

            HasKey(x => x.Kurzzeichen).Property(x => x.Kurzzeichen).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Kurzzeichen).HasColumnName("kurzzeichen").IsRequired().IsUnicode(false);

                 Property(x => x.PersonID).HasColumnName("personID").IsRequired();

                 Property(x => x.Benutzername).HasColumnName("benutzername").IsUnicode(false);

                 Property(x => x.Ordner).HasColumnName("ordner").IsUnicode(false);

                 Property(x => x.Farbe).HasColumnName("farbe");



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblPersonen)
                            .WithMany(x => x.tblLehrer)
                            .HasForeignKey(d => d.PersonID);
            HasMany(x => x.tblLehrfaecher)
                            .WithMany(x => x.tblLehrer)
                            .Map(m =>
                            {
                                m.ToTable("tblLehrkraftLehrfaecher");
                                m.MapLeftKey("lehrfachKurzbezeichnung");
                                m.MapRightKey("lehrerKurzzeichen");
                            });

            #endregion
        }
    }
}