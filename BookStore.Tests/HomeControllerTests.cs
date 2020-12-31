using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using BookStore.Repository;
using BookStore.Models;
using System.Linq;
using BookStore.Controllers;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Books).Returns((new Book[]
            {
                new Book { BookId = 1, Name = "B1" },
                new Book { BookId = 2, Name = "B2" },
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object);

            var result = (controller.Index(genre: null) as ViewResult)
                .ViewData
                .Model as BookListViewModel;

            Book[] bookArray = result.Books.ToArray();

            Assert.True(bookArray.Length == 2);
            Assert.Equal("B1", bookArray[0].Name);
            Assert.Equal("B2", bookArray[1].Name);
        }

        [Fact]
        public void Can_Paginate()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Books).Returns((new Book[]
            {
                new Book { BookId = 1, Name = "B1" },
                new Book { BookId = 2, Name = "B2" },
                new Book { BookId = 3, Name = "B3" },
                new Book { BookId = 4, Name = "B4" },
                new Book { BookId = 5, Name = "B5" },
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            var result = (controller.Index(2, genre: null) as ViewResult)
                .ViewData
                .Model as BookListViewModel;

            Book[] bookArray = result.Books.ToArray();

            Assert.True(bookArray.Length == 2);
            Assert.Equal("B4", bookArray[0].Name);
            Assert.Equal("B5", bookArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Books).Returns((new Book[]
            {
                new Book { BookId = 1, Name = "B1" },
                new Book { BookId = 2, Name = "B2" },
                new Book { BookId = 3, Name = "B3" },
                new Book { BookId = 4, Name = "B4" },
                new Book { BookId = 5, Name = "B5" },
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            var result = (controller.Index(2, genre: null) as ViewResult)
                .ViewData
                .Model as BookListViewModel;

            Book[] bookArray = result.Books.ToArray();

            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Books()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Books).Returns((new Book[]
            {
                new Book { 
                    BookId = 1, Name = "B1", GenreId = 1
                },
                new Book { BookId = 2, Name = "B2", GenreId = 2
                },
                new Book { BookId = 3, Name = "B3", GenreId = 3
                },
                new Book { BookId = 4, Name = "B4", GenreId = 2
                },
                new Book { BookId = 5, Name = "B5", GenreId = 3
                },
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            var result = (controller.Index(1, genre: 2) as ViewResult)
                .ViewData
                .Model as BookListViewModel;

            Book[] bookArray = result.Books.ToArray();

            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(3, bookArray.Length);
            Assert.Equal(1, bookArray[0].BookId);
            Assert.Equal(3, bookArray[1].BookId);
            Assert.Equal(4, bookArray[2].BookId);
        }

        [Fact]
        public void Generate_Genre_Specific_Product_Count()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Books).Returns((new Book[] {
                new Book 
                { 
                    BookId = 1, Name = "B1", 
                    GenreId = 1 
                },
                new Book
                {
                    BookId = 2, Name = "B2",
                    GenreId = 2
                },
                new Book
                {
                    BookId = 3, Name = "B3",
                    GenreId = 3
                },
                new Book
                {
                    BookId = 4, Name = "B4",
                    GenreId = 2
                },
                new Book
                {
                    BookId = 5, Name = "B5",
                    GenreId = 3
                }
            }).AsQueryable<Book>());

            HomeController target = new HomeController(mock.Object);
            target.PageSize = 3;

            Func<ViewResult, BookListViewModel> GetModel = result =>
                result?.ViewData?.Model as BookListViewModel;

            int? res1 = GetPagingInfoCount(target.Index(genre: 1))?.PagingInfo?.TotalItems;
            int? res2 = GetPagingInfoCount(target.Index(genre: 2))?.PagingInfo?.TotalItems;
            int? res3 = GetPagingInfoCount(target.Index(genre: 3))?.PagingInfo?.TotalItems;
            int? resAll = GetPagingInfoCount(target.Index(genre: null))?.PagingInfo?.TotalItems;

            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }

        BookListViewModel GetPagingInfoCount(ViewResult viewResult) 
            => (viewResult?.ViewData?.Model as BookListViewModel);
    }
}
