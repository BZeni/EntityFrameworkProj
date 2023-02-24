using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Lotes")]
public class Lotes
{
    [Key, Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int IdLote { get; set; }    
    public int CodProd { get; set; }   
    public string? Descricao { get; set; }
    [ForeignKey("IdLote")]
    public List<Pecas>? Pecas { get; set; } = new();
}