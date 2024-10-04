var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("TasksDB");
builder.Services.AddDbContext<TaskContext>(
    options => options.UseNpgsql(connectionString));

builder.Services.AddHttpContextAccessor();

builder.Services.AddCarter();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseHttpsRedirection();

app.Run();
