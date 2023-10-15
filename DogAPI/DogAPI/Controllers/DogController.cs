﻿using BLL.Services.Interfaces;
using Common;
using Common.DTO.DogDTO;
using Common.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogAPI.Controllers;

[Route("/")]
[ApiController]
public class DogController : ControllerBase
{
    private readonly IDogService _dogService;

    public DogController(IDogService dogService)
    {
        _dogService = dogService;
    }

    [HttpGet("dog/{name}")]
    public async Task<IActionResult> GetDogByName(string name)
    {
        return Ok(await _dogService.GetDogByName(name));
    }

    [HttpPost("dog")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> InsertDog(CreateDogDTO dog)
    {
        try
        {
            var result = await _dogService.AddDog(dog);
            return CreatedAtAction(nameof(GetDogByName), new { name = result.Name }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("dog/{name}")]
    public async Task<IActionResult> UpdateDog(string name, [FromBody] UpdateDogDTO dog)
    {
        try
        {
            return Ok(await _dogService.UpdateDog(name, dog));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(name);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("dog/{name}")]
    public async Task<IActionResult> DeleteDog(string name)
    {
        return await _dogService.DeleteDog(name) ? Ok() : NotFound();
    }

    [HttpGet("dogs")]
    public IActionResult GetAll([FromQuery] string? attribute = null,
        [FromQuery] string? order = null,
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null)
    {
        if (pageNumber.HasValue ^ pageSize.HasValue)
        {
            return BadRequest("Not found parameters for pagination");
        }
        
        try
        {
            var request = new GetDogsRequest
            {
                Attribute = string.IsNullOrWhiteSpace(attribute) ? null : attribute,
                Order = GetOrderByAbbreviation(order),
                Pagination = pageNumber != null && pageSize != null
                    ? new PaginationModel(pageNumber.Value, pageSize.Value)
                    : null
            };

            var result = _dogService.GetDogs(request);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private Order GetOrderByAbbreviation(string? value)
    {
        return value?.ToLower() == "desc" ? Order.Descending : Order.Ascending;
    }
}