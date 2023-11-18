class new_User : IUser
{
    private readonly string user_Username;
    public new_User(string user_Username) => this.user_Username = user_Username;
    private string user_Password, user_Email;
    public virtual string property_Username { get => user_Username; set => throw new NotImplementedException();}
    public virtual string property_Password { get => user_Password; set => user_Password = value;}
    public virtual string property_Email { get => user_Email; set => user_Email = value;}
    public virtual void user_Prompt() => Console.WriteLine("Account created succesfully," + " " + this.user_Username + "!");
}

