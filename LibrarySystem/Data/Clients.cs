using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public class Clients
    {
        private List<Reader> ClientsList { get; set; }

        private string FilePath { get; set; } = "clients.json";

        public Clients()
        {
            ClientsList = new List<Reader>();
        }

        /// <summary>
        /// Serializing clients to clients.json
        /// </summary>
        /// <param name="clients"></param>
        public void SerializeClientsToJson(Clients clients)
        {
            try
            {
                string json = JsonConvert.SerializeObject(clients, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during serialization: {ex.Message}");
            }
        }

        /// <summary>
        /// Deserializing clients from clients.json
        /// </summary>
        /// <returns></returns>
        public Clients DeserializeClientsFromJson()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    Clients clients = JsonConvert.DeserializeObject<Clients>(json);
                    return clients;
                }
                else
                {
                    Console.WriteLine("Clients file does not exist. Creating a new one.");
                    return new Clients();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during deserialization: {ex.Message}");
                return new Clients();
            }
        }

        /// <summary>
        /// Adds book to borowed book of reader
        /// </summary>
        /// <param name="readerName"></param>
        /// <param name="book"></param>
        public void AddBookToReader(string readerName, Book book)
        {
            Clients clients = DeserializeClientsFromJson();
            Reader reader = clients.ClientsList.FirstOrDefault(r => r.Name.Equals(readerName, StringComparison.OrdinalIgnoreCase));
            if (reader == null)
            {
                reader = new Reader { Name = readerName, BorrowedBooks = new List<Book>() };
                clients.ClientsList.Add(reader);
                Console.WriteLine($"New reader '{reader.Name}' has been created.");
            }

            reader.BorrowedBooks.Add(book);
            Console.WriteLine($"Book with ISBN {book.ISBN} has been added to {reader.Name}'s BorrowedBooks.");
            SerializeClientsToJson(clients);
        }

        /// <summary>
        /// Remove book from borowed book of reader
        /// </summary>
        /// <param name="readerName"></param>
        /// <param name="isbn"></param>
        public void RemoveBookFromReader(string readerName, int isbn)
        {
            Clients clients = DeserializeClientsFromJson();
            Reader reader = clients.ClientsList.FirstOrDefault(r => r.Name.Equals(readerName, StringComparison.OrdinalIgnoreCase));
            if (reader != null)
            {
                Book bookToRemove = reader.BorrowedBooks.FirstOrDefault(book => book.ISBN == isbn);
                if (bookToRemove != null)
                {
                    reader.BorrowedBooks.Remove(bookToRemove);

                    Console.WriteLine($"Book with ISBN {isbn} has been removed from {reader.Name}'s BorrowedBooks.");

                    int index = clients.ClientsList.FindIndex(r => r.Name.Equals(readerName, StringComparison.OrdinalIgnoreCase));
                    if (index >= 0)
                    {
                        clients.ClientsList[index] = reader;
                    }

                    SerializeClientsToJson(clients);
                }
                else
                {
                    Console.WriteLine($"Book with ISBN {isbn} is not in {reader.Name}'s BorrowedBooks.");
                }
            }
            else
            {
                Console.WriteLine($"Reader with name {readerName} not found.");
            }
        }
    }
}
