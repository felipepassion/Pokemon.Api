using Microsoft.AspNetCore.Mvc;
using Pokemon.Api.Docs.Filters;
using Pokemon.Api.Docs.Samples;
using Pokemon.Application.DTO;
using Pokemon.Services;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Pokemon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        /// <summary>
        /// Obt�m uma lista de Pok�mons com base em um limite opcional.
        /// </summary>
        /// <param name="limit">O limite m�ximo de Pok�mons a serem retornados.</param>
        /// <returns>Uma lista de Pok�mons.</returns>
        #region Swagger Examples Attributes
        [SwaggerOperation(Tags = new string[] { "Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<List<PokemonListingItemResponse>>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<List<PokemonListingItemResponse>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.GetPokemonsListing))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        #endregion
        [HttpGet]
        public async Task<GetResponseDTO<List<PokemonListingItemResponse>>> GetAllPokemonsAsync([FromQuery] int? limit = null)
        {
            var pokemons = await _pokemonService.GetAllPokemonsAsync(limit);
            return GetResponseDTO<List<PokemonListingItemResponse>>.Ok(pokemons);
        }

        /// <summary>
        /// 2 - Busca um Pok�mon espec�fico pelo seu nome ou ID na API
        /// </summary>
        /// <param name="pokemonName">O nome do Pok�mon a ser procurado</param>
        /// <returns>Um GetResponseDTO contendo um PokemonDTO se o Pok�mon for encontrado, caso contr�rio retorna NotFound</returns>
        #region Swagger Examples Attributes
        [OperationOrder(1)]
        [SwaggerOperation(Tags = new string[] { "Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<PokemonDTO>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<PokemonDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.SearchPokemon))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        #endregion
        [HttpGet("{pokemonName}")]
        public async Task<GetResponseDTO<PokemonDTO>> SearchPokemonAsync(string pokemonName)
        {
            var pokemon = await _pokemonService.SearchPokemonAsync(pokemonName);
            if (pokemon == null)
            {
                return GetResponseDTO<PokemonDTO>.NotFound();
            }
            return GetResponseDTO<PokemonDTO>.Ok(pokemon);
        }

        /// <summary>
        /// Obt�m informa��es sobre as evolu��es de um Pok�mon especificado.
        /// </summary>
        /// <param name="pokemonName">O nome do Pok�mon a ser pesquisado.</param>
        /// <returns>Uma lista de informa��es sobre as evolu��es do Pok�mon.</returns>
        [HttpGet("{pokemonName}/evolutions")]
        #region Swagger Examples Attributes
        [SwaggerOperation(Tags = new string[] { "Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<List<EvolutionDTO>>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<List<EvolutionDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.GetPokemonEvolutions))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        #endregion
        public async Task<GetResponseDTO<List<EvolutionDTO>>> GetPokemonEvolutionsAsync(string pokemonName)
        {
            var evolutions = await _pokemonService.GetPokemonEvolutionsAsync(pokemonName);
            return GetResponseDTO<List<EvolutionDTO>>.Ok(evolutions);
        }

        /// <summary>
        /// Obt�m uma lista paginada de Pok�mons.
        /// </summary>
        /// <param name="count">O n�mero de Pok�mons a serem retornados em cada p�gina.</param>
        /// <returns>Uma lista paginada de Pok�mons.</returns>
        [HttpGet("list")]
        #region Swagger Examples Attributes
        [SwaggerOperation(Tags = new string[] { "Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<PokemonListingResponse>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<PokemonListingResponse>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.GetPokemons))]
        #endregion
        public async Task<GetResponseDTO<PokemonListingResponse>> GetPokemonsAsync([FromQuery] int count = 10)
        {
            var pokemonList = await _pokemonService.GetPokemonsAsync(count);
            return GetResponseDTO<PokemonListingResponse>.Ok(pokemonList);
        }

        /// <summary>
        /// Obt�m informa��es sobre a esp�cie de um Pok�mon especificado.
        /// </summary>
        /// <param name="pokemonName">O nome do Pok�mon a ser pesquisado.</param>
        /// <returns>Informa��es sobre a esp�cie do Pok�mon.</returns>
        [HttpGet("{pokemonName}/species")]
        #region Swagger Examples Attributes
        [SwaggerOperation(Tags = new string[] { "Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<EvolutionSpeciesResponseDTO>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<EvolutionSpeciesResponseDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.GetPokemonSpieceAsync))]
        #endregion
        public async Task<GetResponseDTO<EvolutionSpeciesResponseDTO>> GetPokemonSpieceAsync(string pokemonName)
        {
            var species = await _pokemonService.GetPokemonSpieceAsync(pokemonName);
            if (species == null)
            {
                return GetResponseDTO<EvolutionSpeciesResponseDTO>.NotFound();
            }
            return GetResponseDTO<EvolutionSpeciesResponseDTO>.Ok(species);
        }

        /// <summary>
        /// 1 - Obt�m uma lista de Pok�mons aleat�rios.
        /// </summary>
        /// <param name="count">O n�mero de Pok�mons a serem retornados.</param>
        /// <returns>Uma lista de Pok�mons aleat�rios.</returns>
        [HttpGet("random")]
        #region Swagger Examples Attributes
        [OperationOrder(0)]
        [SwaggerOperation(Tags = new string[] { "Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<List<PokemonDTO>>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<PokemonDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.GetRandomPokemonsAsync))]
        #endregion
        public async Task<GetResponseDTO<List<PokemonDTO>>> GetRandomPokemonsAsync([FromQuery] int count = 10)
        {
            var randomPokemons = await _pokemonService.GetRandomPokemonsAsync(count);
            return GetResponseDTO<List<PokemonDTO>>.Ok(randomPokemons);
        }

        /// <summary>
        /// Obt�m o n�mero total de Pok�mons dispon�veis na API
        /// </summary>
        /// <returns>O n�mero total de Pok�mons dispon�veis como um GetResponseDTO do tipo int</returns>
        [HttpGet("count")]
        #region Swagger Examples Attributes
        [SwaggerOperation(Tags = new string[] { "Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.TotalPokemonsCountAsync))]
        #endregion
        public async Task<GetResponseDTO<object>> TotalPokemonsCountAsync()
        {
            var count = await _pokemonService.GetTotalPokemonsCountAsync();
            return GetResponseDTO<object>.Ok(count);
        }

        /// <summary>
        /// 3 - Obt�m todos os mestres Pok�mon registrados no sistema.
        /// </summary>
        /// <returns>O n�mero total de mestres Pok�mon registrados no sistema.</returns>
        [HttpGet("masters")]
        #region Swagger Examples Attributes
        [OperationOrder(2)]
        [SwaggerOperation(Tags = new string[] { "Mestres Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.GetAllPokemonMasters))]
        #endregion
        public async Task<GetResponseDTO<List<PokemonMasterDTO>>> GetAllPokemonMasters()
        {
            var masters = await _pokemonService.GetAllPokemonMasters();
            return GetResponseDTO<List<PokemonMasterDTO>>.Ok(masters);
        }

        /// <summary>
        /// Registra um novo mestre Pok�mon
        /// </summary>
        /// <param name="master">As informa��es do mestre Pok�mon a ser registrado</param>
        /// <returns>Um GetResponseDTO contendo o ID do novo mestre Pok�mon criado</returns>
        [HttpPost("masters/register")]
        #region Swagger Examples Attributes
        [SwaggerOperation(Tags = new string[] { "Mestres Pok�mons" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<PokemonMasterDTO>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<PokemonMasterDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.RegisterPokemonMaster))]
        #endregion
        public async Task<GetResponseDTO<PokemonMasterDTO>> RegisterPokemonMaster([Required] PokemonMasterDTO master)
        {
            if (!ModelState.IsValid)
                return GetResponseDTO<PokemonMasterDTO>.BadRequest(ModelState);

            var newMaster = await _pokemonService.RegisterMasterPokemon(master);
            return GetResponseDTO<PokemonMasterDTO>.Ok(newMaster);
        }

        /// <summary>
        /// 4 - Captura um Pok�mon para um mestre Pok�mon especificado.
        /// </summary>
        /// <param name="masterId">O ID do mestre Pok�mon.</param>
        /// <param name="pokemonName">O nome do Pok�mon a ser capturado.</param>
        /// <param name="forceCapture">Define se a captura deve ser for�ada, mesmo que o Pok�mon j� tenha sido capturado anteriormente pelo mestre.</param>
        /// <returns>O n�mero de Pok�mons capturados pelo mestre at� o momento.</returns>
        [HttpPost("{pokemonName}/{masterId}/capture")]
        #region Swagger Examples Attributes
        [OperationOrder(3)]
        [SwaggerOperation(Tags = new string[] { "Captura Pok�mon" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<CapturedPokemonDTO>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<CapturedPokemonDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.CapturePokemon))]
        #endregion
        public async Task<GetResponseDTO<CapturedPokemonDTO>> CapturePokemon([Required] int masterId, [Required] string pokemonName, bool forceCapture = true)
        {
            if (!ModelState.IsValid)
                return GetResponseDTO<CapturedPokemonDTO>.BadRequest(ModelState);

            var pokemon = await _pokemonService.CapturePokemonAsync(pokemonName, masterId, forceCapture);
            return GetResponseDTO<CapturedPokemonDTO>.Ok(pokemon);
        }

        /// <summary>
        /// 5 - Obt�m todos os Pok�mons capturados filtrando por um mestre ou n�o.
        /// </summary>
        /// <param name="masterId">O ID do mestre Pok�mon.</param>
        /// <returns>O n�mero de Pok�mons capturados pelo mestre at� o momento.</returns>
        [HttpGet("captured/{masterId}")]
        [HttpGet("captured")]
        #region Swagger Examples Attributes
        [OperationOrder(4)]
        [SwaggerOperation(Tags = new string[] { "Captura Pok�mon" })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetResponseDTO<CapturedPokemonDTO>))]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(GetResponseDTO<CapturedPokemonDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", typeof(GetResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GetResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "BadRequest", typeof(GetResponseDTO))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(UserErrorsSamples.InternalServerError))]
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(UserErrorsSamples.NotFound))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(UserErrorsSamples.RandomBadRequest))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PokemonOKSamples.CaptureListPokemon))]
        #endregion
        public async Task<GetResponseDTO<List<CapturedPokemonDTO>>> GetAllCapturedPokemons(int? masterId = null)
        {
            if (!ModelState.IsValid)
                return GetResponseDTO<List<CapturedPokemonDTO>>.BadRequest(ModelState);

            var allCapturedPokemons = await _pokemonService.GetAllCapturedPokemons(masterId);
            return GetResponseDTO<List<CapturedPokemonDTO>>.Ok(allCapturedPokemons);
        }
    }
}