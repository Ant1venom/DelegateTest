// A set of classes for handling a bookstore:
namespace Bookstore
{
    using System.Collections;

    public struct Book
    {
        public readonly string Title;        
        private string Author;              
        public readonly decimal Price;       
        public readonly bool Paperback;      

        public Book(string title, string author, decimal price, bool paperBack)
        {
            Title = title;
            Author = author;
            Price = price;
            Paperback = paperBack;
        }
    }
    
    public delegate void ProcessBookCallback(Book book);
    
    public class BookDb
    {
        private readonly ArrayList _list = new();
        
        public void AddBook(string title, string author, decimal price, bool paperBack)
        {
            _list.Add(new Book(title, author, price, paperBack));
        }
        
        public void ProcessPaperbackBooks(ProcessBookCallback processBook)
        {
            foreach (Book b in _list)
            {
                if (b.Paperback)
                    processBook(b);
            }
        }
    }
}

// Using the Bookstore classes:
namespace BookTestClient
{
    using Bookstore;

    // Class to total and average prices of books:
    internal class PriceTotal
    {
        private int _countBooks;
        private decimal _priceBooks;

        internal void AddBookToTotal(Book book)
        {
            _countBooks += 1;
            _priceBooks += book.Price;
        }

        internal decimal AveragePrice()
        {
            return _priceBooks / _countBooks;
        }
    }

    // Class to test the book database:
    internal abstract class Test
    {
        // Print the title of the book.
        private static void PrintTitle(Book b)
        {
            Console.WriteLine($"   {b.Title}");
        }
        
        private static void AddBooks(BookDb bookDb)
        {
            bookDb.AddBook("The C Programming Language", "Brian W. Kernighan and Dennis M. Ritchie", 19.95m, true);
            bookDb.AddBook("The Unicode Standard 2.0", "The Unicode Consortium", 39.95m, true);
            bookDb.AddBook("The MS-DOS Encyclopedia", "Ray Duncan", 129.95m, false);
            bookDb.AddBook("Dogbert's Clues for the Clueless", "Scott Adams", 12.00m, true);
        }
        
        private static void Main()
        {

            BookDb bookDb = new();
            PriceTotal total = new();
            AddBooks(bookDb);
            
            // PrintTitle delegate
            var printVar = PrintTitle;
            ProcessBookCallback printExplicit = PrintTitle;
            
            // AddBookToTotal delegate
            var addBookVar = total.AddBookToTotal;
            ProcessBookCallback addBookExplicit = total.AddBookToTotal;
            
            // Multicast delegate
            var printAndAddTotalVar = printVar + addBookVar;   // this is an Action<Book>
            var printAndAddTotalVar2 = PrintTitle + total.AddBookToTotal;   // this is also an Action<Book>
            
            // ProcessBookCallback printAndAddTotal = PrintTitle + total.AddBookToTotal;
            var printAndAddTotalWithExplicitCast = PrintTitle + total.AddBookToTotal;
            var printAndAddTotalWithExplicitCast2 = printExplicit + addBookExplicit;
            
            // testing
            // bookDb.ProcessPaperbackBooks((ProcessBookCallback)printVar);
            bookDb.ProcessPaperbackBooks((ProcessBookCallback)PrintTitle);  // redundant type casting
            bookDb.ProcessPaperbackBooks(PrintTitle);
            bookDb.ProcessPaperbackBooks(printExplicit);

            Console.WriteLine($"Average Paperback Book Price: {total.AveragePrice():#.##}");
        }


    }
}

