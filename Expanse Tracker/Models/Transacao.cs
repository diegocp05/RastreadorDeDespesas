using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expanse_Tracker.Models
{
    // Atributo que especifica a tabela associada a esta classe no banco de dados.
    [Table("Transacoes")]
    public class Transacao
    {
        // Chave primária da tabela, atributo identificador da transação.
        [Key]
        public int TransacaoId { get; set; }

        // Propriedade que representa o ID da categoria associada à transação.
        [Range(1, int.MaxValue, ErrorMessage = "Selecione a categoria")]
        public int CategoriaId { get; set; }

        // Propriedade de navegação que representa a categoria associada à transação.
        public Categoria? Categoria { get; set; }

        // Propriedade que representa a quantidade da transação.
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que 0")]
        public int Quantidade { get; set; }

        // Propriedade que representa a nota da transação.
        [Column(TypeName = "nvarchar(75)")]
        public string? Nota { get; set; }

        // Propriedade que representa a data da transação, com valor padrão como a data e hora atuais.
        public DateTime Data { get; set; } = DateTime.Now;

        // Propriedade não mapeada no banco de dados que retorna a concatenação do ícone e título da categoria.
        [NotMapped]
        public string? CategoriaTituloComIcone
        {
            get
            {
                return Categoria == null ? "" : Categoria.Icone + " " + Categoria.Titulo;
            }
        }

        // Propriedade não mapeada no banco de dados que retorna a quantidade formatada com o sinal correspondente.
        [NotMapped]
        public string? QuantidadeFormatada
        {
            get
            {
                return ((Categoria == null || Categoria.Tipo == "Expense") ? "- " : "+ ") + Quantidade.ToString("C0");
            }
        }
    }
}
