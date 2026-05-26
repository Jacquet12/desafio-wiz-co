using Microsoft.EntityFrameworkCore;
using WizCo.Orders.Api.Middleware;
using WizCo.Orders.Application;
using WizCo.Orders.Infrastructure;
using WizCo.Orders.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
    EnsureSqliteDataDirectory(connectionString, app.Environment.ContentRootPath);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();

static void EnsureSqliteDataDirectory(string connectionString, string contentRootPath)
{
    const string prefix = "Data Source=";
    var start = connectionString.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
    if (start < 0)
        return;

    var path = connectionString[(start + prefix.Length)..].Split(';')[0].Trim();
    var directory = Path.GetDirectoryName(path);

    if (string.IsNullOrEmpty(directory))
        return;

    if (!Path.IsPathRooted(path))
        directory = Path.Combine(contentRootPath, directory);

    Directory.CreateDirectory(directory);
}
