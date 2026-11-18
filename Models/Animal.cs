using System;

namespace PawHaven.Models
{
    public abstract class Animal
    {
        // Private fields (Encapsulation)
        private string animalId;
        private string name;
        private string species;
        private string breed;
        private int age;
        private string gender;
        private string status;

        // Public Properties (Getters and Setters)
        public string AnimalId
        {
            get { return animalId; }
            set { animalId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Species
        {
            get { return species; }
            set { species = value; }
        }

        public string Breed
        {
            get { return breed; }
            set { breed = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        // Constructor
        public Animal(string animalId, string name, string species, string breed, int age, string gender)
        {
            this.animalId = animalId;
            this.name = name;
            this.species = species;
            this.breed = breed;
            this.age = age;
            this.gender = gender;
            this.status = "Available"; // Default shelter status
        }

        // Virtual method to be overridden (Polymorphism)
        public virtual void DisplayInfo()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Animal ID : {AnimalId}");
            Console.WriteLine($"Name      : {Name}");
            Console.WriteLine($"Species   : {Species}");
            Console.WriteLine($"Breed     : {Breed}");
            Console.WriteLine($"Age       : {Age} year(s)");
            Console.WriteLine($"Gender    : {Gender}");
            Console.WriteLine($"Status    : {Status}");
        }

        // Abstract method (Abstraction)
        public abstract string GetAnimalType();
    }
}