using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblStundenTabellenConfig : EntityTypeConfiguration<tblStundenTabellen>
    {
        public tblStundenTabellenConfig()
        {
            ToTable("tblStundenTabellen");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.Bezeichnung).HasColumnName("bezeichnung").IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasMany(x => x.tblStundenDaten)
                            .WithMany(x => x.tblStundenTabellen)
                            .Map(m =>
                            {
                                m.ToTable("tblStundenZuordnung");
                                m.MapLeftKey("datenID");
                                m.MapRightKey("tabelleID");
                            });

            #endregion
        }
    }
}