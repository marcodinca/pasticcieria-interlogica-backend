using Microsoft.Extensions.Configuration;
using PasticceriaInterlogicaBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace PasticceriaInterlogicaBackend.DataAccess
{
    public static class IngredientiDataAccess
    {
        public static string ConnectionString = Startup.StaticConfig.GetValue<string>("ConnectionStrings:DbConnection");

        public static IEnumerable<Ingrediente> GetIngredienti()
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                return Connection.GetAll<Ingrediente>();
            }
        }

        public static void PutIngrediente(Ingrediente newIngrediente)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Update<Ingrediente>(newIngrediente);
            }
        }

        public static void PostIngrediente(Ingrediente newIngrediente)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Insert<Ingrediente>(newIngrediente);
            }
        }

        public static void DeleteIngrediente(int id)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlTransaction Transaction = Connection.BeginTransaction();

                try
                {
                    string Query = "DELETE FROM Ricette WHERE IdIngrediente = @idIngrediente";
                    Transaction.Connection.Execute(Query, new { idIngrediente = id }, Transaction);

                    Ingrediente ingrediente = Transaction.Connection.Get<Ingrediente>(id, Transaction);
                    Transaction.Connection.Delete<Ingrediente>(ingrediente, Transaction);

                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    throw e;
                }
            }
        }
    }
}
