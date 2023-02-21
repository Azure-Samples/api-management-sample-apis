// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using StarWars.Data;
using StarWars.Data.Models;

namespace StarWars.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly StarWarsData datamodel;

    public PersonController(StarWarsData datamodel)
    {
        this.datamodel = datamodel;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Person>))]
    public IEnumerable<Person> Get()
    {
        return datamodel.People.Values;
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(Person))]
    [ProducesResponseType(404)]
    public IActionResult Get(int id)
    {
        if (datamodel.People.ContainsKey(id))
        {
            return Ok(datamodel.People[id]);
        }
        return NotFound();
    }
}

