using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceBackend.Models
{
    [Table("Reseña")]
    public class Reseña
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int UsuarioId { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public int Calificacion { get; set; }

        public Producto? Producto { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
