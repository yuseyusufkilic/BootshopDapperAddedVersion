using bootShop.DataAccess.Data;
using bootShop.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootShop.DataAccess.Repositories
{
    public class DPProductRepository : IProductRepository
    {
        private IDbConnection db;

        public DPProductRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("db"));
        }
        public async Task<int> Add(Product entity)
        {
            var sql = "INSERT INTO Products (Name,Descriptipn,ImageUrl,CategoryId) VALUES (@Name,@Descriptipn,@ImageUrl,@CategoryId);" +
                "SELECT CAST (SCOPE_IDENTITY() as int);";

            var parameters = new DynamicParameters();
            parameters.Add("Name", entity.Name, DbType.String);
            parameters.Add("Descriptipn", entity.Descriptipn, DbType.String);
            parameters.Add("ImageUrl", entity.ImageUrl, DbType.String);
            parameters.Add("CategoryId", entity.CategoryId, DbType.Int32);

            var r = await db.ExecuteAsync(sql, parameters);

            return r;
        }

        public async  Task Delete(int id)
        {
            var sql = "DELETE FROM Products WHERE Id=@Id";
            await db.ExecuteAsync(sql, new { Id = id });

        }

        public async Task<IList<Product>> GetAllEntities()
        {
            var sql = "SELECT * FROM Products";
            var pr = await db.QueryAsync<Product>(sql);

            return pr.ToList();

        }

        public async Task<Product> GetEntityById(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id =@Id";
            var pr = await db.QuerySingleAsync<Product>(sql, new { Id = id });
            return pr;
        }

        public async Task<IList<Product>> SearchProductsByName(string name)
        {
            var sql = "SELECT * FROM Products WHERE Name =@Name";
            var pr = await db.QuerySingleAsync<Product>(sql, new { Name = name });

            return (IList<Product>)pr;
        }

        public async Task<int> Update(Product entity)
        {
            var sql = "UPDATE Products SET Name=@Name,Descriptipn=@Descriptipn,ImageUrl=@ImageUrl,CategoryId=@CategoryId WHERE Id=@Id";

            var parameters = new DynamicParameters();
            parameters.Add("Name", entity.Name, DbType.String);
            parameters.Add("Descriptipn", entity.Descriptipn, DbType.String);
            parameters.Add("ImageUrl", entity.ImageUrl, DbType.String);
            parameters.Add("CategoryId", entity.CategoryId, DbType.Object);

            var r = await db.ExecuteAsync(sql, parameters);

            return r;

        }
    }
}
