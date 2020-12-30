using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookStore.Infrastructure;
using BookStore.Models;
using System.Linq;
using BookStore.Repository;

namespace BookStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;
        
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
        
        public CartModel(IStoreRepository repo) => repository = repo;

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(long bookId, string returnUrl) {
            Book book = repository.Books
                .FirstOrDefault(x => x.BookId == bookId);

            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart.AddItem(book, 1);

            HttpContext.Session.SetJson("cart", Cart);

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
