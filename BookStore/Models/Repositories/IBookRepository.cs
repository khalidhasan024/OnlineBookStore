using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public interface IBookRepository
    {
        //**************************************************************
        // ----- BOOK OPERATIONS -----------
        //**************************************************************

        //Add a New Book -------------------
        Book AddBook(Book book);

        //Get a single Book ---------------
        Book GetBookById(int? id);
        Book GetBookWithAuthor(int? id);
        Book GetBookWithCategory(int? id);
        Book GetBookWithDetails(int? id);

        //Get All Books ----------------------
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetAllBooksWithAuthor();
        IEnumerable<Book> GetAllBooksWithCategory();
        IEnumerable<Book> GetAllBooksWithDetails();

        //Update a Book ----------------------
        Book UpdateBook(Book model);

        //Delete a Book ----------------------
        Book DeleteBook(Book book);



        //**********************************************************************
        // ----- AUTHOR OPERATIONS -----------
        //**********************************************************************

        //Add a new Author ---------------------
        Author AddAuthor(Author author);

        //Get an Author -------------------
        Author GetAuthorById(int? id);
        Author GetAuthorDetailsById(int? id);
        Author GetAuthorByBookId(int? id);

        //Get All Authors -------------------
        IEnumerable<Author> GetAllAuthors();
        IEnumerable<Author> GetAllAuthorsWithBook();
        IEnumerable<Author> GetAllAuthorsWithDetails();

        //Update an Author -----------------
        Author UpdateAuthor(Author author);

        //Delete an Author -----------------
        Author DeleteAuthor(Author author);



        //*********************************************************************
        // ----- BOOKCATEGORY OPERATIONS -----------
        //*********************************************************************

        //Add a new Author ---------------------
        BookCategory AddCategory(BookCategory category);

        //Get an Author -------------------
        BookCategory GetCategoryById(int? id);
        BookCategory GetCategoryDetailsById(int? id);
        BookCategory GetCategoryByBookId(int? id);

        //Get All Authors -------------------
        IEnumerable<BookCategory> GetAllCategories();
        IEnumerable<BookCategory> GetAllCategoriesWithBook();
        IEnumerable<BookCategory> GetAllCategoriesWithDetails();

        //Update an Author -----------------
        BookCategory UpdateCategory(BookCategory category);

        //Delete an Author -----------------
        BookCategory DeleteCategory(BookCategory category);
    }
}
