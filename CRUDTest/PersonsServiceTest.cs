using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using Xunit;
using Entities;
using Services;

namespace CRUDTests;
public class PersonsServiceTest
{
    //private fields
    private readonly IPersonsService _personService;

    //constructor
    public PersonsServiceTest()
    {
        _personService = new PersonsService();
    }

    #region AddPerson

    //When we supply null value as PersonAddRequest, it ahould throw ArgumentNullException
    [Fact]

    public void AddPerson_NullPerson()
    {
        //Arrange
        PersonAddRequest? personAddRequest = null;

        //Act
        Assert.Throws<ArgumentNullException>(() =>
        {
            _personService.AddPerson(personAddRequest);
        });
    }


    //When we supply null value as PersonName, it ahould throw ArgumentException
    [Fact]

    public void AddPerson_PersonNameIsNull()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = null
        };

        //Act
        Assert.Throws<ArgumentException>(() =>
        {
            _personService.AddPerson(personAddRequest);
        });
    }


    //When we supply null value as Email, it ahould throw ArgumentException
    [Fact]

    public void AddPerson_EmailIsNull()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            Email = null
        };

        //Act
        Assert.Throws<ArgumentException>(() =>
        {
            _personService.AddPerson(personAddRequest);
        });
    }


    //When we supply null value as DateOfBirth, it ahould throw ArgumentException
    [Fact]

    public void AddPerson_DateOfBirthIsNull()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            DateOfBirth = null
        };

        //Act
        Assert.Throws<ArgumentException>(() =>
        {
            _personService.AddPerson(personAddRequest);
        });
    }


    //When we supply proper person details, it should insert the person into the persons list;
    //and it should return an object of PersonResponse, which includes with the newly generated
    //person id  
    [Fact]

    public void AddPerson_ProperPersonDetails()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "Person name:..", Email = "personNull@gmail.com", Address ="Address null",
            CountryID = Guid.NewGuid(), Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse
            ("2020-02-03"), ReceiveNewsLetters = true
        };

        //Act
        PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);
        List<PersonResponse> persons_list = _personService.GetAllPersons();

        //Assert
        Assert.True(person_response_from_add.PersonID != Guid.Empty);

        Assert.Contains(person_response_from_add, persons_list);
    }


    #endregion
}
