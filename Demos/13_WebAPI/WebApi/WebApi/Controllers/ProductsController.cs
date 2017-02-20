using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productsRepository.GetProductsAsync();
            return Ok(products);
        }

        // GET api/products/3
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await _productsRepository.GetProductAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            product = await _productsRepository.CreateProductAsync(product);
            return CreatedAtAction("Get", new { id = product.Id }, product);
        }

        // PUT api/products
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Product product)
        {
            try
            {
                product = await _productsRepository.UpdateProductAsync(product);
                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productsRepository.DeleteAsync(id);
            return new NoContentResult();
        }
    }
}
