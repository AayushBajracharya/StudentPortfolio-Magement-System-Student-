using Application;
using Application.Converters;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence.Contexts;
using kutumbaAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddWebUIServices();
builder.Services.AddCors();


builder.Services.AddControllers().AddJsonOptions(options=>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    //await initialiser.SeedAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); //c=>c.SwaggerEndpoint("/swagger/v1/swagger.json","DemoJWTToken v1"));
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors(x=>x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(o=>true).AllowCredentials());
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
