using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Automaty_z_napojami
{
    public class DatabaseService
    {
        public string connectionString;

        public DatabaseService(string serwer, string baza)
        {
            connectionString = "Server=" + serwer + ";Database=" + baza + ";Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public bool TestPolaczenia()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch { return false; }
        }

        public List<Napoj> PobierzNapoje()
        {
            List<Napoj> lista = new List<Napoj>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT d.DrinkID, d.DrinkName, d.Price, d.Volume_ml, c.CategoryName " +
                             "FROM Drinks d JOIN Categories c ON d.CategoryID = c.CategoryID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    Napoj n = new Napoj();
                    n.Id = (int)r["DrinkID"];
                    n.Nazwa = r["DrinkName"].ToString() ?? "";
                    n.Cena = (decimal)r["Price"];
                    n.Pojemnosc = (int)r["Volume_ml"];
                    n.Kategoria = r["CategoryName"].ToString() ?? "";
                    lista.Add(n);
                }
            }
            return lista;
        }

        public void DodajNapoj(string n, int k, decimal c, int p)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Drinks (DrinkName, CategoryID, Price, Volume_ml) VALUES (@n, @k, @c, @p)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@n", n);
                cmd.Parameters.AddWithValue("@k", k);
                cmd.Parameters.AddWithValue("@c", c);
                cmd.Parameters.AddWithValue("@p", p);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UsunNapoj(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Drinks WHERE DrinkID = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ZapiszTransakcje(int drinkId, string metoda)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Transactions (DrinkID, ClientID, TechnicianID, LocationID, payment_method) VALUES (@id, 1, 1, 1, @met)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", drinkId);
                cmd.Parameters.AddWithValue("@met", metoda);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
   