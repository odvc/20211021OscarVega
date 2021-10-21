using API.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data
{
    public static class Toaster
    {


        /// <summary>
        /// Loads the parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="args">The arguments.</param>
        private static void LoadParameters(IDbCommand command, Object[] args)
        {
            for (int i = 1; i < command.Parameters.Count; i++)
            {
                SqlParameter p = (SqlParameter)command.Parameters[i];
                if (p.DbType == DbType.DateTime)
                {
                    DateTime.TryParseExact(args[i - 1].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime realDate);
                    p.Value = realDate;
                }
                else
                {
                    if (i <= args.Length) args[i - 1] = args[i - 1].ToString() == "null" ? null : args[i - 1];
                    p.Value = i <= args.Length ? args[i - 1] ?? DBNull.Value : null;
                }
                    
            }
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="storeProcedure">The store procedure.</param>
        /// <returns></returns>
        private static IDbCommand CreateCommand(IDbConnection dbConnection, string storeProcedure)
        {
            SqlCommand Command = (SqlCommand)dbConnection.CreateCommand();
            Command.CommandText = storeProcedure;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandTimeout = 0;

            if (Command.Connection.State == ConnectionState.Closed) Command.Connection.Open();
            SqlCommandBuilder.DeriveParameters(Command);
           
            return Command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeProcedure"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IDataReader GetDataReader(IDbConnection dbConnection, string storeProcedure, params object[] args)
        {
            SqlCommand CommandCreated = (SqlCommand)CreateCommand(dbConnection, storeProcedure);
            LoadParameters(CommandCreated, args);
            CommandCreated.CommandTimeout = 0;
            return CommandCreated.ExecuteReader();
        }

        public static IDataReader GetDataReader(IDbConnection dbConnection, string storeProcedure)
        {
            SqlCommand CommandCreated = (SqlCommand)CreateCommand(dbConnection, storeProcedure);
            return CommandCreated.ExecuteReader();
        }


        public static DataSet GetDataSet(IDbConnection dbConnection, string storeProcedure, params object[] args)
        {
            SqlCommand CommandCreated = (SqlCommand)CreateCommand(dbConnection, storeProcedure);
            LoadParameters(CommandCreated, args);

            SqlDataAdapter SqlAdapter = new SqlDataAdapter(CommandCreated);
            DataSet dataReaded = new DataSet();
            SqlAdapter.Fill(dataReaded);

            return dataReaded;
        }

        public static DataSet GetDataSet(IDbConnection dbConnection, string storeProcedure)
        {
            SqlCommand CommandCreated = (SqlCommand)CreateCommand(dbConnection, storeProcedure);

            SqlDataAdapter SqlAdapter = new SqlDataAdapter(CommandCreated);
            DataSet dataReaded = new DataSet();
            SqlAdapter.Fill(dataReaded);

            return dataReaded;
        }


    }
}
