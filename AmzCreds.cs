using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PickPackQuick
{
    public class AmzCreds
    {
        DataSet ds = new DataSet();
        public DataSet getkeys()
        {
            using (SqlConnection con = new myConnection().GetConnection())
            {
        
                using (SqlCommand cmd = new SqlCommand("SP_GetKeys", con))
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();
                                       
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(ds);
                    con.Close();
                }
            }
            return ds;
        }
        public class myConnection
        {
            public SqlConnection GetConnection()
            {
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnn"].ToString());
                if (con.State == ConnectionState.Closed)
                    con.Open();
                return con;
            }

        }
    }
}