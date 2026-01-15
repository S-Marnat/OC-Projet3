using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.P3AddNewFunctionalityDotNetCore.UnitTests
{
    public class CartTests
    {
        private readonly Product product1;
        private readonly Product product2;
        private readonly Product product3;

        public CartTests()
        {
            product1 = new Product { Id = 1, Price = 9.99, Quantity = 10 };
            product2 = new Product { Id = 2, Price = 19.99, Quantity = 20 };
            product3 = new Product { Id = 3, Price = 29.99, Quantity = 0 };
        }

        [Fact]
        public void AddItem_Add2Product1_LinesCount1Quantity2()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(product1, 1);
            cart.AddItem(product1, 1);

            // Assert
            Assert.NotEmpty(cart.Lines);
            Assert.Single(cart.Lines);
            Assert.Equal(2, cart.Lines.First().Quantity);
        }

        [Fact]
        public void AddItem_Add2DifferentsProducts_LinesCount2Quantity1()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);

            // Assert
            Assert.NotEmpty(cart.Lines);
            Assert.Equal(2, cart.Lines.Count());
            foreach (var line in cart.Lines)
            {
                Assert.Equal(1, line.Quantity);
            }
        }

        [Fact]
        public void AddItem_NoStock_NotAdd()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(product3, 1);

            // Assert
            Assert.Empty(cart.Lines);
        }

        [Fact]
        public void AddItem_NoEnoughStock_AddQuantityStockProduct()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(product1, 11);

            // Assert
            Assert.Equal(product1.Quantity, cart.Lines.First().Quantity);
        }

        [Fact]
        public void RemoveLine_RemoveProduct_NoProductInCart()
        {
            // Arrange
            var cart = new Cart();

            cart.AddItem(product1, 1);

            // Act
            cart.RemoveLine(product1);

            // Assert
            Assert.Empty(cart.Lines);
        }

        [Fact]
        public void RemoveLine_RemoveProduct2_Product1StillHereProduct2Removed()
        {
            // Arrange
            var cart = new Cart();

            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);

            // Act
            cart.RemoveLine(product2);

            // Assert
            Assert.Single(cart.Lines);
            Assert.Equal(product1.Id, cart.Lines.First().Product.Id);
        }

        [Fact]
        public void GetTotalValue_CalculateTotalValue_ExpectedTotalValueEqualTotalValue()
        {
            // Arrange
            var cart = new Cart();

            cart.AddItem(product1, 2);
            cart.AddItem(product2, 1);

            // Act
            double totalValue = cart.GetTotalValue();
            double expectedValue = product1.Price * 2 + product2.Price;

            // Assert
            Assert.Equal(expectedValue, totalValue, 0.001);
        }

        [Fact]
        public void GetTotalValue_WithEmptyCart_Return0()
        {
            // Arrange
            var cart = new Cart();

            // Act
            double totalValue = cart.GetTotalValue();

            // Assert
            Assert.Equal(0, totalValue);
        }

        [Fact]
        public void GetAverageValue_CalculateAverageValue_ExpectedAverageValueEqualAverageValue()
        {
            // Arrange
            var cart = new Cart();

            cart.AddItem(product1, 2);
            cart.AddItem(product2, 1);

            // Act
            double averageValue = cart.GetAverageValue();
            double expectedValue = (product1.Price * 2 + product2.Price) / 3;

            // Assert
            Assert.Equal(expectedValue, averageValue, 0.001);
        }

        [Fact]
        public void GetAverageValue_WithEmptyCart_Return0()
        {
            // Arrange
            var cart = new Cart();

            // Act
            double averageValue = cart.GetAverageValue();

            // Assert
            Assert.Equal(0, averageValue);
        }

        [Fact]
        public void Clear_ClearTheCart_EmptyCart()
        {
            // Arrange
            var cart = new Cart();

            cart.AddItem(product1, 2);
            cart.AddItem(product2, 1);

            // Act
            cart.Clear();

            // Assert
            Assert.Equal(0, cart.GetTotalValue());
        }
    }
}