using System.ComponentModel.DataAnnotations;

namespace SalesAPI.Models
{
    public class Filial
{
    public int FilialId { get; set; } // Chave primária
    public string Nome { get; set; }  // Nome da filial
    public string Endereco { get; set; } // Endereço
    public string Telefone { get; set; } // Telefone
    public DateTime DataCadastro { get; set; } // Data de cadastro
}

}
