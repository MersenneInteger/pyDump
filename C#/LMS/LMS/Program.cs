﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    class Program
    {
        static void Main(string[] args)
        {
            string continueOperations = "yes";
            string title;
            string availableBooks;
            ushort menuNavValue = 0;
            bool signedIn = false;
            bool quit = false;
            LibraryDatabase libraryDB = new LibraryDatabase();
            Student user = new Student();

            //load library from JSON file
            libraryDB.LoadLibrary();

            while (continueOperations.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                ClearConsole();

                if (!signedIn)
                {
                    //prompt login or signup
                    Console.WriteLine("\nWelcome to X library. Please choose one of the following:" +
                                    "\n1) Login\n2)Sign Up\n3)Quit");

                    try
                    {
                        menuNavValue = Convert.ToUInt16(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }

                    switch (menuNavValue)
                    {
                        case 1: //login
                            user = libraryDB.Login();
                            if (ValidateLogin(user) == false)
                            {
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Login Successful");
                                System.Threading.Thread.Sleep(100);
                                signedIn = true;
                            }
                            break;

                        case 2: //sign up
                            if (libraryDB.SignUp() == false)
                            {
                                continue;
                            }
                            user = libraryDB.Login();
                            if (ValidateLogin(user) == false)
                            {
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Login Successful");
                                System.Threading.Thread.Sleep(100);
                                signedIn = true;
                            }
                            break;

                        case 3: //quit
                            quit = true;
                            break;

                        default: //input out of range
                            Console.WriteLine("Please enter a valid operation");
                            menuNavValue = 0;
                            continue;
                    }

                    if (quit)
                    {
                        break;
                    }
                }

                ClearConsole();

                //prompt available operations:
                Console.WriteLine("Choose one of the following: " +
                    "\n1)Return book\n2)Check out book\n3)Check if you have overdue books\n4)Show books currently checked out" +
                    "\n5)Show books available to check out\n6)Log out\n7)Quit");
                try
                {
                    menuNavValue = Convert.ToUInt16(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                switch(menuNavValue)
                {
                    case 1: //return book
                        Console.WriteLine("Currently checked out: \n");
                        user.ViewBooksCheckedOut();
                        Console.WriteLine("Enter the title of the name of the book you would like to return");
                        title = Console.ReadLine();
                        Book book = user.GetBookByTitle(title);
                        if (book != null)
                        {
                            user.ReturnBook(book);
                            Console.WriteLine("Book returned. Thank you\n");
                            System.Threading.Thread.Sleep(100);
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("That book is not currently checked out under your name");
                            System.Threading.Thread.Sleep(100);
                            continue;
                        }

                    case 2: //check out book
                        Console.WriteLine("Enter the title of the name of the book you would like to check out");
                        title = Console.ReadLine();
                        Book bookToCheckOut = libraryDB.SearchBook(title);
                        if (bookToCheckOut != null)
                        {
                            user.CheckOutBook(bookToCheckOut);
                            Console.WriteLine("{0} checked out", bookToCheckOut.GetTitle());
                            System.Threading.Thread.Sleep(100);
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Book is currently unavailable");
                            System.Threading.Thread.Sleep(100);
                            continue;
                        }

                    case 3:
                        user.CheckIfStudentHasBooksOverdue();
                        System.Threading.Thread.Sleep(100);
                        continue;

                    case 4: //show books currently checked out
                        user.ViewBooksCheckedOut();
                        System.Threading.Thread.Sleep(100);
                        continue;

                    case 5: //show books available for check out
                        availableBooks = libraryDB.SearchForAvailableBooks();
                        Console.WriteLine(availableBooks);
                        System.Threading.Thread.Sleep(500);
                        continue;

                    case 6: //log out
                        user = null;
                        signedIn = false;
                        continue;

                    case 7: //quit
                        signedIn = false;
                        quit = true;
                        break;

                    default: //input out of range
                        Console.WriteLine("Please enter a valid operation");
                        menuNavValue = 0;
                        continue;
                }

                if (quit)
                {
                    break;
                }

                menuNavValue = 0;
                Console.Write("Continue operations? (yes/no) ");
                continueOperations = Console.ReadLine();
            }
        }

        /// <summary>
        /// Check is Student object is null which indicates an unsuccessful login
        /// </summary>
        /// <param name="user">Student</param>
        /// <returns>Bool</returns>
        static bool ValidateLogin(Student user)
        {
            if (user == null)
            {
                Console.WriteLine("Invalid login or password. Please try again.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sleep for 1 sec and clear console
        /// </summary>
        static void ClearConsole()
        {
            System.Threading.Thread.Sleep(100);
            Console.Clear();
        }
    }
}
