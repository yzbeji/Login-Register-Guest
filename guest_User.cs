using Microsoft.Identity.Client;

class guest_User : new_User
{   
    private readonly string guest_Username;
    public guest_User(string input_username) : base(input_username) {
        guest_Username = input_username;
    }
    public override string property_Username { get => guest_Username; set => throw new NotImplementedException();}
    public override string property_Password { get => throw new NotImplementedException(); set => throw new NotImplementedException();}
    public override string property_Email { get => throw new NotImplementedException(); set => throw new NotImplementedException();}
    public override void user_Prompt() {
        Console.WriteLine("Welcome guest," + " " + this.guest_Username);
    }
}
