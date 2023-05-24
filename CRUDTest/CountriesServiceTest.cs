
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTest;
public class CountriesServiceTest
{
    public readonly ICountriesService? _countriesService;

    //constructor
    public CountriesServiceTest()
    {
        _countriesService = new CountriesService();
    }

    #region AddCountry
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
        Assert.True(response.CountryID != Guid.Empty);
    }
    #endregion

    #region GetAllCountries
    [Fact]
    //The list of countries should be empty by default
    //(before adding any countries)
    public void GetAllCountries_EmptyList()
    {
        //Acts
        List<CountryResponse> _actual_country_response_list = _countriesService.GetAllCountries();

        //Assert
        Assert.Empty(_actual_country_response_list);
    }

    [Fact]
    public void GetAllCountryDetails_AddFewCountries() 
    {
        //Arrange
        List<CountryAddRequest> country_request_list = new 
        List<CountryAddRequest>
        { 
            new CountryAddRequest(){CountryName = "USA"},
            new CountryAddRequest(){CountryName = "UK"}
        };

        //Act
        List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

        foreach (CountryAddRequest country_request in country_request_list)
        {
            countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
        }

        List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();
    }
    #endregion

}
