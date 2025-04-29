using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blazor_Lab_Starter_Code
{
    public class Program
    {
        static LibServiceTest libraryService = new LibServiceTest();

        static void Main()
        {
            libraryService.ReadBooks("./Data/Books.csv");
            libraryService.ReadUsers("./Data/Users.csv");

            string option;

            do
            {
                Console.WriteLine("Library Management System");
                Console.WriteLine("1. Manage Books");
                Console.WriteLine("2. Manage Users");
                Console.WriteLine("3. Borrow Book");
                Console.WriteLine("4. Return Book");
                Console.WriteLine("5. List Borrowed Books");
                Console.WriteLine("6. Exit");

                Console.Write("Choose an option: ");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ManageBooks();
                        break;
                    case "2":
                        ManageUsers();
                        break;
                    case "3":
                        BorrowBook();
                        break;
                    case "4":
                        ReturnBook();
                        break;
                    case "5":
                        ListBorrowedBooks();
                        break;
                }
            } while (option != "6");
        }

        static void ManageBooks()
        {
            string option;

            do
            {
                Console.WriteLine("\nManage Books");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Edit Book");
                Console.WriteLine("3. Delete Book");
                Console.WriteLine("4. List Books");
                Console.WriteLine("5. Back");
                Console.Write("Choose an option: ");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AddBook();
                        break;
                    case "2":
                        EditBook();
                        break;
                    case "3":
                        DeleteBook();
                        break;
                    case "4":
                        ListBooks();
                        break;
                }
            } while (option != "5");
        }

        static void AddBook()
        {
            Console.Write("\nEnter Book Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Author: ");
            string author = Console.ReadLine();

            Console.Write("Enter ISBN: ");
            string isbn = Console.ReadLine();

            libraryService.AddBook(title, author, isbn);
            Console.WriteLine("Book added successfully!\n");
        }

        static void EditBook()
        {
            ListBooks();
            Console.Write("\nEnter Book ID to Edit: ");

            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.Write("Enter new Title (leave blank to keep current): ");
                string title = Console.ReadLine();

                Console.Write("Enter new Author (leave blank to keep current): ");
                string author = Console.ReadLine();

                Console.Write("Enter new ISBN (leave blank to keep current): ");
                string isbn = Console.ReadLine();

                if (libraryService.EditBook(bookId, title, author, isbn))
                {
                    Console.WriteLine("Book updated successfully!\n");
                }
                else
                {
                    Console.WriteLine("Book not found!\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid input!\n");
            }
        }

        static void DeleteBook()
        {
            ListBooks();
            Console.Write("\nEnter Book ID to Delete: ");

            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                if (libraryService.DeleteBook(bookId))
                {
                    Console.WriteLine("Book deleted successfully!\n");
                }
                else
                {
                    Console.WriteLine("Book not found!\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid input!\n");
            }
        }

        static void ListBooks()
        {
            Console.WriteLine("\nAvailable Books:");

            var books = libraryService.Books;
            var bookGroups = books
                .GroupBy(b => b.Id)
                .Select(bookGroup => new { Book = bookGroup.First(), Count = bookGroup.Count() });

            foreach (var group in bookGroups)
            {
                Console.WriteLine($"{group.Book.Id}. {group.Book.Title} by {group.Book.Author} (ISBN: {group.Book.ISBN}) - Available Copies: {group.Count}");
            }

            Console.WriteLine();
        }

        static void ManageUsers()
        {
            string option;

            do
            {
                Console.WriteLine("\nManage Users");
                Console.WriteLine("1. Add User");
                Console.WriteLine("2. Edit User");
                Console.WriteLine("3. Delete User");
                Console.WriteLine("4. List Users");
                Console.WriteLine("5. Back");

                Console.Write("Choose an option: ");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AddUser();
                        break;
                    case "2":
                        EditUser();
                        break;
                    case "3":
                        DeleteUser();
                        break;
                    case "4":
                        ListUsers();
                        break;
                }
            } while (option != "5");
        }

        static void AddUser()
        {
            Console.Write("\nEnter User Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            libraryService.AddUser(name, email);
            Console.WriteLine("User added successfully!\n");
        }

        static void EditUser()
        {
            ListUsers();
            Console.Write("\nEnter User ID to Edit: ");

            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.Write("Enter new Name (leave blank to keep current): ");
                string name = Console.ReadLine();

                Console.Write("Enter new Email (leave blank to keep current): ");
                string email = Console.ReadLine();

                if (libraryService.EditUser(userId, name, email))
                {
                    Console.WriteLine("User updated successfully!\n");
                }
                else
                {
                    Console.WriteLine("User not found!\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid input!\n");
            }
        }

        static void DeleteUser()
        {
            ListUsers();
            Console.Write("\nEnter User ID to Delete: ");

            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                if (libraryService.DeleteUser(userId))
                {
                    Console.WriteLine("User deleted successfully!\n");
                }
                else
                {
                    Console.WriteLine("User not found!\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid input!\n");
            }
        }

        static void ListUsers()
        {
            Console.WriteLine("\nUsers:");

            foreach (var user in libraryService.Users)
            {
                Console.WriteLine($"{user.Id}. {user.Name} (Email: {user.Email})");
            }

            Console.WriteLine();
        }

        static void BorrowBook()
        {
            ListBooks();
            Console.Write("\nEnter Book ID to Borrow: ");

            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                ListUsers();
                Console.Write("\nEnter User ID who is borrowing the book: ");

                if (int.TryParse(Console.ReadLine(), out int userId))
                {
                    if (libraryService.BorrowBook(bookId, userId))
                    {
                        Console.WriteLine("Book borrowed successfully!\n");
                    }
                    else
                    {
                        Console.WriteLine("Book or user not found or no available copies!\n");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input!\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid input!\n");
            }
        }

        static void ReturnBook()
        {
            ListBorrowedBooks();
            Console.Write("\nEnter User ID to return a book for: ");

            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                var user = libraryService.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null && libraryService.BorrowedBooks.ContainsKey(user) && libraryService.BorrowedBooks[user].Count > 0)
                {
                    Console.WriteLine("Borrowed Books:");

                    for (int i = 0; i < libraryService.BorrowedBooks[user].Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {libraryService.BorrowedBooks[user][i].Title} by {libraryService.BorrowedBooks[user][i].Author} (ISBN: {libraryService.BorrowedBooks[user][i].ISBN})");
                    }

                    Console.Write("\nEnter the number of the book to return: ");

                    if (int.TryParse(Console.ReadLine(), out int bookNumber) && bookNumber >= 1 && bookNumber <= libraryService.BorrowedBooks[user].Count)
                    {
                        if (libraryService.ReturnBook(userId, bookNumber - 1))
                        {
                            Console.WriteLine("Book returned successfully!\n");
                        }
                        else
                        {
                            Console.WriteLine("Failed to return book!\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input!\n");
                    }
                }
                else
                {
                    Console.WriteLine("User not found or no borrowed books!\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid input!\n");
            }
        }

        static void ListBorrowedBooks()
        {
            Console.WriteLine("\nBorrowed Books:");

            foreach (var entry in libraryService.BorrowedBooks)
            {
                Console.WriteLine($"User: {entry.Key.Name}");

                foreach (var book in entry.Value)
                {
                    Console.WriteLine($"{book.Title} by {book.Author} (ISBN: {book.ISBN})");
                }

                Console.WriteLine();
            }
        }
    }
}