# Migrations ReadMe

## Prerequisites

1. SQL Server 2016 Express: <https://www.microsoft.com/en-us/sql-server/sql-server-editions-express>
2. SQL Server Management Studio: <https://msdn.microsoft.com/en-us/library/mt238290.aspx>

---

1. Add EF dependencies to project.json

    ```json
    "Microsoft.EntityFrameworkCore.SqlServer":"1.1.0",
    "Microsoft.EntityFrameworkCore.SqlServer.Design": "1.1.0",
    "Microsoft.EntityFrameworkCore.Design": {
        "type": "build", "version": "1.1.0"
    }
    ```

2. Add EF tools to project.json
    - Restore packages: `dotnet restore`

    ```json
    "Microsoft.EntityFrameworkCore.Tools.DotNet": "1.1.0-preview4-final"
    ```

3. Create Category and Product classes.
    - Add to the Models folder
    - Place inside namespace: `HelloWebApi.Models`
    - Make sure to add CategoryId and Category properties to Product.

    ```csharp
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    ```

    ```csharp
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
    ```

4. Create ProductsDbContext class.
    - Add to the Contexts folder
    - Place inside namespace: `HelloWebApi.Contexts`
    - Derive from DbContext
    - Add a ctor accepting DbContextOptions that calls the base ctor
    - Add Categories and Products properties of type DbSet<T>

    ```csharp
    public class ProductsDbContext: DbContext
    {
        public ProductsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
    ```

5. Add an appsettings.json file to the project

    - Add a ConnectionStrings section to appsettings.json
    - Include a connection string to the ProductsDb database

    ```json
    "ConnectionStrings": {
    "ProductsDbConnection": "Data Source=.\\sqlexpress;Initial Catalog=ProductsDb;Integrated Security=True;MultipleActiveResultSets=True"
    }
    ```

6. Add a ctor to Startup that sets a Configuration property.

    ```csharp
    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }
    ```

7. Add a `DbContext` to `ConfigureServices` in `Startup`.
    - Add using directive for `Microsoft.EntityFrameworkCore`

    ```csharp
    services.AddDbContext<ProductsDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("ProductsDbConnection")));
    ```

8. Add initial migration and apply to database.
    - Open command prompt in app directory
    - Open SQL Server Management Studio to verify 
      ProductsDb database and tables

    ```
    dotnet ef migrations add initial
    dotnet ef database update
    ```

9. Populate database tables.
    - Open Data/ProductsDb-Data.sql in SSMS and run it
    - Categories and Products tables should be populated with data


