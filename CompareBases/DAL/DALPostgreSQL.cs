using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpgsqlTypes;
using System.Threading;


namespace CompareBases.DAL
{
    class DALPostgreSQL
	{
		/// <summary>
		/// Коннектион к базе Postgre
		/// </summary>
		private NpgsqlConnection Connection;

		/// <summary>
		/// Строка подключения к базе, обязательна к заполнению
		/// </summary>
		private string ConnectionString;

		/// <summary>
		/// Массив DALPostgreSQL для потоков
		/// </summary>
		private static Dictionary<int, DALPostgreSQL> Pool = new Dictionary<int, DALPostgreSQL>();

		public static void SetConnectionString(string connectionString)
		{
			DALPostgreSQL dal = GetThreadDal();
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
			DALPostgreSQL dal = GetThreadDal();
			if (dal.Connection != null)
			{
				dal.Connection.Close();
				dal.Connection = null;
			}
		}

		public static DataSet ExecuteDataSet(string sql, NpgsqlParameter[] parameters)
		{
			using (var adapter = GetAdapter(sql, parameters))
			{
				var dt = new DataSet();
				adapter.Fill(dt);
				return dt;
			}
		}

		public static DataTable ExecuteDataTable(string sql, NpgsqlParameter[] parameters)
		{
			var dt = new DataTable();
			ExecuteDataTable(sql, parameters, dt);
			return dt;
		}

		public static void ExecuteDataTable(string sql, NpgsqlParameter[] parameters, DataTable dt)
		{
			using (var adapter = GetAdapter(sql, parameters))
			{
				adapter.Fill(dt);
			}
		}

		private void CheckConnection()
		{
			if (Connection == null)
			{
				Connection = new NpgsqlConnection(ConnectionString);
			}
			if (Connection.State == ConnectionState.Broken)
			{
				Connection.Close();
			}
			if (Connection.State == ConnectionState.Closed)
			{
				try
				{
					Connection = new NpgsqlConnection(ConnectionString);
					Connection.Open();
				}
				catch (Exception ex)
				{
					if (Connection.State == ConnectionState.Open) 
						Connection.Close();

					throw new ApplicationException("Ошибка соединения с сервером.", ex);
				}
			}
		}

		/// <summary>
		/// Получает DAL для текущего потока
		/// </summary>
		private static DALPostgreSQL GetThreadDal()
		{
			int threadId = Thread.CurrentThread.ManagedThreadId;
			DALPostgreSQL dal;
			Pool.TryGetValue(threadId, out dal);
			if (dal == null)
			{
				dal = new DALPostgreSQL();
				Pool.Add(threadId, dal);
			}
			return dal;
		}

		private static NpgsqlDataAdapter GetAdapter(string sql, NpgsqlParameter[] parameters)
		{
			DALPostgreSQL dal = GetThreadDal();
			dal.CheckConnection();

			NpgsqlCommand sc = new NpgsqlCommand(sql, dal.Connection);
			sc.CommandType = CommandType.Text;
			sc.CommandTimeout = 0;
			if (parameters != null) sc.Parameters.AddRange(parameters);
			foreach (NpgsqlParameter parameter in sc.Parameters)
			{
				if (parameter.Value == null)
					parameter.Value = DBNull.Value;
				if (parameter.NpgsqlDbType == NpgsqlDbType.Timestamp && parameter.Value.Equals(new DateTime(1, 1, 1)))
					parameter.Value = DBNull.Value;
			}
			return new NpgsqlDataAdapter(sc);
		}
	}
}
