using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_mapper.Map<ICollection<ReviewerDto>>(_unitOfWork.Reviewers.GetAll()));

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            if (!_unitOfWork.Reviewers.IsExists(id))
                return NotFound($"There is no Reviewer with ID: {id}");

            var pokemon = _mapper.Map<ReviewerDto>(_unitOfWork.Reviewers.GetById(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("IsExists")]
        public IActionResult IsExists(int id) => Ok(_unitOfWork.Reviewers.IsExists(id));

        [HttpGet("GetReviewsByAReviewer/{reviewerId}")]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            if (!_unitOfWork.Reviewers.IsExists(reviewerId))
                return NotFound($"There is no Reviewer with ID: {reviewerId}");

            var reviews = _mapper.Map<ICollection<ReviewDto>>(_unitOfWork.Reviewers.GetReviewsByAReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost("CreateReviewer")]
        public IActionResult Create([FromBody] ReviewerDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            var isValid = _unitOfWork.Reviewers.GetAll().
                Where(c => (c.LastName.Trim().ToLower() == dto.LastName.Trim().ToLower()) &&
                           (c.FirstName.Trim().ToLower() == dto.FirstName.Trim().ToLower()))
                .FirstOrDefault();

            if (isValid != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }
            _unitOfWork.Complete();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewer = _mapper.Map<Reviewer>(dto);
            if (!_unitOfWork.Reviewers.Create(reviewer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(reviewer);
        }

        [HttpPut("UpdateReviewer/{reviewerId}")]
        public IActionResult Update(int reviewerId, [FromBody] ReviewerDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            if (!_unitOfWork.Reviewers.IsExists(reviewerId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var reviewer = _unitOfWork.Reviewers.GetById(reviewerId);

            _mapper.Map(dto, reviewer);

            if (!_unitOfWork.Reviewers.Update(reviewer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(reviewer);
        }

        [HttpDelete("DeleteReviewer/{reviewerId}")]
        public IActionResult Delete(int reviewerId)
        {
            if (!_unitOfWork.Reviewers.IsExists(reviewerId)) return NotFound();

            var reviewer = _unitOfWork.Reviewers.GetById(reviewerId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_unitOfWork.Reviewers.Delete(reviewer))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(reviewer);
        }
    }
}
