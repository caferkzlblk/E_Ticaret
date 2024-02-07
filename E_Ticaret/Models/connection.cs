using Microsoft.Data.SqlClient;

namespace E_Ticaret.Models
{
    public class connection
    {
        public static SqlConnection baglanti 
        {
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=CAFERMATEBOOK; Database=ETicaret; Trusted_Connection=True; TrustServerCertificate=True;");
                return sqlcon;
            }

        
        }
    }
}
