using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace bolum1;
class Program
{
    static void Main(string[] args)
    {
        getSqlConnection();
        getMySqlConnection();
    }

    static void getSqlConnection(){
        string sqlconnection = @"Data Source=\Sqlexpress; Initial Catalog=Northwind; Integrated Security=SSPI;";

        using(var connection = new SqlConnection(sqlconnection)){

            try{
                connection.Open();
                Console.WriteLine("Bağlantı Sağlandı");
            }

            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            finally{
                connection.Close();
            }
        }
    }

    static void getMySqlConnection(){
        string mysqlconnection = @"server=localhost; port=3306; database=Northwind; user=root; password=12345;";

        using(var connection = new MySqlConnection(mysqlconnection) ){
            try{
                connection.Open();
                Console.WriteLine("Bağlantı sağlandı.");
            }

            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            finally{
                connection.Close();
            }
        }
    }
}
