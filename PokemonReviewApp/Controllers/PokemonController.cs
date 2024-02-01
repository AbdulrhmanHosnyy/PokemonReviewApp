using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repositories;
using System.Collections.Generic;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PokemonController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_mapper.Map<ICollection<PokemonDto>>(_unitOfWork.Pokemons.GetAll()));

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            if(!_unitOfWork.Pokemons.IsExists(id)) 
                return NotFound($"There is no Pokemon with ID: {id}");

            var pokemon = _mapper.Map<PokemonDto>(_unitOfWork.Pokemons.GetById(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("IsExists")]
        public IActionResult IsExists(int id) => Ok(_unitOfWork.Pokemons.IsExists(id));

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name) => Ok(_mapper.Map<PokemonDto>(_unitOfWork.Pokemons.Find(p => p.Name == name)));

        [HttpGet("GetPokemonRating")]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_unitOfWork.Pokemons.IsExists(id))
                return NotFound($"There is no Pokemon with ID: {id}");

            var rating = _unitOfWork.Pokemons.GetPokemonRating(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }

        [HttpPost("CreatePokemon")]
        public IActionResult Create([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            var isValid = _unitOfWork.Pokemons.GetAll().Where(c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower()).FirstOrDefault();
            if (isValid != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pokemon = _mapper.Map<Pokemon>(dto);
            if (!_unitOfWork.Pokemons.Create(ownerId, categoryId, pokemon))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(pokemon);
        }

        [HttpPut("UpdatePokemon/{pokemonId}")]
        public IActionResult Update(int pokemonId, [FromBody] PokemonDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            if (!_unitOfWork.Pokemons.IsExists(pokemonId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var pokemon = _unitOfWork.Pokemons.GetById(pokemonId);

            _mapper.Map(dto, pokemon);

            if (!_unitOfWork.Pokemons.Update(pokemon))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(pokemon);
        }

        [HttpDelete("DeletePokemon/{pokemonId}")]
        public IActionResult Delete(int pokemonId)
        {
            if (!_unitOfWork.Pokemons.IsExists(pokemonId)) return NotFound();

            var pokemon = _unitOfWork.Pokemons.GetById(pokemonId);
            var reviewsOfPokemon = _unitOfWork.Reviews.GetReviewsOfAPokemon(pokemonId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_unitOfWork.Reviews.DeleteReviews(reviewsOfPokemon.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            if (!_unitOfWork.Pokemons.Delete(pokemon))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(pokemon);
        }
    }
}
