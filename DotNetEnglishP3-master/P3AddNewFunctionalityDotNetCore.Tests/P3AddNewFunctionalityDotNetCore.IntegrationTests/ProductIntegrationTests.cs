using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class ProductIntegrationTests
{
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer<ProductService> _localizerService;
    private readonly IStringLocalizer<ProductController> _localizerController;

    private const string ConnectionString = "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true";

    [Fact]
    public async Task CreateProduct_SaveNewProduct_NewProductInInventory()
    {
        // Arrange
        P3Referential context = CreateDbContext();
        Product product = null;

        try
        {
            LanguageService languageService = new LanguageService();
            Cart cart = new Cart();
            ProductRepository productRepository = new ProductRepository(context);
            OrderRepository orderRepository = new OrderRepository(context);
            ProductService productService = new ProductService(cart, productRepository, orderRepository, _localizerService);

            ProductController productController = new ProductController(productService, languageService, _localizerController);

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Name = "Le produit a sauvegarder",
                Price = "9.99",
                Stock = "10",
                Description = "La description du produit",
                Details = "Les détails du produit"
            };

            int productsBefore = await context.Product.CountAsync();

            // Act
            productController.Create(productViewModel);

            // Assert
            Assert.Equal(productsBefore + 1, context.Product.Count());

            product = await context.Product
                .Where(p => p.Name == "Le produit a sauvegarder")
                .FirstOrDefaultAsync();
            Assert.NotNull(product);
        }
        finally
        {
            if (product != null)
                await CleanTestData(context, product);
        }
    }

    [Fact]
    public async Task DeleteProduct_DeleteNewProduct_NewProductNotInInventory()
    {
        // Arrange
        P3Referential context = CreateDbContext();
        Product product = null;

        try
        {
            LanguageService languageService = new LanguageService();
            Cart cart = new Cart();
            ProductRepository productRepository = new ProductRepository(context);
            OrderRepository orderRepository = new OrderRepository(context);
            ProductService productService = new ProductService(cart, productRepository, orderRepository, _localizerService);

            ProductController productController = new ProductController(productService, languageService, _localizerController);

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Name = "Le produit a supprimer",
                Price = "9.99",
                Stock = "10",
                Description = "La description du produit",
                Details = "Les détails du produit"
            };

            int productsBefore = await context.Product.CountAsync();

            productController.Create(productViewModel);
            product = await context.Product
                .Where(p => p.Name == "Le produit a supprimer")
                .FirstOrDefaultAsync();

            // Act
            productController.DeleteProduct(product.Id);

            // Assert
            Assert.Equal(productsBefore, context.Product.Count());

            var productHereAgain = await context.Product
                .Where(p => p.Name == "Le produit a supprimer")
                .FirstOrDefaultAsync();
            Assert.Null(productHereAgain);
        }
        finally
        {
            if (product != null)
            {
                var productHereAgain = await context.Product
                .Where(p => p.Name == "Le produit a supprimer")
                .FirstOrDefaultAsync();

                if (productHereAgain != null)
                    await CleanTestData(context, product);
            }
        }
    }

    private P3Referential CreateDbContext()
    {
        var optionsContext = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(ConnectionString)
            .Options;

        return new P3Referential(optionsContext, _configuration);
    }

    private async Task CleanTestData(P3Referential context, Product product)
    {
        var lines = context.OrderLine.Where(ol => ol.ProductId == product.Id);
        context.OrderLine.RemoveRange(lines);

        var emptyOrders = context.Order.Where(o => !context.OrderLine.Any(ol => ol.OrderId == o.Id));
        context.Order.RemoveRange(emptyOrders);

        context.Product.Remove(product);

        await context.SaveChangesAsync();
    }
}