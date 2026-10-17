using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public static class ValidadorCpf
    {
        /// <summary>
        /// Valida um CPF
        /// </summary>
        /// <param name="cpf">CPF a ser validado</param>
        /// <returns>True se o CPF for válido, caso contrário False</returns>
        public static bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            int somaPrimeiroDigito = 0;
            for (int i = 0; i < 9; i++)
                somaPrimeiroDigito += (cpf[i] - '0') * (10 - i);
            int resto = somaPrimeiroDigito % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            int somaSegundoDigito = 0;
            for (int i = 0; i < 10; i++)
                somaSegundoDigito += (cpf[i] - '0') * (11 - i);
            resto = somaSegundoDigito % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf[9] - '0' == digito1 && cpf[10] - '0' == digito2;
        }
    }
}
