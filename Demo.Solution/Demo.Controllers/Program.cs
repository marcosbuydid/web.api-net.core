var builder = WebApplication.CreateBuilder(args);

//adds all the controller clases as services
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();


app.Run();
