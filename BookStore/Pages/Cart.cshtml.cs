using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookStore.Infrastructure;
using BookStore.Models;
using BookStore.Repository;

namespace BookStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;
        
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
        
        public CartModel(IStoreRepository repo, Cart cartService)
        {
            repository = repo;
            Cart = cartService;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long bookId, string returnUrl) {
            Book book = repository.Books
                .FirstOrDefault(x => x.BookId == bookId);

            Cart.AddItem(book, 1);

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(long bookId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Book.BookId == bookId).Book);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
