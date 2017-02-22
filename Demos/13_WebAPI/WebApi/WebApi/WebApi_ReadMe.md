# WebApi ReadMe


1. Remove parts related to a user interface

    - Remove HomeController from Controllers
    - Remove Views folder and contents

2. Update Startup to use only parts of MVC needed for Web API

    - Edit Configure to replace `app.UseMvcWithDefaultRoute` 
      with `app.UseMvc`
        + This is because we're going to attribute-based routing
    
    ```csharp
    app.UseMvc();
    ```

    - In ConfigureServices, replace `AddMvc` with `AddMvcCore
    - Add Json input and output formatters

    ```csharp
    services.AddMvcCore()
        .AddJsonFormatters();
    ```

3. Add a Web API controller to the Controllers folder.

    - Right-click, add new item, Web API Controller
      + Name it ProductsController
    - Run the app and navigate to api/products
      + You should see: `["value1","value2"]`

4. Add repository classes to retrieve and update products

    - Add a Products class to the Models folder.

    ```csharp
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
    ```

    - Add IProductsRepository to a Repositories folder.

    ```csharp
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> GetProductAsync(int id);

        Task<Product> CreateProductAsync(Product product);

        Task<Product> UpdateProductAsync(Product product);

        Task DeleteAsync(int id);
    }
    ```

    - Add ProductsRepository to Repositories to implement IProductsRepository.

    ```csharp
    public class ProductRepository : IProductsRepository
    {
        private static readonly ConcurrentDictionary<int, Product> ProductsCache =
            new ConcurrentDictionary<int, Product>();

        static ProductRepository()
        {
            ProductsCache.TryAdd(1, new Product { Id = 1, ProductName = "Ristretto", Price = 10 });
            ProductsCache.TryAdd(2, new Product { Id = 2, ProductName = "Espresso", Price = 20 });
            ProductsCache.TryAdd(3, new Product { Id = 3, ProductName = "Macchiato", Price = 30 });
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Task.FromResult(ProductsCache.Values);
        }

        public async Task<Product> GetProductAsync(int id)
        {
            Product product;
            if (!ProductsCache.TryGetValue(id, out product))
            {
                throw new KeyNotFoundException($"Product with id '{id}' does not exist in cache");
            }
            return await Task.FromResult(product);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await Task.FromResult(ProductsCache.TryAdd(product.Id, product));
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (!ProductsCache.ContainsKey(product.Id))
            {
                throw new KeyNotFoundException($"Product with id '{product.Id}' does not exist in cache");
            }
            await Task.FromResult(ProductsCache.TryUpdate(
                product.Id, product, ProductsCache[product.Id]));
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            Product product;
            await Task.FromResult(ProductsCache.TryRemove(id, out product));
        }
    }
    ```

    - Register IProductsRepository with DI in Startup.ConfigureServices

    ```csharp
    services.AddSingleton<IProductsRepository, ProductRepository>();
    ```

5. Refactor ProductsController to inject IProductsRepository and use it 
   in the actions

    - Add a ctor that accepts IProductsRepository

    ```csharp
    private readonly IProductsRepository _productsRepository;

    public ProductsController(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }
    ```

    - Refactor both Get methods as async and to return Ok with products
      - Run the app to test each method
      - Second method should return 
    
    ```csharp
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
    ```

    - Implement Post, Put and Delete methods

    ```csharp
    // POST api/products
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Product product)
    {
        product = await _productsRepository.CreateProductAsync(product);
        return CreatedAtAction("Get", new {id = product.Id}, product);
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
    ```

6. Use Postman or Fiddler to test the Web API.

    - GET Request: http://localhost:5000/api/products/3
      + GET Response:
      + Status Code: 200
      + Body:

    ```json
    {
      "id": 3,
      "productName": "Macchiato",
      "price": 30
    }
    ```

    - GET Response:
      + Status Code: 200

    ---

    - POST Request: http://localhost:5000/api/products
      + Header: Content-Type application/json
      + Body:

    ```json
    {
      "id": 4,
      "productName": "Latte",
      "price": 40
    }
    ```

    - POST Response:
      + Status Code: 201
      + Location Header: http://localhost:5000/api/Products/4

    ---

    - PUT Request: http://localhost:5000/api/products
      - Header: Content-Type application/json
      - Body:

    ```json
    {
      "id": 4,
      "productName": "Latte",
      "price": 50
    }
    ```

    - PUT Response:
      + Status Code: 200

    ---

    - DELETE Request: http://localhost:5000/api/products/4

    - DELETE Response:
      + Status Code: 204

