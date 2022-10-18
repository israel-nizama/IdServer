﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace IdServer.Infraestructure.Data.Query
{
    public class IdServerQueryContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public IdServerQueryContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("IdServer");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
