using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public class LibraryScreen
    {
        private Library library = new Library();
        private Clients clients = new Clients();

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("Library Menu:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Add a book");
                Console.WriteLine("2. Delete a book");
                Console.WriteLine("3. Lend a book");
                Console.WriteLine("4. Return a book");
                Console.WriteLine("5. Show available book");
                Console.Write("Enter your choice: ");

                if (Enum.TryParse(Console.ReadLine(), out MenuChoices choice))
                {
                    switch (choice)
                    {
                        case MenuChoices.AddBook:
                            AddBookToLibrary();
                            Console.WriteLine("Press any button to continue");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case MenuChoices.RemoveBook:
                            RemoveBookFromLibrary();
                            Console.WriteLine("Press any button to continue");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case MenuChoices.LendBook:
                            LendBookFromLibrary();
                            Console.WriteLine("Press any button to continue");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case MenuChoices.ReturnBook:
                            ReturnBookToLibrary();
                            Console.Clear();
                            break;
                        case MenuChoices.ShowAvailableBooks:
                            ShowAvailableBooks();
                            break;
                        case MenuChoices.Exit:
                            Console.WriteLine("Goodbye!!!!");
                            Thread.Sleep(700);
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid menu option.");
                }
            }
        }

        #region Private Methods

        private void ShowAvailableBooks()
        {
            Library availableBooksLibrary = library.GetAvailableBooks();

            Console.WriteLine("Available Books:");
            foreach (Book book in availableBooksLibrary.Books)
            {
                Console.WriteLine($"Title: {book.Title,-50} |()| Author: {book.Author,-30} |()| ISBN: {book.ISBN}");
            }
        }

        private void ReturnBookToLibrary()
        {
            Console.Write("Enter the ISBN of the book to return: ");
            if (int.TryParse(Console.ReadLine(), out int returnIsbn))
            {
                Console.Write("Enter the reader's name: ");
                string returnReaderName = Console.ReadLine();
                clients.DeserializeClientsFromJson();
                library.ReturnBook(returnIsbn, returnReaderName);
            }
            else
            {
                Console.WriteLine("Invalid ISBN format. Enter a valid ISBN (6 digits).");
            }
        }

        private void LendBookFromLibrary()
        {
            Console.Write("Enter the ISBN of the book to lend: ");
            if (int.TryParse(Console.ReadLine(), out int isbn))
            {
                Console.Write("Enter the reader's name: ");
                string readerName = Console.ReadLine();

                library.LendBook(isbn, readerName, clients);
            }
            else
            {
                Console.WriteLine("Invalid ISBN format. Enter a valid ISBN (6 digits).");
            }
        }

        private void RemoveBookFromLibrary()
        {
            Console.Write("Please enter ISBN of book , you want to remove: ");
            int isbnToRemove = Convert.ToInt32(Console.ReadLine());
            library.RemoveBook(isbnToRemove);
        }

        private void AddBookToLibrary()
        {
            Console.WriteLine("Enter book information:");

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Author: ");
            string author = Console.ReadLine();

            Console.Write("ISBN (6 digits): ");
            if (!int.TryParse(Console.ReadLine(), out int isbn) || isbn < 100000 || isbn > 999999)
            {
                Console.WriteLine("Invalid ISBN format. Enter 6 digits.");
                return;
            }

            Book newBook = new Book
            {
                Title = title,
                Author = author,
                ISBN = isbn,
                IsAvailable = true
            };
            library.AddBook(newBook);
        }
        #endregion
    }
}
