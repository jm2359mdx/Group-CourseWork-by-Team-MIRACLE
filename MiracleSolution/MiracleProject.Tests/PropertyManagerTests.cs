/*
 * Author: Tester, Scrum Master 
 * Purpose: Unit tests for the PropertyManager class.
 * Last Updated: 10/04/2025
 * Notes: Designed for use in a C# console-based property management app.
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiracleProject.Models;
using MiracleProject.Services;

namespace MiracleProject.Tests
{
    [TestClass]
    public class PropertyManagerTests
    {
        [TestMethod]
        public void AddProperty_ShouldAddPropertyToList()
        {
            var manager = new PropertyManager();
            var property = new Property
            {
                PropertyID = 1,
                Address = "123 Main Street",
                Rent = 1500
            };

            manager.AddProperty(property);

            Assert.AreEqual(1, manager.Properties.Count);
            Assert.AreEqual("123 Main Street", manager.Properties[0].Address);
        }


        [TestMethod]
        public void AddProperty_ShouldNotAddDuplicatePropertyID()
        {
            var manager = new PropertyManager();
            var property1 = new Property { PropertyID = 1, Address = "1 First St", Rent = 1000 };
            var property2 = new Property { PropertyID = 1, Address = "2 Second St", Rent = 1200 };

            manager.AddProperty(property1);
            manager.AddProperty(property2); // Duplicate ID

            Assert.AreEqual(1, manager.Properties.Count);
        }


        [TestMethod]
        public void AddProperty_ShouldHandleNullProperty()
        {
            var manager = new PropertyManager();

            manager.AddProperty(null);

            Assert.AreEqual(0, manager.Properties.Count);
        }

        [TestMethod]
        public void AddProperty_ShouldHandleMultipleValidProperties()
        {
            var manager = new PropertyManager();

            manager.AddProperty(new Property { PropertyID = 1, Address = "A", Rent = 1000 });
            manager.AddProperty(new Property { PropertyID = 2, Address = "B", Rent = 1100 });

            Assert.AreEqual(2, manager.Properties.Count);
        }

    }
}
