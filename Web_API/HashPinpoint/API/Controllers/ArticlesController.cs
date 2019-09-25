using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/{controller}/[action]/{id?}")]
    [ApiController]
    [Authorize]
    public class ArticlesController : ControllerBase
    {

        private readonly IArticlesRepository _articlesRepository;
        private readonly ILogger _logger;

        public ArticlesController(IArticlesRepository articlesRepository, ILogger<ArticlesController> logger)
        {
            _articlesRepository = articlesRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var items = _articlesRepository.GetAll();
                return Ok(items);
            }
            catch (DataException ex)
            {
                _logger.LogError(ex, "ArticlesController/Get");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ArticlesController/Get");
                throw;
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody]Article model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                //var articleExists = _articlesRepository.GetByName(setting.name);
                //if (settingExists != null)
                //{
                //    return BadRequest("SettingWithSameNameExists");
                //}


                _articlesRepository.Insert(model);
                _articlesRepository.Save();

                return NoContent();
            }
            catch (DataException ex)
            {
                _logger.LogError(ex, "ArticlesController/Add");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ArticlesController/Add");
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                var entity = _articlesRepository.GetById(id);

                if (entity == null)
                {
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (DataException ex)
            {
                _logger.LogError(ex, "ArticlesController/GetById");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ArticlesController/GetById");
                throw;
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody]Article model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                var entity = _articlesRepository.GetById(model.Id);

                if (entity == null)
                {
                    return NotFound();
                }

                _articlesRepository.Update(entity);

                return NoContent();
            }
            catch (DataException ex)
            {
                _logger.LogError(ex, "ArticlesController/Update");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ArticlesController/Update");
                throw;
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var settingFromRepo = _articlesRepository.GetById(id);

                if (settingFromRepo == null)
                {
                    return NotFound();
                }

                _articlesRepository.Delete(settingFromRepo);

                return NoContent();
            }
            catch (DataException ex)
            {
                _logger.LogError(ex, "ArticlesController/Delete");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ArticlesController/Delete");
                throw;
            }
        }

    }
}