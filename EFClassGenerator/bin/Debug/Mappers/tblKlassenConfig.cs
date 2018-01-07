using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblKlassenConfig : EntityTypeConfiguration<tblKlassen>
    {
        public tblKlassenConfig()
        {
            ToTable("tblKlassen");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.SchuljahrID).HasColumnName("schuljahrID").IsRequired();

                 Property(x => x.Bezeichnung).HasColumnName("bezeichnung").IsRequired().IsUnicode(false);

                 Property(x => x.Fachbereich).HasColumnName("fachbereich").IsUnicode(false);

                 Property(x => x.Jahrgang).HasColumnName("jahrgang");

                 Property(x => x.LehrerKurzzeichen).HasColumnName("lehrerKurzzeichen").IsUnicode(false);

                 Property(x => x.TurnusKonfigurationID).HasColumnName("turnusKonfigurationID");

                 Property(x => x.Gruppen).HasColumnName("gruppen").IsRequired();



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblLehrer)
                            .WithMany(x => x.tblKlassen)
                            .HasForeignKey(d => d.LehrerKurzzeichen);
            HasRequired(x => x.TblSchuljahre)
                            .WithMany(x => x.tblKlassen)
                            .HasForeignKey(d => d.SchuljahrID);
            HasRequired(x => x.TblTurnuskonfigurationen)
                            .WithMany(x => x.tblKlassen)
                            .HasForeignKey(d => d.TurnusKonfigurationID);

            #endregion
        }
    }
}