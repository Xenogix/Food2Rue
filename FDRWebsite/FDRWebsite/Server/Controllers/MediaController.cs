using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class ImageController : CRUDController<Image, int, EmptyFilterParameters>
    {
        public ImageController(IRepositoryBase<Image, int> repository, IFilter<EmptyFilterParameters> filter) : base(repository, filter)
        {
        }
    }

    public class VideoController : CRUDController<Video, int, EmptyFilterParameters>
    {
        public VideoController(IRepositoryBase<Video, int> repository, IFilter<EmptyFilterParameters> filter) : base(repository, filter)
        {
        }
    }
}