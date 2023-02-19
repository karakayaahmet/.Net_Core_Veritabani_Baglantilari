namespace bolum3;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var products = getAllProducts();

        foreach (var item in products)
        {
            Console.WriteLine($"Product Id : {item.ProductId}, Product Name : {item.Name}, Product Price : {item.Price}");
        }
    }

    static List<Product> getAllProducts(){
    
        List<Product> products = null;
        using(var con = mySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı sağlandı.");

                string sql = "select * from products";

                MySqlCommand command = new MySqlCommand(sql, con);

                MySqlDataReader reader = command.ExecuteReader();

                products = new List<Product>();

                while(reader.Read()){
                    products.Add(new Product{
                        ProductId = int.Parse(reader["id"].ToString()),
                        Name = reader["product_name"].ToString(),
                        Price = double.Parse(reader["list_price"].ToString())
                    });
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

        return products;
    }

    static MySqlConnection mySqlConnection(){
        string connection = @"server=localhost; port=3306; database=Northwind; user=root; password=mysql123;";

        return new MySqlConnection(connection);
    }
}
