using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblKlassenSchuelerConfig : EntityTypeConfiguration<tblKlassenSchueler>
    {
        public tblKlassenSchuelerConfig()
        {
            ToTable("tblKlassenSchueler");

            #region Primary Key

            HasKey(x => x.KlasseID).Property(x => x.KlasseID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.PersonID).Property(x => x.PersonID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.KlasseID).HasColumnName("klasseID").IsRequired();

                 Property(x => x.PersonID).HasColumnName("personID").IsRequired();

                 Property(x => x.Gruppe).HasColumnName("Gruppe");



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblPersonen)
                            .WithMany(x => x.tblKlassenSchueler)
                            .HasForeignKey(d => d.PersonID);
            HasRequired(x => x.TblKlassen)
                            .WithMany(x => x.tblKlassenSchueler)
                            .HasForeignKey(d => d.KlasseID);

            #endregion
        }
    }
}