using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblStundenZuordnungConfig : EntityTypeConfiguration<tblStundenZuordnung>
    {
        public tblStundenZuordnungConfig()
        {
            ToTable("tblStundenZuordnung");

            #region Primary Key

            HasKey(x => x.TabelleID).Property(x => x.TabelleID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.DatenID).Property(x => x.DatenID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.TabelleID).HasColumnName("tabelleID").IsRequired();

                 Property(x => x.DatenID).HasColumnName("datenID").IsRequired();



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblStundenDaten)
                            .WithMany(x => x.tblStundenZuordnung)
                            .HasForeignKey(d => d.DatenID);
            HasRequired(x => x.TblStundenTabellen)
                            .WithMany(x => x.tblStundenZuordnung)
                            .HasForeignKey(d => d.TabelleID);

            #endregion
        }
    }
}