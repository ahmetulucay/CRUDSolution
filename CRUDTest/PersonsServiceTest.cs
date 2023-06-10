using ServiceContracts;
using ServiceContracts.DTO;
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
}
