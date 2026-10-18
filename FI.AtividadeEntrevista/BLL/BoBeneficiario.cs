using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        /// <returns>Id do beneficiário incluído</returns>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Alterar(beneficiario);
        }

        /// <summary>
        /// Exclui um beneficiário pelo id
        /// </summary>
        /// <param name="id">Id do beneficiário</param>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiários de um cliente
        /// </summary>
        /// <param name="idCliente">Id do cliente</param>
        /// <returns>Lista de beneficiários do cliente</returns>
        public List<DML.Beneficiario> Listar(long idCliente)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Listar(idCliente);
        }

        /// <summary>
        /// Verifica se já existe outro beneficiário com o mesmo CPF para o cliente
        /// </summary>
        /// <param name="cpf">CPF do beneficiário</param>
        /// <param name="idCliente">Id do cliente</param>
        /// <param name="id">Id do beneficiário a ignorar (0 na inclusão)</param>
        /// <returns>True se o CPF já existir para o cliente</returns>
        public bool VerificarExistencia(string cpf, long idCliente, long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.VerificarExistencia(cpf, idCliente, id);
        }
    }
}