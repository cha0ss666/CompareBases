using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace CompareBases.DAL
{
	public class DALSql
	{
        /// <summary>
        /// Строка подключения к базе, обязательна к заполнению
        /// </summary>
        private string ConnectionString;

        public static void SetConnectionString(string connectionString)
        {
            DALSql dal = GetThreadDal();
            if (dal.ConnectionString != connectionString)
            {
                if (dal.Connection != null)
                {
                    dal.Connection.Close();
                    dal.Connection = null;
                }
                dal.ConnectionString = connectionString;
            }
        }

        public static void CloseConnection()
        {
            DALSql dal = GetThreadDal();
            if (dal.Connection != null)
            {
                dal.Connection.Close();
                dal.Connection = null;
            }
        }

		private static Dictionary<int, DALSql> Pool = new Dictionary<int, DALSql>();

		public static DataSet ExecuteDataSet(string sql, SqlParameter[] parameters)
		{
            using (var adapter = GetAdapter(sql, parameters))
            {
                var dt = new DataSet();
                adapter.Fill(dt);
                return dt;
            }
		}

		public static DataTable ExecuteDataTable(string sql, SqlParameter[] parameters)
		{
			var dt = new DataTable();
            ExecuteDataTable(sql, parameters, dt);
			return dt;
		}

        public static void ExecuteDataTable(string sql, SqlParameter[] parameters, DataTable dt)
        {
            using (var adapter = GetAdapter(sql, parameters))
            {
                adapter.Fill(dt);
            }
        }
        
		private SqlConnection Connection;

        private void CheckConnection()
        {
			if (Connection == null)
			{
				Connection = new SqlConnection(ConnectionString);
			}
			if (Connection.State == ConnectionState.Broken)
			{
				Connection.Close();
			}
			if (Connection.State == ConnectionState.Closed)
			{
				try
				{
					Connection = new SqlConnection(ConnectionString);
					Connection.Open();
					SqlCommand cmd = new SqlCommand("set language Russian", Connection);
					cmd.ExecuteNonQuery();
				}
				catch (SqlException ex)
				{
					if (Connection.State == ConnectionState.Open) Connection.Close();
					if (ex.Number == 18456)
					{
						throw new ApplicationException("Неверное имя пользователя или пароль.", ex);
					}
					throw new ApplicationException("Ошибка соединения с сервером.", ex);
				}
			}
		}

		/// <summary>
		/// Получает DAL для текущего потока
		/// </summary>
		private static DALSql GetThreadDal()
		{
			int threadId = Thread.CurrentThread.ManagedThreadId;
			DALSql dal;
			Pool.TryGetValue(threadId, out dal);
			if (dal == null)
			{
				dal = new DALSql();
				Pool.Add(threadId, dal);
			}
			return dal;
		}

        private SqlInfoMessageEventHandler EventText = null;

        public static void AddEventText(Action<string> act)
        {
            DALSql dal = GetThreadDal();
            dal.CheckConnection();

            if (dal.EventText != null) dal.Connection.InfoMessage -= dal.EventText;
            dal.EventText = new SqlInfoMessageEventHandler(
                (sender, e) =>
                {
                    act(e.Message);
                });
            dal.Connection.InfoMessage += dal.EventText;
        }

        private static SqlDataAdapter GetAdapter(string sql, SqlParameter[] parameters)
		{
			DALSql dal = GetThreadDal();
            dal.CheckConnection();

            SqlCommand sc = new SqlCommand(sql, dal.Connection);
			sc.CommandType = CommandType.Text;
			sc.CommandTimeout = 0;
			if (parameters != null) sc.Parameters.AddRange(parameters);
			foreach (SqlParameter parameter in sc.Parameters)
			{
				if (parameter.Value == null)
					parameter.Value = DBNull.Value;
				if (parameter.SqlDbType == SqlDbType.DateTime && parameter.Value.Equals(new DateTime(1, 1, 1)))
					parameter.Value = DBNull.Value;
			}
            return new SqlDataAdapter(sc);
		}

		public static void BulkCopy(string tableName, DataTable insertData)
		{
			DALSql dal = GetThreadDal();
            dal.CheckConnection();
            SqlBulkCopy bulkCopy = new SqlBulkCopy(dal.Connection);
			bulkCopy.DestinationTableName = tableName;
			foreach (DataColumn dc in insertData.Columns)
			{
				bulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
			}
			bulkCopy.WriteToServer(insertData);
		}

	}
}
