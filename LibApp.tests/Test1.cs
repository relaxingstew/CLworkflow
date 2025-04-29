using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blazor_Lab_Starter_Code;

namespace LibApp.tests
{

    [TestClass]
    public class LibraryTests
    {
        private List<Book> books;
        private List<User> users;
        private Dictionary<User, List<Book>> borrowedBooks;

        [TestClass]
        public class BookTests
        {
            [TestMethod]
            public void AddBook_ValidData_True()
            {
                
                var library = new LibServiceTest();
                string title = "Test Book";
                string author = "Test Author";
                string isbn = "123-456-789";

                
                var result = library.AddBook(title, author, isbn);

                
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(title, result.Title);
                Assert.AreEqual(author, result.Author);
                Assert.AreEqual(isbn, result.ISBN);
                Assert.AreEqual(1, library.Books.Count);
            }

            [TestMethod]
            public void AddBook_MultipleBooks_True()
            {
                
                var library = new LibServiceTest();

                
                var book1 = library.AddBook("Book 1", "Author 1", "isbn-1");

                var book2 = library.AddBook("Book 2", "Author 2", "ISBN-2");
                var book3 = library.AddBook("Book 3", "Author 3", "isbn-3");

                
                Assert.AreEqual(1, book1.Id);
                Assert.AreEqual(2, book2.Id);
                Assert.AreEqual(3, book3.Id);
                Assert.AreEqual(3, library.Books.Count);
            }

            [TestMethod]
            public void EditBook_ExistingBook_True()
            {
                
                var library = new LibServiceTest();
                var book = library.AddBook("Original Title", "Original Author", "Original ISBN");
                string newTitle = "Updated Title";
                string newAuthor = "Updated Author";
                string newISBN = "Updated ISBN";

                
                bool result = library.EditBook(book.Id, newTitle, newAuthor, newISBN);
                var updatedBook = library.Books.FirstOrDefault(b => b.Id == book.Id);

                
                Assert.IsTrue(result);
                Assert.IsNotNull(updatedBook);
                Assert.AreEqual(newTitle, updatedBook.Title);
                Assert.AreEqual(newAuthor, updatedBook.Author);
                Assert.AreEqual(newISBN, updatedBook.ISBN);
            }

            [TestMethod]
            public void EditBook_NonExistingBook_False()
            {
                var library = new LibServiceTest();
                library.AddBook("Original Title", "Original Author", "Original ISBN");
                bool result = library.EditBook(999, "New Title", "New Author", "New ISBN");

                Assert.IsFalse(result);
            }


            [TestMethod]
            public void EditBook_EmptyFields_True()
            {
                var library = new LibServiceTest();
                var book = library.AddBook("Original Title", "Original Author", "Original ISBN");

                bool result = library.EditBook(book.Id, "", "", "New ISBN");
                var updatedBook = library.Books.FirstOrDefault(b => b.Id == book.Id);


                Assert.IsTrue(result);
                Assert.AreEqual("Original Title", updatedBook.Title);
            
                Assert.AreEqual("Original Author", updatedBook.Author);
                Assert.AreEqual("New ISBN", updatedBook.ISBN);
            }

            [TestMethod]
            public void DeleteBook_ExistingBook_True()
            {
                var library = new LibServiceTest();
                var book = library.AddBook("Test Book", "Test Author", "Test ISBN");

                bool result = library.DeleteBook(book.Id);

                Assert.IsTrue(result);
                Assert.AreEqual(0, library.Books.Count);
            }

            [TestMethod]
            public void DeleteBook_NonExistingBook_False()
            {

                var library = new LibServiceTest();
                library.AddBook("Test Book", "Test Author", "Test ISBN");

                bool result = library.DeleteBook(999);
                Assert.IsFalse(result);
                Assert.AreEqual(1, library.Books.Count);
            }

            [TestMethod]
            public void ReadBooks_ValidCsvFile_True()
            {
                var library = new LibServiceTest();
                string testFilePath = "TestBooks.csv";
                File.WriteAllText(testFilePath, "1,Test Book 1,Test Author 1,ISBN-1\n2,Test Book 2,Test Author 2,ISBN-2");

                library.ReadBooks(testFilePath);

                Assert.AreEqual(2, library.Books.Count);
                Assert.AreEqual(1, library.Books[0].Id);
                Assert.AreEqual("Test Book 1", library.Books[0].Title);
                Assert.AreEqual("Test Author 1", library.Books[0].Author);
                Assert.AreEqual("ISBN-1", library.Books[0].ISBN);

                File.Delete(testFilePath);
            }

