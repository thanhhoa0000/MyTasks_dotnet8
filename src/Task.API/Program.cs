using System.Reflection;
using Task.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("TasksDB");
builder.Services.AddDbContextFactory<TaskContext>(
    options => options.UseNpgsql(connectionString));


builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddHttpContextAccessor();

builder.Services.AddCarter();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.MapCarter();

app.UseHttpsRedirection();

app.Run();
