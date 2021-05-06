using Academia.Interfaces;
using Academia.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Academia.Repository
{
    public class EnderecoClienteRepositorio : IEnderecoClienteRepositorio
    {
        private readonly IConfiguration _configuration;

        private readonly IRepositoryConnection _repositoryConnection;

        private readonly Dictionary<string, string> dados = new Dictionary<string, string>();
        public EnderecoClienteRepositorio(IConfiguration configuration,
                                          IRepositoryConnection repositoryConnection)
        {
            _configuration = configuration;
            _repositoryConnection = repositoryConnection;
        }

        public void AtualizarEnderecoCliente(EnderecoCliente enderecoCliente)
        {
            try
            {
                dados.Add("LogradouroCliente", enderecoCliente.LogradouroCliente);
                dados.Add("@BairroCliente", enderecoCliente.BairroCliente);
                dados.Add("IdCliente", enderecoCliente.IdCliente.ToString());
                _repositoryConnection.CommandExecucaoSimples("AtualizaEnderecoCliente", dados);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EnderecoCliente BuscarEnderecoPorIdCliente(int idCliente)
        {
            try
            {
                EnderecoCliente enderecoCliente = null;

                dados.Add("@IdCliente", idCliente.ToString());

                var leitura = _repositoryConnection.CommandBusca("BuscaEnderecoPorIdCliente", dados);

                //DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(leitura);

                //while (leitura.Read())
                foreach (DataRow row in leitura.Rows)
                {
                    enderecoCliente = new EnderecoCliente();

                    enderecoCliente.IdEnderecoCliente = Convert.ToInt32(row["IdEnderecoCliente"]);
                    enderecoCliente.LogradouroCliente = row["LogradouroCliente"].ToString();
                    enderecoCliente.BairroCliente = row["BairroCliente"].ToString();
                }

                //leitura.Close();
                //leitura.Dispose();

                return enderecoCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InserirEnderecoCliente(EnderecoCliente enderecoCliente)
        {
            try
            {
                dados.Add("@LogradouroCliente", enderecoCliente.LogradouroCliente);
                dados.Add("@BairroCliente", enderecoCliente.BairroCliente);
                dados.Add("IdCliente", enderecoCliente.IdCliente.ToString());

                _repositoryConnection.CommandExecucaoSimples("InsereEnderecoCliente", dados);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
