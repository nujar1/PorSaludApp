using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PorSaludApp.Models
{
    [Table("Documentos")]
    public class Documento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentoId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El nombre del archivo es requerido")]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Display(Name = "Nombre del Archivo")]
        public string NombreArchivo { get; set; }

        [Required(ErrorMessage = "La ruta del archivo es requerida")]
        [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
        [Display(Name = "Ruta del Archivo")]
        public string RutaArchivo { get; set; }

        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        [Display(Name = "Tipo de Archivo")]
        public string TipoArchivo { get; set; } = "pdf";

        [Display(Name = "Fecha de Carga")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCarga { get; set; } = DateTime.Now;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Relación con Cliente
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }
    }
}