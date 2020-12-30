using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using BookStore.Models;
using BookStore.Pages;
using BookStore.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualBasic;
using Moq;
using Xunit;

namespace BookStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            Book b1 = new Book { BookId = 1, Name = "B1" };
            Book b2 = new Book { BookId = 2, Name = "B2" };

            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Books).Returns((new Book[] { b1, b2 })
                .AsQueryable());

            Cart testCart = new Cart();
            testCart.AddItem(b1, 2);
            testCart.AddItem(b2, 1);


            Mock<ISession> mockSession = new Mock<ISession>();
            byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(testCart));
            mockSession.Setup(x => x.TryGetValue(It.IsAny<string>(), out data));

            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(x => x.Session).Returns(mockSession.Object);

            CartModel cartModel = new CartModel(mock.Object);
            cartModel.PageContext = new PageContext(new Microsoft.AspNetCore.Mvc.ActionContext
            {
                HttpContext = mockContext.Object,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                {

                }
            });

            cartModel.OnGet("myUrl");

            Assert.Equal(2, cartModel.Cart.Lines.Count());
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            Book b1 = new Book { BookId = 1, Name = "B1" };

            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Books).Returns((new Book[] { b1 })
                .AsQueryable());

            Cart testCart = new Cart();
            Mock<ISession> mockSession = new Mock<ISession>();
            mockSession.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, val) => {
                    testCart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val));
                });

            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(x => x.Session).Returns(mockSession.Object);

            CartModel cartModel = new CartModel(mock.Object)
            {
                PageContext = new PageContext(new ActionContext
                {
                    HttpContext = mockContext.Object,
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                })
            };

            cartModel.OnPost(1, "myUrl");

            Assert.Single(testCart.Lines);
            Assert.Equal("B1", testCart.Lines.First().Book.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
