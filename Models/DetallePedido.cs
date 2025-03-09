using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceBackend.Models
{
    [Table("DetallePedido")] // 👈 Evita que EF Core pluralice el nombre
    public class DetallePedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        // Relación con Pedido
        [ForeignKey("PedidoId")]
        public virtual Pedido? Pedido { get; set; } // 👈 Marcado como opcional para evitar problemas de carga

        // Relación con Producto
        [ForeignKey("ProductoId")]
        public virtual Producto? Producto { get; set; } // 👈 También marcado como opcional
    }
}
