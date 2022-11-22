using BuberDinner.Application;
using BuberDinner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

{
    var x = builder.Configuration;
  builder.Services.AddApplication();
  builder.Services.AddInfrastructure(builder.Configuration);
  builder.Services.AddControllers();
}

var app = builder.Build();
{
  app.UseHttpsRedirection();

  app.MapControllers();

  app.Run();
}
