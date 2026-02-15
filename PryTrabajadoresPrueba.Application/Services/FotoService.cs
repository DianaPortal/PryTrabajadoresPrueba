using Microsoft.AspNetCore.Http;
using PryTrabajadoresPrueba.Application.Interfaces;
using System.Text.Json;
namespace PryTrabajadoresPrueba.Application.Services
{

    public class FotoService : IFotoService
    {
        private readonly string _apiKey = ""; 

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