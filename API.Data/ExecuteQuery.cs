using API.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace API.Data
{
    public partial class ExecuteQuery : DBAL
    {

        public DataSet Run(string storeProcedure)
        {
            IDbConnection dbConnection = db.Database.Connection;
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            return Toaster.GetDataSet(dbConnection, storeProcedure); 
        }
        public IEnumerable<dynamic> Run(string storeProcedure, params object[] args)
        {
            IDbConnection dbConnection = db.Database.Connection;
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            yield return Toaster.GetDataSet(dbConnection, storeProcedure, args);
        }
    }
}
