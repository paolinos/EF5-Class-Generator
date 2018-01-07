using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WkstStPlan.Mapping;

namespace WkstStPlan.Config
{
    public class tblLehrkraftLehrfaecherConfig : EntityTypeConfiguration<tblLehrkraftLehrfaecher>
    {
        public tblLehrkraftLehrfaecherConfig()
        {
            ToTable("tblLehrkraftLehrfaecher");

            #region Primary Key

            HasKey(x => x.LehrerKurzzeichen).Property(x => x.LehrerKurzzeichen).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.LehrfachKurzbezeichnung).Property(x => x.LehrfachKurzbezeichnung).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            #endregion



            #region Direct Properties

                 Property(x => x.LehrerKurzzeichen).HasColumnName("lehrerKurzzeichen").IsRequired().IsUnicode(false);

                 Property(x => x.LehrfachKurzbezeichnung).HasColumnName("lehrfachKurzbezeichnung").IsRequired().IsUnicode(false);



            #endregion



            #region Complex Properties

            #endregion



            #region Lists
            HasRequired(x => x.TblLehrer)
                            .WithMany(x => x.tblLehrkraftLehrfaecher)
                            .HasForeignKey(d => d.LehrerKurzzeichen);
            HasRequired(x => x.TblLehrfaecher)
                            .WithMany(x => x.tblLehrkraftLehrfaecher)
                            .HasForeignKey(d => d.LehrfachKurzbezeichnung);

            #endregion
        }
    }
}