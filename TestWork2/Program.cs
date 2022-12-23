using Microsoft.EntityFrameworkCore;
using TestWork2.Configurations;
using TestWork2.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddAppValidators();
builder.Services.AddDbContext<AppDbContext>(configuration =>
{
    configuration.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCoreAdmin();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAdvancedDependencyInjection();

app.UseAuthorization();

app.MapControllers();

app.UseCoreAdminCustomUrl("admin");

app.Run();