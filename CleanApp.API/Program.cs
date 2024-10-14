using App.Application.Extensions;
using App.Persistence.Extensions;
using CleanApp.API.ExceptionHandler;
using CleanApp.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //referans tipler için null kontrolünü gerçekleþtirme. Bunu yazmazsak .net kendi de fluent validation da hata yazýyordu.
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repositories ve Services içeirindeki extinsions metodlarý ekle
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);
builder.Services.AddScoped(typeof(NotFoundFilter<,>)); //2 tane generic aldýðý için(T,TId) araya virgül koy <,>
builder.Services.AddExceptionHandler<CriticalExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

//içerisine mutlaka bir deðer set edilmesi isteniyor. Services\Extensions\ServiceExtension.cs içerisinde exception handler larý set etmiþtik.
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