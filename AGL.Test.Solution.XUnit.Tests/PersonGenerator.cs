﻿using Bogus;
using System;

namespace AGL.Test.Solution.XUnit.Tests
{
    public partial class PetRepositoryTests
    {
        public class PersonGenerator
        {
            public static Faker<Domain.Person> Generate(int minPets, int maxPets)
            {
                var petType = new[] { "Dog", "Cat" };
                var gender = new[] { "Male", "Female" };
                var r = new Random();

                var pet = new Faker<Domain.Pet>()
                          .RuleFor(p => p.Name, (f, p) => f.Person.FirstName)
                          .RuleFor(p => p.Type, f => f.PickRandom(petType));

                return new Faker<Domain.Person>()
                       .RuleFor(p => p.Age, f => f.Person.DateOfBirth.Year)
                       .RuleFor(p => p.Gender, f => f.PickRandom(gender))
                       .RuleFor(p => p.Name, f => $"{f.Person.FirstName} {f.Person.LastName}")
                       .RuleFor(p => p.Pets, (f, p) => pet.Generate(r.Next(minPets, maxPets)).ToArray());                                    
            }
        }
    }
}