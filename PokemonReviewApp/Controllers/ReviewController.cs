using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_mapper.Map<ICollection<ReviewDto>>(_unitOfWork.Reviews.GetAll()));

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            if (!_unitOfWork.Reviews.IsExists(id))
                return NotFound($"There is no Review with ID: {id}");

            var pokemon = _mapper.Map<ReviewDto>(_unitOfWork.Reviews.GetById(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("IsExists")]
        public IActionResult IsExists(int id) => Ok(_unitOfWork.Reviews.IsExists(id));

        [HttpGet("GetReviewsOfAPokemon/{pokemonId}")]
        public IActionResult GetReviewsOfAPokemon(int pokemonId)
        {
            var reviews = _mapper.Map<ICollection<ReviewDto>>(_unitOfWork.Reviews.GetReviewsOfAPokemon(pokemonId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost("CreateReview")]
        public IActionResult Create([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            var isValid = _unitOfWork.Reviews.GetAll().
                Where(c => (c.Title.Trim().ToLower() == dto.Title.Trim().ToLower()))
                .FirstOrDefault();

            if (isValid != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isValidReviewer = _unitOfWork.Reviewers.IsExists(reviewerId);
            if (!isValidReviewer) return BadRequest("Invalid Reviewer Id");

            var isValidPokemon = _unitOfWork.Pokemons.IsExists(pokemonId);
            if (!isValidPokemon) return BadRequest("Invalid Pokemon Id");

            var review = _mapper.Map<Review>(dto);
            review.Reviewer = _unitOfWork.Reviewers.GetById(reviewerId);
            review.Pokemon = _unitOfWork.Pokemons.GetById(pokemonId);
            if (!_unitOfWork.Reviews.Create(review))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(review);
        }

        [HttpPut("UpdateReview/{reviewId}")]
        public IActionResult Update(int reviewId, [FromBody] ReviewDto dto)
        {
            if (dto == null) return BadRequest(ModelState);

            if (!_unitOfWork.Reviews.IsExists(reviewId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var review = _unitOfWork.Reviews.GetById(reviewId);

            _mapper.Map(dto, review);

            if (!_unitOfWork.Reviews.Update(review))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(review);
        }

        [HttpDelete("DeleteReview/{reviewId}")]
        public IActionResult Delete(int reviewId)
        {
            if (!_unitOfWork.Reviews.IsExists(reviewId)) return NotFound();
                
            var review = _unitOfWork.Reviews.GetById(reviewId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_unitOfWork.Reviews.Delete(review))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            _unitOfWork.Complete();
            return Ok(review);
        }

        [HttpDelete("DeleteReviewsByReviewer/{reviewerId}")]
        public IActionResult DeleteReviewsByReviewer(int reviewerId)
        {
            if (!_unitOfWork.Reviewers.IsExists(reviewerId))
                return NotFound();

            var reviewsToDelete = _unitOfWork.Reviewers.GetReviewsByAReviewer(reviewerId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_unitOfWork.Reviews.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

    }
}
