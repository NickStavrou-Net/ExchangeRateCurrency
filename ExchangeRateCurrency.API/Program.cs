using ExchangeRateCurrency.API;
using ExchangeRateCurrency.Application.Stategies;
using ExchangeRateCurrency.Domain.Interfaces;
using ExchangeRateCurrency.Infrastructure.EF;
using ExchangeRateCurrency.Infrastructure.EF.DatabaseProvider;
using ExchangeRateCurrency.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddRateService(builder.Configuration);
builder.Services.AddExchangeRateCurrencyJob(builder.Configuration);
builder.Services.AddScoped<IExchangeRateCurrrencyRepository, ExchangeRateCurrrencyRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();

//strategy services
builder.Services.AddScoped<AddFundsStrategy>();
builder.Services.AddScoped<SubtractFundsStrategy>();
builder.Services.AddScoped<ForceSubtractFundsStrategy>();
builder.Services.AddSingleton<IWalletStrategyResolver, WalletStrategyResolver>();

builder.Services.AddDbContext<ExchageRateCurrencyDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
		   .EnableSensitiveDataLogging());

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
