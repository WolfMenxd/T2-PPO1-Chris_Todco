using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using POOI_T2_Todco.Models;

namespace POOI_T2_Todco.Controllers
{
    public class AlumnoController : Controller
    {
        private static String lista = @"[]";
        private const string ArchivoJson = "~/App_Data/alumnos.json";

        private string ObtenerRuta()
        {
            return Server.MapPath(ArchivoJson);
        }

        private Alumno[] Deserializar()
        {
            string ruta = ObtenerRuta();
            if (System.IO.File.Exists(ruta))
                lista = System.IO.File.ReadAllText(ruta);

            Alumno[] alumnos = JsonConvert.DeserializeObject<Alumno[]>(lista);
            return alumnos ?? new Alumno[0];
        }

        private void Serializar(Alumno[] alumnos)
        {
            lista = JsonConvert.SerializeObject(alumnos, Formatting.Indented);
            string ruta = ObtenerRuta();
            Directory.CreateDirectory(Path.GetDirectoryName(ruta));
            System.IO.File.WriteAllText(ruta, lista);
            TempData["Mensaje"] = "Los datos se guardaron correctamente en el archivo JSON.";
        }

        public ActionResult Index()
        {
            return View(Deserializar());
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Alumno alumno)
        {
            Alumno[] alumnos = Deserializar();
            if (alumnos.Any(a => a.Dni == alumno.Dni))
                ModelState.AddModelError("Dni", "Ya existe un alumno con ese DNI.");

            if (!ModelState.IsValid)
                return View(alumno);

            Alumno nuevo = new Alumno(alumno.Dni, alumno.Nombres, alumno.Apellidos, alumno.Carrera, alumno.Ciclo);
            Serializar(alumnos.Concat(new[] { nuevo }).ToArray());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detalles(string dni)
        {
            Alumno alumno = Deserializar().FirstOrDefault(a => a.Dni == dni);
            if (alumno == null)
                return HttpNotFound();
            return View(alumno);
        }

        [HttpGet]
        public ActionResult Actualizar(string dni)
        {
            Alumno alumno = Deserializar().FirstOrDefault(a => a.Dni == dni);
            if (alumno == null)
                return HttpNotFound();
            return View(alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(string dniOriginal, Alumno alumno)
        {
            Alumno[] alumnos = Deserializar();
            Alumno existente = alumnos.FirstOrDefault(a => a.Dni == dniOriginal);
            if (existente == null)
                return HttpNotFound();
            if (alumnos.Any(a => a.Dni == alumno.Dni && a.Dni != dniOriginal))
                ModelState.AddModelError("Dni", "Ya existe otro alumno con ese DNI.");

            if (!ModelState.IsValid)
            {
                ViewBag.DniOriginal = dniOriginal;
                return View(alumno);
            }

            Alumno actualizado = new Alumno(alumno.Dni, alumno.Nombres, alumno.Apellidos, alumno.Carrera, alumno.Ciclo);
            Alumno[] resultado = alumnos.Select(a => a.Dni == dniOriginal ? actualizado : a).ToArray();
            Serializar(resultado);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(string dni)
        {
            Alumno[] alumnos = Deserializar();
            Alumno[] resultado = alumnos.Where(a => a.Dni != dni).ToArray();
            if (resultado.Length == alumnos.Length)
                TempData["Mensaje"] = "No se encontró un alumno con ese DNI.";
            else
                Serializar(resultado);
            return RedirectToAction("Index");
        }
    }
}





