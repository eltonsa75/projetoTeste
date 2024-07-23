namespace TesteCorrentista.Models
{
    public class Transacao
    {

        public int Id { get; set; }
        public DateTime Data { get; set; }

        public string Tipo {  get; set; }
        public decimal Valor { get; set; }
    }
}
