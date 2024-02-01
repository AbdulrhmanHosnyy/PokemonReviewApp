using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_mapper.Map<ICollection<CountryDto>>(_unitOfWork.Countries.GetAll()));

        [HttpGet("GetById/{countryId}")]
        public IActionResult GetById(int countryId)
        {
            if (!_unitOfWork.Countries.IsExists(countryId))
                return NotFound($"There is no country with this Id: {countryId}");

            var country = _mapper.Map<CountryDto>(_unitOfWork.Countries.GetById(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("IsExists/{countryId}")]
        public IActionResult IsExists(int countryId) => Ok(_unitOfWork.Countries.IsExists(countryId));

        [HttpGet("GetContryByOwner/{ownerId}")]
        public IActionResult GetContryByOwner(int ownerId) 
        {
            var country = _mapper.Map<CountryDto>(_unitOfWork.Countries.GetCountryByOwner(ownerId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("GetOwnersByCountry/{countryId}")]
        public IActionResult GetOwnersByCountry(int countryId)
        {
            if (!_unitOfWork.Countries.IsExists(countryId))
                return NotFound($"There is no country with this Id: {countryId}");

            var owners = _mapper.Map<ICollection<OwnerDto>>(_unitOfWork.Countries.GetOwnersByCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpPost("CreateCountry")]
        public IActionResult Create([FromBody] CountryDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            var isValid = _unitOfWork.Countries.GetAll().Where(c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower()).FirstOrDefault();
            if (isValid != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid) return BadRequest(ModelState);

            var country = _mapper.Map<Country>(dto);
            if (!_unitOfWork.Countries.Create(country))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(country);
        }

        [HttpPut("UpdateCountry/{countryId}")]
        public IActionResult Update(int countryId, [FromBody] CountryDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            if (!_unitOfWork.Countries.IsExists(countryId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var country = _unitOfWork.Countries.GetById(countryId);

           _mapper.Map(dto, country);

            if (!_unitOfWork.Countries.Update(country))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(country);
        }

        [HttpDelete("DeleteCountry/{countryId}")]
        public IActionResult Delete(int countryId)
        {
            if (!_unitOfWork.Countries.IsExists(countryId)) return NotFound();

            var country = _unitOfWork.Countries.GetById(countryId);
                
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_unitOfWork.Countries.Delete(country))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();

            return Ok(country);
        }
    }
}
