namespace Finance.WebApi.Controllers
{
    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Queries.Tag;
    using Finance.WebApi.Models.Tag.Get;

    using Microsoft.AspNetCore.Mvc;

    [Route("/tags")]
    public class TagsController : Controller
    {
        private readonly GetAllQuery getAll;
        private readonly IMapper mapper;

        public TagsController(
            GetAllQuery getAll,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var entities = this.getAll.GetResult("junolive@gmail.com", this.Request.QueryString.Value);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }
    }
}