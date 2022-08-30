using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<StoreContext>(x=>x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
