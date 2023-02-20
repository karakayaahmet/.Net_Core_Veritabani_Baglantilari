namespace bolum4;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

interface IProductDal{
    List<Products> GetProducts();

    Products GetProductById(int id);

    void Create(Products p);

    void Update(Products p);

    void Delete(int productId);
}

class MySqlProductDal : IProductDal
{
    private MySqlConnection getMySqlConnection(){
        string connection = @"server=localhost; port=3306; database=Northwind; user=root; password=mysql123;";

        return new MySqlConnection(connection);
    }
    public void Create(Products p)
    {
        
    }

    public void Delete(int productId)
    {
        
    }

    public Products GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Products> GetProducts()
    {
        List<Products> products = null;
        using(var con = getMySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı Sağlandı.");

                string sql = "select * from products";

                MySqlCommand command = new MySqlCommand(sql, con);

                MySqlDataReader reader = command.ExecuteReader();

                products = new List<Products>();

                while(reader.Read()){
                    products.Add(new Products{
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

    public void Update(Products p)
    {
        
    }
}
class Program
{
    static void Main(string[] args)
    {
        var productdal = new MySqlProductDal();

        var products = productdal.GetProducts();

        foreach (var item in products)
        {
            Console.WriteLine($"Ürün id : {item.ProductId}, Ürün Ad : {item.Name}, Ürün Fiyat : {item.Price}");
        }
    }
}
