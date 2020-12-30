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
        IQueryable<BookGenre> BookGenres { get; }
    }
}
