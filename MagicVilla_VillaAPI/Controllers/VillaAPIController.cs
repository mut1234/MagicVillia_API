

using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VilliAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VilliAPI.Controllers
{
	[Route("api/[Controller]")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult <IEnumerable<VillaDTO>> GetVillas() {
			return Ok(VillaStore.villaList);

		}
		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		//[ProducesResponseType(200,Type=typeof(VillaDTO))]
		//[ProducesResponseType(400)]
		//[ProducesResponseType(404)]

		public ActionResult GetVilla(int id)
		{	
			if(id == 0) {
				return BadRequest();
			}
			var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			if (villa == null)
			{
				return NotFound();
			}
			return Ok(villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
		{
			if (villaDTO == null) 
			{
			return BadRequest(villaDTO);
			}
			if (villaDTO.Id>0) 
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			villaDTO.Id= VillaStore.villaList.OrderByDescending(U=>U.Id).FirstOrDefault().Id+1;
			VillaStore.villaList.Add(villaDTO);
			return Ok(villaDTO); 
		}
	}
}
