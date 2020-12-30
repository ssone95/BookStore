using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(Book book, int quantity)
        {
            CartLine line = Lines
                .Where(x => x.Book.BookId == book.BookId)
                .FirstOrDefault();

            if (line == null)
                Lines.Add(new CartLine
                {
                    Book = book,
                    Quantity = quantity
                });
            else
                line.Quantity += quantity;
        }

        public void RemoveLine(Book book) =>
            Lines.RemoveAll(x => x.Book.BookId == book.BookId);

        public decimal ComputeTotalValue() =>
            Lines.Sum(x => x.Book.Price * x.Quantity);

        public void Clear() => Lines.Clear();
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