            [TestMethod]
            public void ReadBooks_InvalidCsvFormat_True()
            {
                
                var library = new LibServiceTest();
                string testFilePath = "./Data/Books.csv";
                File.WriteAllText(testFilePath, "1,Test Book 1,Test Author 1,ISBN-1\nInvalid Line\n2,Test Book 2,Test Author 2,ISBN-2");

                
                library.ReadBooks(testFilePath);

                
                Assert.AreEqual(2, library.Books.Count);

                File.Delete(testFilePath);
            }

            [TestMethod]
            [ExpectedException(typeof(FileNotFoundException))]
            public void ReadBooks_FileNotFound_Ex()
            {
                
                var library = new LibServiceTest();
                string nonExistentFile = "NonExistentFile.csv";

                
                library.ReadBooks(nonExistentFile);

            }
        }

        [TestClass]
        public class UserTests
        {
            [TestMethod]
            public void AddUser_ValidData_True()
            {
                
                var library = new LibServiceTest();
                string name = "Test User";
                string email = "test@example.com";

                
                var result = library.AddUser(name, email);

                
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(name, result.Name);
                Assert.AreEqual(email, result.Email);
                Assert.AreEqual(1, library.Users.Count);
            }

            [TestMethod]
            public void AddUser_MultipleUsers_True()
            {
                
                var library = new LibServiceTest();

                
                var user1 = library.AddUser("User 1", "user1@example.com");
                var user2 = library.AddUser("User 2", "user2@example.com");
                var user3 = library.AddUser("User 3", "user3@example.com");

                
                Assert.AreEqual(1, user1.Id);
                Assert.AreEqual(2, user2.Id);
                Assert.AreEqual(3, user3.Id);
                Assert.AreEqual(3, library.Users.Count);
            }

            [TestMethod]
            public void EditUser_ExistingUser_True()
            {
                
                var library = new LibServiceTest();
                var user = library.AddUser("Original Name", "original@example.com");
                string newName = "Updated Name";
                string newEmail = "updated@example.com";

                
                bool result = library.EditUser(user.Id, newName, newEmail);
                var updatedUser = library.Users.FirstOrDefault(u => u.Id == user.Id);

                
                Assert.IsTrue(result);
                Assert.IsNotNull(updatedUser);
                Assert.AreEqual(newName, updatedUser.Name);
                Assert.AreEqual(newEmail, updatedUser.Email);
            }

            [TestMethod]
            public void EditUser_NonExistingUser_False()
            {
                
                var library = new LibServiceTest();
                library.AddUser("Original Name", "original@example.com");

                
                bool result = library.EditUser(999, "New Name", "new@example.com");

                
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void EditUser_EmptyFields_True()
            {
                
                var library = new LibServiceTest();
                var user = library.AddUser("Original Name", "original@example.com");

                
                bool result = library.EditUser(user.Id, "", "new@example.com");
                var updatedUser = library.Users.FirstOrDefault(u => u.Id == user.Id);

                
                Assert.IsTrue(result);
                Assert.AreEqual("Original Name", updatedUser.Name);
                Assert.AreEqual("new@example.com", updatedUser.Email);
            }

            [TestMethod]
            public void DeleteUser_ExistingUser_True()
            {
                
                var library = new LibServiceTest();
                var user = library.AddUser("Test User", "test@example.com");

                
                bool result = library.DeleteUser(user.Id);

                
                Assert.IsTrue(result);
                Assert.AreEqual(0, library.Users.Count);
            }

            [TestMethod]
            public void DeleteUser_NonExistingUser_False()
            {
                
                var library = new LibServiceTest();
                library.AddUser("Test User", "test@example.com");

                
                bool result = library.DeleteUser(999);

                
                Assert.IsFalse(result);
                Assert.AreEqual(1, library.Users.Count);
            }

            [TestMethod]
            public void ReadUsers_ValidCsvFile_PopulatesUsersList()
            {
                
                var library = new LibServiceTest();
                string testFilePath = "./Data/Users.csv";
                File.WriteAllText(testFilePath, "1,User 1,user1@example.com\n2,User 2,user2@example.com");

                
                library.ReadUsers(testFilePath);

                
                Assert.AreEqual(2, library.Users.Count);
                Assert.AreEqual(1, library.Users[0].Id);
                Assert.AreEqual("User 1", library.Users[0].Name);
                Assert.AreEqual("user1@example.com", library.Users[0].Email);

                File.Delete(testFilePath);
            }

            [TestMethod]
            public void ReadUsers_InvalidCsvFormat_SkipsInvalidLines()
            {
                
                var library = new LibServiceTest();
                string testFilePath = "TestUsers.csv";
                File.WriteAllText(testFilePath, "1,User 1,user1@example.com\nInvalid Line\n2,User 2,user2@example.com");

                
                library.ReadUsers(testFilePath);

                
                Assert.AreEqual(2, library.Users.Count);

                File.Delete(testFilePath);
            }

            [TestMethod]
            [ExpectedException(typeof(FileNotFoundException))]
            public void ReadUsers_FileNotFound_Ex()
            {
                
                var library = new LibServiceTest();
                string nonExistentFile = "NonExistentFile.csv";

                
                library.ReadUsers(nonExistentFile);


            }
        }

