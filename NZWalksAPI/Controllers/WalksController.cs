using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }



        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
                // Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                // Map Domain Model to DTO
                var walkDto = mapper.Map<WalkDto>(walkDomainModel);

                return Ok(walkDto); 
        }
        
        

        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true,
                pageNumber, pageSize);

            var walksDto = mapper.Map<List<WalkDto>>(walksDomainModel);

            return Ok(walksDto);

        }




        [HttpGet]
        //mapping the id from route
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if(walkDomainModel == null)
            {
                return NotFound(); 
            }
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }




        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
                //map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);


                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                //map domain model back to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }




        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            var deletedDomainModel = await walkRepository.DeleteAsync(id);
            if(deletedDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model To DTO
            return Ok(mapper.Map<WalkDto>(deletedDomainModel));
        }
    }

}