// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using StarWars.Data;
using StarWars.Data.Models;

namespace StarWars.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilmController : ControllerBase
{
    private readonly StarWarsData datamodel;

    public FilmController(StarWarsData datamodel)
    {
        this.datamodel = datamodel;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Film>))]
    public IEnumerable<Film> GetAllFilms()
    {
        return datamodel.Films.Values;
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type=typeof(Film))]
    [ProducesResponseType(404)]
    public IActionResult GetFilmById([FromRoute] int id)
    {
        if (datamodel.Films.ContainsKey(id))
        {
            return Ok(datamodel.Films[id]);
        }
        return NotFound();
    }
}
