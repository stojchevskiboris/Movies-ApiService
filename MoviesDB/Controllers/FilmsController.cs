using Microsoft.AspNetCore.Mvc;
using MoviesDB.Data.Repository;
using MoviesDB.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace MoviesDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly IMoviesRepository<Film> _filmsRepository;
        private readonly IMapper _mapper;


        public FilmsController(IMoviesRepository<Film> filmsRepository, IMapper mapper)
        {
            _filmsRepository = filmsRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("All", Name = "GetAllFilms")]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilmsAsync()
        {
            var films = await _filmsRepository.GetAllAsync();

            //OK - 200 - Success
            return Ok(films);
        }

        [HttpGet("lastid")]
        public async Task<ActionResult<int>> GetLastId()
        {
            var lastFilm = await _filmsRepository.GetLastAsync();

            return lastFilm != null ? Ok(lastFilm.FilmId) : NotFound();
        }

        [HttpGet("firstid")]
        public async Task<ActionResult<int>> GetFirstId()
        {
            var firstFilm = await _filmsRepository.GetFirstAsync();

            return firstFilm != null ? Ok(firstFilm.FilmId) : NotFound();
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetFilmById")]
        public async Task<ActionResult<Film>> GetFilmByIdAsync(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
            {
                return BadRequest();
            }

            var film = await _filmsRepository.GetByIdAsync(f => f.FilmId == id);
            //NotFound - 404 - NotFound - Client error
            if (film == null)
            {
                return NotFound($"The film with id {id} not found");
            }

            //OK - 200 - Success
            return Ok(film);
        }

        [HttpGet]
        [Route("{page:int}/{size:int}", Name = "GetFilmsPaginated")]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilmsPaginatedAsync(int page, int size)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (page <= 0 || size <= 0)
            {
                return BadRequest();
            }
            var films = _filmsRepository.GetPaginated(page, size);
            return Ok(films);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<Film>> CreateFilmAsync([FromBody] Film model)
        {
            if (model == null)
                return BadRequest();

            await _filmsRepository.CreateAsync(model);

            //Status - 201

            return Ok(model);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> UpdateFilmAsync([FromBody] Film model)
        {
            if (model == null || model.FilmId <= 0)
                BadRequest();

            var existing = await _filmsRepository.GetByIdAsync(f => f.FilmId == model.FilmId, true);

            if (existing == null)
                return NotFound();

            var newRecord = _mapper.Map<Film>(model);

            await _filmsRepository.UpdateAsync(newRecord);

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        public async Task<ActionResult> UpdateFilmPartialAsync(int id, [FromBody] JsonPatchDocument<Film> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                BadRequest();

            var existing = await _filmsRepository.GetByIdAsync(s => s.FilmId == id, true);

            if (existing == null)
                return NotFound();

            var dto = _mapper.Map<Film>(existing);

            patchDocument.ApplyTo(dto);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existing = _mapper.Map<Film>(dto);

            await _filmsRepository.UpdateAsync(existing);

            //204 - NoContent
            return NoContent();
        }


        [HttpDelete("Delete/{id}", Name = "DeleteFilmById")]
        public async Task<ActionResult<bool>> DeleteFilmAsync(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
                return BadRequest();

            var film = await _filmsRepository.GetByIdAsync(s => s.FilmId == id);
            //NotFound - 404 - NotFound - Client error
            if (film == null)
                return NotFound($"The film with id {id} is not found");

            await _filmsRepository.DeleteAsync(film);

            //OK - 200 - Success
            return Ok(true);
        }

    }
}
