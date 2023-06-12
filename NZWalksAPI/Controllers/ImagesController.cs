using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        public IImageRepository ImageRepository { get; }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm]ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if(ModelState.IsValid)
            {
                //convert DTO to Domain Model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };


                //use repository to upload file
                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);

        }
        private void ValidateFileUpload(ImageUploadRequestDto uploadRequest)
        {
            var allowedExtensions = new string[] { ".jpeg", ".png", ".jpg", ".pdf" };

            if (!allowedExtensions.Contains(Path.GetExtension(uploadRequest.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            if(uploadRequest.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size is larger than 10MB, Upload smaller size File");
            }
        }
    }
} 
