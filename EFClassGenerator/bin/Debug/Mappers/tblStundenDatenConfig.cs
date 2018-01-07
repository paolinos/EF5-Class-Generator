using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblStundenDatenConfig : EntityTypeConfiguration<tblStundenDaten>
    {
        public tblStundenDatenConfig()
        {
            ToTable("tblStundenDaten");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Start).HasColumnName("start").IsRequired();

                 Property(x => x.Dauer).HasColumnName("dauer").IsRequired();



            #endregion



            #region Complex Properties

            #endregion



            #region Lists

            #endregion
        }
    }
}