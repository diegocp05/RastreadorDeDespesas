using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expanse_Tracker.Models
{
    // Atributo que especifica a tabela associada a esta classe no banco de dados.
    [Table("Categorias")]
    public class Categoria
    {
        // Chave primária da tabela, atributo identificador da categoria.
        [Key]
        public int CategoriaId { get; set; }

        // Propriedade que representa o título da categoria.
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Título é obrigatório")]
        public string Titulo { get; set; }

        // Propriedade que representa o ícone da categoria.
        [Column(TypeName = "nvarchar(5)")]
        public string Icone { get; set; } = "";

        // Propriedade que representa o tipo da categoria (Expense ou Income).
        [Column(TypeName = "nvarchar(10)")]
        public string Tipo { get; set; } = "Expense";

        // Propriedade não mapeada no banco de dados que retorna o título com o ícone.
        [NotMapped]
        public string? TituloComIcone
        {
            get
            {
                return this.Icone + " " + this.Titulo;
            }
        }


    }
}
