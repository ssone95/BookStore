using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookStore.Components;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using Xunit;

namespace BookStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        private Genre[] GetGenres()
        {
            return new Genre[] {
                new Genre() { GenreId = 1, Name = "P1", Description = "D1" },
                new Genre() { GenreId = 2, Name = "P2", Description = "D2" },
                new Genre() { GenreId = 3, Name = "P3", Description = "D3" },
                new Genre() { GenreId = 4, Name = "P4", Description = "D4" },
            };
        }

        [Fact]
        public void Can_Select_Genres()
        {
            var genres = GetGenres();
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Genres)
                .Returns(genres.AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            var componentResult = target.Invoke() as ViewViewComponentResult;
            var result = componentResult.ViewData.Model as IEnumerable<Genre>;

            Assert.True(Enumerable.SequenceEqual(genres, result));
        }

        [Fact]
        public void Indicates_Selected_Genre()
        {
            long genreToSelect = 1;
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Genres)
                .Returns(GetGenres().AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };

            target.RouteData.Values["genre"] = genreToSelect;

            var result = (target.Invoke() as ViewViewComponentResult).ViewData["SelectedGenre"] as long?;

            Assert.Equal(genreToSelect, result);
        }
    }
}
