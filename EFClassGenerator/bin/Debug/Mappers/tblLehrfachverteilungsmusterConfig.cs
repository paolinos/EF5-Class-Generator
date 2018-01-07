using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblLehrfachverteilungsmusterConfig : EntityTypeConfiguration<tblLehrfachverteilungsmuster>
    {
        public tblLehrfachverteilungsmusterConfig()
        {
            ToTable("tblLehrfachverteilungsmuster");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Fachbereich).HasColumnName("fachbereich").IsUnicode(false);

                 Property(x => x.Jahrgang).HasColumnName("jahrgang");



            #endregion



            #region Complex Properties

            #endregion



            #region Lists

            #endregion
        }
    }
}