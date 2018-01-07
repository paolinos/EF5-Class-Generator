using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblLehrfachwertigkeitenConfig : EntityTypeConfiguration<tblLehrfachwertigkeiten>
    {
        public tblLehrfachwertigkeitenConfig()
        {
            ToTable("tblLehrfachwertigkeiten");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Bezeichnung).HasColumnName("bezeichnung").IsRequired().IsUnicode(false);

                 Property(x => x.Werteinheit).HasColumnName("werteinheit").IsRequired();



            #endregion



            #region Complex Properties

            #endregion



            #region Lists

            #endregion
        }
    }
}