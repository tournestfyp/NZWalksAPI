using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        [HttpGet]
        //[Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                
                //get data from domain model
                //var regionsDomain = dbContext.Regions.ToList();

                var regionsDomain = await regionRepository.GetAllAsync();


                /*Using autoMapper so we don't need this
                //map the domain model data in DTOs
                var regionsDto = new List<RegionDto>();
                foreach (var regionDomain in regionsDomain) 
                {
                    regionsDto.Add(new RegionDto()
                    {
                        Id = regionDomain.Id,
                        Code = regionDomain.Code,
                        Name = regionDomain.Name,
                        RegionImageUrl = regionDomain.RegionImageUrl,
                    });
                }*/

                //map the domain model data in DTOs
                var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

                return Ok(regionsDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }

        }




        [HttpGet]
        //[Authorize(Roles = "Reader, Writer")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            //you can use this for id

            //var regions = dbContext.Regions.Find(id);

            //this can be used for everything
            //var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //map Domain Model to DTOs
            /*
             var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };
            */
            var regionDto = mapper.Map<RegionDto>(regionDomain);
                return Ok(regionDto);

        }



        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
                // Map or Convert DTO to Domain Model
                /*
                var regionDomainModel = new Region
                {
                    Code = addRegionRequestDto.Code,
                    Name = addRegionRequestDto.Name,
                    RegionImageUrl = addRegionRequestDto.RegionImageUrl
                };
                */
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                // Use Domain Model to create Region
                //await dbContext.Regions.AddAsync(regionDomainModel);
                //await dbContext.SaveChangesAsync();

                //coming from repository
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                // Map Domain model back to DTO or to create a new entry
                /*
                var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };
                */
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }
            



        // Update region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        { 
                // Check if region exists
                //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

                //first map DTO to Domain Model Then repository
                /*
                var regionDomainModel = new Region
                {
                    Code = updateRegionRequestDto.Code,
                    Name = updateRegionRequestDto.Name,
                    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
                };
                */
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);


                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                // Map DTO to Domain model
                /* 
                 regionDomainModel.Code = updateRegionRequestDto.Code;
                 regionDomainModel.Name = updateRegionRequestDto.Name;
                 regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

                 await dbContext.SaveChangesAsync();
                */
                // Convert Domain Model to DTO
                /*
                var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };
                */
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDto);
            }
            



        // Delete Region
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete region
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();
            // (moved to repository)

            // return deleted Region back
            // map Domain Model to DTO
            /*
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            */
            // converting regionDomainModel to RegionDto
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
