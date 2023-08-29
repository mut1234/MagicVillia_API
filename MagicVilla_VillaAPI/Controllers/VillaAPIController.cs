

using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VilliAPI.Controllers
{
	[Route("api/[Controller]")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		private readonly ILogger<VillaAPIController> _logger;

		public VillaAPIController(ILogger<VillaAPIController> logger)
        {
			_logger = logger;
		}


        [HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
		{
			_logger.LogInformation("Getting all villas");
			return Ok(VillaStore.villaList);

		}


		//[ProducesResponseType(200,Type=typeof(VillaDTO))]
		//[ProducesResponseType(400)]
		//[ProducesResponseType(404)]
		[HttpGet("{id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetVilla(int id)
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
			return Ok(villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
		{
			var result = VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower());
			if (result != null)
			{
				ModelState.AddModelError("CustomError", "Villa Already Exists!");
				return BadRequest(ModelState);
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(villaDTO);
			}
			if (villaDTO == null)
			{
				return BadRequest(villaDTO);
			}
			if (villaDTO.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			villaDTO.Id = VillaStore.villaList.OrderByDescending(U => U.Id).FirstOrDefault().Id + 1;
			VillaStore.villaList.Add(villaDTO);
			return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);

		}
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpDelete("{id:int}", Name = "DeleteVilla")]
		public IActionResult DeleteVilla(int id)//IActionResult you can not return with it
		{
			if (id == 0)
			{
				_logger.LogError("Get villa error with id " + id);
				return BadRequest();
			}
			var result = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			VillaStore.villaList.Remove(result);
			return NoContent();

		}
		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
		{
			if (villaDTO == null || id != villaDTO.Id)
			{
				return BadRequest();
			}
			var result = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			result.Name = villaDTO.Name;
			result.Sqft = villaDTO.Sqft;
			villaDTO.Occupancy = villaDTO.Occupancy;
			return NoContent();
		}
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
		{
			if (patchDTO == null || id == 0)
			{
				return BadRequest();
			}
			var result = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			if (result == null)
			{
				return BadRequest();
			}
			patchDTO.ApplyTo(result, ModelState);
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return NoContent();
		}

	}


}
