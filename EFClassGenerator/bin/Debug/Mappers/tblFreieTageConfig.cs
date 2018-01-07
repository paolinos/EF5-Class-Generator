using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblFreieTageConfig : EntityTypeConfiguration<tblFreieTage>
    {
        public tblFreieTageConfig()
        {
            ToTable("tblFreieTage");

            #region Primary Key

            HasKey(x => x.SchuljahrID).Property(x => x.SchuljahrID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.SchuljahrID).HasColumnName("schuljahrID").IsRequired();

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Bezeichnung).HasColumnName("bezeichnung").IsUnicode(false);

                 Property(x => x.Anfang).HasColumnName("anfang").IsRequired();

                 Property(x => x.Ende).HasColumnName("ende").IsRequired();



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblSchuljahre)
                            .WithMany(x => x.tblFreieTage)
                            .HasForeignKey(d => d.SchuljahrID);

            #endregion
        }
    }
}