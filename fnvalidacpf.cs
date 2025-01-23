using System;
using System.IO; 
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace httpValidaCpf
{
    public static class fnvalidacpf
    {
        [FunctionName("fnvalidacpf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Iniciando a Validação de CPF");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            if (data?.cpf == null)
            {
                return new BadRequestObjectResult("Por favor, informe um CPF válido");
            }
            string cpf = data?.cpf;
            
            if (ValidaçãoCpf(cpf)== false)
            {
                return new BadRequestObjectResult("CPF inválido");
            }

            var responseMessage = "CPF válido";

            return new OkObjectResult(responseMessage));
        }
    }
    public static bool ValidaçãoCpf(string cpf)
    {
        int[] v = new int[11];
        int j, soma = 0, resto, digito1, digito2;
        bool cpfOk = false;

        if (cpf.Length != 11)
        {
            return false;
        }

        for (int i = 0; i < 11; i++)
        {
            v[i] = int.Parse(cpf[i].ToString());
        }

        for (int i = 0; i < 9; i++)
        {
            soma += v[i] * (10 - i);
        }

        resto = soma % 11;
        if (resto < 2)
        {
            digito1 = 0;
        }
        else
        {
            digito1 = 11 - resto;
        }

        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += v[i] * (11 - i);
        }

        resto = soma % 11;
        if (resto < 2)
        {
            digito2 = 0;
        }
        else
        {
            digito2 = 11 - resto;
        }

        if (digito1 == v[9] && digito2 == v[10])
        {
            cpfOk = true;
        }

        return cpfOk;
    }
}