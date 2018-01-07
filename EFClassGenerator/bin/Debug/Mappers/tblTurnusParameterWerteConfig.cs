using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblTurnusParameterWerteConfig : EntityTypeConfiguration<tblTurnusParameterWerte>
    {
        public tblTurnusParameterWerteConfig()
        {
            ToTable("tblTurnusParameterWerte");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Wert).HasColumnName("wert").IsRequired().IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists

            #endregion
        }
    }
}