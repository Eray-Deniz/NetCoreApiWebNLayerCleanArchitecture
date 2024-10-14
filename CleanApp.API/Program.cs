using App.Application.Extensions;
using App.Persistence.Extensions;
using CleanApp.API.ExceptionHandler;
using CleanApp.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //referans tipler i�in null kontrol�n� ger�ekle�tirme. Bunu yazmazsak .net kendi de fluent validation da hata yaz�yordu.
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repositories ve Services i�eirindeki extinsions metodlar� ekle
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);
builder.Services.AddScoped(typeof(NotFoundFilter<,>)); //2 tane generic ald��� i�in(T,TId) araya virg�l koy <,>
builder.Services.AddExceptionHandler<CriticalExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

//i�erisine mutlaka bir de�er set edilmesi isteniyor. Services\Extensions\ServiceExtension.cs i�erisinde exception handler lar� set etmi�tik.
app.UseExceptionHandler(x => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();