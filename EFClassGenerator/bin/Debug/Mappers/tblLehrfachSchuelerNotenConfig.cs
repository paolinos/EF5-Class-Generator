using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblLehrfachSchuelerNotenConfig : EntityTypeConfiguration<tblLehrfachSchuelerNoten>
    {
        public tblLehrfachSchuelerNotenConfig()
        {
            ToTable("tblLehrfachSchuelerNoten");

            #region Primary Key

            HasKey(x => x.KlassenLehrfachID).Property(x => x.KlassenLehrfachID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.PersonID).Property(x => x.PersonID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.KlassenLehrfachID).HasColumnName("klassenLehrfachID").IsRequired();

                 Property(x => x.PersonID).HasColumnName("personID").IsRequired();

                 Property(x => x.Note).HasColumnName("note").IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblPersonen)
                            .WithMany(x => x.tblLehrfachSchuelerNoten)
                            .HasForeignKey(d => d.PersonID);
            HasRequired(x => x.TblKlassenLehrfaecher)
                            .WithMany(x => x.tblLehrfachSchuelerNoten)
                            .HasForeignKey(d => d.KlassenLehrfachID);

            #endregion
        }
    }
}