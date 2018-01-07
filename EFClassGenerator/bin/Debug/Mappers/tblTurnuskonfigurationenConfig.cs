using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblTurnuskonfigurationenConfig : EntityTypeConfiguration<tblTurnuskonfigurationen>
    {
        public tblTurnuskonfigurationenConfig()
        {
            ToTable("tblTurnuskonfigurationen");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Bezeichnung).HasColumnName("bezeichnung").IsUnicode(false);

                 Property(x => x.BerechnungsDLL).HasColumnName("berechnungsDLL").IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists

            #endregion
        }
    }
}