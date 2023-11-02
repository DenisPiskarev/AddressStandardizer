using AddressStandardizer.Mappings;
using Microsoft.Net.Http.Headers;
using System.Net.Http;

namespace AddressStandardizer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(AppMappingProfile));

            builder.Services.AddHttpClient("DadataClient", client =>
            {
                client.BaseAddress = new Uri("https://cleaner.dadata.ru/");
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            builder.Services.AddCors();

            var app = builder.Build();


            app.UseHttpsRedirection();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin(); // Разрешить запросы с любых источников
                options.AllowAnyMethod(); // Разрешить любые HTTP методы
                options.AllowAnyHeader(); // Разрешить любые заголовки
            });

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}