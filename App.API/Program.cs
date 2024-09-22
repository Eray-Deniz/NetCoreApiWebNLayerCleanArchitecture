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
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //referans tipler için null kontrolünü gerçekleþtirme. Bunu yazmazsak .net kendi de fluent validation da hata yazýyordu.
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
 * Proram.cs API projesindeki service leri ve middleware leri barýndýrdýðý için aþaðýdaki Sql server ýn connection stringi belirttiðimiz kodu extension method haline getirip repository katmanýna taþýyoruz.

 * Repositories\Extensions\RepositoryExtensions.cs

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionStrings = builder.Configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

    //connectionStrings te uyarý veriyordu. bunun null olmayacaðýný belirtmet için connectionStrings! þeklinde yazdýk.
    options.UseSqlServer(connectionStrings!.SqlServer);
});

*/

//Repositories ve Services içeirindeki extinsions metodlarý ekle
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);

var app = builder.Build();

//içerisine mutlaka bir deðer set edilmesi isteniyor. Services\Extensions\ServiceExtension.cs içerisinde exception handler larý set etmiþtik.
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