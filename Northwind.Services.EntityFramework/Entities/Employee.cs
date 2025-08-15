// Employee.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Northwind.Services.Repositories;

namespace Northwind.Services.EntityFramework.Entities;
public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    public string? Title { get; set; }
    public string? TitleOfCourtesy { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? HireDate { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? HomePhone { get; set; }
    public string? Extension { get; set; }
    public byte[]? Photo { get; set; }
    public string? Notes { get; set; }
    public int? ReportsTo { get; set; }
    public string? PhotoPath { get; set; }

    // Navigation to manager (self‑reference)
    [ForeignKey(nameof(ReportsTo))]
    public virtual Employee? Manager { get; set; }

    public virtual ICollection<Employee> Reports { get; set; } = new HashSet<Employee>();
    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}
