// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repositorio.Models;
using System;
using System.Linq;

Console.WriteLine("Hello, World!");
var json = System.IO.File.ReadAllText("C:\\projetos-cris\\lojacarro\\DrogaEhOBryan\\DrogaEhOBryan\\cars.json");
var carrosJson = JsonConvert.DeserializeObject<List<Carro>>(json);

using var db = new CarrosContext();
var marca = new Marca();
marca.Nome = "VolksWagen";
marca.Logo = "https://w0.peakpx.com/wallpaper/403/173/HD-wallpaper-chrome-and-carbon-fiber-logo-volkswagen-background-volkswagen-logo-volkswagen-automobiles-volkswagen-volkswagen-volkswagen-cars-volkswagen-emblem-volkswagen-motors.jpg";
marca.Id = 1;


//Adiciona Json no Banco de Dados
Console.WriteLine($"Database path: {db.DbPath}.");

var carrosParaAdicionarNoBanco = carrosJson.Select(carroJson => new Carro
{
    title = carroJson.title,
    image = carroJson.image,
    Class = carroJson.Class ?? "",
    start_production = carroJson.start_production,
    Marca = marca
});

db.Carros.AddRange(carrosParaAdicionarNoBanco);
db.SaveChanges();
// Create
//carro.title = "Mate";
//carro.start_production = 2005;
//carro.image = "content/uploads/2022/09/14102436/Mate.png";
//carro.Class = "Carros";
//if (db.Carros.FirstOrDefault() == null)
//{
//    carro.Id = carro.Id + 1;
//} 

//db.Carros.Add(carro);
//db.SaveChanges();

// Read
Console.WriteLine("Querying for a carro");
var carro = db.Carros.FirstOrDefault();
Console.WriteLine($"Car ID: {carro.Id}, Title: {carro.title}");

Console.ReadKey();

////Update
//Console.WriteLine("Updating the carro and adding a new carro");
//carro.title = "Relampago Marquinhos";
//carro.start_production = carro.start_production;
//carro.image = "https://i.ytimg.com/vi/XOs8cOiUG4w/maxresdefault.jpg";
//carro.Id = carro.Id;
//carro.Class = "Carros Filme";

//db.Carros.Update(carro);
//db.SaveChanges();

//// Delete
//Console.WriteLine("Deleting the carro");
//db.Carros.Remove(carro);
//db.SaveChanges();

