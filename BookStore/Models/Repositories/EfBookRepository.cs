using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class EfBookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public EfBookRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        //**************************************************************
        // ----- BOOK OPERATIONS -----------
        //**************************************************************

        //Add a New Book -------------------
        public Book AddBook(Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            return book;
        }

        //Get a single Book ---------------
        public Book GetBookById(int? id)
        {
            return _db.Books.Find(id);
        }
        public Book GetBookWithAuthor(int? id)
        {
            return _db.Books.Include(b => b.Author).Where(b => b.BookId == id).FirstOrDefault();
        }
        public Book GetBookWithCategory(int? id)
        {
            return _db.Books.Include(b => b.Category).Where(b => b.BookId == id).FirstOrDefault();
        }
        public Book GetBookWithDetails(int? id)
        {
            return _db.Books.Include(b => b.Author).Include(b => b.Category).Where(b => b.BookId == id).FirstOrDefault();
        }

        //Get All Books ----------------------
        public IEnumerable<Book> GetAllBooks()
        {
            return _db.Books;
        }
        public IEnumerable<Book> GetAllBooksWithAuthor()
        {
            return _db.Books.Include(b => b.Author);
        }
        public IEnumerable<Book> GetAllBooksWithCategory()
        {
            return _db.Books.Include(b => b.Category);
        }
        public IEnumerable<Book> GetAllBooksWithDetails()
        {
            return _db.Books.Include(b => b.Author).Include(b => b.Category);
        }

        //Update a Book ----------------------
        public Book UpdateBook(Book book)
        {
            var tracker = _db.Attach(book);
            tracker.State = EntityState.Modified;
            _db.SaveChanges();
            return book;
        }

        //Delete a Book ----------------------
        public Book DeleteBook(Book book)
        {
            _db.Books.Remove(book);
            _db.SaveChanges();
            return book;
        }



        //**********************************************************************
        // ----- AUTHOR OPERATIONS -----------
        //**********************************************************************

        //Add a new Author ---------------------
        public Author AddAuthor(Author author)
        {
            _db.Authors.Add(author);
            _db.SaveChanges();
            return author;
        }

        //Get an Author -------------------
        public Author GetAuthorById(int? id)
        {
            return _db.Authors.Find(id);
        }
        public Author GetAuthorDetailsById(int? id)
        {
            return _db.Authors.Include(a => a.Books).ThenInclude(b => b.Category)
                        .SingleOrDefault(a => a.AuthorId == id);
        }
        public Author GetAuthorByBookId(int? id)
        {
            var book = _db.Books.Find(id);
            if(book != null)
            {
                return _db.Authors.Find(book.AuthorId);
            }
            return new Author();
        }

        //Get All Authors -------------------
        public IEnumerable<Author> GetAllAuthors()
        {
            return _db.Authors;
        }
        public IEnumerable<Author> GetAllAuthorsWithBook()
        {
            return _db.Authors.Include(a => a.Books);
        }
        public IEnumerable<Author> GetAllAuthorsWithDetails()
        {
            return _db.Authors.Include(a => a.Books).ThenInclude(b => b.Category);
        }

        //Update an Author -----------------
        public Author UpdateAuthor(Author author)
        {
            var tracker =_db.Attach(author);
            tracker.State = EntityState.Modified;
            _db.SaveChanges();
            return author;
        }

        //Delete an Author -----------------
        public Author DeleteAuthor(Author author)
        {
            _db.Authors.Remove(author);
            _db.SaveChanges();
            return author;
        }



        //*********************************************************************
        // ----- BOOKCATEGORY OPERATIONS -----------
        //*********************************************************************

        //Add a new Author ---------------------
        public BookCategory AddCategory(BookCategory category)
        {
            _db.BookCategories.Add(category);
            _db.SaveChanges();
            return category;
        }

        //Get an Author -------------------
        public BookCategory GetCategoryById(int? id)
        {
            return _db.BookCategories.Find(id);
        }
        public BookCategory GetCategoryDetailsById(int? id)
        {
            return _db.BookCategories.Include(c => c.Books).ThenInclude(b => b.Author)
                            .SingleOrDefault(c => c.Id == id);
        }
        public BookCategory GetCategoryByBookId(int? id)
        {
            var book = _db.Books.Find(id);
            if(book != null)
            {
                return _db.BookCategories.Find(book.CategoryId);
            }
            return new BookCategory();
        }

        //Get All Authors -------------------
        public IEnumerable<BookCategory> GetAllCategories()
        {
            return _db.BookCategories;
        }
        public IEnumerable<BookCategory> GetAllCategoriesWithBook()
        {
            return _db.BookCategories.Include(c => c.Books);
        }
        public IEnumerable<BookCategory> GetAllCategoriesWithDetails()
        {
            return _db.BookCategories.Include(c => c.Books).ThenInclude(b => b.Author);
        }

        //Update an Author -----------------
        public BookCategory UpdateCategory(BookCategory category)
        {
            var tracker = _db.Attach(category);
            tracker.State = EntityState.Modified;
            _db.SaveChanges();
            return category;
        }

        //Delete an Author -----------------
        public BookCategory DeleteCategory(BookCategory category)
        {
            _db.BookCategories.Remove(category);
            _db.SaveChanges();
            return category;
        }
    }
}
