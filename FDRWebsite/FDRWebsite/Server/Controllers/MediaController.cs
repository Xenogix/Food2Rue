using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class ImageController : CRUDController<Image, int>
    {
        public ImageController(IRepositoryBase<Image, int> repository) : base(repository)
        {
        }
    }

    public class VideoController : CRUDController<Video, int>
    {
        public VideoController(IRepositoryBase<Video, int> repository) : base(repository)
        {
        }
    }
}