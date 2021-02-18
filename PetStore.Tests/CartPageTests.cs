using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using PetStore.Models;
using PetStore.Pages;
using PetStore.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualBasic;
using Moq;
using Xunit;

namespace PetStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            Article b1 = new Article { ArticleId = 1, Name = "Article 1" };
            Article b2 = new Article { ArticleId = 2, Name = "Article 2" };

            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Articles).Returns((new Article[] { b1, b2 })
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
            Article b1 = new Article { ArticleId = 1, Name = "Article 1" };

            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Articles).Returns((new Article[] { b1 })
                .AsQueryable());

            Cart testCart = new Cart();
            CartModel cartModel = new CartModel(mock.Object, testCart);

            cartModel.OnPost(1, "myUrl");

            Assert.Single(testCart.Lines);
            Assert.Equal("Article 1", testCart.Lines.First().Article.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
