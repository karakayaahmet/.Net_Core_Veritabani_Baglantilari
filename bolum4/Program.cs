namespace bolum4;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

interface IProductDal{
    List<Products> GetProducts();

    Products GetProductById(int id);

    List<Products> Find(string productName);

    void Create(Products p);

    int Update(Products p);

    int Delete(int productId);

    int Total();
}

class MySqlProductDal : IProductDal
{
    private MySqlConnection getMySqlConnection(){
        string connection = @"server=localhost; port=3306; database=Northwind; user=root; password=mysql123;";

        return new MySqlConnection(connection);
    }
    public void Create(Products p)
    {
        using(var con = getMySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı Sağlandı.");

                string sql = "insert into products (product_name, discontinued, list_price) values(@product_name, @discontinued, @list_price)";

                MySqlCommand command = new MySqlCommand(sql, con);

                command.Parameters.AddWithValue("@product_name",p.Name);
                command.Parameters.AddWithValue("@discontinued",1);
                command.Parameters.AddWithValue("@list_price",p.Price);

                int result = command.ExecuteNonQuery();

                Console.WriteLine($"{result} adet kayıt eklendi.");
            }

            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            finally{
                con.Close();
            }
        }
        
    }

    public int Delete(int productId)
    {
        int result = 0;

        using(var con = getMySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı Sağlandı.");

                string sql = "delete from products where id = @id";

                MySqlCommand command = new MySqlCommand(sql, con);

                command.Parameters.Add("@id", MySqlDbType.Int32).Value = productId;

                result = command.ExecuteNonQuery();
            }

            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            finally{
                con.Close();
            }
        }

        return result;
    }

    public Products GetProductById(int id)
    {
        Products product = null;

        using(var con = getMySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı Sağlandı.");

                string sql = "select * from products where id = @id";

                MySqlCommand command = new MySqlCommand(sql, con);

                command.Parameters.Add("@id", MySqlDbType.Int32).Value=id;

                MySqlDataReader reader = command.ExecuteReader();

                reader.Read();

                if (reader.HasRows){
                    product = new Products(){
                        ProductId = int.Parse(reader["id"].ToString()),
                        Name = reader["product_name"].ToString(),
                        Price = double.Parse(reader["list_price"].ToString())
                    };

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

        return product;

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

    public int Update(Products p)
    {
        int result = 0;

        using(var con = getMySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı Sağlandı.");

                string sql = "update products set product_name=@product_name, list_price=@list_price where id = @id";

                MySqlCommand command = new MySqlCommand(sql, con);

                command.Parameters.AddWithValue("@product_name",p.Name);
                command.Parameters.AddWithValue("@list_price",p.Price);
                command.Parameters.AddWithValue("@id",p.ProductId);

                result = command.ExecuteNonQuery();

            }

            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            finally{
                con.Close();
            }
        }

        return result;
    }

    public List<Products> Find(string productName)
    {
        List<Products> products = null;

        using(var con = getMySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı Sağlandı.");

                string sql = "select * from products where product_name like @productName";

                MySqlCommand command = new MySqlCommand(sql, con);

                command.Parameters.Add("@productName", MySqlDbType.String).Value = "%"+productName+"%";

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

    public int Total()
    {
        int toplam = 0;

        using(var con = getMySqlConnection()){
            try{
                con.Open();
                Console.WriteLine("Bağlantı Sağlandı.");

                string sql = "select count(*) from products";

                MySqlCommand command = new MySqlCommand(sql, con);

                object result = command.ExecuteScalar();

                if (result is not null){
                    toplam = Convert.ToInt32(result);
                }
            }

            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            finally{
                con.Close();
            }
        }

        return toplam;
    }
}
class Program
{
    static void Main(string[] args)
    {

        //*************************************************
        var productdal = new MySqlProductDal();

        var products = productdal.GetProducts();

        foreach (var item in products)
        {
            Console.WriteLine($"Ürün id : {item.ProductId}, Ürün Ad : {item.Name}, Ürün Fiyat : {item.Price}");
        }

        //*********************************************

        var product = productdal.GetProductById(3);

        Console.WriteLine($"Product id : {product.ProductId}, Product Name : {product.Name}, Product Price : {product.Price}");

        //********************************************************

        var new_products = new MySqlProductDal();

        var urunler = new_products.Find("Northwind");

        foreach (var item in urunler)
        {
            Console.WriteLine($"ürün ad : {item.Name}, ürün id : {item.ProductId}, ürün fiyat : {item.Price}");
        }

        //********************************************************

        var total = new MySqlProductDal();

        var urun_toplam = total.Total();

        Console.WriteLine($"Toplam Ürün Sayısı : {urun_toplam}");

        //*******************************************************

        var create = new MySqlProductDal();

        Products p = new Products(){
            Name = "Samsung S8",
            Price = 4000
        };

        create.Create(p);  

        //*********************************************************

        var update = new MySqlProductDal();

        Products u = new Products(){
            ProductId = 100,
            Name = "Samsung S7",
            Price = 3000
        };

        var sonuc = update.Update(u);

        Console.WriteLine($"{sonuc} adet kayıt güncellenmiştir.");

        //*********************************************************

        var delete = new MySqlProductDal();

        int silinen = delete.Delete(101);

        Console.WriteLine($"{silinen} adet kayıt silinmiştir.");

    }
}
