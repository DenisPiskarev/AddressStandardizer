using AddressStandardizer.Mappings;
using Microsoft.Net.Http.Headers;

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
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseAuthorization();


app.MapControllers();

app.Run();