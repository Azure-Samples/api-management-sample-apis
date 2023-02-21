// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using StarWars.Data;
using StarWars.Data.Models;

namespace StarWars.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly StarWarsData datamodel;

    public VehicleController(StarWarsData datamodel)
    {
        this.datamodel = datamodel;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Vehicle>))]
    public IEnumerable<Vehicle> GetAllVehicles()
    {
        return datamodel.Vehicles.Values;
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(Vehicle))]
    [ProducesResponseType(404)]
    public IActionResult GetVehicleById([FromRoute] int id)
    {
        if (datamodel.Vehicles.ContainsKey(id))
        {
            return Ok(datamodel.Vehicles[id]);
        }
        return NotFound();
    }
}
