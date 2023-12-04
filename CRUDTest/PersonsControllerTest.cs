using AutoFixture;
using Moq;
using ServiceContracts;

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
}


