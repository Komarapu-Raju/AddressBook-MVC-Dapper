using System.Data.SqlClient;

namespace AddressBook.DBConnection
{
    public interface IDBConnection
    {
        SqlConnection GetSqlConnection();
    }
}
