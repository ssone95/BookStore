using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Repository.DbSeed
{
    public static class SeedData
    {
        public static void EnsureDataExists(IApplicationBuilder appBuilder)
        {
            DatabaseContext _context = appBuilder
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<DatabaseContext>();

            if (_context.Database.GetPendingMigrations().Any())
                _context.Database.Migrate();


            var anyBooks = _context.Books.Any();

            if (!anyBooks)
            {
                var genres = GetMainGenres();
                _context.Genres.AddRange(genres);

                _context.SaveChanges();

                var booksWithGenres = GetBooks();

                var books = booksWithGenres.Select(x => x.Key)
                    .ToList();

                _context.Books.AddRange(books);

                _context.SaveChanges();

                foreach (var book in booksWithGenres.Keys)
                {
                    var bookGenres = booksWithGenres
                        .Where(x => x.Key == book)
                        .FirstOrDefault().Value ?? new List<string>();

                    var genresToAdd = new List<BookGenre>();

                    foreach (var genre in bookGenres)
                    {
                        var genreFromDb = genres.FirstOrDefault(x => x.Name == genre);

                        var bookGenre = new BookGenre()
                        {
                            BookId = book.BookId,
                            GenreId = genreFromDb.GenreId
                        };

                        genresToAdd.Add(bookGenre);
                    }

                    _context.BookGenres.AddRange(genresToAdd);
                }

                _context.SaveChanges();
            }
        }

        static List<string> GetTableNames(this DatabaseContext context)
        {

            return new List<string>()
            {
                "Books", "Genres", "BookGenres"
            };
        }

        static Dictionary<Book, List<string>> GetBooks()
        {
            return new Dictionary<Book, List<string>>()
            {
                {
                    new Book()
                    {
                        Name = "Knjiga 1",
                        Description = "Opis knjige 1",
                        Price = GetRandomPrice()
                    }, GetRandomGenres()
                },
                {
                    new Book()
                    {
                        Name = "Knjiga 2",
                        Description = "Opis knjige 2",
                        Price = GetRandomPrice()
                    }, GetRandomGenres()
                },
                {
                    new Book()
                    {
                        Name = "Knjiga 3",
                        Description = "Opis knjige 3",
                        Price = GetRandomPrice()
                    }, GetRandomGenres()
                },
                {
                    new Book()
                    {
                        Name = "Knjiga 4",
                        Description = "Opis knjige 4",
                        Price = GetRandomPrice()
                    }, GetRandomGenres()
                },
                {
                    new Book()
                    {
                        Name = "Knjiga 5",
                        Description = "Opis knjige 5",
                        Price = GetRandomPrice()
                    }, GetRandomGenres()
                }
            };
        }

        private static List<string> GetRandomGenres()
        {
            var random = new Random();
            int numRandomGenres = random.Next(1, 5);
            var genres = GetMainGenres();
            var genresCount = genres.Count;
            List<string> selectedGenres = new List<string>();
            for (int i = 1; i <= numRandomGenres; i++)
            {
                var randomGenre = random.Next(1, genresCount);

                var selectedGenre = genres[randomGenre];

                if (!selectedGenres.Any(x => x == selectedGenre.Name))
                    selectedGenres.Add(selectedGenre.Name);
            }
            return selectedGenres;
        }

        private static decimal GetRandomPrice()
        {
            var random = new Random();
            var basePrice = (decimal)(random.Next(5, 75) * 1.0);
            var decimalPrice = (decimal)(random.NextDouble() * 99);
            return basePrice + decimalPrice;
        }

        static List<Genre> GetMainGenres()
        {
            return new List<Genre>()
            {
                new Genre() { Name = "Fantasy", Description = "Fantasy" },
                new Genre() { Name = "Action", Description = "Action" },
                new Genre() { Name = "Classics", Description = "Classics" },
                new Genre() { Name = "Mystery", Description = "Mystery" },
                new Genre() { Name = "Sci-Fi", Description = "Science fiction" },
            };
        }
    }
}
