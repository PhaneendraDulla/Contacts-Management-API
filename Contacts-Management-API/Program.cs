using Contacts_Management_API.Handlers.CommandHandlers;
using Contacts_Management_API.Handlers.QueryHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IGetContactsQueryHandler, GetContactsQueryHandler>();
builder.Services.AddScoped<IAddContactCommandHandler, AddContactCommandHandler>();
builder.Services.AddScoped<IUpdateContactCommandHandler, UpdateContactCommandHandler>();
builder.Services.AddScoped<IDeleteContactCommandHandler, DeleteContactCommandHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.WithMethods("GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS");
    x.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
