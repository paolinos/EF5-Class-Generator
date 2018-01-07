using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblLehrfaecherConfig : EntityTypeConfiguration<tblLehrfaecher>
    {
        public tblLehrfaecherConfig()
        {
            ToTable("tblLehrfaecher");

            #region Primary Key

            HasKey(x => x.Kurzbezeichnung).Property(x => x.Kurzbezeichnung).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.Kurzbezeichnung).HasColumnName("kurzbezeichnung").IsRequired().IsUnicode(false);

                 Property(x => x.Langbezeichnung).HasColumnName("langbezeichnung").IsUnicode(false);

                 Property(x => x.WertigkeitID).HasColumnName("wertigkeitID").IsRequired();

                 Property(x => x.Kompetenzen).HasColumnName("kompetenzen").IsUnicode(false);

                 Property(x => x.Inhalt).HasColumnName("inhalt").IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblLehrfachwertigkeiten)
                            .WithMany(x => x.tblLehrfaecher)
                            .HasForeignKey(d => d.WertigkeitID);

            #endregion
        }
    }
}