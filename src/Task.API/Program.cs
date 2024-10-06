using Task.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("TasksDB");
builder.Services.AddDbContext<TaskContext>(
    options => options.UseNpgsql(connectionString));

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
