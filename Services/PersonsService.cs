﻿using System;
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

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

        //Validate PersonName
        if (string.IsNullOrEmpty(personAddRequest.PersonName))
        {
            throw new ArgumentException("PersonName can't be blank.");
        }

        //convert personAddRequest intoPerson type
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
        throw new NotImplementedException();
    }
}
