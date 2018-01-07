using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblRaumLehrfaecherConfig : EntityTypeConfiguration<tblRaumLehrfaecher>
    {
        public tblRaumLehrfaecherConfig()
        {
            ToTable("tblRaumLehrfaecher");

            #region Primary Key

            HasKey(x => x.Raumnummer).Property(x => x.Raumnummer).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.LehrfachKurzbezeichnung).Property(x => x.LehrfachKurzbezeichnung).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Raumnummer).HasColumnName("raumnummer").IsRequired().IsUnicode(false);

                 Property(x => x.LehrfachKurzbezeichnung).HasColumnName("lehrfachKurzbezeichnung").IsRequired().IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblRaeume)
                            .WithMany(x => x.tblRaumLehrfaecher)
                            .HasForeignKey(d => d.Raumnummer);
            HasRequired(x => x.TblLehrfaecher)
                            .WithMany(x => x.tblRaumLehrfaecher)
                            .HasForeignKey(d => d.LehrfachKurzbezeichnung);

            #endregion
        }
    }
}