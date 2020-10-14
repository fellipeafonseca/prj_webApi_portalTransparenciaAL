using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace infiniti_challenge.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Licitacoes()
        {

            return View();
        }


        //Médoto para Consumo de API http://transparencia.al.gov.br/portal/api/
        // Parametros 
        // Key: Endipoint a ser chamado (1- licitacao, 2 - Servidores Ativos, 3 Servidores Inativo)
        // Ano: Ano de Pesquisa para Filtro
        [HttpGet]
        public ActionResult SearchApiTransParenciaAl(int key , string ano)
        {

            using (var client = new System.Net.Http.HttpClient())
            {
              
                switch (key)
                {
                    //Licitação
                    case 1:
                        client.BaseAddress = new Uri("http://transparencia.al.gov.br/licitacao/json-editais/");
                        break;

                    //Servidores Ativos
                    case 2:
                        if (!ano.Equals(string.Empty))
                        {
                            client.BaseAddress = new Uri("http://transparencia.al.gov.br/pessoal/json-servidores/?ano="+ano);
                        }
                        else {
                            client.BaseAddress = new Uri("http://transparencia.al.gov.br/pessoal/json-servidores/");
                        }
                            break;

                    // Servidores Ativos
                    case 3:
                        if (!ano.Equals(string.Empty))
                        {
                            client.BaseAddress = new Uri("http://transparencia.al.gov.br/pessoal/json-servidores-inativos/?ano=" + ano);
                        }
                        else
                        {
                            client.BaseAddress = new Uri("http://transparencia.al.gov.br/pessoal/json-servidores-inativos/");
                        }
                        break;

                    default:
                        break;
                }
              
                var responseTask = client.GetAsync("");

                try
                {
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readJob = result.Content.ReadAsStringAsync();
                        readJob.Wait();
                        var value = readJob.Result;


                        return Json(new { success = true, value = value },
                            JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        return Json(new { success = false, erro = "Erro no Processamento da API - " + result.StatusCode },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, erro = "Erro Chamada da API - " + ex.Message },
                          JsonRequestBehavior.AllowGet);
                }

               
            }
          
        }


        public ActionResult ServidoresAtivos()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ServidoresInativos()
        {
           ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}