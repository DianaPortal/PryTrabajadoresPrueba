using Microsoft.AspNetCore.Http;
using PryTrabajadoresPrueba.Application.Interfaces;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace PryTrabajadoresPrueba.Infrastructure.Services
{

    [ExcludeFromCodeCoverage]
    public class FotoService : IFotoService
    {
        private readonly string _apiKey = "26cae24456ad2a9eab21812461f3c470"; 

        public async Task<string> SubirFoto(IFormFile foto)
        {
            try
            {
                using (var client = new HttpClient())
                using (var content = new MultipartFormDataContent())
                {
                    using (var stream = foto.OpenReadStream())
                    {
                        content.Add(new StreamContent(stream), "image", foto.FileName);
                        var response = await client.PostAsync($"https://api.imgbb.com/1/upload?key={_apiKey}", content);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                            {
                                return doc.RootElement.GetProperty("data").GetProperty("url").GetString() ?? "";
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            return "";
        }

    }
}