namespace bolum2;
using MySql.Data.MySqlClient;
using System;
class Program
{
    static void Main(string[] args)
    {
        getAllProducts();
    }

    static void getAllProducts(){
        using(var con = GetMySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı Sağlandı.");

                string sql = "select * from products";

                MySqlCommand command = new MySqlCommand(sql, con);

                MySqlDataReader reader = command.ExecuteReader();

                while(reader.Read()){
                    Console.WriteLine($"name:{reader[3]} , price: {reader[6]}");
                } 
                
                reader.Close();
            }

            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            finally{
                con.Close();
            }
        }
    }

    static MySqlConnection GetMySqlConnection(){
        string connection = @"server=localhost; port=3306; database=Northwind; user=root; password=mysql123";
        return new MySqlConnection(connection);
    }
}
