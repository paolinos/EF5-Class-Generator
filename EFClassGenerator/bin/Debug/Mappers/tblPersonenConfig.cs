using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblPersonenConfig : EntityTypeConfiguration<tblPersonen>
    {
        public tblPersonenConfig()
        {
            ToTable("tblPersonen");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Vorname).HasColumnName("vorname").IsUnicode(false);

                 Property(x => x.Nachname).HasColumnName("nachname").IsRequired().IsUnicode(false);

                 Property(x => x.AdresseID).HasColumnName("adresseID");

                 Property(x => x.Tel1).HasColumnName("tel1").IsUnicode(false);

                 Property(x => x.Tel2).HasColumnName("tel2").IsUnicode(false);

                 Property(x => x.Email).HasColumnName("email").IsUnicode(false);

                 Property(x => x.Geburtstag).HasColumnName("geburtstag");

                 Property(x => x.Eintritt).HasColumnName("eintritt");

                 Property(x => x.Austritt).HasColumnName("austritt");



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblAdressen)
                            .WithMany(x => x.tblPersonen)
                            .HasForeignKey(d => d.AdresseID);

            #endregion
        }
    }
}