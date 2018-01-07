using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblKlassenLehrfaecherConfig : EntityTypeConfiguration<tblKlassenLehrfaecher>
    {
        public tblKlassenLehrfaecherConfig()
        {
            ToTable("tblKlassenLehrfaecher");

            #region Primary Key

            HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Id).HasColumnName("id").IsRequired();

                 Property(x => x.KlasseID).HasColumnName("klasseID").IsRequired();

                 Property(x => x.LehrfachKurzzeichen).HasColumnName("lehrfachKurzzeichen").IsRequired().IsUnicode(false);

                 Property(x => x.Tag).HasColumnName("tag");

                 Property(x => x.StundeStart).HasColumnName("stundeStart");

                 Property(x => x.AnzahlStunden).HasColumnName("anzahlStunden");

                 Property(x => x.LehrerKurzzeichen).HasColumnName("lehrerKurzzeichen").IsUnicode(false);

                 Property(x => x.Raumnummer).HasColumnName("raumnummer").IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblLehrer)
                            .WithMany(x => x.tblKlassenLehrfaecher)
                            .HasForeignKey(d => d.LehrerKurzzeichen);
            HasRequired(x => x.TblRaeume)
                            .WithMany(x => x.tblKlassenLehrfaecher)
                            .HasForeignKey(d => d.Raumnummer);
            HasRequired(x => x.TblLehrfaecher)
                            .WithMany(x => x.tblKlassenLehrfaecher)
                            .HasForeignKey(d => d.LehrfachKurzzeichen);
            HasRequired(x => x.TblKlassen)
                            .WithMany(x => x.tblKlassenLehrfaecher)
                            .HasForeignKey(d => d.KlasseID);

            #endregion
        }
    }
}