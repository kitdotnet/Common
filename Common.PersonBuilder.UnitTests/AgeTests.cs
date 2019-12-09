﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Common.PersonBuilder.UnitTests
{
    public partial class PersonBuilderTests
    {
        [Fact]
        public void Implicit_Set()
        {
            Age age = 15;

            Assert.Equal<int>(15, age);
        }

        [Fact]
        public void Explicit_Fetch()
        {
            Age age = 24;
            int other = age;
            Assert.Equal(24, other);
        }

        [Fact]
        public void Age_AgeLeast_AtMost()
        {
            Age age = new Age().AtLeast(20).AtMost(20);

            Assert.Equal<int>(20, age);

            for (int i = 0; i < 100; i++)
            {
                age = new Age().AtLeast(10).AtMost(100);
                Assert.True(age >= 10 && age <= 100);
            }
        }

        [Fact]
        public void Age_Between()
        {
            for (int i = 0; i < 100; i++)
            {
                Age age = new Age().Between(56, 110);
                Assert.True(age >= 56 && age <= 110);
            }
        }

        [Fact]
        public void PersonWithAge()
        {
            var person = new PersonBuilder()
                .WithName(Gender.Male)
                .WithAge(24)
                .Build();
        }

        [Fact]
        public void SettingAge_SetsDateOfBirth()
        {
            var person = new PersonBuilder()
                .WithName(Gender.Male)
                .WithAge(24)
                .Build();
            Assert.NotNull(person.DateOfBirth);
            Assert.Equal<int>(person.Age, Maths.Calculate.AgeInYears(person.DateOfBirth.Value, DateTime.Now));
        }

        [Fact]
        public void SettingDateOfBirth_SetsAge()
        {
            DateTime dateOfBirth = new DateTime(1980, 5, 19);
            var person = new PersonBuilder()
                .WithName(Gender.Male)
                .WithDateOfBirth(dateOfBirth)
                .Build();
            Assert.NotNull(person.DateOfBirth);
            Assert.Equal<int>(person.Age, Maths.Calculate.AgeInYears(person.DateOfBirth.Value, DateTime.Now));
        }
    }
}
