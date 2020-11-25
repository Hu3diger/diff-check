using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;
using JsonDiffer;
using Newtonsoft.Json.Linq;

namespace diffCheck.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/diff")]
    public class DiffCheckController : ControllerBase
    {
        [HttpPost]
        [Route("{id:int}/from")]
        public String postFrom(ApiVersion version, int id, [FromBody] JsonDocument jsonString)
        {
            string msg = $"Arquivo armazenado com sucesso!! Utilize o endpoint /v{version}/{id}/to";
            using (var db = new DiffEntityContext())
            {
                DiffEntity entity = null;
                List<DiffEntity> listEntities = db.DiffEntities.Where(x => x.codigo == id).ToList();
                if (listEntities.Count > 0)
                {
                    entity = listEntities.Where(x => x.isfrom == 1).FirstOrDefault();
                }
                
                if (entity == null){
                    entity = new DiffEntity();
                    entity.codigo = id;
                    entity.jsonUploaded = jsonString.RootElement.ToString();
                    entity.isfrom = 1;
                    db.DiffEntities.Add(entity);
                    db.SaveChanges();
                } else {
                    msg = $"Com este ID, já existe um arquivo salvo, tente com outro ID ou então envie o segundo arquivo";
                }
            }
            return msg;
        }

        [HttpPost]
        [Route("{id:int}/to")]
        public String postTo(ApiVersion version, int id, [FromBody] JsonDocument jsonString)
        {
            string msg = $"Arquivo armazenado com sucesso!! Utilize o endpoint /v{version}/{id}/diff para visualizar a diferença entre os arquivos.";
            using (var db = new DiffEntityContext())
            {
            
                DiffEntity entity = null;
                List<DiffEntity> listEntities = db.DiffEntities.Where(x => x.codigo == id).ToList();
                if (listEntities.Count > 0)
                {
                    entity = listEntities.Where(x => x.isfrom == 0).FirstOrDefault();
                }

                if (entity == null){
                    entity = new DiffEntity();
                    entity.codigo = id;
                    entity.jsonUploaded = jsonString.RootElement.ToString();
                    entity.isfrom = 0;
                    db.DiffEntities.Add(entity);
                    db.SaveChanges();
                } else {
                    msg = $"Com este ID, já existe um arquivo salvo, tente com outro ID ou então visualize a diferença entre os dois arquivos.";
                }
            }
            return msg;
        }

        [HttpGet]
        [Route("{id:int}/diff")]
        public String getDiff(ApiVersion version, int id)
        {
            string msg = "";
            using (var db = new DiffEntityContext())
            {
                List<DiffEntity> listEntities = db.DiffEntities.Where(x => x.codigo == id).ToList();
                if (listEntities.Count == 0 || listEntities.Count == 1){
                    msg = $"Não foram encontrados todos os arquivos para este código, por favor verifique!";
                } else {
                    DiffEntity entityFrom = listEntities.Where(x => x.isfrom == 1).First();
                    DiffEntity entityTo = listEntities.Where(x => x.isfrom == 0).First();

                    var first = JToken.Parse(entityFrom.jsonUploaded);
                    var second = JToken.Parse(entityTo.jsonUploaded);

                    var diff = JsonDifferentiator.Differentiate(first,second);
                    msg = $"Legenda: \n - Linha removida\n + Linha adicionada \n * Linha alterada \n\n {diff.ToString()}";
                }
            }
            return msg;
        }
    }
}