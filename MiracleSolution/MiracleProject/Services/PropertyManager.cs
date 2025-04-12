/*
 * File: PropertyManager.cs
 * Author: Scrum Master 
 * Purpose: Backend service to manage properties and tenants.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MiracleProject.Models;

namespace MiracleProject.Services
{
    /// <summary>
    /// Manages operations related to Properties and Tenants,
    /// such as adding, assigning, and retrieving records.
    /// </summary>
    public class PropertyManager
    {
        private readonly Dictionary<string, string> _propertyAssignments = new();

        public List<Property> Properties { get; private set; } = new List<Property>();
        public List<Tenant> Tenants { get; private set; } = new List<Tenant>();

        /// <summary>
        /// Adds a new property to the system.
        /// </summary>
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

        /// <summary>
        /// Returns a list of all properties.
        /// </summary>
        public List<Property> ViewProperties() => Properties;

        /// <summary>
        /// Adds a new tenant to the system.
        /// </summary>
        public void AddTenant(Tenant tenant)
        {
            if (tenant == null)
            {
                Console.WriteLine("Invalid tenant. Cannot add null.");
                return;
            }

            if (Tenants.Any(t => t.TenantID == tenant.TenantID))
            {
                Console.WriteLine("A tenant with this ID already exists.");
                return;
            }

            Tenants.Add(tenant);
            Console.WriteLine("Tenant added successfully.");
        }

        /// <summary>
        /// Returns a list of all tenants.
        /// </summary>
        public List<Tenant> ViewTenants() => Tenants;

        /// <summary>
        /// Assigns a tenant to a property if the property is available.
        /// </summary>
        public void AssignTenantToProperty(string tenantId, string propertyId)
        {
            var tenant = Tenants.FirstOrDefault(t => t.TenantID.Equals(tenantId, StringComparison.OrdinalIgnoreCase));
            var property = Properties.FirstOrDefault(p => p.PropertyID.Equals(propertyId, StringComparison.OrdinalIgnoreCase));

            if (tenant == null)
                throw new Exception("Tenant not found.");
            if (property == null)
                throw new Exception("Property not found.");
            if (property.IsOccupied)
                throw new Exception("Property is already occupied.");

            property.IsOccupied = true;
            _propertyAssignments[propertyId] = tenantId;
        }

        /// <summary>
        /// Vacates a property by unassigning the tenant and marking it available.
        /// </summary>
        public void VacateProperty(string propertyId)
        {
            var property = Properties.FirstOrDefault(p => p.PropertyID.Equals(propertyId, StringComparison.OrdinalIgnoreCase));

            if (property == null)
                throw new Exception("Property not found.");
            if (!property.IsOccupied)
                throw new Exception("Property is already vacant.");

            property.IsOccupied = false;
            _propertyAssignments.Remove(propertyId);
        }
    }
}
