using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeOlho.Search.Politico.Application.Models;
using DeOlho.Search.Politico.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeOlho.Search.Politico.Controllers
{
    [Route("api/pesquisa/politico")]
    [ApiController]
    public class PoliticoMandatoController : ControllerBase
    {
        readonly IMediator _mediator;

        public PoliticoMandatoController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{termoPesquisa}")]
        public Task<IEnumerable<PoliticoMandatoModel>> List(string termoPesquisa)
        {
            return _mediator.Send(new ListPoliticoMandatoQuery { TermoPesquisa = termoPesquisa });
        }

    }
}
