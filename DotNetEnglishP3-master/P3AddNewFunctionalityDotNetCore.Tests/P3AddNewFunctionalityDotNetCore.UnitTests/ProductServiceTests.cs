using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using Xunit;
using System.Globalization;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using NuGet.ContentModel;

namespace P3AddNewFunctionalityDotNetCore.Tests.P3AddNewFunctionalityDotNetCore.UnitTests
{
    public class ProductServiceTests
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>

        // Utilisation d'un constructeur pour éviter de recréer des services avec les mocks pour chaque test

        // Champs privés
        private readonly Mock<ICart> _mockCart;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IStringLocalizer<ProductService>> _mockLocalizer;

        private readonly ProductService _productService;

        private readonly Product product1;
        private readonly Product product2;

        // Constructeur
        public ProductServiceTests()
        {
            // Initialisation des mocks
            _mockCart = new Mock<ICart>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

            // Création du service avec les mocks
            _productService = new ProductService(
                _mockCart.Object,
                _mockProductRepository.Object,
                _mockOrderRepository.Object,
                _mockLocalizer.Object
            );

            // Initialisation des produits
            product1 = new Product
            {
                Id = 1,
                Name = "Le produit 1",
                Price = 9.99,
                Quantity = 10,
                Description = "La description du produit 1",
                Details = "Les détails du produit 1"
            };
            product2 = new Product
            {
                Id = 2,
                Name = "Le produit 2",
                Price = 19.99,
                Quantity = 20,
                Description = "La description du produit 2",
                Details = "Les détails du produit 2"
            };
        }

        // Tests unitaires
        [Fact]
        public void GetAllProductsViewModel_GetAllProducts_Return2Products()
        {
            // Arrange
            // Simulation de GetAllProducts()
            _mockProductRepository.Setup(r => r.GetAllProducts())
                                  .Returns(new List<Product> { product1, product2});

            // Act
            // Appel de la méthode à tester
            var value = _productService.GetAllProductsViewModel();

            // Assert
            Assert.NotNull(value);
            Assert.Equal(2, value.Count);
        }

        [Fact]
        public void GetAllProductsViewModel_GetAllProducts_ReturnValuesForProduct1()
        {
            // Arrange
            // Simulation de GetAllProducts()
            _mockProductRepository.Setup(r => r.GetAllProducts())
                                  .Returns(new List<Product> { product1 });

            // Act
            // Appel de la méthode à tester
            var value = _productService.GetAllProductsViewModel();

            // Assert
            Assert.Equal(product1.Id, value[0].Id);
            Assert.Equal(product1.Name, value[0].Name);
            Assert.Equal(product1.Price.ToString(CultureInfo.InvariantCulture), value[0].Price);
            Assert.Equal(product1.Quantity.ToString(), value[0].Stock);
            Assert.Equal(product1.Description, value[0].Description);
            Assert.Equal(product1.Details, value[0].Details);
        }

