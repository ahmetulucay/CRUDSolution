﻿using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Entities;
using Services;
using Xunit.Abstractions;

namespace CRUDTests;
public class PersonsServiceTest
{
    //private fields
    private readonly IPersonsService _personService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    //constructor
    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _personService = new PersonsService();
        _countriesService = new CountriesService();
        _testOutputHelper = testOutputHelper;
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

    #region GetPersonPersonID

    //If we supply null as PersonID, it should return null as
    //PersonResponse
    [Fact]
    public void GetPersonByPersonID_NullPersonID()
    {
        //Arrange
        Guid? personID = null;

        //Act
        PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(personID);

        //Assert
        Assert.Null(person_response_from_get);
    }

    //if we supply a valid person id, it should return the valid
    //person details as PersonResponse object
    [Fact]
    public void GetPersonByPersonID_WithPersonID()
    {
        //Arrange
        CountryAddRequest? country_request = new CountryAddRequest()
        {CountryName = "Canada"};
        CountryResponse country_response = _countriesService.AddCountry(country_request);

        //Act
        PersonAddRequest person_request = new PersonAddRequest()
        {
            PersonName = "person name...",
            Email = "email@sample.com",
            Address = "...",
            CountryID = country_response.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = false,
        };
        PersonResponse person_response_from_add = _personService.AddPerson(person_request);
        
        PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.PersonID);

        //Assert
        Assert.Equal(person_response_from_add, person_response_from_get);
    }
    #endregion

    #region GetAllPersons

    //The GetAllPersons() should return an empty list by default
    [Fact]
    public void GetAllPersons_EmptyList()
    {
        //Act
        List<PersonResponse> persons_from_get = _personService.GetAllPersons();

        //Assert
        Assert.Empty(persons_from_get);
    }

    //First, we will add few persons; and then when we call GetAllPersons(),
    //it should return the same persons that were added

    [Fact]

    public void GetAllPersons_AddFewPersons()
    {
        //Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest()
        { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest()
        { CountryName = "United Kingdom" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest()
        {   PersonName = "George", 
            Email = "george@gmail.com", 
            Gender = GenderOptions.Male,
            Address = "address of george", 
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"), 
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request_2 = new PersonAddRequest()
        {
            PersonName = "Jane",
            Email = "jane@gmail.com",
            Gender = GenderOptions.Female,
            Address = "address of jane",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2000-03-02"),
            ReceiveNewsLetters = false
        };

        PersonAddRequest person_request_3 = new PersonAddRequest()
        {
            PersonName = "Veronica",
            Email = "veronica@gmail.com",
            Gender = GenderOptions.Female,
            Address = "address of veronica",
            CountryID = country_response_2.CountryID,
            DateOfBirth = DateTime.Parse("2000-04-03"),
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
        {
            person_request_1, person_request_2, person_request_3
        };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //Print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected: ");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        //Act
        List<PersonResponse> persons_list_from_get = _personService.GetAllPersons();

        //Print person_list_from_get
        _testOutputHelper.WriteLine("Actual: ");
        foreach (PersonResponse person_response_from_get in persons_list_from_get)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, persons_list_from_get);
        }
    }

    #endregion

    #region GetFilteredPersons

    //If the search text is empty and search by is "PersonName", it should return all persons

    [Fact]

    public void GetFilteredPersons_EmptySearchText()
    {
        //Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest()
        { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest()
        { CountryName = "United Kingdom" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest()
        {
            PersonName = "George",
            Email = "george@gmail.com",
            Gender = GenderOptions.Male,
            Address = "address of george",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request_2 = new PersonAddRequest()
        {
            PersonName = "Jane",
            Email = "jane@gmail.com",
            Gender = GenderOptions.Female,
            Address = "address of jane",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2000-03-02"),
            ReceiveNewsLetters = false
        };

        PersonAddRequest person_request_3 = new PersonAddRequest()
        {
            PersonName = "Veronica",
            Email = "veronica@gmail.com",
            Gender = GenderOptions.Female,
            Address = "address of veronica",
            CountryID = country_response_2.CountryID,
            DateOfBirth = DateTime.Parse("2000-04-03"),
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
        {
            person_request_1, person_request_2, person_request_3
        };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //Print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected: ");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        //Act
        List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

        //Print person_list_from_get
        _testOutputHelper.WriteLine("Actual: ");
        foreach (PersonResponse person_response_from_get in persons_list_from_search)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, persons_list_from_search);
        }
    }

    //First we will add few persons; and then we will search based on person name with some
    //search string. It should return the matching persons.

    [Fact]

    public void GetFilteredPersons_SearchByPersonName()
    {
        //Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest()
        { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest()
        { CountryName = "United Kingdom" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest()
        {
            PersonName = "George",
            Email = "george@gmail.com",
            Gender = GenderOptions.Male,
            Address = "address of george",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request_2 = new PersonAddRequest()
        {
            PersonName = "Monica",
            Email = "monica@gmail.com",
            Gender = GenderOptions.Female,
            Address = "address of monica",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2000-03-02"),
            ReceiveNewsLetters = false
        };

        PersonAddRequest person_request_3 = new PersonAddRequest()
        {
            PersonName = "Veronica",
            Email = "veronica@gmail.com",
            Gender = GenderOptions.Female,
            Address = "address of veronica",
            CountryID = country_response_2.CountryID,
            DateOfBirth = DateTime.Parse("2000-04-03"),
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
        {
            person_request_1, person_request_2, person_request_3
        };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //Print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected: ");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        //Act
        List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "nica");

        //Print person_list_from_get
        _testOutputHelper.WriteLine("Actual: ");
        foreach (PersonResponse person_response_from_get in persons_list_from_search)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            if (person_response_from_add is not null)
            {
                if (person_response_from_add.PersonName.Contains("nica",
                StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(person_response_from_add, persons_list_from_search);
                }
            }
        }
    }

    #endregion

    #region GetSortedPersons

    //When we sort based on PersonName in DESC, it should return persons list in descending
    //on PersonName
    [Fact]

    public void GetSortedPersons()
    {
        //Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest()
        { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest()
        { CountryName = "United Kingdom" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest()
        {
            PersonName = "George",
            Email = "george@gmail.com",
            Gender = GenderOptions.Male,
            Address = "address of george",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request_2 = new PersonAddRequest()
        {
            PersonName = "Monica",
            Email = "monica@gmail.com",
            Gender = GenderOptions.Female,
            Address = "address of monica",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2000-03-02"),
            ReceiveNewsLetters = false
        };

        PersonAddRequest person_request_3 = new PersonAddRequest()
        {
            PersonName = "Veronica",
            Email = "veronica@gmail.com",
            Gender = GenderOptions.Female,
            Address = "address of veronica",
            CountryID = country_response_2.CountryID,
            DateOfBirth = DateTime.Parse("2000-04-03"),
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
        {
            person_request_1, person_request_2, person_request_3
        };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //Print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected: ");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }


        List<PersonResponse> allPersons = _personService.GetAllPersons();

        //Act
        List<PersonResponse> persons_list_from_sort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

        //Print person_list_from_get
        _testOutputHelper.WriteLine("Actual: ");
        foreach (PersonResponse person_response_from_get in persons_list_from_sort)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => 
        temp.PersonName).ToList();

        //Assert

        for (int i = 0; i< person_response_list_from_add.Count; i++)
        {
            Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
        }
    }
    #endregion
}
