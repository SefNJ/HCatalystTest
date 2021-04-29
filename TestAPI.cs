using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;

namespace HCatalystTest
{
    [TestClass]
    public class TestAPI
    {
        private HttpClient m_client = null;

        public void Setup()
        {
            if (m_client == null) m_client = new HttpClient();
            m_client.BaseAddress = new Uri("https://localhost:5001/");
            m_client.Timeout = new TimeSpan(0, 0, 30);
            m_client.DefaultRequestHeaders.Clear();
        }

        [TestMethod]
        public async Task GetPerson()
        {
            const int ID_VALUE_FOR_TESTING = 1;
            Setup();

            // Set the request message
            var request = new HttpRequestMessage(HttpMethod.Get, "api/HCpersons/" + ID_VALUE_FOR_TESTING.ToString());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Call API and get the response
            using (var response = await m_client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content
                var content = await response.Content.ReadAsStringAsync();
                
                HCatalyst.Models.HCperson hcPerson = JsonConvert.DeserializeObject<HCatalyst.Models.HCperson>(content);
                Assert.AreEqual(hcPerson.ID, ID_VALUE_FOR_TESTING);
            }
        }

        [TestMethod]
        public async Task GetPersons()
        {
            Setup();

            // Set the request message
            var request = new HttpRequestMessage(HttpMethod.Get, "api/HCpersons");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Call API and get the response
            using (var response = await m_client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content
                var content = await response.Content.ReadAsStringAsync();
                List<HCatalyst.Models.HCperson> ahcPerson = JsonConvert.DeserializeObject<List<HCatalyst.Models.HCperson>>(content);
            }
        }


        [TestMethod]
        public async Task AddPerson()
        {
            Setup();

            HCatalyst.Models.HCperson hcPerson = new HCatalyst.Models.HCperson { FirstName = "Bernard", LastName = "King", Street = "5 Valley Road", City = "Franklin Lakes", State = "NJ", Zip = "07321", Age = 50, Interests = "Basketball, Reading" };
            string sJSON = JsonConvert.SerializeObject(hcPerson);

            // Set the request message
            var request = new HttpRequestMessage(HttpMethod.Post, "api/HCpersons/");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent stringContent = new StringContent(sJSON, Encoding.UTF8, "application/json");
            request.Content = stringContent;

            // Call API and get the response
            using (var response = await m_client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content
                var content = await response.Content.ReadAsStringAsync();

                HCatalyst.Models.HCperson hcPersonAdded = JsonConvert.DeserializeObject<HCatalyst.Models.HCperson>(content);
                Assert.AreNotEqual(hcPersonAdded.ID, 0);
            }
        }

        [TestMethod]
        public async Task SearchPerson()
        {
            Setup();

            HCatalyst.Models.HCperson hcPerson = new HCatalyst.Models.HCperson { LastName = "Williams" };
            string sJSON = JsonConvert.SerializeObject(hcPerson);

            // Set the request message
            var request = new HttpRequestMessage(HttpMethod.Post, "api/HCpersons/Search");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent stringContent = new StringContent(sJSON, Encoding.UTF8, "application/json");
            request.Content = stringContent;

            // Call API and get the response
            using (var response = await m_client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content
                var content = await response.Content.ReadAsStringAsync();

                List<HCatalyst.Models.HCperson> ahcPerson = JsonConvert.DeserializeObject<List<HCatalyst.Models.HCperson>>(content);
            }
        }

        [TestMethod]
        public async Task SearchContainsPerson()
        {
            Setup();

            HCatalyst.Models.HCperson hcPerson = new HCatalyst.Models.HCperson { LastName = "Smith" };
            string sJSON = JsonConvert.SerializeObject(hcPerson);

            // Set the request message
            var request = new HttpRequestMessage(HttpMethod.Post, "api/HCpersons/SearchContains");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent stringContent = new StringContent(sJSON, Encoding.UTF8, "application/json");
            request.Content = stringContent;

            // Call API and get the response
            using (var response = await m_client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content
                var content = await response.Content.ReadAsStringAsync();

                List<HCatalyst.Models.HCperson> ahcPerson = JsonConvert.DeserializeObject<List<HCatalyst.Models.HCperson>>(content);
            }
        }

    }
}
