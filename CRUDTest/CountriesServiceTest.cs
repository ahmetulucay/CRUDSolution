
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;

namespace CRUDTest;
public class CountriesServiceTest
{
    public readonly ICountriesService? _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesServices();
    }

    //When CountryAddRequest is null, it should throw ArgumentNullException
    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //Act
            _countriesService.AddCountry(request);
        });
    }

    //When CountryName is null, it should throw ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest()
        {
            CountryName = null
        };

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _countriesService.AddCountry(request);
        });
    }

    //When CountryName is duplicate, it should throw ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        //Arrange
        CountryAddRequest? request1 = new CountryAddRequest()
        {  CountryName = "USA" };
        CountryAddRequest? request2 = new CountryAddRequest()
        { CountryName = "USA" };

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);
        });
    }


    //When you supply proper country name, it should insert (add) the country
    //to the existing list of countries
    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest()
        {   CountryName = "Japan"  };

        //Assert
     
            //Act
            CountryResponse response = _countriesService.AddCountry(request);

        //Assert
        Assert.True(response.CountryId != Guid.Empty);
    }

}
