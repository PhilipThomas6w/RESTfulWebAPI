using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;


[Route("api/villaAPI")]   // This is an attribute route/endpoint. Action methods on controllers annotated with ApiController attribute must be attribute routed.
[ApiController] // the ApiController attribute notifies ASP.NET that this class will be an API controller
public class VillaAPIController : ControllerBase
{
    // first API endpoint
    
    [HttpGet] // identifies the action method as an HTTP method
    public IEnumerable<VillaDTO> GetVillas()
    {
        return new List<VillaDTO>
        {
            new VillaDTO { Id = 1, Name = "Pool View" },
            new VillaDTO { Id = 2, Name = "Beach View" }
        };
    }

}
