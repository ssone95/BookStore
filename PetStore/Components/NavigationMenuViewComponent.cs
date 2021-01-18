using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace PetStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IStoreRepository storeRepository;

        public NavigationMenuViewComponent(IStoreRepository repo) => storeRepository = repo;

        public ViewViewComponentResult Invoke()
        {
            string ArticleTypeKey = RouteData?.Values["ArticleType"]?.ToString();
            long.TryParse(ArticleTypeKey, out long ArticleType);

            if (ArticleType > 0)
                ViewBag.SelectedArticleType = ArticleType;
            else
                ViewBag.SelectedArticleType = null;
            return View(storeRepository.ArticleTypes.Distinct()
                .OrderBy(x => x.Name));
        }
    }
}
