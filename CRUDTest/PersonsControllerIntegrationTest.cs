using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class PersonsControllerIntegrationTest
    {
        #region Index

        [Fact]
        public void Index_ToReturnView()
        {
            //Arrange

            //Act
            HttpResponseMessage response = _client.GetAsync("/Persons/Index");

            //Assert
            response.Should().BeSuccessful();
        }

        #endregion
    }
}
