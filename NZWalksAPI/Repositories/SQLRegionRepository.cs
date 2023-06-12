﻿using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLRegionRepository: IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
                this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }



        public async Task<Region?> DeleteAsync(Guid id)
        {
           var exisitinRegion =  await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (exisitinRegion == null) 
            {
                return null;
            }
            dbContext.Regions.Remove(exisitinRegion);
            await dbContext.SaveChangesAsync();
            return exisitinRegion;
        }



        public async Task<List<Region>>GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();

        }



        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
        


        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync( x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
