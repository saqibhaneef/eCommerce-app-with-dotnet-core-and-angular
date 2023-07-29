using System.Linq;
using api.Errors;
using api.Extensions;
using api.Helpers;
using api.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppIdentityDbContext>(x =>
{
    x.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddDbContext<StoreContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IConnectionMultiplexer>(c=>{
    var configuration=ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(configuration);
});

//Services extension
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);
// builder.Services.AddSwaggerGen(c=>{
// c.SwaggerDoc("V1",new OpenApiInfo(){Title="API",Version="V1"});
// });
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors();

var app = builder.Build();

//data seeding and migration on start
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
//using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
//{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    var usermanager = services.GetRequiredService<UserManager<AppUser>>();
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        await context.Database.MigrateAsync();
        await identityContext.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(context, loggerFactory);
        await AppIdentityDbContextSeed.SeedUsersAsync(usermanager);
    }
    catch (Exception ex)
    {

        logger.LogError(ex, "An error occured during migration");
    }

//}



// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

//Extension method
app.UseSwaggerDocumentation();

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();
