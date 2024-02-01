using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OwnerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_mapper.Map<ICollection<OwnerDto>>(_unitOfWork.Owners.GetAll()));

        [HttpGet("GetById/{ownerId}")]
        public IActionResult GetById(int ownerId)
        {
            if (!_unitOfWork.Owners.IsExists(ownerId))
                return NotFound($"There is no category with this Id: {ownerId}");

            var owner = _mapper.Map<OwnerDto>(_unitOfWork.Owners.GetById(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("IsExists/{ownerId}")]
        public IActionResult IsExists(int ownerId) => Ok(_unitOfWork.Owners.IsExists(ownerId));

        [HttpGet("GetOwnersOfAPokemon/{pokemonId}")]
        public IActionResult GetOwnersOfAPokemon(int pokemonId) 
        {
            var owners = _mapper.Map<ICollection<OwnerDto>>(_unitOfWork.Owners.GetOwnersOfAPokemon(pokemonId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("GetPokemonsOfAnOwner/{ownerId}")]
        public IActionResult GetPokemonsOfAnOwner(int ownerId)
        {
            if (!_unitOfWork.Owners.IsExists(ownerId))
                return NotFound($"There is no category with this Id: {ownerId}");

            var pokemons = _mapper.Map<ICollection<PokemonDto>>(_unitOfWork.Owners.GetPokemonsOfAnOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost("CreateOwner")]
        public IActionResult Create([FromQuery] int countryId, [FromBody] OwnerDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            var isValid = _unitOfWork.Owners.GetAll().
                Where(c => (c.LastName.Trim().ToLower() == dto.LastName.Trim().ToLower()) && 
                           (c.FirstName.Trim().ToLower() == dto.FirstName.Trim().ToLower()))
                .FirstOrDefault();
            if (isValid != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isValidCountry = _unitOfWork.Countries.IsExists(countryId);
            if(!isValidCountry) return BadRequest("Invalid Country Id");

            var owner = _mapper.Map<Owner>(dto);
            owner.Country = _unitOfWork.Countries.GetById(countryId);
            if (!_unitOfWork.Owners.Create(owner))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(owner);
        }

        [HttpPut("UpdateOwner/{ownerId}")]
        public IActionResult Update(int ownerId, [FromBody] OwnerDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            if (!_unitOfWork.Owners.IsExists(ownerId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var owner = _unitOfWork.Owners.GetById(ownerId);

            _mapper.Map(dto, owner);

            if (!_unitOfWork.Owners.Update(owner))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(owner);
        }

        [HttpDelete("DeleteOwner/{ownerId}")]
        public IActionResult Delete(int ownerId)
        {
            if (!_unitOfWork.Owners.IsExists(ownerId)) return NotFound();

            var owner = _unitOfWork.Owners.GetById(ownerId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_unitOfWork.Owners.Delete(owner))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(owner);
        }
    }
}
