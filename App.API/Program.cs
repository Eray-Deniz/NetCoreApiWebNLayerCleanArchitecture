using App.Repositories;
using Microsoft.EntityFrameworkCore;
using App.Repositories.Extensions;
using App.Services.Extensions;
using Microsoft.AspNetCore.Mvc;
using App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //referans tipler i�in null kontrol�n� ger�ekle�tirme. Bunu yazmazsak .net kendi de fluent validation da hata yaz�yordu.
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
 * Proram.cs API projesindeki service leri ve middleware leri bar�nd�rd��� i�in a�a��daki Sql server �n connection stringi belirtti�imiz kodu extension method haline getirip repository katman�na ta��yoruz.

 * Repositories\Extensions\RepositoryExtensions.cs

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionStrings = builder.Configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

    //connectionStrings te uyar� veriyordu. bunun null olmayaca��n� belirtmet i�in connectionStrings! �eklinde yazd�k.
    options.UseSqlServer(connectionStrings!.SqlServer);
});

*/

//Repositories ve Services i�eirindeki extinsions metodlar� ekle
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);

var app = builder.Build();

//i�erisine mutlaka bir de�er set edilmesi isteniyor. Services\Extensions\ServiceExtension.cs i�erisinde exception handler lar� set etmi�tik.
app.UseExceptionHandler(x => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();