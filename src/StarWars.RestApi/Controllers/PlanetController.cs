// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using StarWars.Data;
using StarWars.Data.Models;

namespace StarWars.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlanetController : ControllerBase
{
    private readonly StarWarsData datamodel;

    public PlanetController(StarWarsData datamodel)
    {
        this.datamodel = datamodel;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Planet>))]
    public IEnumerable<Planet> GetAllPlanets()
    {
        return datamodel.Planets.Values;
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(Planet))]
    [ProducesResponseType(404)]
    public IActionResult GetPlanetById([FromRoute] int id)
    {
        if (datamodel.Planets.ContainsKey(id))
        {
            return Ok(datamodel.Planets[id]);
        }
        return NotFound();
    }
}
