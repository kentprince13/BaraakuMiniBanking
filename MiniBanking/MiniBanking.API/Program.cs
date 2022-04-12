using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MiniBanking.API.Filter;
using MiniBanking.API.Middleware;
using MiniBanking.Core.Configuration;
using MiniBanking.Core.Services;
using MiniBanking.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:yyy-MM-dd HH:mm:ss.fff zzz} {Level}] {Message} ({SourceContext:l}){NewLine}{Exception}"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>c.EnableAnnotations());
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(Program)));
builder.Services.AddMvc(c => c.Filters.Add(typeof(HttpGlobalExceptionFilter)));



var dbConnection = builder.Configuration["ConnectionString"];
builder.Services.AddDbContextPool<MiniBankingContext>(options =>
    {
        options.UseMySql(dbConnection,ServerVersion.AutoDetect(dbConnection)
            ,sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(MiniBankingContext).GetTypeInfo().Assembly.GetName()
                    .Name);
                sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            
            }
                );
    }
);


builder.Services.AddScoped<IGenericService, GenericService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddHttpClient();
builder.Services.AddOptions<PayStackSettings>().Bind(builder.Configuration.GetSection(nameof(PayStackSettings)));



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseMiddleware<BasicAuth>();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<MiniBankingContext>();
    dataContext.Database.Migrate();
}

app.Run();