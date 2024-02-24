
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services.Helpers;
using ServiceContracts.Enums;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using OfficeOpenXml;
using CsvHelper.Configuration;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Services;
public class PersonsService : IPersonsService
{

    //private field
    private readonly IPersonsRepository _personsRepository;
    private readonly ILogger<PersonsService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    //constructor
    public PersonsService(IPersonsRepository personsRepository, ILogger<PersonsService> logger, IDiagnosticContext diagnosticContext)
    {
        _personsRepository = personsRepository;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public PersonsService()
    {
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.Country = person.Country?.CountryName;
        return personResponse;
    }

    public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
    {
        //check if PersonAddRequest is not null
        if (personAddRequest == null)
        {
            throw new ArgumentNullException(nameof(personAddRequest));
        }

        //Model Validation
        ValidationHelper.ModelValidation(personAddRequest);

        //convert personAddRequest into Person type
        Person person = personAddRequest.ToPerson();

        //generate PersonID
        person.PersonID = Guid.NewGuid();

        //add person object to persons list
        await _personsRepository.AddPerson(person);
        //_db.sp_InsertPerson(person);

        //convert the Person object into PersonResponse type
        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetAllPersons()
    {
        _logger.LogInformation("GetAllPersons of PersonsService");
        var persons = await _personsRepository.GetAllPersons();

        return persons.Select(temp => temp.ToPersonResponse()).ToList();
    }

    public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
    {
        if (personID == null)
            return null;

        Person? person = await _personsRepository.GetPersonByPersonId(personID.Value);
        
        if(person == null)
            return null;

        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
    {
        _logger.LogInformation("GetFilteredPersons of PersonsService");

        List<Person> persons = searchBy switch
        {
            nameof(PersonResponse.PersonName) =>
                await _personsRepository.GetFilteredPersons(temp =>
                temp.PersonName.Contains(searchString)),

            nameof(PersonResponse.Email) =>
                await _personsRepository.GetFilteredPersons(temp =>
                temp.Email.Contains(searchString)),

            nameof(PersonResponse.DateOfBirth) =>
                await _personsRepository.GetFilteredPersons(temp =>
                temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),

            nameof(PersonResponse.Gender) =>
                await _personsRepository.GetFilteredPersons(temp =>
                temp.Gender.Contains(searchString)),

            nameof(PersonResponse.CountryID) =>
                await _personsRepository.GetFilteredPersons(temp =>
                temp.Country.CountryName.Contains(searchString)),

            nameof(PersonResponse.Address) =>
                await _personsRepository.GetFilteredPersons(temp =>
                temp.Address.Contains(searchString)),

            _ => await _personsRepository.GetAllPersons()
        };

        _diagnosticContext.Set("Persons", persons);

        return persons.Select(temp => temp.ToPersonResponse()).ToList();
    }

    public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
        _logger.LogInformation("GetSortedPersons of PersonsService");

        if (string.IsNullOrEmpty(sortBy))
            return allPersons;

        List<PersonResponse> sortedPersons = (sortBy, sortOrder)
        switch
        {
            (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
            => allPersons.OrderBy(temp => temp.PersonName,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
            => allPersons.OrderByDescending(temp => temp.PersonName,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Email), SortOrderOptions.ASC)
            => allPersons.OrderBy(temp => temp.Email,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Email), SortOrderOptions.DESC)
            => allPersons.OrderByDescending(temp => temp.Email,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
            => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
            => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

            (nameof(PersonResponse.Age), SortOrderOptions.ASC)
            => allPersons.OrderBy(temp => temp.Age).ToList(),

            (nameof(PersonResponse.Age), SortOrderOptions.DESC)
            => allPersons.OrderByDescending(temp => temp.Age).ToList(),

            (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
            => allPersons.OrderBy(temp => temp.Gender,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
            => allPersons.OrderByDescending(temp => temp.Gender,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Country), SortOrderOptions.ASC)
            => allPersons.OrderBy(temp => temp.Country,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Country), SortOrderOptions.DESC)
            => allPersons.OrderByDescending(temp => temp.Country,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Address), SortOrderOptions.ASC)
            => allPersons.OrderBy(temp => temp.Address,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Address), SortOrderOptions.DESC)
            => allPersons.OrderByDescending(temp => temp.Address,
            StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
            => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
            => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

            _ => allPersons

        };
        return sortedPersons;
    }

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(Person));

        //validation
        ValidationHelper.ModelValidation(personUpdateRequest);

        //get matching person object to update
        Person? matchingPerson = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonID);
        if (matchingPerson == null)
        {
            throw new ArgumentException("Given person id doesn't exist");
        }

