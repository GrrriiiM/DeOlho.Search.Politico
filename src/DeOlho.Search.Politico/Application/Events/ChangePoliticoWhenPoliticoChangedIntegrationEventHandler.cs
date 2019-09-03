using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Messages;
using DeOlho.EventBus.MediatR;
using DeOlho.Search.Politico.Domain;
using DeOlho.SeedWork.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeOlho.Search.Politico.Application.Events
{
    public class ChangePoliticoWhenPoliticoChangedIntegrationEventHandler : EventBusConsumerHandler<PoliticoChangedMessage>
    {
        readonly DeOlhoDbContext _deOlhoDbContext;
        //readonly IRepository<Domain.Politico> _politicoRepository;

        public ChangePoliticoWhenPoliticoChangedIntegrationEventHandler(
            DeOlhoDbContext deOlhoDbContext)
        {
            _deOlhoDbContext = deOlhoDbContext;
        }
        public async override Task<Unit> Handle(PoliticoChangedMessage message, CancellationToken cancellationToken)
        {
            var politicoMandatoRepository = _deOlhoDbContext.Set<Domain.PoliticoMandato>();
            var palavraSubstitutaRepository = _deOlhoDbContext.Set<Domain.PalavraSubstituta>();
            var palavrasSubstitutas = await palavraSubstitutaRepository.ToListAsync(cancellationToken);

            foreach(var messagePolitico in message.Politicos)
            {
                var politicoMandato = (await politicoMandatoRepository.SingleOrDefaultAsync(_ => 
                    _.CPF == messagePolitico.NR_CPF_CANDIDATO && _.Ano == messagePolitico.ANO_ELEICAO, cancellationToken))
                    ?? new PoliticoMandato();
                
                politicoMandato.CPF = messagePolitico.NR_CPF_CANDIDATO;
                politicoMandato.Nome = messagePolitico.NM_CANDIDATO;
                politicoMandato.Apelido = messagePolitico.NM_URNA_CANDIDATO;
                politicoMandato.Ano = messagePolitico.ANO_ELEICAO;
                politicoMandato.Partido = messagePolitico.NM_PARTIDO;
                politicoMandato.Cargo = messagePolitico.DS_CARGO;
                politicoMandato.Abrangencia = messagePolitico.NM_UE;
                politicoMandato.Eleito = messagePolitico.DS_SIT_TOT_TURNO.StartsWith("ELEITO");
                politicoMandato.TermoPesquisa = string.Join(' ', new string[] 
                { 
                    politicoMandato.Nome,
                    politicoMandato.Apelido,
                    politicoMandato.Ano.ToString(),
                    politicoMandato.Partido,
                    politicoMandato.Cargo,
                    politicoMandato.Abrangencia
                });

                if (politicoMandato.Id == 0)
                    await politicoMandatoRepository.AddAsync(politicoMandato, cancellationToken);

                if (!palavrasSubstitutas.Any(_ => _.Palavra.ToUpper() == messagePolitico.SG_PARTIDO.ToUpper()))
                {
                    var palavraSubstituta = new PalavraSubstituta 
                    { 
                        Palavra = messagePolitico.SG_PARTIDO,
                        Substituta = messagePolitico.NM_PARTIDO
                    };
                    palavrasSubstitutas.Add(palavraSubstituta);
                    await palavraSubstitutaRepository.AddAsync(palavraSubstituta, cancellationToken);
                }

                if (!palavrasSubstitutas.Any(_ => _.Palavra.ToUpper() == messagePolitico.NR_PARTIDO.ToString()))
                {
                    var palavraSubstituta = new PalavraSubstituta 
                    { 
                        Palavra = messagePolitico.NR_PARTIDO.ToString(),
                        Substituta = messagePolitico.NM_PARTIDO
                    };
                    palavrasSubstitutas.Add(palavraSubstituta);
                    await palavraSubstitutaRepository.AddAsync(palavraSubstituta, cancellationToken);
                }

                if (!palavrasSubstitutas.Any(_ => _.Palavra.ToUpper() == messagePolitico.SG_UF.ToUpper()))
                {
                    var palavraSubstituta = new PalavraSubstituta 
                    { 
                        Palavra = messagePolitico.SG_UF.ToUpper(),
                        Substituta = messagePolitico.NM_UE
                    };
                    palavrasSubstitutas.Add(palavraSubstituta);
                    await palavraSubstitutaRepository.AddAsync(palavraSubstituta, cancellationToken);
                }
                
            }

            await _deOlhoDbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}