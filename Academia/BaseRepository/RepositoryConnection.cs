using Academia.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Academia.BaseRepository
{
    public class RepositoryConnection : IRepositoryConnection
    {
        private readonly IConfiguration _configuration;

        public RepositoryConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection Conexao()
        {
            SqlConnection _conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            return _conn;
        }

        public DataTable CommandBusca(string nomeProcedure, Dictionary<string, string> Parametros)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = nomeProcedure;

                foreach (var val in Parametros)
                {
                    cmd.Parameters.AddWithValue(val.Key, val.Value);
                }

                cmd.Connection = Conexao();
                cmd.Connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                var dataTable = new DataTable();
                dataTable.Load(dataReader);

                //return JsonConvert.SerializeObject(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CommandInserir(string nomeProcedure, Dictionary<string, string> Parametros)
        {
            int IdCliente = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = nomeProcedure;

                    foreach (var val in Parametros)
                    {
                        cmd.Parameters.AddWithValue(val.Key, val.Value);
                    }

                    cmd.Connection = Conexao();
                    cmd.Connection.Open();

                    IdCliente = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd.Connection.Close();
                    cmd.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IdCliente;
        }

        public void CommandExecucaoSimples(string nomeProcedure, Dictionary<string, string> Parametros)
        {
            //int IdCliente = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = nomeProcedure;

                    foreach (var val in Parametros)
                    {
                        cmd.Parameters.AddWithValue(val.Key, val.Value);
                    }

                    cmd.Connection = Conexao();
                    cmd.Connection.Open();

                    cmd.ExecuteNonQuery();

                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return IdCliente;
        }
    }
}

