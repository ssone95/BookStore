using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetStore.Infrastructure;
using PetStore.Models;
using PetStore.Repository;

namespace PetStore.Pages
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

        public IActionResult OnPost(long ArticleId, string returnUrl) {
            Article Article = repository.Articles
                .FirstOrDefault(x => x.ArticleId == ArticleId);

            Cart.AddItem(Article, 1);

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(long ArticleId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Article.ArticleId == ArticleId).Article);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
