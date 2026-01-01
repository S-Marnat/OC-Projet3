using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using Xunit;
using System.Globalization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Linq;

namespace P3AddNewFunctionalityDotNetCore.Tests.P3AddNewFunctionalityDotNetCore.UnitTests
{
    /* Le projet demande de réaliser des tests unitaires pour tester les spécifications
       mais étant passée par les DataAnnotations, je n'ai plus la méthode à tester dans ProductService.
       Je contourne donc un peu cette problématique en testant le ViewModel. */
    public class ProductViewModelTests
    {
        // Tester un ViewModel avec DataAnnotations nécessite une méthode utilitaire pour éviter les répétitions
        private IList<ValidationResult> ValidateModel(object model)
        {
            // Création d'une liste pour les erreurs
            var validationResults = new List<ValidationResult>();

            // Création d'un contexte de validation (quel objet, quel environnement, quels services) 
            var context = new ValidationContext(model, null, null);

            // Validation de l'objet selon ses DataAnnotations et placement des erreurs dans validationResults
            Validator.TryValidateObject(model, context, validationResults, true);

            return validationResults;
        }

        [Fact]
        public void ProductViewModel_MissingName_ReturnErrorForName()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "",
                Price = "10",
                Stock = "5"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.NotEmpty(results);
            // Vérification qu'il y a bien une erreur de validation sur le champ Name
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(ProductViewModel.Name)));
        }

        [Fact]
        public void ProductViewModel_MissingPrice_ReturnErrorForPrice()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "",
                Stock = "5"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(ProductViewModel.Price)));
        }

        [Fact]
        public void ProductViewModel_PriceNotANumber_ReturnErrorForPrice()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "abc",
                Stock = "5"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(ProductViewModel.Price)));
        }

        [Fact]
        public void ProductViewModel_PriceNotGreaterThanZero_ReturnErrorForPrice()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "0",
                Stock = "5"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(ProductViewModel.Price)));
        }

        [Fact]
        public void ProductViewModel_MissingQuantity_ReturnErrorForQuantity()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "10",
                Stock = ""
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(ProductViewModel.Stock)));
        }

        [Fact]
        public void ProductViewModel_QuantityNotAnInteger_ReturnErrorForQuantity()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "10",
                Stock = "abc"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(ProductViewModel.Stock)));
        }

        [Fact]
        public void ProductViewModel_QuantityNotGreaterThanZero_ReturnErrorForQuantity()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Produit 1",
                Price = "10",
                Stock = "0"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(ProductViewModel.Stock)));
        }
    }
}