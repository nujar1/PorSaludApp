using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PorSaludApp.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La identidad es requerida")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [Display(Name = "Identidad")]
        public string Identidad { get; set; }

        [Required(ErrorMessage = "El nombre completo es requerido")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [Display(Name = "Teléfono")]
        [RegularExpression(@"^[\d\s\(\)\.\-\+]+$", ErrorMessage = "Solo ingrese numeros")]
        public string Telefono { get; set; }

        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [Display(Name = "Celular")]
        [RegularExpression(@"^[\d\s\(\)\.\-\+]+$", ErrorMessage = "Solo ingrese numeros")]
        public string Celular { get; set; }

        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El sexo es requerido")]
        [RegularExpression("^[MFO]$", ErrorMessage = "Sexo debe ser M, F u O")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}