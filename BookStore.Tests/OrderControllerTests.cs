using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using BookStore.Controllers;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();

            Order order = new Order();

            OrderController target = new OrderController(mock.Object, cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_Shipping_Details()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            Order order = new Order();

            OrderController target = new OrderController(mock.Object, cart);

            target.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(order) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            OrderController target = new OrderController(mock.Object, cart);

            RedirectToPageResult result = target.Checkout(new Order())
                as RedirectToPageResult;

            
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            Assert.Equal("/Completed", result.PageName);
        }
    }
}
