using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;
public class CountriesService : ICountriesService
{
    //private field
    private readonly List<Country> _countries;

    //constructor
    public CountriesService(bool initialize = true)
    {
        _countries = new List<Country>();
        if (initialize)
        {
            _countries.AddRange(new List<Country>() {
            new Country()
            {CountryID = Guid.Parse("A308DFE4-479D-42E0-8506-5ED01B8F1C62"),
            CountryName = "USA"},
            new Country()
            {CountryID = Guid.Parse("828EDC8D-438C-4C8A-A03F-3368F8C4EA19"),
            CountryName = "Canada"},
            new Country()
            {CountryID = Guid.Parse("41E6C698-100B-45E6-8F21-831E106AAAE3"),
            CountryName = "UK"},
            new Country()
            {CountryID = Guid.Parse("7CCF8F21-4757-415A-B696-94A8465A6D65"),
            CountryName = "France"},
            new Country()
            {CountryID = Guid.Parse("06E1FDA9-68B4-4688-AEB3-5BBC33FAA559"),
            CountryName = "Germany"},
            new Country()
            {CountryID = Guid.Parse("44E78C26-8B7D-4207-9980-3F7740D9C03E"),
            CountryName = "Norway"}
            });
        }

    }
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        //Validation: countryAddRequest parameter can't be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        //Validation: CountryName can't be null
        if(countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        //Validation: CountryName can't be duplicate
        if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
        {
            throw new ArgumentException("Given country name already exists.");
        }

        //Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry(); 

        //generate CountryId
        country.CountryID = Guid.NewGuid();

        //Add country object into _countries
        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryID(Guid? countryID)
    {
        if (countryID == null)
            return null;
        Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

        if (country_response_from_list == null)
            return null;

        return country_response_from_list.ToCountryResponse();
    }
}