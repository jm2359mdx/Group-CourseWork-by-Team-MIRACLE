# 🏠 SmartRent – A Simple Property Management System

## 🎯 Purpose of the App

SmartRent is a **console-based property management system** designed for property agencies or landlords to manage rental properties. The system allows users to:

- Add and manage property listings  
- Search properties by location  
- View available properties  
- Track rental status and basic reports  
- Manage user login (admin/agent)
- Use SQL database to store and retrieve property data

## 👤 Target Users

- Landlords or agents managing multiple rental properties  
- Tenants (for future expansion)

## 💻 Technology Stack

| Component         | Tech Used             |
|------------------|------------------------|
| Programming Lang | C# (.NET Console App) |
| Data Storage     | SQL Database           |
| Platform         | Windows Console Application |
| Testing          | Automated testing using MSTest |
| Documentation    | Markdown (.md) in GitHub |

## 🧱 System Modules

### 1. 🏠 Property Module
- Add new properties
- View all properties
- Search by location

### 2. 📋 PropertyManager Module
- Connects with the database
- Includes methods:
  - `AddProperty()`
  - `ViewAllProperties()`
  - `SearchByLocation()`

### 3. 🔐 User Login Module
- Login system with credentials
- Different roles: Admin / Agent
- Simple credential validation using SQL

### 4. 🧑‍💻 Console UI (Program.cs)
- Text-based menu:
  ```
  1. Login
  2. Add Property
  3. View All Properties
  4. Search by Location
  5. Exit
  ```

### 5. ✅ Testing Module
- MSTest project created in `/test/`
- Automated unit tests for all core methods
- Output stored as test results

### 6. 📑 Documentation
- `/docs/MeetingMinutes.md` for daily logs  
- `/docs/Project_Summary.md`  
- `/docs/TeamLead_Notes.md` for task management  

## 📁 Project Folder Structure

```
SmartRent/
├── src/               # Source code (C# files)
├── docs/              # Meeting notes, report, summary
├── test/              # Test plans, test results
├── sql/               # SQL scripts
├── video/             # Demo recording for submission
└── README.md          # Project overview
```
