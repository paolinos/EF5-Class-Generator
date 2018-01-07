using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Namespace.Mapping;

namespace Namespace.Config
{
    public class tblSchuelerConfig : EntityTypeConfiguration<tblSchueler>
    {
        public tblSchuelerConfig()
        {
            ToTable("tblSchueler");

            #region Primary Key

            HasKey(x => x.Kennzahl).Property(x => x.Kennzahl).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Kennzahl).HasColumnName("kennzahl").IsRequired().IsUnicode(false);

                 Property(x => x.PersonID).HasColumnName("personID").IsRequired();



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblPersonen)
                            .WithMany(x => x.tblSchueler)
                            .HasForeignKey(d => d.PersonID);

            #endregion
        }
    }
}