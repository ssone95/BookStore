using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BookStore.Models;
using Xunit;

namespace BookStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            Book b1 = new Book { BookId = 1, Name = "B1" };
            Book b2 = new Book { BookId = 2, Name = "B2" };

            Cart target = new Cart();

            target.AddItem(b1, 1);
            target.AddItem(b2, 1);

            CartLine[] results = target.Lines.ToArray();
            Assert.Equal(2, results.Length);
            Assert.Equal(b1, results[0].Book);
            Assert.Equal(b2, results[1].Book);
        }

        [Fact]
        public void Can_Ad_Quantity_For_Existing_Lines()
        {
            Book b1 = new Book { BookId = 1, Name = "B1" };
            Book b2 = new Book { BookId = 2, Name = "B2" };

            Cart target = new Cart();

            target.AddItem(b1, 1);
            target.AddItem(b2, 1);

            target.AddItem(b1, 10);

            CartLine[] results = target.Lines.ToArray();


            Assert.Equal(2, results.Length);

            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            Book b1 = new Book { BookId = 1, Name = "B1" };
            Book b2 = new Book { BookId = 2, Name = "B2" };
            Book b3 = new Book { BookId = 3, Name = "B3" };

            Cart target = new Cart();

            target.AddItem(b1, 1);
            target.AddItem(b2, 3);
            target.AddItem(b3, 5);

            target.AddItem(b2, 1);

            target.RemoveLine(b2);

            Assert.Empty(target.Lines.Where(x => x.Book == b2));
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            Book b1 = new Book { BookId = 1, Name = "B1", Price = 100M };
            Book b2 = new Book { BookId = 2, Name = "B2", Price = 50M };

            Cart target = new Cart();
            target.AddItem(b1, 1);
            target.AddItem(b2, 1);
            target.AddItem(b1, 3);

            decimal result = target.ComputeTotalValue();

            Assert.Equal(450M, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            Book b1 = new Book { BookId = 1, Name = "B1", Price = 100M };
            Book b2 = new Book { BookId = 2, Name = "B2", Price = 50M };

            Cart target = new Cart();
            target.AddItem(b1, 1);
            target.AddItem(b2, 1);

            target.Clear();

            Assert.Empty(target.Lines);
        }
    }
}
