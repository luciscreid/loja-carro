using Microsoft.AspNetCore.Mvc;
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
using Repositorio.Models;
using DrogaEhOBryan.Models;

namespace DrogaEhOBryan.Controllers
{

    public class HomeController : Controller

    {
        private readonly ILogger<HomeController> _logger;
        private static List<Carro> _carros;


        private const int ItensPorPagina = 20;
        private const int numeroDeProximasPaginas = 2;
        private const int numeroDePaginasAnterores = -1;
        private const int posicao = 0;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            if (_carros == default)
            {
                //var json = System.IO.File.ReadAllText("./cars.json");


                //for (int i = 0; i < _carros.Count; i++)
                //{
                //    _carros[i].Id = i + 1;
                //    int posicao = _carros[i].Id;

                //}
            }
        }

        public IActionResult Index(int paginaAtual = 0, string q = null)
        {
            using var db = new CarrosContext();

            var carros = db.Carros.Include(c => c.Marca).AsQueryable();

            if (q != null)
                carros = carros.Where(carro => carro.title.Contains(q));

            int quantidadeDePaginas = (int)Math.Ceiling((decimal)carros.Count() / ItensPorPagina);

            var carrosPaginado = carros.Skip(paginaAtual * 20).Take(20);
            var carrosPaginadaFinal = carrosPaginado.ToList();


            ViewBag.paginaAtual = paginaAtual;
            ViewBag.q = q;
            ViewBag.ItensPorPagina = ItensPorPagina;
            ViewBag.numeroDeProximasPaginas = numeroDeProximasPaginas;
            ViewBag.numeroDePaginasAnterores = numeroDePaginasAnterores;

            return base.View(carrosPaginadaFinal);

        }

        private bool ContemCarro(Carro carro)
        {
            using var db = new CarrosContext();
            return carro.title.Contains("chevette", StringComparison.InvariantCultureIgnoreCase);
        }

        [HttpGet]
        public IActionResult CriaCarro()
        {
            using var db = new CarrosContext();
            var marcas = db.Marcas.ToList();
            ViewBag.Marcas = marcas;
            return View();
        }

        [HttpPost]
        public IActionResult CriaCarro(Carro carro)
        {
            using var db = new CarrosContext();
            if (ModelState.IsValid)
            {
                db.Add(carro);
                db.SaveChanges();
                //_carros.Insert(carro.Id, carro);
                return RedirectToAction("Index");
            }
            return View(carro);
        }

        [HttpGet]
        public IActionResult EditarCarro(int id)
        {
            using var db = new CarrosContext();
            var carro = db.Carros.Include(c => c.Marca).FirstOrDefault(c => c.Id == id);
            var marcas = db.Marcas.ToList();
            ViewBag.Marcas = marcas;
            return View(carro);
        }

        [HttpPost]
        public IActionResult EditarCarro(Carro carro)
        {
            using var db = new CarrosContext();
            if (ModelState.IsValid)
            {
                var carroExistente = db.Carros.Include(c => c.Marca).FirstOrDefault(c => c.Id == carro.Id);
                if (carroExistente != null)
                {
                    carroExistente.title = carro.title;
                    carroExistente.Class = carro.Class;
                    carroExistente.image = carro.image;
                    carroExistente.start_production = carro.start_production;
                    carroExistente.MarcaId = carro.MarcaId; // Atualiza a referência à marca selecionada
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(carro);
        }

        [HttpGet]
        public IActionResult ExcluirCarro(int id)
        {
            using var db = new CarrosContext();
            //var carro = _carros.FirstOrDefault(c => c.Id == id);
            var carro = db.Carros.FirstOrDefault(c => c.Id == id);

            return View(carro);
        }

        [HttpPost]
        public IActionResult ExcluirCarro(Carro carro)
        {
            using var db = new CarrosContext();
            if (ModelState.IsValid)
            {
                var carroExistente = db.Carros.FirstOrDefault(c => c.Id == carro.Id);
                if (carroExistente != null)
                {
                    //    var carroAtual = _carros.IndexOf(carroExistente);
                    db.Remove(carroExistente);
                    db.SaveChanges();
                    //    _carros.Remove(carroExistente);
                }
                return RedirectToAction("Index");
            }
            return View(carro);
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


//var Carros = new List<Carro>();

//for (int i = 0; i < 20; i++)
//{   
//    var carroAtual = _carros[i];

//    Carros.Add(carroAtual);
//}

//return View(Carros);




//carros = from carro in carros
//where carro.title.Contains("chevette", StringComparison.InvariantCultureIgnoreCase)
//select carro;

//carros = carros.Where(carro => { return carro.title.Contains("chevette", StringComparison.InvariantCultureIgnoreCase)});


//carros = carros.Where(ContemCarro);





//Carro model
//    = _carros[19];
//return base.View(_carros);
//return View(new List<Carro>() { new Carro() { title = "Camaro" } });