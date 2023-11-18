class existing_User : IUser
{
    private string user_Password, user_Email, user_Username;
    public string property_Username { get => user_Username; set => user_Username = value;}
    public string property_Password { get => user_Password; set => user_Password = value;}
    public string property_Email { get => user_Email; set => user_Email = value;}
    public void user_Prompt() => Console.WriteLine("Welcome back," + " " + this.user_Username + "!");
}





