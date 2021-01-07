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

        public void CreateBook(Book b)
        {
            _context.Add(b);
            _context.SaveChanges();
        }

        public void CreateGenre(Genre g)
        {
            _context.Add(g);
            _context.SaveChanges();
        }

        public void DeleteBook(Book b)
        {
            _context.Remove(b);
            _context.SaveChanges();
        }

        public void DeleteGenre(Genre g)
        {
            _context.Remove(g);
            _context.SaveChanges();
        }

        public void SaveBook(Book b)
        {
            _context.SaveChanges();
        }

        public void SaveGenre(Genre g)
        {
            _context.SaveChanges();
        }
    }
}