        //update all details
        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryID = personUpdateRequest.CountryID;
        matchingPerson.Address = personUpdateRequest.Address;
        matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        await _personsRepository.UpdatePerson(matchingPerson); //UPDATE

        return matchingPerson.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? personID)
    {
        if (personID == null)
        {
            throw new ArgumentNullException(nameof(personID));
        }

        Person? person = await _personsRepository.GetPersonByPersonId(personID.Value);

        if (person == null)
            return false;

        await _personsRepository.DeletePersonByPersonID(personID.Value);

        return true;
    }

    public async Task<MemoryStream> GetPersonsCSV()
    {
        MemoryStream memoryStream = new MemoryStream();
        StreamWriter sWriter = new StreamWriter(memoryStream);
        
        CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        CsvWriter csvWriter = new CsvWriter(sWriter, csvConfiguration);

        //PersonName, Email, DateOfBirth, Age, Gender, Country, Address, ReceiveNewsLetters
        csvWriter.WriteField(nameof(PersonResponse.PersonName));
        csvWriter.WriteField(nameof(PersonResponse.Email));
        csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
        csvWriter.WriteField(nameof(PersonResponse.Age));
        csvWriter.WriteField(nameof(PersonResponse.Gender));
        csvWriter.WriteField(nameof(PersonResponse.Country));
        csvWriter.WriteField(nameof(PersonResponse.Address));
        csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
        csvWriter.NextRecord();

        List<PersonResponse> persons = await GetAllPersons();

        foreach (PersonResponse person in persons) 
        {
            csvWriter.WriteField(person.PersonName);
            csvWriter.WriteField(person.Email);
            if (person.DateOfBirth.HasValue)
                csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
            else
                csvWriter.WriteField("");
            csvWriter.WriteField(person.Age);
            csvWriter.WriteField(person.Gender);
            csvWriter.WriteField(person.Country);
            csvWriter.WriteField(person.Address);
            csvWriter.WriteField(person.ReceiveNewsLetters);
            csvWriter.NextRecord();
            csvWriter.Flush();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<MemoryStream> GetPersonsExcel()
    {
        MemoryStream memoryStream = new MemoryStream();
        using (ExcelPackage excelPage = new ExcelPackage(memoryStream))
        {
            ExcelWorksheet excelWorksheet = excelPage.Workbook.Worksheets.
                Add("PersonsSheet");
            excelWorksheet.Cells["A1"].Value = "Person Name";
            excelWorksheet.Cells["B1"].Value = "Email";
            excelWorksheet.Cells["C1"].Value = "Date of Birth";
            excelWorksheet.Cells["D1"].Value = "Age";
            excelWorksheet.Cells["E1"].Value = "Gender";
            excelWorksheet.Cells["F1"].Value = "Country";
            excelWorksheet.Cells["G1"].Value = "Address";
            excelWorksheet.Cells["H1"].Value = "Receive News Letters";

            using (ExcelRange headerCells = excelWorksheet.Cells["A1:H1"] )
            {
                headerCells.Style.Fill.PatternType =
                    OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(
                    System.Drawing.Color.LightGray);
                headerCells.Style.Font.Bold = true;
            }

            int row = 2;
            List<PersonResponse> persons = await GetAllPersons();
            foreach (PersonResponse person in persons)
            {
                excelWorksheet.Cells[row, 1].Value = person.PersonName;
                excelWorksheet.Cells[row, 2].Value = person.Email;
                if (person.DateOfBirth.HasValue)
                    excelWorksheet.Cells[row, 3].Value = 
                        person.DateOfBirth.Value.ToString("yyyy-MM-dd");
                excelWorksheet.Cells[row, 4].Value = person.Age;
                excelWorksheet.Cells[row, 5].Value = person.Gender;
                excelWorksheet.Cells[row, 6].Value = person.Country;
                excelWorksheet.Cells[row, 7].Value = person.Address;
                excelWorksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

                row++;
            }

            excelWorksheet.Cells[$"A1:H{row}"].AutoFitColumns();

            await excelPage.SaveAsync();
        }

        memoryStream.Position = 0;

        return memoryStream;
    }
}
