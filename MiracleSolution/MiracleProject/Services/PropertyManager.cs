/*
 * File: PropertyManager.cs
 * Author: Developer 1 , Scrum Master 
 * Purpose: Backend service to manage properties and tenants.
 * Last Updated: 10/04/2025
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MiracleProject.Models;

namespace MiracleProject.Services
{
    /// <summary>
    /// Manages operations related to Properties and Tenants,
    /// such as adding and tracking property records.
    /// </summary>
    public class PropertyManager
    {
        /// <summary>
        /// List of all properties in the system.
        /// </summary>
        public List<Property> Properties { get; private set; } = new List<Property>();

        /// <summary>
        /// List of all tenants in the system.
        /// </summary>
        public List<Tenant> Tenants { get; private set; } = new List<Tenant>();

        /// <summary>
        /// Adds a new property to the system.
        /// </summary>
        /// <param name="property">Property object to add</param>
        public void AddProperty(Property property)
        {
            if (property == null)
            {
                Console.WriteLine("Invalid property. Cannot add null.");
                return;
            }

            if (Properties.Any(p => p.PropertyID == property.PropertyID))
            {
                Console.WriteLine("A property with this ID already exists.");
                return;
            }

            Properties.Add(property);
            Console.WriteLine("Property added successfully.");
        }


    }
}
