using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SorterAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SorterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SorterController : ControllerBase
    {
        private readonly ISorterService _service;
        public SorterController(ISorterService service)
        {
            _service = service;
        }

        /// <summary>
        /// Sends string containing sequence of numbers for sorter to sort
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: /api/Sorter/Sort
        ///     {
        ///         "2 8 0 4 5 9 3 1010 1 0 99999 11 2 2 2 1337 2 6 0 0 -991 69 32 42 39 7 111 1111"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">If string has been sent succesfully</response>
        [HttpPost]
        [Route("Sort")]
        public IActionResult Sort([FromBody] string numbers)
        {
            _service.SortAndSave(numbers);
            return Ok();
        }

        /// <summary>
        /// Gets contents of last saved file with sorted numbers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: /api/sorter/result
        ///     
        /// </remarks>
        /// <response code="200">If the file has been loaded succesfully</response>
        /// <response code="404">If the file was not found</response>
        [HttpGet]
        [Route("Result")]
        public IActionResult Result()
        {
            var content = _service.LoadLatestFile();
            if (content == null)
            {
                return NotFound("File not found");
            }
            return Ok(content);
        }
    }
}
