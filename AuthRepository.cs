using System.Text.Json;
using PawHaven.Models;

namespace PawHaven.Services;

public class AuthRepository
{
    private readonly string _filePath;
    private List<User> _users = new();

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public AuthRepository(string filePath)
    {
        _filePath = filePath;
        Load();
        SeedAdmin();
    }

    private void SeedAdmin()
    {
        if (_users.Any(u => u.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            return;

        _users.Add(new User
        {
            UserId = "ADM001",
            LastName = "Admin",
            FirstName = "PawHaven",
            Birthday = "01/01/2000",
            Gender = "Other",
            Email = "admin@pawhaven.com",
            Password = "Admin123",
            Role = "Admin"
        });
        Save();
    }

    public bool RegisterAdopter(User newUser, out string error)
    {
        error = "";
        if (_users.Any(u => u.Email.Equals(newUser.Email.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            error = "That email address is already registered.";
            return false;
        }

        int max = _users
            .Where(u => u.UserId.StartsWith("USR"))
            .Select(u => int.TryParse(u.UserId.Substring(3), out int n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();

        newUser.UserId = $"USR{max + 1:D3}";
        newUser.Email = newUser.Email.Trim();
        newUser.Role = "Adopter";

        _users.Add(newUser);
        Save();
        return true;
    }

    public User? Login(string email, string password, string expectedRole, out string error)
    {
        error = "";
        var user = _users.FirstOrDefault(u =>
            u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase) &&
            u.Password == password);

        if (user == null)
        {
            error = "Invalid email address or password.";
            return null;
        }

        if (!user.Role.Equals(expectedRole, StringComparison.OrdinalIgnoreCase))
        {
            error = $"Access denied. This account is registered as {user.Role}, not {expectedRole}.";
            return null;
        }

        return user;
    }

    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        string json = File.ReadAllText(_filePath);
        _users = JsonSerializer.Deserialize<List<User>>(json) ?? new();
    }

    private void Save()
    {
        string? dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_users, JsonOptions));
    }
}