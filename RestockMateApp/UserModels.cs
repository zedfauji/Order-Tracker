public class CreateUserDto
{
    public string Username { get; set; }
    public string Passcode { get; set; }
    public string Role { get; set; }
}

public class UserDto
{
    public string username { get; set; }
    public string role { get; set; }
}