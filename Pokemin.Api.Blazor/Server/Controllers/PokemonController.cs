using Microsoft.AspNetCore.Mvc;
using Pokemon.Application.DTO;
using Pokemon.Services;
using System.ComponentModel.DataAnnotations;

namespace Pokemin.Api.Blazor.Server.Controllers
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
        [HttpGet]
        public async Task<ActionResult<List<PokemonListingItemResponse>>> GetAllPokemonsAsync([FromQuery] int? limit = null)
        {
            var pokemons = await _pokemonService.GetAllPokemonsAsync(limit);
            return Ok(pokemons);
        }

        /// <summary>
        /// Busca um Pok�mon pelo seu nome na API
        /// </summary>
        /// <param name="pokemonName">O nome do Pok�mon a ser procurado</param>
        /// <returns>Um ActionResult contendo um PokemonDTO se o Pok�mon for encontrado, caso contr�rio retorna NotFound</returns>
        [HttpGet("{pokemonName}")]
        public async Task<ActionResult<PokemonDTO>> SearchPokemonAsync(string pokemonName)
        {
            var pokemon = await _pokemonService.SearchPokemonAsync(pokemonName);
            if (pokemon == null)
            {
                return NotFound();
            }
            return Ok(pokemon);
        }

        /// <summary>
        /// Obt�m informa��es sobre as evolu��es de um Pok�mon especificado.
        /// </summary>
        /// <param name="pokemonName">O nome do Pok�mon a ser pesquisado.</param>
        /// <returns>Uma lista de informa��es sobre as evolu��es do Pok�mon.</returns>
        [HttpGet("{pokemonName}/evolutions")]
        public async Task<ActionResult<List<EvolutionDTO>>> GetPokemonEvolutionsAsync(string pokemonName)
        {
            var evolutions = await _pokemonService.GetPokemonEvolutionsAsync(pokemonName);
            return Ok(evolutions);
        }

        /// <summary>
        /// Obt�m uma lista paginada de Pok�mons.
        /// </summary>
        /// <param name="count">O n�mero de Pok�mons a serem retornados em cada p�gina.</param>
        /// <returns>Uma lista paginada de Pok�mons.</returns>
        [HttpGet("list")]
        public async Task<ActionResult<PokemonListingResponse>> GetPokemonsAsync([FromQuery] int count = 10)
        {
            var pokemonList = await _pokemonService.GetPokemonsAsync(count);
            return Ok(pokemonList);
        }

        /// <summary>
        /// Obt�m informa��es sobre a esp�cie de um Pok�mon especificado.
        /// </summary>
        /// <param name="pokemonName">O nome do Pok�mon a ser pesquisado.</param>
        /// <returns>Informa��es sobre a esp�cie do Pok�mon.</returns>
        [HttpGet("{pokemonName}/species")]
        public async Task<ActionResult<EvolutionSpeciesResponseDTO>> GetPokemonSpieceAsync(string pokemonName)
        {
            var species = await _pokemonService.GetPokemonSpieceAsync(pokemonName);
            if (species == null)
            {
                return NotFound();
            }
            return Ok(species);
        }

        /// <summary>
        /// Obt�m uma lista de Pok�mons aleat�rios.
        /// </summary>
        /// <param name="count">O n�mero de Pok�mons a serem retornados.</param>
        /// <returns>Uma lista de Pok�mons aleat�rios.</returns>
        [HttpGet("random")]
        public async Task<ActionResult<List<PokemonDTO>>> GetRandomPokemonsAsync([FromQuery] int count = 10)
        {
            var randomPokemons = await _pokemonService.GetRandomPokemonsAsync(count);
            return Ok(randomPokemons);
        }

        /// <summary>
        /// Obt�m o n�mero total de Pok�mons dispon�veis na API
        /// </summary>
        /// <returns>O n�mero total de Pok�mons dispon�veis como um ActionResult do tipo int</returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> TotalPokemonsCountAsync()
        {
            var count = await _pokemonService.GetTotalPokemonsCountAsync();
            return Ok(count);
        }

        /// <summary>
        /// Obt�m todos os mestres Pok�mon registrados no sistema.
        /// </summary>
        /// <returns>O n�mero total de mestres Pok�mon registrados no sistema.</returns>
        [HttpGet("masters")]
        public async Task<ActionResult<int>> GetAllPokemonMasters()
        {
            var masters = await _pokemonService.GetAllPokemonMasters();
            return Ok(masters);
        }

        /// <summary>
        /// Registra um novo mestre Pok�mon
        /// </summary>
        /// <param name="master">As informa��es do mestre Pok�mon a ser registrado</param>
        /// <returns>Um ActionResult contendo o ID do novo mestre Pok�mon criado</returns>
        [HttpPost("masters/register")]
        public async Task<ActionResult<int>> RegisterPokemonMaster([Required] PokemonMasterDTO master)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newMaster = await _pokemonService.RegisterMasterPokemon(master);
            return Ok(newMaster);
        }

        /// <summary>
        /// Captura um Pok�mon para um mestre Pok�mon especificado.
        /// </summary>
        /// <param name="masterId">O ID do mestre Pok�mon.</param>
        /// <param name="pokemonName">O nome do Pok�mon a ser capturado.</param>
        /// <param name="forceCapture">Define se a captura deve ser for�ada, mesmo que o Pok�mon j� tenha sido capturado anteriormente pelo mestre.</param>
        /// <returns>O n�mero de Pok�mons capturados pelo mestre at� o momento.</returns>
        [HttpPost("{pokemonName}/{masterId}/capture")]
        public async Task<ActionResult<int>> CapturePokemon([Required] int masterId, [Required] string pokemonName, bool? forceCapture = true)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = await _pokemonService.CapturePokemonAsync(pokemonName, masterId, forceCapture.Value);
            return Ok(count);
        }

        /// <summary>
        /// Obt�m todos os Pok�mons capturados por um mestre Pok�mon especificado.
        /// </summary>
        /// <param name="masterId">O ID do mestre Pok�mon.</param>
        /// <returns>O n�mero de Pok�mons capturados pelo mestre at� o momento.</returns>
        [HttpGet("captured/{masterId}")]
        [HttpGet("captured")]
        public async Task<ActionResult<int>> GetAllCapturedPokemons(int? masterId = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var allCapturedPokemons = await _pokemonService.GetAllCapturedPokemons(masterId);
            return Ok(allCapturedPokemons);
        }
    }
}