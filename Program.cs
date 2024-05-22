using System;

namespace niceone
{
    internal class Program
    {
        private static void Main()
        {
            using var context = new BloggingContext();
            var service = new BloggingService(context);

            // Ensure database is created
            service.EnsureDatabaseCreated();

            // Check if there are any users in the database
            if (!service.UsersExist())
            {
                // No users exist, prompt to create a new user
                Console.WriteLine("No users found. Please create a new user.");
                service.CreateUser();
            }
            else
            {
                // Users exist, prompt for credentials
                Console.Write("Enter username: ");
                string? username = Console.ReadLine();
                Console.Write("Enter password: ");
                string? password = Console.ReadLine();

                // Authenticate user
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && service.AuthenticateUser(username, password))
                {
                    Console.WriteLine("Authentication successful.");
                    
                    Console.WriteLine("Do you want to delete a blog? (yes/no)");
                    string? deleteResponse = Console.ReadLine()?.ToLower();

                    if (deleteResponse == "yes" || deleteResponse == "y")
                    {
                        service.DeleteBlog();
                    }

                    Console.WriteLine("Do you want to add a new blog? (yes/no)");
                    string? addResponse = Console.ReadLine()?.ToLower();

                    if (addResponse == "yes" || addResponse == "y")
                    {
                        service.AddNewData();
                    }

                    // Clear console and display data
                    Console.Clear();
                    // Task.Delay(8000);
                    service.DisplayData();
                }
                else
                {
                    Console.WriteLine("Authentication failed. Access denied.");
                }
            }
        }
    }
}

// username= ad 
// password=passusing System;