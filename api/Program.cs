using System.Linq;
using api.Errors;
using api.Extensions;
using api.Helpers;
using api.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<StoreContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IConnectionMultiplexer>(c=>{
    var configuration=ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(configuration);
});

//Services extension
builder.Services.AddApplicationServices();
// builder.Services.AddSwaggerGen(c=>{
// c.SwaggerDoc("V1",new OpenApiInfo(){Title="API",Version="V1"});
// });
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors();

var app = builder.Build();

//data seeding and migration on start
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    try
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<StoreContext>();
        context.Database.EnsureCreated();
        //await context.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(context, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred during migration");
    }

}



// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

//Extension method
app.UseSwaggerDocumentation();

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
app.UseAuthorization();
app.MapControllers();

app.Run();
