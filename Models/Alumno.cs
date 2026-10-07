using System.ComponentModel.DataAnnotations;

namespace POOI_T2_Todco.Models
{
    public class Alumno
    {
        [Required(ErrorMessage = "Ingrese el DNI.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 caracteres.")]
        [Display(Name = "DNI")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Ingrese los nombres.")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Ingrese los apellidos.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Ingrese la carrera.")]
        public string Carrera { get; set; }

        [Required(ErrorMessage = "Ingrese el ciclo.")]
        public string Ciclo { get; set; }

        public Alumno() { }

        public Alumno(string dni, string nombres, string apellidos, string carrera, string ciclo)
        {
            Dni = dni;
            Nombres = nombres;
            Apellidos = apellidos;
            Carrera = carrera;
            Ciclo = ciclo;
        }
    }
}

