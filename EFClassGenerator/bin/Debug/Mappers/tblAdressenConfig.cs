using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblAdressenConfig : EntityTypeConfiguration<tblAdressen>
    {
        public tblAdressenConfig()
        {
            ToTable("tblAdressen");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Strasse).HasColumnName("strasse").IsUnicode(false);

                 Property(x => x.Hausnummer).HasColumnName("hausnummer").IsUnicode(false);

                 Property(x => x.Zusatz).HasColumnName("zusatz").IsUnicode(false);

                 Property(x => x.Plz).HasColumnName("plz").IsUnicode(false);

                 Property(x => x.Ort).HasColumnName("ort").IsUnicode(false);

                 Property(x => x.LandID).HasColumnName("landID").IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblLaender)
                            .WithMany(x => x.tblAdressen)
                            .HasForeignKey(d => d.LandID);

            #endregion
        }
    }
}