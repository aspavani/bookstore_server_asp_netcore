using BookstoreApi.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Serilog;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


//Configure Serilog
Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("Logs/BookstoreLog.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

builder.Host.UseSerilog();  // Use Serilog for logging


//Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB limit for files
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore API", Version = "v1" });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});



var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

builder.Services.AddDbContext<BookstoreContext>(
            dbContextOptions => dbContextOptions
                .UseMySql("server=localhost;user=root;password=Mysql@287212;database=booksfromentity",
                serverVersion));

//builder.Host.UseSerilog();

var app = builder.Build();

var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
if (!Directory.Exists(uploadsFolderPath))
{
    Directory.CreateDirectory(uploadsFolderPath);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("corsapp");



app.UseStaticFiles(); // To serve static files like images

app.MapControllers();

app.Run();









