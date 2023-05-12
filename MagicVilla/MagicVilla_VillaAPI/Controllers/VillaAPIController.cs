using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;


[Route("api/VillaAPI")]   // This is an attribute route/endpoint. Action methods on controllers annotated with ApiController attribute must be attribute routed.
[ApiController] // the ApiController attribute notifies ASP.NET that this class will be an API controller
public class VillaAPIController : ControllerBase
{
    // first API endpoint
    
    [HttpGet] // identifies the action method as an HTTP method
    public IEnumerable<VillaDTO> GetVillas()
    {
        return VillaStore.villaList;

    }


    [HttpGet("id")]
    public VillaDTO GetVillas(int id)
    {
        // ReturnFor each u in the collection, check if its Id property is equal to the given id value."
        return VillaStore.villaList.FirstOrDefault(u => u.Id == id);    

    }

}
