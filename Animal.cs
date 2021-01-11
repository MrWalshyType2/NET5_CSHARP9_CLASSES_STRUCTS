using System;
using System.Collections.Generic;
using System.Text;

namespace ClassesAndStructs
{
    abstract class Animal
    {
        public Animal()
        {
            Breed = "Unknown";
        }

        public Animal(string breed)
        {
            breed = breed;
        }

        // Auto-implemented readonly property
        //  - Creates a private field in the background
        public string Breed { get; }

        // Override base class 'System.Object.ToString'
        public override string ToString()
        {
            return Breed;
        }
    }
}
