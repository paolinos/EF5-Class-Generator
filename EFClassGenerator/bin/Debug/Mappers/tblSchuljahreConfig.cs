using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblSchuljahreConfig : EntityTypeConfiguration<tblSchuljahre>
    {
        public tblSchuljahreConfig()
        {
            ToTable("tblSchuljahre");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Bezeichnung).HasColumnName("bezeichnung").IsRequired().IsUnicode(false);

                 Property(x => x.Start).HasColumnName("start").IsRequired();

                 Property(x => x.Ende).HasColumnName("ende").IsRequired();

                 Property(x => x.StartVersetzt).HasColumnName("startVersetzt");

                 Property(x => x.EndeVersetzt).HasColumnName("endeVersetzt");



            #endregion



            #region Complex Properties

            #endregion



            #region Lists

            #endregion
        }
    }
}