        [Fact]
        public void GetProductByIdViewModel_GetIdProduct1_ReturnId1()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetAllProducts())
                                  .Returns(new List<Product> { product1, product2});
            
            // Act
            var value = _productService.GetProductByIdViewModel(1);

            // Assert
            Assert.NotNull(value);
            Assert.Equal(1, value.Id);
        }

        [Fact]
        public void GetProductByIdViewModel_GetIdProduct10_ReturnNull()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetAllProducts())
                                  .Returns(new List<Product> { product1, product2 });

            // Act
            var value = _productService.GetProductByIdViewModel(10);

            // Assert
            Assert.Null(value);
        }

        [Fact]
        public void GetProductByName_GetNameProduct1_ReturnLeProduit1()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetAllProducts())
                                  .Returns(new List<Product> { product1, product2});
            
            // Act
            var value = _productService.GetProductByName("Le produit 1");

            // Assert
            Assert.NotNull(value);
            Assert.Equal("Le produit 1", value.Name);
        }

        [Fact]
        public void GetProductByName_GetNameProduct1_ReturnNull()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetAllProducts())
                                  .Returns(new List<Product> { product1, product2 });

            // Act
            var value = _productService.GetProductByName("Le produit 10");

            // Assert
            Assert.Null(value);
        }

        [Fact]
        public void UpdateProductQuantities_CartProduct1Quantity2Product2Quantity3_UptdateStocksProduct1Quantity2Product2Quantity3()
        {
            // Arrange
            // Nécessite l'utilisation d'un vrai panier
            var cart = new Cart();
            cart.AddItem(product1, 2);
            cart.AddItem(product2, 3);

            // Création d'un nouveau service utilisant ce panier, et non l'interface
            var newProductService = new ProductService(
                cart,
                _mockProductRepository.Object,
                _mockOrderRepository.Object,
                _mockLocalizer.Object
            );

            // Act
            newProductService.UpdateProductQuantities();

            // Assert
            // Vérification que la méthode est bien appelée, avec les bons paramètres et le bon nombre de fois
            _mockProductRepository.Verify(r => r.UpdateProductStocks(product1.Id, 2),
                                               Times.Once);
            _mockProductRepository.Verify(r => r.UpdateProductStocks(product2.Id, 3),
                                               Times.Once);
        }

        [Fact]
        public void CheckProductModelErrors_ValidProduct_ReturnNoError()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "10",
                Stock = "5"
            };

            // Act
            var results = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void CheckProductModelErrors_ValidProductWithPriceDecimalPoint_ReturnNoError()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "10.50",
                Stock = "5"
            };

            // Act
            var results = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void CheckProductModelErrors_ValidProductWithPriceDecimalVirgule_ReturnNoError()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "10,50",
                Stock = "5"
            };

            // Act
            var results = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void CheckProductModelErrors_MissingData_ReturnErrorsForNamePriceAndStock()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "",
                Price = "",
                Stock = ""
            };

            // Act
            var results = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Contains("Veuillez saisir un nom", results);
            Assert.Contains("Veuillez saisir un prix", results);
            Assert.Contains("Veuillez saisir un stock", results);
        }

        [Fact]
        public void CheckProductModelErrors_PriceAndStockNotANumber_ReturnErrorForPriceAndStock()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "abc",
                Stock = "abc"
            };

            // Act
            var results = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Contains("La valeur saisie pour le prix doit être un nombre, avec au maximum 2 décimales", results);
            Assert.Contains("La valeur saisie pour le stock doit être un entier", results);
        }

        [Fact]
        public void CheckProductModelErrors_PriceAndStockWithDecimals_ReturnErrorForPriceAndStock()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "10.555",
                Stock = "5.5"
            };

            // Act
            var results = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Contains("La valeur saisie pour le prix doit être un nombre, avec au maximum 2 décimales", results);
            Assert.Contains("La valeur saisie pour le stock doit être un entier", results);
        }

        [Fact]
        public void CheckProductModelErrors_PriceAndStockEqualZero_ReturnErrorForPriceAndStock()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "0",
                Stock = "0"
            };

            // Act
            var results = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Contains("Le prix doit être supérieur à zéro", results);
            Assert.Contains("Le stock doit être supérieur à zéro", results);
        }

        [Fact]
        public void CheckProductModelErrors_PriceAndStockNegative_ReturnErrorForPriceAndStock()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "-10",
                Stock = "-5"
            };

            // Act
            var results = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Contains("Le prix doit être supérieur à zéro", results);
            Assert.Contains("Le stock doit être supérieur à zéro", results);
        }

        [Fact]
        public void SaveProduct_SaveProduct3_Product3Mapped()
        {
            // Arrange
            // Création d'un produit provenant du formulaire
            var product3 = new ProductViewModel
            {
                Name = "Le produit 3",
                Price = "29.99",
                Stock = "30",
                Description = "La description du produit 3",
                Details = "Les détails du produit 3"
            };

            // Act
            _productService.SaveProduct(product3);

            // Assert
            // On vérifie que le produit passé en paramètre est correctement mappé
            _mockProductRepository.Verify(r => r.SaveProduct(
                It.Is<Product>(p =>
                    p.Name == "Le produit 3" &&
                    // Ajout d'une tolérance puisque les types double ne sont jamais précis
                    Math.Abs(p.Price - 29.99) < 0.0001 &&
                    p.Quantity == 30 &&
                    p.Description == "La description du produit 3" &&
                    p.Details == "Les détails du produit 3"
                )
            ), Times.Once);
        }

        [Fact]
        public void DeleteProduct_DeleteProduct1_CallRomoveLineForProduct1()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetAllProducts())
                                  .Returns(new List<Product> { product1, product2 });

            // Act
            _productService.DeleteProduct(1);

            // Assert
            _mockCart.Verify(c => c.RemoveLine(product1), Times.Once);
        }

        [Fact]
        public void DeleteProduct_DeleteProduct1_CallDeleteProductForProduct1()
        {
            // Arrange
            int id = 1;

            _mockProductRepository.Setup(r => r.GetAllProducts())
                                  .Returns(new List<Product> { product1, product2 });

            // Act
            _productService.DeleteProduct(id);

            // Assert
            _mockProductRepository.Verify(r => r.DeleteProduct(id), Times.Once);
        }
    }
}