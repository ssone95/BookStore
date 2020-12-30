using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BookStore.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider provider)
        {
            ISession session = provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext
                .Session;

            SessionCart cart = session?.GetJson<SessionCart>("cart")
                ?? new SessionCart();

            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(Book book, int quantity)
        {
            base.AddItem(book, quantity);
            Session.SetJson("cart", this);
        }

        public override void RemoveLine(Book book)
        {
            base.RemoveLine(book);
            Session.SetJson("cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("cart");
        }
    }
}
