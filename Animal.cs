using System.Text.Json.Serialization;

namespace PawHaven.Models;

// These two attributes let System.Text.Json save and reload Dog/Cat
// correctly even though the list is typed as Animal.
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Dog), "dog")]
[JsonDerivedType(typeof(Cat), "cat")]
public abstract class Animal
{
    // Encapsulation: private fields, public properties that validate input.
    private string _name = "";
    private string _breed = "";
    private int _age;

    public string AnimalId { get; set; } = "";

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name is required.");
            _name = value.Trim();
        }
    }

    public string Species { get; protected set; } = "";

    public string Breed
    {
        get => _breed;
        set => _breed = value?.Trim() ?? "";
    }

    public int Age
    {
        get => _age;
        set
        {
            if (value < 0)
                throw new ArgumentException("Age cannot be negative.");
            _age = value;
        }
    }

    public string Gender { get; set; } = "Unknown";
    public AnimalStatus Status { get; set; } = AnimalStatus.Available;
    public string HealthNotes { get; set; } = "";
    public DateTime DateAdmitted { get; set; } = DateTime.Now;

    public bool IsAvailable() => Status == AnimalStatus.Available;

    public void UpdateStatus(AnimalStatus newStatus) => Status = newStatus;

    // Polymorphism: each subclass supplies its own version.
    public abstract string GetAnimalType();
    public abstract string DisplayInfo();
}
