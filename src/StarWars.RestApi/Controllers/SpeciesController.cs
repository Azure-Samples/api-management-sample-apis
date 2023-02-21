// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using StarWars.Data;
using StarWars.Data.Models;

namespace StarWars.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeciesController : ControllerBase
{
    private readonly StarWarsData datamodel;

    public SpeciesController(StarWarsData datamodel)
    {
        this.datamodel = datamodel;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Species>))]
    public IEnumerable<Species> GetAllSpeciess()
    {
        return datamodel.Species.Values;
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(Species))]
    [ProducesResponseType(404)]
    public IActionResult GetSpeciesById([FromRoute] int id)
    {
        if (datamodel.Species.ContainsKey(id))
        {
            return Ok(datamodel.Species[id]);
        }
        return NotFound();
    }
}
