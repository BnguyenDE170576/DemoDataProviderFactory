using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();

        var strConnection = config["ConnectionStrings:MyStoreDB"];
        return strConnection;
    }
    static void ViewProducts()
    {
        DbProviderFactory factory = SqlClientFactory.Instance;
        using DbConnection connection = factory.CreateConnection();
        if (connection == null)
        {
            Console.WriteLine($"Unable to create the connection object.");
            return;
        }
        connection.ConnectionString = GetConnectionString();
        connection.Open();

        DbCommand command = factory.CreateCommand();
        if (command == null)
        {
            Console.WriteLine($"Unable to create the command object");
            return;
        }
        command.Connection = connection;
        command.CommandText = "Select ProductID,ProductName From Product";

        using DbDataReader dataReader = command.ExecuteReader();
        Console.WriteLine("** Product List **");
        while (dataReader.Read())
        {
            Console.WriteLine($"ProductID: {dataReader["ProductID"]}, " + $"ProductName: {dataReader["ProductName"]}.");
        }
    }
    static void Main(string[] args)
    {
        ViewProducts();
        Console.ReadLine();
    }
}