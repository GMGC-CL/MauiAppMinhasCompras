using MauiAppMinhasCompras.Models;
using SQLite;
using System.Diagnostics;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path) 
        { 
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p) 
        {
            return _conn.InsertAsync(p);
        }

         public Task<int> Update(Produto p)
        {         
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, DataCompra=?, Comprado=?, Categoria=? WHERE Id=?";
            return _conn.ExecuteAsync(sql, p.Descricao, p.Quantidade, p.Preco, p.DataCompra, p.Comprado, p.Categoria, p.Id);
        }

        public Task<int> Delete(int id) 
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll() 
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q) 
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

        public Task<List<Produto>> GetByPeriodo(DateTime inicio, DateTime fim, bool? comprado = null)
        {
            string sql = @"
        SELECT * FROM Produto 
        WHERE DataCompra IS NOT NULL 
        AND DataCompra BETWEEN ? AND ?";

            var parametros = new List<object> { inicio, fim };

            if (comprado.HasValue)
            {
                sql += " AND Comprado = ?";
                parametros.Add(comprado.Value ? 1 : 0);
            }

            return _conn.QueryAsync<Produto>(sql, parametros.ToArray());
        }

        public Task<List<Produto>> GetProdutosFiltrados(string categoria, bool? comprado, bool? ncomprado, DateTime inicio, DateTime fim)
        {
            string sql = "SELECT * FROM Produto WHERE 1=1";

            var parametros = new List<object>();

            if (!string.IsNullOrEmpty(categoria))
            {
                sql += " AND Categoria = ?";
                parametros.Add(categoria);
            }

            if (comprado == true)
            {
                sql += " AND Comprado = ?";
                parametros.Add(1);
            }
            else if (ncomprado == true)
            {
                sql += " AND Comprado = ?";
                parametros.Add(0);
            }
            else
            {
            }

            if (inicio != null && fim != null)
            {
                if (comprado == true)
                {
                    sql += " AND DataCompra >= ? AND DataCompra <= ?";
                    parametros.Add(inicio);
                    parametros.Add(fim);
                }
                else if (ncomprado == true)
                {
                    sql += " AND DataCompra IS NULL";
                    parametros.Add(inicio);
                    parametros.Add(fim);
                }
                else 
                {
                    sql += " AND (DataCompra >= ? AND DataCompra <= ? OR DataCompra IS NULL)";
                    parametros.Add(inicio);
                    parametros.Add(fim);
                }
            }

            Debug.WriteLine($"Consulta SQL: {sql}");
            Debug.WriteLine($"Parâmetros: {string.Join(", ", parametros)}");

            return _conn.QueryAsync<Produto>(sql, parametros.ToArray());

        }
    }
}
