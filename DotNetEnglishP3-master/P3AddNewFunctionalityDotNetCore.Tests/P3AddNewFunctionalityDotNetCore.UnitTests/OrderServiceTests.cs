using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.P3AddNewFunctionalityDotNetCore.UnitTests
{
    public class OrderServiceTests
    {
        private readonly Mock<ICart> _mockCart;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IProductService> _mockProductService;

        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockCart = new Mock<ICart>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockProductService = new Mock<IProductService>();

            _orderService = new OrderService(
                _mockCart.Object,
                _mockOrderRepository.Object,
                _mockProductService.Object
            );
        }

        [Fact]
        public async Task GetOrder_GetOrderWithId1_GetOrderCalled1TimeAndReturnOrderId1()
        {
            // Arrange
            int id = 1;
            var expectedOrder = new Order { Id = id };

            _mockOrderRepository.Setup(r => r.GetOrder(id))
                .ReturnsAsync(expectedOrder);

            // Act
            var returnedOrder = await _orderService.GetOrder(id);

            // Assert
            _mockOrderRepository.Verify(r => r.GetOrder(id),
                Times.Once);
            Assert.Equal(expectedOrder, returnedOrder);
        }

        [Fact]
        public async Task GetOrders_For2Orders_Called1TimeAndReturn2Orders()
        {
            // Arrange
            var expectedOrder1 = new Order {Id = 1};
            var expectedOrder2 = new Order {Id = 2};

            _mockOrderRepository.Setup(r => r.GetOrders())
                .ReturnsAsync(new List<Order> { expectedOrder1, expectedOrder2 });

            // Act
            var returnedOrder = await _orderService.GetOrders();

            // Assert
            _mockOrderRepository.Verify(r => r.GetOrders(),
                Times.Once);
            Assert.Equal(2, returnedOrder.Count);
        }

        [Fact]
        public async Task GetOrders_For1Order_ReturnExpectedOrder1()
        {
            // Arrange
            var expectedOrder1 = new Order {Id = 1};

            _mockOrderRepository.Setup(r => r.GetOrders())
                .ReturnsAsync(new List<Order> { expectedOrder1 });

            // Act
            var returnedOrder = await _orderService.GetOrders();

            // Assert
            Assert.Contains(expectedOrder1, returnedOrder);
        }

        [Fact]
        public void SaveOrder_Save1Order_Called1TimeAndOrderNotNull()
        {
            // Arrange
            var order = CreateOrderViewModel();
            Order saveOrder = null;

            _mockOrderRepository.Setup(r => r.Save(It.IsAny<Order>()))
                .Callback<Order>(o => saveOrder = o);

            // Act
            _orderService.SaveOrder(order);

            // Assert
            _mockOrderRepository.Verify(r => r.Save(It.IsAny<Order>()),
                Times.Once);
            Assert.NotNull(saveOrder);
        }

        [Fact]
        public void SaveOrder_Save1Order_ReturnMapForValuesForm()
        {
            // Arrange
            var order = CreateOrderViewModel();
            Order saveOrder = null;

            _mockOrderRepository.Setup(r => r.Save(It.IsAny<Order>()))
                .Callback<Order>(o => saveOrder = o);

            // Act
            _orderService.SaveOrder(order);

            // Assert
            Assert.Equal("Mon nom", saveOrder.Name);
            Assert.Equal("Mon adresse", saveOrder.Address);
            Assert.Equal("Ma ville", saveOrder.City);
            Assert.Equal("12345", saveOrder.Zip);
            Assert.Equal("Mon pays", saveOrder.Country);
        }

        [Fact]
        public void SaveOrder_Save1Order_ReturnMapForValuesOrder()
        {
            // Arrange
            var order = CreateOrderViewModel();
            Order saveOrder = null;

            _mockOrderRepository.Setup(r => r.Save(It.IsAny<Order>()))
                .Callback<Order>(o => saveOrder = o);

            // Act
            _orderService.SaveOrder(order);

            var lines = saveOrder.OrderLine.ToList();

            // Assert
            Assert.Single(saveOrder.OrderLine);
            Assert.Equal(1, lines[0].ProductId);
            Assert.Equal(2, lines[0].Quantity);
        }

        [Fact]
        public void SaveOrder_Save1Order_CallSaveOrder1Time()
        {
            // Arrange
            var order = CreateOrderViewModel();
            Order saveOrder = null;

            _mockOrderRepository.Setup(r => r.Save(It.IsAny<Order>()))
                .Callback<Order>(o => saveOrder = o);

            // Act
            _orderService.SaveOrder(order);

            // Assert
            _mockProductService.Verify(r => r.UpdateProductQuantities(),
                Times.Once);
        }

        [Fact]
        public void SaveOrder_Save1Order_CallUpdateInventory1Time()
        {
            // Arrange
            var order = CreateOrderViewModel();
            Order saveOrder = null;

            _mockOrderRepository.Setup(r => r.Save(It.IsAny<Order>()))
                .Callback<Order>(o => saveOrder = o);

            // Act
            _orderService.SaveOrder(order);

            // Assert
            _mockCart.Verify(r => r.Clear(),
                Times.Once);
        }

        private OrderViewModel CreateOrderViewModel()
        {
            return new OrderViewModel
            {
                Lines = new List<CartLine>
                {
                    new CartLine
                    {
                        Product = new Product { Id = 1 },
                        Quantity = 2
                    }
                },
                Name = "Mon nom",
                Address = "Mon adresse",
                City = "Ma ville",
                Zip = "12345",
                Country = "Mon pays"
            };
        }
    }
}