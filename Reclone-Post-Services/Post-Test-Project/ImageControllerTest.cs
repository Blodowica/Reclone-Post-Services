using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NBomber;
using NBomber.Contracts;
using NBomber.CSharp;

namespace Post_Test_Project
{
    public class ImageControllerTest
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly string BaseUrl = "https://localhost:7041"; // Replace with the base URL of your API

        public static void Run()
        {
            var loadStep = Step.Create("LoadStep", async context =>
            {
                // Define the URL of the online image
                var imageUrl = "https://cdn.vox-cdn.com/thumbor/TmgXcq6_4URVd0YN0SotUf5WYeA=/1400x1400/filters:format(jpeg)/cdn.vox-cdn.com/uploads/chorus_asset/file/9556001/chicks.0.0.0.jpg";

                using (var form = new MultipartFormDataContent())
                {
                    // Download the image from the URL
                    var imageBytes = await HttpClient.GetByteArrayAsync(imageUrl);

                    // Create a memory stream from the downloaded image
                    using (var imageStream = new MemoryStream(imageBytes))
                    {
                        // Add the image stream to the multipart form data
                        form.Add(new StreamContent(imageStream), "file", "test-image.jpg");

                        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/images/upload")
                        {
                            Content = form
                        };

                        Console.WriteLine(request);
                        var response = await HttpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            return Response.Ok(response);
                        }
                        else
                        {
                            return Response.Ok(response);
                        }
                    }
                }
            });

            var scenario = ScenarioBuilder
                .CreateScenario("ImageControllerLoadTest", loadStep)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(rate: 1, during: TimeSpan.FromSeconds(100))
                });

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}
