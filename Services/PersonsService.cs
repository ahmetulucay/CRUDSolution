
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services.Helpers;
using ServiceContracts.Enums;

namespace Services;
public class PersonsService : IPersonsService
{

    //private field
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;

    //constructor
    public PersonsService(bool initialize = true)
    {
        _persons = new List<Person>();
        _countriesService = new CountriesService();

        if (initialize)
        {
            _persons.Add(new Person()
            { PersonID = Guid.Parse("BF0DFD8F-EDE8-4D27-846D-6A2FA0414FAB"),
                PersonName = "Jorey", 
                Email = "jnore0@google.com",
                DateOfBirth = DateTime.Parse("2004-06-06"),
                Gender = "Female",
                Address = "0913 Eggendart Lane",
                ReceiveNewsLetters = false,
                CountryID = Guid.Parse("A308DFE4-479D-42E0-8506-5ED01B8F1C62")
            });

            _persons.Add(new Person()
            {
                PersonID = Guid.Parse("6402182C-55A1-47B0-A12B-A2F11E595A27"),
                PersonName = "Jenda",
                Email = "jmccahey1@plala.or.jp",
                DateOfBirth = DateTime.Parse("1995-11-21"),
                Gender = "Female",
                Address = "0064 Monument Park",
                ReceiveNewsLetters = true,
                CountryID = Guid.Parse("828EDC8D-438C-4C8A-A03F-3368F8C4EA19")
            });

            _persons.Add(new Person()
            {
                PersonID = Guid.Parse("43F26DF8-2F58-4D81-B4FB-0133EA6A2219"),
                PersonName = "Hewitt",
                Email = "hzecchetti2@omniture.com",
                DateOfBirth = DateTime.Parse("2004-06-23"),
                Gender = "Male",
                Address = "172 Badeau Street",
                ReceiveNewsLetters = true,
                CountryID = Guid.Parse("41E6C698-100B-45E6-8F21-831E106AAAE3")
            });

            _persons.Add(new Person()
            {
                PersonID = Guid.Parse("0C77CCF3-58BA-4909-B263-C3067AB3564B"),
                PersonName = "Lotta",
                Email = "lwinkworth3@hhs.gov",
                DateOfBirth = DateTime.Parse("2008-08-02"),
                Gender = "Female",
                Address = "350 Evergreen Junction",
                ReceiveNewsLetters = true,
                CountryID = Guid.Parse("7CCF8F21-4757-415A-B696-94A8465A6D65")
            });

            _persons.Add(new Person()
            {
                PersonID = Guid.Parse("AB642EBB-0D06-4A01-9E99-6EF8BB8116A6"),
                PersonName = "Dolly",
                Email = "dhierro4@hatena.ne.jp",
                DateOfBirth = DateTime.Parse("2009-07-12"),
                Gender = "Female",
                Address = "458 Kings Pass",
                ReceiveNewsLetters = true,
                CountryID = Guid.Parse("06E1FDA9-68B4-4688-AEB3-5BBC33FAA559")
            });

            _persons.Add(new Person()
            {
                PersonID = Guid.Parse("AEA39288-27D8-4D88-9FC8-9D8038C89157"),
                PersonName = "Adlai",
                Email = "akeesman5@slashdot.org",
                DateOfBirth = DateTime.Parse("1997-07-27"),
                Gender = "Male",
                Address = "66 Anthes Crossing",
                ReceiveNewsLetters = false,
                CountryID = Guid.Parse("44E78C26-8B7D-4207-9980-3F7740D9C03E")
            });
            /*
Kikelia,kpocklington6@themeforest.net,1990-11-18,Female,548 7th Circle,true
Blaine,bbanes7@pagesperso-orange.fr,2001-05-09,Male,75881 Tennessee Alley,true
Hunt,hgouth8@fema.gov,1997-12-19,Male,0217 Hudson Drive,false
Alexandr,await9@zimbio.com,2009-12-27,Male,3 La Follette Drive,false
             */
        }
    }

        private PersonResponse ConvertPersontToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
        return personResponse;
    }

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
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
        _persons.Add(person);

        //convert the Person object into PersonResponse type
        return ConvertPersontToPersonResponse(person);

    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(temp => temp.ToPersonResponse()).ToList();
    }

    public PersonResponse? GetPersonByPersonID(Guid? personID)
    {
        if (personID == null)
            return null;

        Person? person = _persons.FirstOrDefault(p => p.PersonID == personID);

        if(person == null)
            return null;

        return person.ToPersonResponse();
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.PersonName)?
                temp.PersonName.Contains(searchString, 
                StringComparison.OrdinalIgnoreCase): true)).ToList();
                break;

            case nameof(Person.Email):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Email) ?
                temp.Email.Contains(searchString,
                StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Person.DateOfBirth):
                matchingPersons = allPersons.Where(temp =>
                (temp.DateOfBirth != null) ?
                temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString,
                StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(Person.Gender):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Gender) ?
                temp.Gender.Contains(searchString,
                StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Person.CountryID):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Country) ?
                temp.Country.Contains(searchString,
                StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Person.Address):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Address) ?
                temp.Address.Contains(searchString,
                StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            default: matchingPersons = allPersons;
                break;
        }

        return matchingPersons;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
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

    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(Person));

        //validation
        ValidationHelper.ModelValidation(personUpdateRequest);

        //get matching person object to update
        Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
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

        return matchingPerson.ToPersonResponse();
    }

    public bool DeletePerson(Guid? personID)
    {
        if (personID == null)
        {
            throw new ArgumentNullException(nameof(personID));
        }

        Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);

        if (person == null)
            return false;

        _persons.RemoveAll(temp => temp.PersonID == personID);

        return true;
    }
}
