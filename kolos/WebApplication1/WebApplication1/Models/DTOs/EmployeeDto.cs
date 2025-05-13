namespace WebApplication1.Models.DTOs;

public class EmployeeDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string EmployeeNumber { get; set; }
}