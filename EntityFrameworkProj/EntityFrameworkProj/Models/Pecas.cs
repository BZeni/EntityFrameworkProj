using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Pecas")]
public class Pecas
{
    public Pecas(int idLote, double altura, double largura, double comprimento)
    {
        IdLote = idLote;
        Altura = altura;
        Largura = largura;
        Comprimento = comprimento;
    }

    [Key]
    public int IdPeca { get; set; }
    public int IdLote { get; set; }
    public double Altura { get; set; }
    public double Largura { get; set; }
    public double Comprimento { get; set; }
}