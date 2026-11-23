namespace PawHaven.Legacy;

public enum AnimalStatus
{
    Available,
    Adopted,
    UnderCare
}

// Parent class (Inheritance + Abstraction)
public abstract class Animal
{
    // Encapsulation: private fields, public properties
    private int _animalId;
    private string _name = "";
    private string _breed = "";
    private int _age;
    private string _gender = "";
    private string _healthInfo = "";
    private AnimalStatus _status = AnimalStatus.Available;

    public int AnimalId
    {
        get { return _animalId; }
        set { _animalId = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Breed
    {
        get { return _breed; }
        set { _breed = value; }
    }

    public int Age
    {
        get { return _age; }
        set { _age = value < 0 ? 0 : value; }
    }

    public string Gender
    {
        get { return _gender; }
        set { _gender = value; }
    }

    public string HealthInfo
    {
        get { return _healthInfo; }
        set { _healthInfo = value; }
    }

    public AnimalStatus Status
    {
        get { return _status; }
        set { _status = value; }
    }

    public abstract string GetAnimalType();

    // Each subclass gives its own details (Polymorphism)
    public abstract string GetDetails();
}

public class Dog : Animal
{
    public string Size { get; set; } = "";

    public override string GetAnimalType()
    {
        return "Dog";
    }

    public override string GetDetails()
    {
        return "Size: " + Size;
    }
}

public class Cat : Animal
{
    public string Color { get; set; } = "";

    public override string GetAnimalType()
    {
        return "Cat";
    }

    public override string GetDetails()
    {
        return "Color: " + Color;
    }
}

public class AnimalManager
{
    private readonly List<Animal> _animals = new List<Animal>();
    private int _nextId = 1;

    public void AddAnimal(Animal animal)
    {
        animal.AnimalId = _nextId++;
        _animals.Add(animal);
    }

    public List<Animal> GetAll()
    {
        return _animals;
    }

    public Animal? FindById(int id)
    {
        foreach (Animal a in _animals)
        {
            if (a.AnimalId == id) return a;
        }
        return null;
    }

    public bool DeleteAnimal(int id)
    {
        Animal? animal = FindById(id);
        if (animal == null) return false;
        _animals.Remove(animal);
        return true;
    }
}
