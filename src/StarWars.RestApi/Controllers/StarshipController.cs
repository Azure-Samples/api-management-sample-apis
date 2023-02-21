// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using StarWars.Data;
using StarWars.Data.Models;

namespace StarWars.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StarshipController : ControllerBase
{
    private readonly StarWarsData datamodel;

    public StarshipController(StarWarsData datamodel)
    {
        this.datamodel = datamodel;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Starship>))]
    public IEnumerable<Starship> GetAllStarships()
    {
        return datamodel.Starships.Values;
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(Starship))]
    [ProducesResponseType(404)]
    public IActionResult GetStarshipById([FromRoute] int id)
    {
        if (datamodel.Starships.ContainsKey(id))
        {
            return Ok(datamodel.Starships[id]);
        }
        return NotFound();
    }
}
