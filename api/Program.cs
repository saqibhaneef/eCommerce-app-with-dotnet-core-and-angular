using System.Linq;
using api.Errors;
using api.Helpers;
using api.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreContext>(x=>x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<ApiBehaviorOptions>(options=>{
    options.InvalidModelStateResponseFactory=actioncontext=>
    {
        var errors=actioncontext.ModelState
           .Where(e=>e.Value.Errors.Count>0)
           .SelectMany(x=>x.Value.Errors)
           .Select(x=>x.ErrorMessage).ToArray();
           var errorResponse=new ApiValidationErrorResponse()
           {
            Errors=errors
           };
           return new BadRequestObjectResult(errorResponse);
    };
});
// builder.Services.AddSwaggerGen(c=>{
// c.SwaggerDoc("V1",new OpenApiInfo(){Title="API",Version="V1"});
// });

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

app.UseSwagger();
app.UseSwaggerUI();

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
