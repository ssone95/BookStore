using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models.ViewModels;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _storeRepository;

        public int PageSize = 4;
        public HomeController(IStoreRepository repo) => _storeRepository = repo;

        public ViewResult Index(int bookPage = 1, int? genre = null)
        {
            BookListViewModel bookListViewModel = new BookListViewModel();

            var books = _storeRepository.Books
                    .Include(x => x.Genre)
                    .Where(x => genre == null || x.GenreId == genre)
                    .OrderBy(x => x.BookId)
                    .Skip((bookPage - 1) * PageSize)
                    .Take(PageSize);
            bookListViewModel.Books = books;
            bookListViewModel.CurrentGenre = genre;

            bookListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = bookPage,
                ItemsPerPage = PageSize,
                TotalItems = _storeRepository.Books.Where(x => genre == null || x.GenreId == genre).Count()
            };
            return View(bookListViewModel);
        }
    }
}
