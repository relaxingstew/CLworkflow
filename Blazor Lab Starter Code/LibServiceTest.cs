using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_Lab_Starter_Code
{
    public class LibServiceTest
    {
        public List<Book> Books { get; private set; } = new List<Book>();
        public List<User> Users { get; private set; } = new List<User>();
        public Dictionary<User, List<Book>> BorrowedBooks { get; private set; } = new Dictionary<User, List<Book>>();

        public void ReadBooks(string filePath)
        {
            Books.Clear();
            foreach (var line in File.ReadLines(filePath))
            {
                var fields = line.Split(',');

                if (fields.Length >= 4)
                {
                    var book = new Book
                    {
                        Id = int.Parse(fields[0].Trim()),
                        Title = fields[1].Trim(),
                        Author = fields[2].Trim(),
                        ISBN = fields[3].Trim()
                    };

                    Books.Add(book);
                }
            }
        }

        public Book AddBook(string title, string author, string isbn)
        {
            int id = Books.Any() ? Books.Max(b => b.Id) + 1 : 1;
            var book = new Book { Id = id, Title = title, Author = author, ISBN = isbn };
            Books.Add(book);
            return book;
        }

        public bool EditBook(int bookId, string title, string author, string isbn)
        {
            Book book = Books.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                if (!string.IsNullOrEmpty(title)) book.Title = title;
                if (!string.IsNullOrEmpty(author)) book.Author = author;
                if (!string.IsNullOrEmpty(isbn)) book.ISBN = isbn;
                return true;
            }
            return false;
        }

        public bool DeleteBook(int bookId)
        {
            Book book = Books.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                Books.Remove(book);
                return true;
            }
            return false;
        }

        public void ReadUsers(string filePath)
        {
            Users.Clear();
            foreach (var line in File.ReadLines(filePath))
            {
                var fields = line.Split(',');

                if (fields.Length >= 3)
                {
                    var user = new User
                    {
                        Id = int.Parse(fields[0].Trim()),
                        Name = fields[1].Trim(),
                        Email = fields[2].Trim()
                    };

                    Users.Add(user);
                }
            }
        }

        public User AddUser(string name, string email)
        {
            int id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
            var user = new User { Id = id, Name = name, Email = email };
            Users.Add(user);
            return user;
        }

        public bool EditUser(int userId, string name, string email)
        {
            User user = Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                if (!string.IsNullOrEmpty(name)) user.Name = name;
                if (!string.IsNullOrEmpty(email)) user.Email = email;
                return true;
            }
            return false;
        }

        public bool DeleteUser(int userId)
        {
            User user = Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                Users.Remove(user);
                return true;
            }
            return false;
        }

        public bool BorrowBook(int bookId, int userId)
        {
            Book book = Books.FirstOrDefault(b => b.Id == bookId);
            User user = Users.FirstOrDefault(u => u.Id == userId);

            if (book != null && user != null)
            {
                if (!BorrowedBooks.ContainsKey(user))
                {
                    BorrowedBooks[user] = new List<Book>();
                }
                BorrowedBooks[user].Add(book);
                Books.Remove(book);
                return true;
            }
            return false;
        }

        public bool ReturnBook(int userId, int bookIndex)
        {
            User user = Users.FirstOrDefault(u => u.Id == userId);

            if (user != null && BorrowedBooks.ContainsKey(user) &&
                bookIndex >= 0 && bookIndex < BorrowedBooks[user].Count)
            {
                Book bookToReturn = BorrowedBooks[user][bookIndex];
                BorrowedBooks[user].RemoveAt(bookIndex);
                Books.Add(bookToReturn);
                return true;
            }
            return false;
        }
    }

}
