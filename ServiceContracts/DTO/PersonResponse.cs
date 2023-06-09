using ServiceContracts.Enums;
using System;

namespace ServiceContracts.DTO;

/// <summary>
/// Represents DTO class that is used as return type of most methods
/// of Persons Service
/// </summary>
public class PersonResponse
{
    public Guid PersonID { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryID { get; set; }
    public string? Country {get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public double? Age { get; set; }
}
