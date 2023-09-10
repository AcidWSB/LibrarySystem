using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public class Library
    {
        public List<Book> Books { get; set; }
        private string FilePath { get; set; } = "books.json";
        
        // Serializing books to json
        public void SerializeBooksToJson(Library books)
        {
            try
            {
                string json = JsonConvert.SerializeObject(books, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during serialization: {ex.Message}");
            }
        }

        // Deserializing books from books.json
        public Library DeserializeBooksFromJson()
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                Library deserializedBooks = JsonConvert.DeserializeObject<Library>(json);

                if (deserializedBooks != null)
                {
                    return deserializedBooks;
                }
                else
                {
                    Console.WriteLine("No books were found in the file.");
                    return new Library();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during deserialization: {ex.Message}");
                return new Library();
            }
        }

        // Get's all books that available in Library
        public Library GetAvailableBooks()
        {
            Library availableBooksLibrary = DeserializeBooksFromJson();
            availableBooksLibrary.Books = availableBooksLibrary.Books.Where(book => book.IsAvailable).ToList();
            return availableBooksLibrary;
        }

        // Add's new Book to Library
        public void AddBook(Book newBook)
        {
            Library existingBooks = DeserializeBooksFromJson();

            if (existingBooks.Books != null && (existingBooks.Books.Any(book => book.Title.Equals(newBook.Title, StringComparison.OrdinalIgnoreCase) &&
                                   book.Author.Equals(newBook.Author, StringComparison.OrdinalIgnoreCase)) ||
                existingBooks.Books.Any(book => book.ISBN == newBook.ISBN)))
            {
                Console.WriteLine("A book with the same title, author, or ISBN already exists in the library.");
                return;
            }

            existingBooks.Books.Add(newBook);
            SerializeBooksToJson(existingBooks);

            Console.WriteLine("The book has been successfully added to the library.");
        }

        // Removes book from library
        public void RemoveBook(int isbnToRemove)
        {
            Library existingBooks = DeserializeBooksFromJson();
            Book bookToRemove = existingBooks.Books.FirstOrDefault(book => book.ISBN == isbnToRemove);

            if (bookToRemove != null)
            {
                existingBooks.Books.Remove(bookToRemove);
                SerializeBooksToJson(existingBooks);

                Console.WriteLine("The book has been successfully removed from the file.");
            }
            else
            {
                Console.WriteLine("The book with the specified ISBN was not found in the file.");
            }
        }

        // Method that lend a book from library
        public void LendBook(int isbn, string readerName, Clients clients)
        {
            Library existingBooks = DeserializeBooksFromJson();
            Book bookToLend = existingBooks.Books.FirstOrDefault(book => book.ISBN == isbn);


            if (bookToLend != null)
            {
                if (bookToLend.IsAvailable)
                {
                    bookToLend.IsAvailable = false;
                    clients.AddBookToReader(readerName, bookToLend); 
               
                    SerializeBooksToJson(existingBooks);
                    Console.WriteLine($"The book (ISBN: {isbn}) has been successfully lent to {readerName}.");
                }
                else
                {
                    Console.WriteLine($"The book (ISBN: {isbn}) is not available for lending.");
                }
            }
            else
            {
                Console.WriteLine($"The book with ISBN {isbn} was not found in the library.");
            }
        }

        // Method that help to return book to library
        public void ReturnBook(int isbn, string readerName)
        {
            Library existingBooks = DeserializeBooksFromJson();
            Clients clients = new Clients();
            clients.DeserializeClientsFromJson();
            Book bookToReturn = existingBooks.Books.FirstOrDefault(book => book.ISBN == isbn);

            if (bookToReturn != null)
            {
                bookToReturn.IsAvailable = true;
                clients.RemoveBookFromReader(readerName, isbn);
                SerializeBooksToJson(existingBooks);

                Console.WriteLine($"The book (ISBN: {isbn}) has been successfully returned by {readerName}.");
            }
            else
            {
                Console.WriteLine($"The book with ISBN {isbn} was not found in the library.");
            }
        }
    }
}
