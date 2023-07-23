using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealWordUnitTest.Web.Models;
using RealWordUnitTest.Web.Repositories;

namespace RealWordUnitTest.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductsApiController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        // GET: api/ProductsApi
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetAll();
            return Ok(products);
        }

        // GET: api/ProductsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
        
            var product = await _repository.GetByID(id);

            if(product == null) return NotFound();

            return Ok(product);
        }

        // PUT: api/ProductsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }

            _repository.Update(product);          

            return NoContent(); //Güncelleme için NoContent dön
        }

        // POST: api/ProductsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
         
            await _repository.Create(product);

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/ProductsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _repository.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }
            _repository.Delete(product);
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            Product product = _repository.GetByID(id).Result;
            if (product == null) { return false; }
            else return true;
          
        }
    }
}
