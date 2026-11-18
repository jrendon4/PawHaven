using System;
using System.Collections.Generic;
using PawHaven.Models;

namespace PawHaven
{
    class Program
    {
        static List<Animal> animals = new List<Animal>();
        static int nextAnimalId = 1;

        static void Main(string[] args)
        {
            string choice;
            do
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("              PAWHAVEN");
                Console.WriteLine("      Animal Shelter Management System");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Add Animal");
                Console.WriteLine("2. View Animals");
                Console.WriteLine("0. Exit");
                Console.WriteLine("========================================");
                Console.Write("Select an option: ");

                choice = Console.ReadLine() ?? "";
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        AddAnimal();
                        break;
                    case "2":
                        ViewAnimals();
                        break;
                    case "0":
                        Console.WriteLine("Thank you for using PawHaven!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (choice != "0")
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }

            } while (choice != "0");
        }

        static void AddAnimal()
        {
            Console.WriteLine("========== ADD ANIMAL ==========");
            Console.Write("Animal Type (Dog/Cat): ");
            string type = (Console.ReadLine() ?? "").Trim().ToLower();

            if (type != "dog" && type != "cat")
            {
                Console.WriteLine("Invalid animal type. Only 'Dog' and 'Cat' are supported for now.");
                return;
            }

            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "Unknown";

            Console.Write("Breed: ");
            string breed = Console.ReadLine() ?? "Unknown";

            int age;
            Console.Write("Age (years): ");
            while (!int.TryParse(Console.ReadLine(), out age) || age < 0)
            {
                Console.Write("Please enter a valid number for age: ");
            }

            Console.Write("Gender: ");
            string gender = Console.ReadLine() ?? "Unknown";

            if (type == "dog")
            {
                Console.Write("Size (Small/Medium/Large): ");
                string size = Console.ReadLine() ?? "Medium";
                Dog dog = new Dog(nextAnimalId.ToString(), name, breed, age, gender, size);
                animals.Add(dog);
            }
            else if (type == "cat")
            {
                Console.Write("Color: ");
                string color = Console.ReadLine() ?? "Unknown";
                Cat cat = new Cat(nextAnimalId.ToString(), name, breed, age, gender, color);
                animals.Add(cat);
            }

            Console.WriteLine($"\nAnimal successfully added with ID: {nextAnimalId}!");
            nextAnimalId++;
        }

        static void ViewAnimals()
        {
            Console.WriteLine("========== ANIMAL RECORDS ==========");
            if (animals.Count == 0)
            {
                Console.WriteLine("No animal records found.");
                return;
            }

            foreach (Animal animal in animals)
            {
                animal.DisplayInfo();
            }
            Console.WriteLine("----------------------------------------");
        }
    }
}
