using System;

namespace PawHaven.Models
{
    public class Cat : Animal
    {
        private string color;

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public Cat(string animalId, string name, string breed, int age, string gender, string color)
            : base(animalId, name, "Cat", breed, age, gender)
        {
            this.color = color;
        }

        public override string GetAnimalType()
        {
            return "Cat";
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Color     : {Color}");
        }
    }
}