using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblTurnusParameterwertezuordnungenConfig : EntityTypeConfiguration<tblTurnusParameterwertezuordnungen>
    {
        public tblTurnusParameterwertezuordnungenConfig()
        {
            ToTable("tblTurnusParameterwertezuordnungen");

            #region Primary Key

            HasKey(x => x.TurnuskonfigurationID).Property(x => x.TurnuskonfigurationID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.TurnusparameterID).Property(x => x.TurnusparameterID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.WertID).Property(x => x.WertID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.TurnuskonfigurationID).HasColumnName("turnuskonfigurationID").IsRequired();

                 Property(x => x.TurnusparameterID).HasColumnName("turnusparameterID").IsRequired();

                 Property(x => x.WertID).HasColumnName("wertID").IsRequired();



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblTurnuskonfigurationen)
                            .WithMany(x => x.tblTurnusParameterwertezuordnungen)
                            .HasForeignKey(d => d.TurnuskonfigurationID);
            HasRequired(x => x.TblTurnusParameter)
                            .WithMany(x => x.tblTurnusParameterwertezuordnungen)
                            .HasForeignKey(d => d.TurnusparameterID);
            HasRequired(x => x.TblTurnusParameterWerte)
                            .WithMany(x => x.tblTurnusParameterwertezuordnungen)
                            .HasForeignKey(d => d.WertID);

            #endregion
        }
    }
}