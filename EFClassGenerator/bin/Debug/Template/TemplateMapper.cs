using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
{5}

namespace {0}
{
    public class {1}Config : EntityTypeConfiguration<{1}>
    {
        public {1}Config()
        {
            ToTable("{1}");

            #region Primary Key

{2}

            #endregion



            #region Direct Properties

{3}

            #endregion



            #region Complex Properties

            #endregion



            #region Lists
{4}
            #endregion
        }
    }
}