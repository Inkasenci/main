using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace SZI
{
    public static class ExceptionHandling
    {
        private static void PrimaryKeyViolation()
        {
            MessageBox.Show("Istnieje już wpis z takim kluczem głównym", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private static void OtherException(string Message)
        {
            MessageBox.Show("Błąd wprowadzania danych\n" + Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private static void SpecificException(int Number, string Message)
        {
            switch (Number)
            {
                case (2601):
                    PrimaryKeyViolation();
                    break;

                case (2627):
                    PrimaryKeyViolation();
                    break;

                default:
                    OtherException(Message);
                    break;
            }
        }

        public static void ShowException(DbUpdateException Ex)
        {
            var innerEx = Ex.InnerException;

            while (innerEx.InnerException != null)
                innerEx = innerEx.InnerException;

            SqlException ProjectedException = (SqlException)innerEx;

            ExceptionHandling.SpecificException(ProjectedException.Number, ProjectedException.Message);
        }

        public static void ShowException(System.ComponentModel.DataAnnotations.ValidationException Ex)
        {
            var innerEx = Ex.InnerException;

            while (innerEx.InnerException != null)
                innerEx = innerEx.InnerException;

            SqlException ProjectedException = (SqlException)innerEx;

            ExceptionHandling.SpecificException(ProjectedException.Number, ProjectedException.Message);
        }
       
    }
}
