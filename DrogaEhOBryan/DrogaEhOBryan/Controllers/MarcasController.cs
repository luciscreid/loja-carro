using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data;
using System.Reflection.Metadata;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Repositorio.Models;
using DrogaEhOBryan.Models;

namespace DrogaEhOBryan.Controllers
{
    public class MarcasController : Controller
    {
        private const int ItensPorPagina = 20;
        private const int numeroDeProximasPaginas = 2;
        private const int numeroDePaginasAnterores = -1;
        private const int posicao = 0;
        public MarcasController()
        {

        }

        public IActionResult Marcas(int paginaAtual = 0, string q = null)
        {
            using var db = new CarrosContext();

            var marcas = db.Marcas.Include(c => c.Carros).AsQueryable();

            if (q != null)
                marcas = marcas.Where(carro => carro.Nome.Contains(q));

            int quantidadeDePaginas = (int)Math.Ceiling((decimal)marcas.Count() / ItensPorPagina);

            var marcasPaginado = marcas.Skip(paginaAtual * 20).Take(20);
            var marcasPaginadaFinal = marcasPaginado.ToList();

            //ViewBag.paginaAtual = paginaAtual;
            //ViewBag.q = q;
            //ViewBag.ItensPorPagina = ItensPorPagina;
            //ViewBag.numeroDeProximasPaginas = numeroDeProximasPaginas;
            //ViewBag.numeroDePaginasAnterores = numeroDePaginasAnterores;

            return base.View(marcasPaginadaFinal);
        }

        [HttpGet]
        public IActionResult CriaMarca()
        {
            using var db = new CarrosContext();
            return View();
        }

        [HttpPost]
        public IActionResult CriaMarca(Marca marca)
        {
            using var db = new CarrosContext();
            if (ModelState.IsValid)
            {
                db.Add(marca);
                db.SaveChanges();
                //_marcas.Insert(marca.Id, marca);
                return RedirectToAction("Marcas");
            }
            return View(marca);
        }
        [HttpGet]
        public IActionResult EditarMarca(int id)
        {
            using var db = new CarrosContext();
            // buscar carro e retornar na view
            var marca = db.Marcas.FirstOrDefault(c => c.Id == id);

            return View(marca);
        }

        [HttpPost]
        public IActionResult EditarMarca(Marca marca)
        {
            using var db = new CarrosContext();
            if (ModelState.IsValid)
            {
                var marcaExistente = db.Marcas.FirstOrDefault(c => c.Id == marca.Id);
                if (marcaExistente != null)
                {
                    //var carroAtual = _carros.IndexOf(carroExistente);
                    //_carros[carroAtual] = carro;

                    marcaExistente.Nome = marca.Nome;
                    marcaExistente.Logo = marca.Logo;
                    
                    db.SaveChanges();

                }
                return RedirectToAction("Marcas");
            }
            return View(marca);
        }

        [HttpGet]
        public IActionResult ExcluirMarca(int id)
        {
            using var db = new CarrosContext();
            //var carro = _carros.FirstOrDefault(c => c.Id == id);
            var marca = db.Marcas.FirstOrDefault(c => c.Id == id);

            return View(marca);
        }

        [HttpPost]
        public IActionResult ConfirmarExclusaoMarca(int id)
        {
            using var db = new CarrosContext();
            var marcaExistente = db.Marcas.FirstOrDefault(car1 => car1.Id == id);
            if (marcaExistente != null)
            {
                if (!db.Carros.Any(car2 => car2.MarcaId == id))
                {
                    db.Remove(marcaExistente);
                    db.SaveChanges();
                    ViewBag.podeExcluir = true;
                }
                else
                {
                    ViewBag.podeExcluir = false;
                }
            }

            return RedirectToAction("Marcas");
        }
        public IActionResult Privacy()
        {
            using var db = new CarrosContext();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            using var db = new CarrosContext();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
