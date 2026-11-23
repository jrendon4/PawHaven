using System.Text.Json;
using PawHaven.Models;

namespace PawHaven.Services;

// Stores animals in memory and saves them to a JSON file.
public class AnimalRepository
{
    private readonly string _filePath;
    private List<Animal> _animals = new();

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public AnimalRepository(string filePath)
    {
        _filePath = filePath;
        Load();
    }

    public IReadOnlyList<Animal> GetAll() => _animals;

    public Animal? GetById(string id) =>
        _animals.FirstOrDefault(a => a.AnimalId == id);

    public void Add(Animal animal)
    {
        animal.AnimalId = NextId();
        _animals.Add(animal);
        Save();
    }

    public void Update(Animal animal)
    {
        int index = _animals.FindIndex(a => a.AnimalId == animal.AnimalId);
        if (index < 0) return;
        _animals[index] = animal;   // replaced, so changing Dog <-> Cat also works
        Save();
    }

    public void Delete(string id)
    {
        _animals.RemoveAll(a => a.AnimalId == id);
        Save();
    }

    // Matches name, species, breed or status text; optional status filter.
    public List<Animal> Search(string keyword, AnimalStatus? status = null)
    {
        keyword = (keyword ?? "").Trim();
        return _animals
            .Where(a => status == null || a.Status == status)
            .Where(a => keyword.Length == 0
                || a.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || a.Species.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || a.Breed.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || a.Status.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private string NextId()
    {
        int max = _animals
            .Select(a => int.TryParse(a.AnimalId.TrimStart('A'), out int n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();
        return $"A{max + 1:000}";
    }

    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        string json = File.ReadAllText(_filePath);
        _animals = JsonSerializer.Deserialize<List<Animal>>(json) ?? new();
    }

    private void Save()
    {
        string? dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_animals, JsonOptions));
    }
}
