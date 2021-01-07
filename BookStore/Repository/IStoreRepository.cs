using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Repository
{
    public interface IStoreRepository
    {
        IQueryable<Book> Books { get; }
        IQueryable<Genre> Genres { get; }

        void SaveBook(Book b);
        void CreateBook(Book b);
        void DeleteBook(Book b);
        void SaveGenre(Genre g);
        void CreateGenre(Genre g);
        void DeleteGenre(Genre g);
    }
}
