using AutoMapper.QueryableExtensions;
using DAL.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClubController : ControllerBase
{
    private readonly IClubRepository _clubRepository;

    public ClubController(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Club>>> GetClubs(){

        return (_clubRepository.GetClubs() is IQueryable<Club> allClubs)
            ? Ok(await allClubs
                .ToListAsync())
            : NotFound("No clubs found");
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Club>>> GetClub(int id){

        return (_clubRepository.GetClubById(id) is IQueryable<Club> clubById)
            ? Ok(await clubById
                .ToListAsync())
            : NotFound($"No club found with id {id}");
    }
    
    [HttpPost]
    public async Task<ActionResult<Club>> CreateClub([FromBody]  newBook)
    {
        return (await _bookRepo.CreateBookAsync(_mapper.Map<Book>(newBook)) is Book createdBook)
            ? Created(Url.Link("GetBookById", new { id = createdBook?.Bno }),
                _mapper.Map<BookReadDTO>(createdBook))
            : BadRequest();
        //Or: return CreatedAtAction(nameof(GetBook), new { id = createdBook.Bno }, _mapper.Map<BookReadDTO>(createdBook));
    }

    
    



}
