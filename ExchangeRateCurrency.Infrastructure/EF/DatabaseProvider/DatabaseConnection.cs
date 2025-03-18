using ExchangeRateCurrency.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateCurrency.Infrastructure.EF.DatabaseProvider;
public class DatabaseConnection : IDatabaseConnection
{
	private readonly ExchageRateCurrencyDbContext _context;

	public DatabaseConnection(ExchageRateCurrencyDbContext context)
	{
		_context = context;
	}

	public SqlConnection GetSqlConnection()
		=> (SqlConnection)_context.Database.GetDbConnection();
}
