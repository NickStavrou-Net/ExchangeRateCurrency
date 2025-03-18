using Microsoft.Data.SqlClient;

namespace ExchangeRateCurrency.Domain.Interfaces;
public interface IDatabaseConnection
{
	SqlConnection GetSqlConnection();
}
