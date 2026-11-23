namespace PawHaven.Models;

public class User
{
    public string UserId { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string Birthday { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "Adopter"; // "Admin" or "Adopter"
}

public static class UserSession
{
    public static User? CurrentUser { get; set; }
    public static string SelectedRole { get; set; } = "Adopter";

    public static void Clear()
    {
        CurrentUser = null;
        SelectedRole = "Adopter";
    }
}