using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceBackend.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public bool EsAdmin { get; set; }

        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        public string? ResetToken { get; set; } 
        public DateTime? ResetTokenExpiration { get; set; } 

        public ICollection<Pedido>? Pedidos { get; set; }
        public ICollection<Reseña>? Reseñas { get; set; }
    }
}
