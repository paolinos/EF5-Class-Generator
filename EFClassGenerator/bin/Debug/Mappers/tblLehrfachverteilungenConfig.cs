using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblLehrfachverteilungenConfig : EntityTypeConfiguration<tblLehrfachverteilungen>
    {
        public tblLehrfachverteilungenConfig()
        {
            ToTable("tblLehrfachverteilungen");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.MusterId).HasColumnName("musterId").IsRequired();

                 Property(x => x.LehrfachKurzbezeichnung).HasColumnName("lehrfachKurzbezeichnung").IsRequired().IsUnicode(false);

                 Property(x => x.AnzahlStunden).HasColumnName("anzahlStunden");



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblLehrfaecher)
                            .WithMany(x => x.tblLehrfachverteilungen)
                            .HasForeignKey(d => d.LehrfachKurzbezeichnung);
            HasRequired(x => x.TblLehrfachverteilungsmuster)
                            .WithMany(x => x.tblLehrfachverteilungen)
                            .HasForeignKey(d => d.MusterId);

            #endregion
        }
    }
}