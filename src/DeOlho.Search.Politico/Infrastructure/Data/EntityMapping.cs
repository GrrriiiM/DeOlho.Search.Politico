using DeOlho.Search.Politico.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeOlho.Search.Politico.Infrastructure.Data
{
    public class PoliticoMandatoEntityMapping : IEntityTypeConfiguration<PoliticoMandato>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PoliticoMandato> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasIndex(_ => new { _.CPF, _.Ano }).IsUnique();
        }
    }

    public class PalavraSubstitutaEntityMapping : IEntityTypeConfiguration<PalavraSubstituta>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PalavraSubstituta> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasIndex(_ => _.Palavra).IsUnique();
        }
    }
}