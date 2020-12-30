using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace BookStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IStoreRepository storeRepository;

        public NavigationMenuViewComponent(IStoreRepository repo) => storeRepository = repo;

        public ViewViewComponentResult Invoke()
        {
            string genreKey = RouteData?.Values["genre"]?.ToString();
            long.TryParse(genreKey, out long genre);

            if (genre > 0)
                ViewBag.SelectedGenre = genre;
            else
                ViewBag.SelectedGenre = null;
            return View(storeRepository.Genres.Distinct()
                .OrderBy(x => x.Name));
        }
    }
}
