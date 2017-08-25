namespace Finance.WebApi.Controllers
{
    using System;

    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Queries.Tag;
    using Finance.WebApi.Models.Tag.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TagsController : Controller
    {
        private readonly GetAllQuery getAll;
        private readonly GetAllByStoreQuery getAllByStore;
        private readonly IMapper mapper;

        public TagsController(
            GetAllQuery getAll,
            GetAllByStoreQuery getAllByStore,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.getAllByStore = getAllByStore;
            this.mapper = mapper;
        }

        [HttpGet("tags")]
        public IActionResult Get()
        {
            var entities = this.getAll.GetResult("junolive@gmail.com", this.Request.QueryString.Value);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }

        [HttpGet("stores/{store}/tags")]
        public IActionResult GetByStore(string store)
        {
            var entities = this.getAllByStore.GetResult("junolive@gmail.com", store, this.Request.QueryString.Value);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }

        [HttpPut("tags/{tag}")]
        public IActionResult Update(string tag)
        {
            throw new NotImplementedException();
        }
    }
}