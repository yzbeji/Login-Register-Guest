using System;
using System.Net;
using System.Net.Mail;
using System.Diagnostics.Metrics;
using System.Reflection;
using BCrypt;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
public class Login {
    static string connectionString;
    static SqlConnection connection;
    public void VerificationCode(string code, string email) {
        string stringMail = "YOUR-EMAIL"; // Please put your email here
        string stringPassowrd = "YOUR-APP-PASSWORD"; // Please put your email password here so the connection to the portal is made
        MailMessage message = new MailMessage();
        message.From = new MailAddress(stringMail);
        message.Subject = "Verification code";
        message.To.Add(new MailAddress(email));
        message.Body = code;
        var smtpClient = new SmtpClient("smtp.gmail.com") {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(stringMail, stringPassowrd),
        };
        smtpClient.Send(message);
    }
    public async Task EstablishConnection() {
        connectionString = @"data source=localhost;initial catalog=master;user id=sa;password=<<YourPassword>>;TrustServerCertificate=True;";
        connection = new SqlConnection(connectionString);
        await Task.Run(() => {
            try {
                connection.Open();
            }
            catch {
                System.Console.WriteLine("\n" + "Oops! Something went wrong with the connection:(. Please try again later.");
                System.Environment.Exit(0);
            }

        });
    }
    static void Main(string[] args) {
        Login base_ = new Login();
        base_.EstablishConnection();
        Console.WriteLine("Welcome to beji.dev login form.");
        Console.WriteLine("Write the number you want after the options appear.");
        Console.WriteLine("What option do you choose?");
        Console.WriteLine("1.Login" + "\n" + "2.Register" + "\n" + "3.Guest");
        Console.Write("Your option is:");
        int number_input = 0;
        int error_counter = 3;
        while(error_counter != 0) {
            try {
                number_input = Convert.ToInt32(Console.ReadLine());
                break;
            }
            catch {
                Console.WriteLine("Sorry, I do not recognize this number, please try again!");
                if(error_counter > 1)
                    Console.Write("Your option is:");
                error_counter --;
            }
        }
        if(error_counter == 0) {
            Console.WriteLine("The app will now close. Bye!");
            System.Environment.Exit(0);
        }
        switch(number_input) {
            case 1:
                existing_User client = new existing_User();
                Console.WriteLine("Please tell me your username.");
                string existing_username = Console.ReadLine();
                client.property_Username = existing_username;
                Console.WriteLine("Now you're password please!");
                string existing_password = Console.ReadLine();
                client.property_Password = existing_password;
                string selectCommand = "SELECT * from dbo.[database] WHERE username = @username";
                SqlCommand cmd = new SqlCommand(selectCommand, connection);
                cmd.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@username"].Value = client.property_Username;
                try {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read()) {
                        string database_password = Convert.ToString(reader[1]);
                        if(BCrypt.Net.BCrypt.EnhancedVerify(client.property_Password, database_password) == true)
                            client.user_Prompt();
                        else {
                            Console.WriteLine("Sorry, it appears that the password is incorrect. We cannot continue the process.");
                            System.Environment.Exit(0);
                        }            
                    }
                }
                catch {
                    Console.WriteLine("Error");
                }
                break;
            case 2: 
                Console.WriteLine("It's our pleasure joining us! Please tell us your name:");
                string client_username = Console.ReadLine();
                Console.WriteLine("Great! Now your password.");
                string client_password = Console.ReadLine();
                Console.WriteLine("Last step, your email!");
                string client_email = Console.ReadLine();
                Console.WriteLine("Thanks for joining us!");
                new_User user_New = new new_User(client_username);
                Console.WriteLine("Almost there! We'll send you a 4-digit number to check that you are human!");
                Random R = new Random();
                string digit_code = Convert.ToString(R.Next(1000, 9999));
                Console.WriteLine(digit_code);
                int erorr_digit = 3;
                bool flag = false;
                while(erorr_digit != 0) {
                    try {
                            base_.VerificationCode(digit_code, client_email);
                            Console.WriteLine("The digit code was sent! Please write it down.");
                            string input_digit_code = Console.ReadLine();
                            if(input_digit_code == digit_code) {
                                flag = true;
                                break;
                            } 
                            else {
                                Console.WriteLine("Code Incorrect. Try again!");
                                erorr_digit --;
                            }                         
                    }
                    catch {
                        Console.WriteLine("It appears that your email is incorrect, the app will now close.");
                        System.Environment.Exit(0);
                        break;
                    }
                }
                if(flag == false) {
                    Console.WriteLine("The app will not shutdown.");
                    System.Environment.Exit(0);
                }
                else {
                    string stringCommand = "INSERT INTO dbo.[database] VALUES (@username, @password, @email)";
                    SqlCommand command = new SqlCommand(stringCommand, connection);
                    new_User new__User = new new_User(client_username);
                    new__User.property_Password = Convert.ToString(BCrypt.Net.BCrypt.EnhancedHashPassword(client_password));
                    new__User.property_Email = Convert.ToString(BCrypt.Net.BCrypt.EnhancedHashPassword(client_email));
                    command.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
                    command.Parameters["@username"].Value =  new__User.property_Username;
                    command.Parameters.Add("@password", System.Data.SqlDbType.VarChar);
                    command.Parameters["@password"].Value =  new__User.property_Password;
                    command.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
                    command.Parameters["@email"].Value = new__User.property_Email;
                    try {
                        command.ExecuteNonQuery();
                        new__User.user_Prompt();
                    }
                    catch {
                        Console.WriteLine("Sorry, this name already exists. The app will now close and try again.");
                        System.Environment.Exit(0);
                    }
                    System.Environment.Exit(0);
                }
                break;
            case 3:
                Console.WriteLine("Great! Tell us how we would like to call you:");
                string guest_Username = Console.ReadLine();
                guest_User user_Guest = new guest_User(guest_Username);
                Console.WriteLine("Welcome," + " " + user_Guest.property_Username + "!");
                break;
            default:
                Console.WriteLine("Sorry, we do not have current option! The console will now close.");
                System.Environment.Exit(0);
                break;
        }
        connection.Close();
    }
}
