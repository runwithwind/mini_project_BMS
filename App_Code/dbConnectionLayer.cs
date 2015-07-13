using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlServerCe;

/// <summary>
/// 
/// </summary>
public class DBConnectionLayer
{
    private string dbLocation = "Data Source=C:\\Users\\jordan_pan\\Desktop\\BMS_release\\BMS\\App_Data\\db_BMS.sdf";
    private SqlCeConnection sqlConn;
    
    //建立连接对象
    public DBConnectionLayer()
    {
        sqlConn = new SqlCeConnection(dbLocation);
    }

    public void OpenConnection()
    {
        sqlConn.Open();
    }
    public void CloseConnection()
    {
        sqlConn.Close();
    }
    public void Dispose()
    {
        sqlConn.Dispose();
    }

    //执行数据库操作语句
    //穿入操作，通过参数化查询防止sql注入
    //SqlCeParameter[] pms = new SqlCeParameter[] {
    //                new SqlCeParameter("@uid", txtUid.Text.Trim()),
    //                new SqlCeParameter("@pwd", txtPwd.Text)
    //                };
    public void ExecuteCommand(string commStr,SqlCeParameter[] pms)
    {
        try
        {
            using (SqlCeCommand cmd = new SqlCeCommand(commStr, sqlConn))
            {
                cmd.Parameters.AddRange(pms);
                cmd.ExecuteNonQuery();
            }
        }
        //检查数据库操作错误，将SqlException异常放在普通异常Exceprion中抛出供外界捕获
        catch (SqlException ex)
        {
            Exception error = new Exception("执行数据库操作错误！", ex);
            throw error;
        }
    }

    //获取DataTable
    public DataTable GetDataTable(string commStr,SqlCeParameter[] pms)
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlCeCommand cmd = new SqlCeCommand(commStr, sqlConn))
            {
                cmd.Parameters.AddRange(pms);
                SqlCeDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();
            }
            return dt;
        }
        catch (SqlException ex)
        {
            Exception error = new Exception("执行数据库操作错误！", ex);
            throw error;
        }
    }

    //返回查询结果的第一行第一列
    public object getTheResult(string sqlStr,SqlCeParameter[] pms)
    {
        SqlCeCommand cmd = new SqlCeCommand(sqlStr, sqlConn);
        cmd.Parameters.AddRange(pms);
        return cmd.ExecuteScalar();
    }

}
