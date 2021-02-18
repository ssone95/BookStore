using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using PetStore.Models;
using Xunit;

namespace PetStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            Article b1 = new Article { ArticleId = 1, Name = "Article 1" };
            Article b2 = new Article { ArticleId = 2, Name = "Article 2" };

            Cart target = new Cart();

            target.AddItem(b1, 1);
            target.AddItem(b2, 1);

            CartLine[] results = target.Lines.ToArray();
            Assert.Equal(2, results.Length);
            Assert.Equal(b1, results[0].Article);
            Assert.Equal(b2, results[1].Article);
        }

        [Fact]
        public void Can_Ad_Quantity_For_Existing_Lines()
        {
            Article b1 = new Article { ArticleId = 1, Name = "Article 1" };
            Article b2 = new Article { ArticleId = 2, Name = "Article 2" };

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
            Article b1 = new Article { ArticleId = 1, Name = "Article 1" };
            Article b2 = new Article { ArticleId = 2, Name = "Article 2" };
            Article b3 = new Article { ArticleId = 3, Name = "Article 3" };

            Cart target = new Cart();

            target.AddItem(b1, 1);
            target.AddItem(b2, 3);
            target.AddItem(b3, 5);

            target.AddItem(b2, 1);

            target.RemoveLine(b2);

            Assert.Empty(target.Lines.Where(x => x.Article == b2));
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            Article b1 = new Article { ArticleId = 1, Name = "Article 1", Price = 100M };
            Article b2 = new Article { ArticleId = 2, Name = "Article 2", Price = 50M };

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
            Article b1 = new Article { ArticleId = 1, Name = "Article 1", Price = 100M };
            Article b2 = new Article { ArticleId = 2, Name = "Article 2", Price = 50M };

            Cart target = new Cart();
            target.AddItem(b1, 1);
            target.AddItem(b2, 1);

            target.Clear();

            Assert.Empty(target.Lines);
        }
    }
}
