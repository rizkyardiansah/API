using API.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;

        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<Entity> Get()
        {
            IEnumerable<Entity> result = repository.Get();
            return Ok(result);
        }

        [HttpGet("{key}")]
        public ActionResult<Entity> Get(Key key)
        {
            return Ok(repository.Get(key));
        }

        [HttpPost]
        public ActionResult<Entity> Insert(Entity entity)
        {
            return Ok(repository.Insert(entity));
        }

        [HttpDelete("{key}")]
        public ActionResult<Entity> Delete(Key key)
        {
            return Ok(repository.Delete(key));
        }

        [HttpPut("{key}")]
        public ActionResult<Entity> Update(Entity entity, Key key)
        {
            return Ok(repository.Update(entity, key));
        }
    }
}
