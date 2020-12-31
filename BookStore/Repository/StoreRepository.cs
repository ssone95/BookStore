using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private DatabaseContext _context;

        public StoreRepository(DatabaseContext ctx) => _context = ctx;

        public IQueryable<Book> Books => _context.Books;

        public IQueryable<Genre> Genres => _context.Genres;

        public void SaveBook(Book b)
        {
            throw new NotImplementedException();
        }

        public void SaveGenre(Genre g)
        {
            throw new NotImplementedException();
        }
    }
}
