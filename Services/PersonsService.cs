using System;
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;

namespace Services;
public class PersonsService : IPersonsService
{

    //private field
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;

    //constructor
    public PersonsService()
    {
        _persons = new List<Person>();
        _countriesService = new CountriesService();
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
}
