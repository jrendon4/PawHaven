namespace PawHaven.Models;

public class Dog : Animal
{
    public string Size { get; set; } = "";

    public Dog() { Species = "Dog"; }

    public override string GetAnimalType() => "Dog";

    public override string DisplayInfo() =>
        $"{Name} - {Breed}, {Age} yr(s), {Gender}, Size: {Size}";
}
