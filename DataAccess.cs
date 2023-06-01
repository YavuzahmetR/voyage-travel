using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class DataAccess
    {
        public static string WorkstationId;

        //public static string SqlConnectionString= "Provider=MSOLEDBSQL;Server=(localdb)\\MSSQLLocalDB;Database=myDataBase;UID=admin;PWD=DB12345;";
        //private string oleDB12 = String.Format(@"Provider = Microsoft.ACE.OLEDB.12.0;Data Source = {0}a2datalocal.accdb;", AppContext.BaseDirectory);
        //private string oleDB4 = String.Format(@"Provider = Microsoft.Jet.OLEDB.4.0;Data Source = {0}a2datalocal.mdb;User Id=admin;Password=;", AppContext.BaseDirectory);


        private static string ConnectionString;

        public SqlCommand comm;

        public SqlConnection conn;

        public DataAccess(string connectionString)
        {
            ConnectionString = connectionString;
            conn = new SqlConnection(ConnectionString);
        }


        public string StringResult(string query)
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();

                    comm = new SqlCommand(query, conn);

                    object result = comm.ExecuteScalar();

                    if (result == null)
                    {
                        result = 0;
                    }

                    conn.Close();

                    return result.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return string.Empty;
        }

        public DataSet DataSetResult(string query)
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();

                    comm = new SqlCommand(query, conn);

                    var da = new SqlDataAdapter(comm);

                    var ds = new DataSet();

                    da.Fill(ds);


                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return null;
        }

        public DataTable DataTableResult(string query)
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                    comm = new SqlCommand(query, conn);
                    var da = new SqlDataAdapter(comm);
                    DataTable dt;
                    da.Fill(dt = new DataTable());
                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return null;
        }

        public DataRow DataRowResult(string query)
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();

                    comm = new SqlCommand(query, conn);

                    var da = new SqlDataAdapter(comm);

                    var ds = new DataSet();

                    da.Fill(ds);

                    DataRow dr = null;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dr = ds.Tables[0].Rows[0];
                    }

                    conn.Close();

                    return dr;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return null;
        }

        public SqlDataReader DataReaderResult(string query)
        {
            SqlDataReader dr;

            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();

                    comm = new SqlCommand(query, conn);
                    
                    dr = comm.ExecuteReader();

                    return dr;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return null;
        }

        public int InsertUpdate(string query)
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();

                    comm = new SqlCommand(query, conn);

                    int rtrn = comm.ExecuteNonQuery();

                    conn.Close();

                    return rtrn;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return int.MinValue;
        }

        public int InsertUpdate(SqlCommand comm)
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    comm.Connection = conn;

                    int rtrn = comm.ExecuteNonQuery();

                    conn.Close();

                    return rtrn;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return int.MinValue;
        }

        public object ExecStoredProcedure(string spName)
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();

                    comm = new SqlCommand(spName, conn);

                    SqlDataReader rtrn = comm.ExecuteReader();

                    conn.Close();

                    return rtrn;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }

            return null;
        }

        public object ExecStoredProcedurewithParameters(string spName, List<SqlParameter> parameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();

                    comm = new SqlCommand(spName, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    foreach (SqlParameter SQLp in parameters)
                    {
                        comm.Parameters.Add(SQLp);
                    }

                    SqlDataReader rtrn = comm.ExecuteReader();

                    conn.Close();

                    return rtrn;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return null;
        }


    }

}
