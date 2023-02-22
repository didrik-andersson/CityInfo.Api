using System.Text.Json;
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
// [Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cities")]
public class CitiesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICityInfoRepository _cityInfoRepository;
    const int maxCitiesPageSize = 20;

    public CitiesController(ICityInfoRepository cityInfoRepository,
    IMapper mapper)
    {
        _mapper = mapper;
        _cityInfoRepository = cityInfoRepository ??
            throw new ArgumentNullException(nameof(cityInfoRepository));
    }

    public ICityInfoRepository CityInfoRepository { get; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        [FromQuery] string? name, [FromQuery] string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > maxCitiesPageSize) 
        {
            pageSize = maxCitiesPageSize;
        }

        var (cityEntities, paginationMetadata) = await _cityInfoRepository
            .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

        Response.Headers.Add("X-Pagination", 
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    [HttpGet("{id}",  Name = "GetCity")]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

        if (city == null)
        {
            return NotFound();
        }

        if (includePointsOfInterest)
        {
            return Ok(_mapper.Map<CityDto>(city));
        }

        return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }

    [HttpPost]
    public async Task<ActionResult> CreateCity(CityForCreationDto cityForCreationDto)
    {
        var finalCity = _mapper.Map<Entities.City>(cityForCreationDto);

        _cityInfoRepository.AddCity(finalCity);
        await _cityInfoRepository.SaveChangesAsync();

        var cityToReturn = _mapper.Map<CityDto>(finalCity);

        return CreatedAtRoute("GetCity",
        new
        {
            id = cityToReturn.Id,
            includePointsOfInterest = false
        },
        cityToReturn);
    }
}