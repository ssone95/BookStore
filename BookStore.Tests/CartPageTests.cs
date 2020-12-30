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

            CartModel cartModel = new CartModel(mock.Object, testCart);
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
            CartModel cartModel = new CartModel(mock.Object, testCart);

            cartModel.OnPost(1, "myUrl");

            Assert.Single(testCart.Lines);
            Assert.Equal("B1", testCart.Lines.First().Book.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
