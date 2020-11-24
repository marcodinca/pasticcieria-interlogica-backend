using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using PasticceriaInterlogicaBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PasticceriaInterlogicaBackend.DataAccess
{
    public static class ProdottiDataAccess
    {
        public static string ConnectionString = Startup.StaticConfig.GetValue<string>("ConnectionStrings:DbConnection");

        public static IEnumerable<Prodotto> GetProdotti()
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                IEnumerable<Prodotto> Prodotti = Connection.GetAll<Prodotto>();

                foreach(Prodotto P in Prodotti)
                {
                    string Query = @"SELECT Id, Nome, Um, Quantita
                                     FROM Ingredienti i
                                     JOIN Ricette r ON (i.Id = r.IdIngrediente AND r.IdProdotto = @idProdotto)";

                    P.Ingredienti = Connection.Query<Ingrediente>(Query, new { idProdotto = P.Id });
                }

                return Prodotti;
            }
        }

        public static void PutProdotto(Prodotto newProdotto)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlTransaction Transaction = Connection.BeginTransaction();
                try
                {
                    string QueryDelete = "DELETE FROM Ricette WHERE IdProdotto = @idProdotto";
                    Transaction.Connection.Execute(QueryDelete, new { idProdotto = newProdotto.Id }, Transaction);

                    Transaction.Connection.Update<Prodotto>(newProdotto, Transaction);
                    
                    foreach(Ingrediente i in newProdotto.Ingredienti)
                    {
                        Ricetta newRicetta = new Ricetta()
                        {
                            IdIngrediente = i.Id,
                            IdProdotto = newProdotto.Id,
                            Quantita = i.Quantita
                        };

                        Transaction.Connection.Insert<Ricetta>(newRicetta, Transaction);
                    }

                    Transaction.Commit();
                } catch(Exception e)
                {
                    Transaction.Rollback();
                    throw e;
                }
            }
        }

        public static void PostProdotto(Prodotto newProdotto, IEnumerable<Ricetta> newRicette)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlTransaction Transaction = Connection.BeginTransaction();

                try
                {
                    int id = (int)Transaction.Connection.Insert<Prodotto>(newProdotto, Transaction);

                    foreach (Ricetta r in newRicette)
                    {
                        r.IdProdotto = id;
                        Transaction.Connection.Insert<Ricetta>(r, Transaction);
                    }

                    Transaction.Commit();
                } catch(Exception e)
                {
                    Transaction.Rollback();
                    throw e;
                }
            }
        }

        public static void DeleteProdotto(int id)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlTransaction Transaction = Connection.BeginTransaction();

                try
                {
                    string Query = "DELETE FROM Ricette WHERE IdProdotto = @idProdotto";
                    Transaction.Connection.Execute(Query, new { idProdotto = id }, Transaction);

                    Prodotto prodotto = Transaction.Connection.Get<Prodotto>(id, Transaction);
                    Transaction.Connection.Delete<Prodotto>(prodotto, Transaction);

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
