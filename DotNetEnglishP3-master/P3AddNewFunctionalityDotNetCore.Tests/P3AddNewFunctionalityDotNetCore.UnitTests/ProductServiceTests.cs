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

namespace P3AddNewFunctionalityDotNetCore.Tests.P3AddNewFunctionalityDotNetCore.UnitTests
{
    public class ProductServiceTests
    {
        private readonly Mock<ICart> _mockCart;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IStringLocalizer<ProductService>> _mockLocalizer;

        private readonly ProductService _productService;

        private readonly Product product1;
        private readonly Product product2;

        public ProductServiceTests()
        {
            _mockCart = new Mock<ICart>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

            _productService = new ProductService(
                _mockCart.Object,
                _mockProductRepository.Object,
                _mockOrderRepository.Object,
                _mockLocalizer.Object
            );

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
            _mockProductRepository.Verify(r => r.SaveProduct(
                It.Is<Product>(p =>
                    p.Name == "Le produit 3" &&
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
            _mockCart.Verify(c => c.RemoveLine(product1),
                Times.Once);
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
            _mockProductRepository.Verify(r => r.DeleteProduct(id),
                Times.Once);
        }
    }
}