        [TestClass]
        public class BorrowingTests
        {
            [TestMethod]
            public void BorrowBook_ValidBookAndUser_AddsToBorrowedAndRemovesFromLibrary_True()
            {
                
                var library = new LibServiceTest();
                var book = library.AddBook("Test Book", "Test Author", "Test ISBN");
                var user = library.AddUser("Test User", "test@example.com");

                
                bool result = library.BorrowBook(book.Id, user.Id);

                
                Assert.IsTrue(result);
                Assert.AreEqual(0, library.Books.Count);
                Assert.IsTrue(library.BorrowedBooks.ContainsKey(user));
                Assert.AreEqual(1, library.BorrowedBooks[user].Count);
                Assert.AreEqual(book.Title, library.BorrowedBooks[user][0].Title);
            }

            [TestMethod]
            public void BorrowBook_InvalidBook_ReturnsFalse()
            {
                
                var library = new LibServiceTest();
                var user = library.AddUser("Test User", "test@example.com");

                
                bool result = library.BorrowBook(999, user.Id);

                
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void BorrowBook_InvalidUser_False()
            {
                
                var library = new LibServiceTest();
                var book = library.AddBook("Test Book", "Test Author", "Test ISBN");

                
                bool result = library.BorrowBook(book.Id, 999);

                
                Assert.IsFalse(result);
                Assert.AreEqual(1, library.Books.Count);
            }

            [TestMethod]
            public void ReturnBook_ValidUserAndBook_RemovesFromBorrowedAndAddsToLibrary()
            {
                
                var library = new LibServiceTest();
                var book = library.AddBook("Test Book", "Test Author", "Test ISBN");
                var user = library.AddUser("Test User", "test@example.com");
                library.BorrowBook(book.Id, user.Id);

                
                bool result = library.ReturnBook(user.Id, 0);

                
                Assert.IsTrue(result);
                Assert.AreEqual(1, library.Books.Count);
                Assert.AreEqual(0, library.BorrowedBooks[user].Count);
            }

            [TestMethod]
            public void ReturnBook_InvalidUser_False()
            {
                
                var library = new LibServiceTest();
                var book = library.AddBook("Test Book", "Test Author", "Test ISBN");
                var user = library.AddUser("Test User", "test@example.com");
                library.BorrowBook(book.Id, user.Id);

                
                bool result = library.ReturnBook(999, 0);

                
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ReturnBook_InvalidBookIndex_False()
            {
                
                var library = new LibServiceTest();
                var book = library.AddBook("Test Book", "Test Author", "Test ISBN");
                var user = library.AddUser("Test User", "test@example.com");
                library.BorrowBook(book.Id, user.Id);
                bool result = library.ReturnBook(user.Id, 5);
                Assert.IsFalse(result);
            }
        }
    }
}
