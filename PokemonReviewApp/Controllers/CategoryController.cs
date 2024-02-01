using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_mapper.Map<ICollection<CategoryDto>>(_unitOfWork.Categories.GetAll()));

        [HttpGet("GetById/{categoryId}")]
        public IActionResult GetById(int categoryId)
        {
            if(!_unitOfWork.Categories.IsExists(categoryId))
                return NotFound($"There is no category with this Id: {categoryId}");
            
            var category = _mapper.Map<CategoryDto>(_unitOfWork.Categories.GetById(categoryId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("IsExists/{categoryId}")]
        public IActionResult IsExists(int categoryId) => Ok(_unitOfWork.Categories.IsExists(categoryId));

        [HttpGet("GetPokemonsByCategory/{categoryId}")]
        public IActionResult GetPokemonsByCategory(int categoryId)
        {
            if (!_unitOfWork.Categories.IsExists(categoryId))
                return NotFound($"There is no category with this Id: {categoryId}");

            var pokemons = _mapper.Map<ICollection<PokemonDto>>(_unitOfWork.Categories.GetPokemonsByCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost("CreateCategory")]
        public IActionResult Create([FromBody] CategoryDto dto)
        {
            if(dto == null) return BadRequest(ModelState);

            var isValid = _unitOfWork.Categories.GetAll().Where(c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower()).FirstOrDefault();
            if (isValid != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }


            if(!ModelState.IsValid) return BadRequest(ModelState);

            var category = _mapper.Map<Category>(dto);
            if (!_unitOfWork.Categories.Create(category))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(category);
        }

        [HttpPut("UpdateCategory/{categoryId}")]
        public IActionResult Update(int categoryId, [FromBody] CategoryDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            if(!_unitOfWork.Categories.IsExists(categoryId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var category = _unitOfWork.Categories.GetById(categoryId);

            _mapper.Map(dto, category);

            if (!_unitOfWork.Categories.Update(category))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(category);
        }

        [HttpDelete("DeleteCategory/{categoryId}")]
        public IActionResult Delete(int categoryId)
        {
            if(!_unitOfWork.Categories.IsExists(categoryId)) return NotFound();

            var category = _unitOfWork.Categories.GetById(categoryId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if(!_unitOfWork.Categories.Delete(category))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(category);
        }

    }
}
