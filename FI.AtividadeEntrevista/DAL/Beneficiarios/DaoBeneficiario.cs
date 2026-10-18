using System.Collections.Generic;
using System.Data;

namespace FI.AtividadeEntrevista.DAL
{
    internal class DaoBeneficiario : AcessoDados
    {

            /// <summary>
            /// Inclui um novo beneficiário
            /// </summary>
            /// <param name="beneficiario"></param>
            /// <returns>Id do beneficiário incluído</returns>
            public long Incluir(DML.Beneficiario beneficiario)
            {
                List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

                parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
                parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
                parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", beneficiario.IdCliente));

                DataSet ds = base.Consultar("FI_SP_IncBenef", parametros); // conferir
                long ret = 0;
                if (ds.Tables[0].Rows.Count > 0)
                    long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
                return ret;
            }

            /// <summary>
            /// Altera um beneficiário
            /// </summary>
            /// <param name="beneficiario">Objeto de beneficiário</param>
            internal void Alterar(DML.Beneficiario beneficiario)
            {
                List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

                parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
                parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
                parametros.Add(new System.Data.SqlClient.SqlParameter("Id", beneficiario.Id));

                base.Executar("FI_SP_AltBenef", parametros);
            }

            /// <summary>
            /// Exclui um beneficiário
            /// </summary>
            /// <param name="id">Id do beneficiário</param>
            internal void Excluir(long id)
            {
                List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

                parametros.Add(new System.Data.SqlClient.SqlParameter("Id", id));

                base.Executar("FI_SP_DelBenef", parametros); // conferir
            }

            /// <summary>
            /// Lista os beneficiários de um cliente
            /// </summary>
            /// <param name="idCliente">Id do cliente</param>
            /// <returns>Lista de beneficiários</returns>
            internal List<DML.Beneficiario> Listar(long idCliente)
            {
                List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

                parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", idCliente));

                DataSet ds = base.Consultar("FI_SP_ConsBenef", parametros); // conferir
                return Converter(ds);
            }

            /// <summary>
            /// Verifica se já existe outro beneficiário com o mesmo CPF para o cliente
            /// </summary>
            /// <param name="cpf">CPF do beneficiário</param>
            /// <param name="idCliente">Id do cliente</param>
            /// <param name="id">Id do beneficiário a ignorar (0 na inclusão)</param>
            /// <returns>True se o CPF já existir para o cliente</returns>
            internal bool VerificarExistencia(string cpf, long idCliente, long id)
            {
                List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

                parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cpf));
                parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", idCliente));
                parametros.Add(new System.Data.SqlClient.SqlParameter("Id", id));

                DataSet ds = base.Consultar("FI_SP_VerificaBenef", parametros); // conferir
                return ds.Tables[0].Rows.Count > 0;
            }

            private List<DML.Beneficiario> Converter(DataSet ds)
            {
                List<DML.Beneficiario> lista = new List<DML.Beneficiario>();
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DML.Beneficiario ben = new DML.Beneficiario();
                        ben.Id = row.Field<long>("Id");
                        ben.CPF = row.Field<string>("CPF");
                        ben.Nome = row.Field<string>("Nome");
                        ben.IdCliente = row.Field<long>("IdCliente");
                        lista.Add(ben);
                    }
                }

                return lista;
            }
        }
    }









     
    