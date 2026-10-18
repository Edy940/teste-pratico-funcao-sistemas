using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!ValidadorCpf.ValidarCPF(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido");
            }

            model.CPF = ValidadorCpf.FormatarCpf(model.CPF);

            if (bo.VerificarExistencia(model.CPF, model.Id))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado");
            }
           
            string erroValidacaoBeneficiarios = ValidarBeneficiarios(model.Beneficiarios);
            if (erroValidacaoBeneficiarios != null)
            {
                Response.StatusCode = 400;
                return Json(erroValidacaoBeneficiarios);
            }
           
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                SalvarBeneficiarios(model.Id, model.Beneficiarios);
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!ValidadorCpf.ValidarCPF(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido");
            }

            model.CPF = ValidadorCpf.FormatarCpf(model.CPF);

            if (bo.VerificarExistencia(model.CPF, model.Id))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado");
            }

            string erroValidacaoBeneficiarios = ValidarBeneficiarios(model.Beneficiarios);
            if (erroValidacaoBeneficiarios != null)
            {
                Response.StatusCode = 400;
                return Json(erroValidacaoBeneficiarios);
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                SalvarBeneficiarios(model.Id, model.Beneficiarios);
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = new BoBeneficiario().Listar(cliente.Id).Select(b => new BeneficiarioModel()
                    {
                        Id = b.Id,
                        Nome = b.Nome,
                        CPF = b.CPF
                    }).ToList()
                };
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Valida a lista de beneficiários
        /// </summary>
        /// <param name="beneficiarios">Lista de beneficiários a ser validada</param>
        /// <returns>Mensagem de erro, ou null se a lista for válida</returns>
        private string ValidarBeneficiarios(List<BeneficiarioModel> beneficiarios)
        {
            if (beneficiarios == null)
            {
                return null;
            }
            HashSet<string> cpfs = new HashSet<string>();
            foreach (BeneficiarioModel b in beneficiarios)
            {
                if (string.IsNullOrWhiteSpace(b.Nome))
                {
                    return "Informe o nome do beneficiário de CPF " + b.CPF + ".";
                }

                if (!ValidadorCpf.ValidarCPF(b.CPF))
                {
                    return "O CPF do beneficiário " + b.Nome + " é inválido.";
                }
                b.CPF = ValidadorCpf.FormatarCpf(b.CPF);

                if (!cpfs.Add(b.CPF))
                {
                    return "Já existe um beneficiário com o CPF " + b.CPF + " para este cliente.";
                }
            }
            return null;
        }

        /// <summary>
        /// Grava os beneficiários de um cliente: inclui, altera e exclui
        /// </summary>
        /// <param name="idCliente">Id do cliente</param>
        /// <param name="beneficiarios">Lista de beneficiários vinda da tela</param>
        private void SalvarBeneficiarios(long idCliente, List<BeneficiarioModel> beneficiarios)
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario(); // objeto que acessa o banco
            List<Beneficiario> beneficiariosExistentes = boBeneficiario.Listar(idCliente); // beneficiários que já existem no banco para o cliente
            List<BeneficiarioModel> lista = beneficiarios ?? new List<BeneficiarioModel>(); // beneficiários que vieram do front-end 
            foreach (Beneficiario existente in beneficiariosExistentes)
            {
                if (!lista.Any(ben => ben.Id == existente.Id))
                {
                    boBeneficiario.Excluir(existente.Id); // excluir beneficiário que não está na lista do front-end
                }
            }
            foreach (BeneficiarioModel ben in lista)
            {
                Beneficiario beneficiario = new Beneficiario()
                {
                    Id = ben.Id,
                    Nome = ben.Nome,
                    CPF = ben.CPF,
                    IdCliente = idCliente
                };
                if (ben.Id > 0 && beneficiariosExistentes.Any(b => b.Id == ben.Id))
                {
                    boBeneficiario.Alterar(beneficiario); 
                }
                else
                {
                    boBeneficiario.Incluir(beneficiario); 
                }
            }
        }
    }
}
