﻿using BLL.Services.Interfaces;
using Common.DTO.DogDTO;
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
        try
        {
            var result = pageNumber == null || pageSize == null
                ? _dogService.GetDogs(attribute ?? "name", order)
                : _dogService.GetDogs(attribute ?? "name", order, pageNumber!.Value, pageSize!.Value);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}