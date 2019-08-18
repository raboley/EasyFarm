using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleLogic.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // A.
            // Possible user input:
            string value = "Dog";

            // B.
            // Try to convert the string to an enum:
            PetType pet = (PetType)Enum.Parse(typeof(PetType), value);

            // C.
            // See if the conversion succeeded:
            Assert.AreEqual(PetType.Dog, pet);
        }
    }

    public enum PetType
    {
        None,
        Cat = 1,
        Dog = 2
    }
}
