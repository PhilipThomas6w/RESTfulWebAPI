using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;


[Route("api/VillaAPI")] // This is an attribute route/endpoint. Action methods on controllers annotated with ApiController attribute must be attribute routed.
[ApiController] // the ApiController attribute notifies ASP.NET that this class will be an API controller
public class VillaAPIController : ControllerBase
{
    // first API endpoint

    [HttpGet] // identifies the action method as an HTTP method
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        return Ok(VillaStore.villaList);

    }


    [HttpGet("{id:int}", Name = "GetVilla")]

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    //[ProducesResponseType(200, Type = typeof(VillaDTO))] // If we used this, we could just use ActionResult<> as the return type
    public ActionResult<VillaDTO> GetVillas(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

        if (villa == null)
        {
            return NotFound("No villa with that id exists in the datastore.");
        }

        // In the villaList stored in the VillaStore, return the first u (i.e., VillaDTO) in the collection whose Id property is equal to the given id value."
        return Ok(villa);

    }

    [HttpPost]
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}
        // We would only add the above if statement if we hadn't assigned the [ApiController] attribute. It's checking if the required properties have been included in the Http request. The [ApiController] attribute does this automatically.

        // custom ModelState validation
        if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null);
        {
            ModelState.AddModelError("", "Villa already exists in datastore!");
        }


        if (villaDTO == null)
        {
            return BadRequest(villaDTO);
        }

        if (villaDTO.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // Below is very convoluted, because we are using a fake database i.e., the VillaStore.cs, which is just a list. Auto-increment is easy with SQL.
        villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

        VillaStore.villaList.Add(villaDTO);

        return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);  // Notice we are passing the name declared in [HttpGet("{id:int}", Name = "GetVilla")]

    }


    [HttpDelete("{id:int}", Name = "DeleteVilla")]

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteVilla(int id)    // If we use IActionResult rather than ActionResult<>, we don't need to declare the return type
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

        if (villa == null)
        {
            return NotFound();
        }

        
        VillaStore.villaList.Remove(villa);
        return NoContent();
    }


    [HttpPut ("{id:int}", Name = "UpdateVilla")]

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
    {
        if (villaDTO == null || id != villaDTO.Id)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
        villa.Name = villaDTO.Name;
        villa.Occupancy = villaDTO.Occupancy;
        villa.Sqft = villaDTO.Sqft;

        return NoContent();

    }


    // Refer to https://jsonpatch.com/ for instructions on how to edit the body of the Http request
    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

        if (villa == null)
        {
            return BadRequest();
        }

        patchDTO.ApplyTo(villa, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }


}

