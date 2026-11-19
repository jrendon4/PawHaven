namespace PawHaven.Models;

public class Cat : Animal
{
    public string Color { get; set; } = "";

    public Cat() { Species = "Cat"; }

    public override string GetAnimalType() => "Cat";

    public override string DisplayInfo() =>
        $"{Name} - {Breed}, {Age} yr(s), {Gender}, Color: {Color}";
}
