namespace DeOlho.Search.Politico.Application.Models
{
    public class PoliticoMandatoModel
    {
        public long CPF { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public int Ano { get; set; }
        public string Partido { get; set; }
        public string Cargo { get; set; }
        public string Abrangencia { get; set; }
        public bool Eleito { get; set; }
    }
}