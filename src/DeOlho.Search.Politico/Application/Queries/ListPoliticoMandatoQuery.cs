using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeOlho.Search.Politico.Application.Models;
using DeOlho.Search.Politico.Domain;
using DeOlho.SeedWork.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeOlho.Search.Politico.Application.Queries
{
    public class ListPoliticoMandatoQuery : IRequest<IEnumerable<PoliticoMandatoModel>>
    {
        public string TermoPesquisa { get; set; }
    }

    public class ListPoliticoMandatoQueryHandler : IRequestHandler<ListPoliticoMandatoQuery, IEnumerable<PoliticoMandatoModel>>
    {
        readonly DeOlhoDbContext _deOlhoDbContext;

        public ListPoliticoMandatoQueryHandler(
            DeOlhoDbContext deOlhoDbContext)
        {
            _deOlhoDbContext = deOlhoDbContext;
        }

        public async Task<IEnumerable<PoliticoMandatoModel>> Handle(ListPoliticoMandatoQuery request, CancellationToken cancellationToken)
        {
            var palavrasSubstitutas = await _deOlhoDbContext.Set<PalavraSubstituta>()
                .ToDictionaryAsync(_ => _.Palavra.ToUpper(), _ => _.Substituta,  cancellationToken);

            var palavrasTermoPesquisa = request.TermoPesquisa.Split(' ');

            for(var i = 0; i < palavrasTermoPesquisa.Length; i++)
            {
                var palavra = palavrasTermoPesquisa[i];
                if (palavrasSubstitutas.ContainsKey(palavra.ToUpper()))
                {
                    palavra = palavrasSubstitutas[palavra];
                }
            }

            var termoPesquisa = string.Join(' ', palavrasTermoPesquisa);


            var query = $"SELECT * from PoliticoMandato WHERE MATCH(TermoPesquisa) AGAINST('{termoPesquisa}')";


            return await _deOlhoDbContext.Set<PoliticoMandato>()
                .FromSql(query)
                .Take(10)
                .Select(_ => new PoliticoMandatoModel
                {
                    CPF = _.CPF,
                    Nome = _.Nome,
                    Apelido = _.Apelido,
                    Ano = _.Ano,
                    Partido = _.Partido,
                    Cargo = _.Cargo,
                    Abrangencia = _.Abrangencia,
                    Eleito = _.Eleito
                })
                .ToListAsync(cancellationToken);

        }
    }
}