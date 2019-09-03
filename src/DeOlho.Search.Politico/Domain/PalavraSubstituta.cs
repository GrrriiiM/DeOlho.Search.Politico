using DeOlho.SeedWork.Domain;

namespace DeOlho.Search.Politico.Domain
{
    public class PalavraSubstituta : Entity
    {
        public string Palavra { get; set; }
        public string Substituta { get; set; }
    }
}