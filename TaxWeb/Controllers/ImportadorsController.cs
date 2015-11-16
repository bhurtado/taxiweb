using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaxWeb.Model;
using TaxWeb.Models;
using System.IO;

namespace TaxWeb.Controllers
{
    public class ImportadorsController : Controller
    {
        private TaxWebContext db = new TaxWebContext();

        // GET: Importadors
        public ActionResult Index()
        {
            return View(db.Importadors.ToList());
        }

        // GET: Importadors/Details/5

        public ActionResult listar(Guid g)
        {

            return null;
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Importador importador = db.Importadors.Find(id);
            if (importador == null)
            {
                return HttpNotFound();
            }
            return View(importador);
        }

        // GET: Importadors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Importadors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Comprador,Preco,quantidade,endereco,fornecedor")] Importador importador)
        {
            if (ModelState.IsValid)
            {
                db.Importadors.Add(importador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(importador);
        }

        // GET: Importadors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Importador importador = db.Importadors.Find(id);
            if (importador == null)
            {
                return HttpNotFound();
            }
            return View(importador);
        }

        // POST: Importadors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Comprador,Preco,quantidade,endereco,fornecedor")] Importador importador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(importador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(importador);
        }

        // GET: Importadors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Importador importador = db.Importadors.Find(id);
            if (importador == null)
            {
                return HttpNotFound();
            }
            return View(importador);
        }

        // POST: Importadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Importador importador = db.Importadors.Find(id);
            db.Importadors.Remove(importador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult Todos()
        {
            return View("Todos", db.Importadors.ToList());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Importar(FormCollection f)
        {
            string folder = ("../temp");
            var guide = Guid.NewGuid();
            //Verifica se a pasta temp existe e salva o arquivo nela.
            HttpPostedFileBase arquivo = Request.Files[0];
            if (arquivo.ContentLength > 0)
            {
                
                DirectoryInfo dir = new System.IO.DirectoryInfo(folder);
                if (!dir.Exists)
                {

                    //dir.Create(folder);
                    Directory.CreateDirectory(folder);
                }
                

                var uploadPath = Server.MapPath("../temp");

                string caminhoArquivo = Path.Combine(@uploadPath, guide + "_" + Path.GetFileName(arquivo.FileName));
                arquivo.SaveAs(caminhoArquivo);

                //Le o aruivo texto
                string[] dados = System.IO.File.ReadAllLines(caminhoArquivo);
                int linha = 0;
                foreach (var item in dados)
                {
                    if(linha > 0)
                    {
                        string[] valor = item.Split('\t');
                        var comprador = valor[0];
                        var descricao = valor[1];
                        var precoUnitario = Convert.ToDecimal( valor[2]);
                        var quantidade = Convert.ToInt32(valor[3]);
                        var endereco = valor[4];
                        var fornecedor = valor[5];

                        var Imp = new Importador();
                        Imp.Comprador = comprador;
                        Imp.descricao = descricao;
                        Imp.Preco = precoUnitario;
                        Imp.quantidade = quantidade;
                        Imp.endereco = endereco;
                        Imp.fornecedor = fornecedor;
                        Imp.chave =  guide.ToString();
                        db.Importadors.Add(Imp);
                        db.SaveChanges();



                        
                    }
                    linha++;
                  
                }
            }




           return View("listar", db.Importadors.ToList().Where(a => a.chave == guide.ToString()));
            //return View("listar",);



        }
    }
}
