using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.OpenApi.Models;
using WebApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình kết nối MySQL
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("StudentDBConnectionString"),
        new MySqlServerVersion(new Version(8, 0, 23)) // Thay đổi phiên bản theo MySQL bạn đang dùng
    )
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
});

app.UseAuthorization();
app.MapControllers();

Console.WriteLine("Open Swagger UI at: http://localhost:5004/swagger/index.html");

app.Run();
