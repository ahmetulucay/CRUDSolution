﻿using AutoFixture;
using Moq;
using Xunit;
using System.Threading.Tasks;
using ServiceContracts;
using FluentAssertions;
using CRUDExample.Controllers;
using ServiceContracts.DTO;
using ServiceContracts.Enums;


namespace CRUDTests;
public class PersonsControllerTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    private readonly Mock<IPersonsService> _personsServiceMock;
    private readonly Mock<ICountriesService> _countriesServiceMock;

    private readonly Fixture _fixture;

    public PersonsControllerTest()
    {
        _fixture = new Fixture();

        _countriesServiceMock = new Mock<ICountriesService>();
        _personsServiceMock = new Mock<IPersonsService>(); 

        _countriesService = _countriesServiceMock.Object;
        _personsService = _personsServiceMock.Object;
    }

    #region Index

    [Fact]
    public async Task Index_ShouldReturnIndexViewWithPersonsList()
    {
        //Arrange
        List<PersonResponse> persons_response_list =
            _fixture.Create<List<PersonResponse>>();

        PersonsController personsController = new PersonsController(_personsService, _countriesService);

        _personsServiceMock
            .Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(persons_response_list);

        _personsServiceMock
            .Setup(temp => temp.GetSortedPersons
            (It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), 
            It.IsAny<SortOrderOptions>()))
            .ReturnsAsync(persons_response_list);

    }



    #endregion
}


