using System;

namespace PawHaven.Models
{
    public class Dog : Animal
    {
        private string size;

        public string Size
        {
            get { return size; }
            set { size = value; }
        }

        public Dog(string animalId, string name, string breed, int age, string gender, string size)
            : base(animalId, name, "Dog", breed, age, gender)
        {
            this.size = size;
        }

        public override string GetAnimalType()
        {
            return "Dog";
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Size      : {Size}");
        }
    }
}