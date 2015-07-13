using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//增加必要引用
using System.Data.SqlServerCe;
using System.Data;

/// <summary>
/// Operation 的摘要说明
/// </summary>
public class Operation
{
    public Operation()
    {

    }

    /*具体实现时发现为无法使用的服务层
     * 借书的逻辑在查询书籍之后，BID可确定
    //借书
    //传入参数为ID（辅助函数将name转为PSID）
    //返回是否借阅成功
    //之后配套函数需要执行，返回employee可借数量
    //前置条件：PSID存在，Title可以不存在
    public bool Borrow(string PSID, string Title)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();

        //查Title，如果没书，return false；有书返回第一本BID(不能在tb_borrowed中）
        string sqlSearchBook = "select BID from tb_book where Title=@title except (select BID from tb_borrowed))";
        SqlCeParameter[] pmsBook = new SqlCeParameter[]{
            new SqlCeParameter("@title",Title)
        };
        DataTable dtBook = db.GetDataTable(sqlSearchBook, pmsBook);
        if (dtBook.Rows.Count == 0)
        {
            return false;
        }
        string bid = dtBook.Rows[0][1].ToString();

        //查Employee，如果达到最大借阅，return false
        if (CanBorrowNum(PSID) == 0)
        {
            return false;
        }

        //满足可借条件
        string sql = "insert into tb_borrowed(PSID,BID) values(PSID=@psid,BID=@bid)";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",PSID),
            new SqlCeParameter("@title",Title)
        };
        db.ExecuteCommand(sql, pms);
        db.CloseConnection();
        return true;
    }
    */
    //在查询书籍逻辑之后，故书籍一定存在，不能借阅只能是达到了最大借阅量
    //BID可确定，之后search employee，PSID也可确定
    public bool Borrow(string PSID, string BID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();

        //检查是否达到最大借阅量
        if (CanBorrowNum(PSID) == 0)
        {
            return false;
        }

        //满足可借条件
        string sql = "insert into tb_borrowed(PSID,BID) values(@psid,@bid)";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",PSID),
            new SqlCeParameter("@bid",BID)
        };
        db.ExecuteCommand(sql, pms);
        db.CloseConnection();
        return true;
    }

    //还书
    //仅需要知道BID便可归还
    //同借书后三条
    //前置条件：无
    public void Return(string BID)
    {
        //建立打开数据库连接
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "delete from tb_borrowed where BID=@bid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@bid",BID)
        };
        db.ExecuteCommand(sql, pms);
        db.CloseConnection();
    }


    //查询用户借阅数量
    //可借条件的判断<5,借还书之后返回可借数量
    //前置条件：PSID必须存在
    public int CanBorrowNum(string PSID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select count(*) from tb_borrowed where PSID=@psid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",PSID)
        };
        int borrowedNum = Convert.ToInt32(db.getTheResult(sql, pms));
        db.CloseConnection();
        return MaxBorrowNum(PSID)-borrowedNum;
    }
    //默认姓名无重复，姓名可能在不数据库中
    public string NameToPSID(string Name)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select PSID from tb_employee where Name=@name";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@name",Name)
        };
        return Convert.ToString(db.getTheResult(sql, pms));
    }

    //为弹出提示提供信息
    //前置条件：PSID一定存在
    public string PSIDToName(string PSID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select Name from tb_employee where PSID=@psid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",PSID)
        };
        return Convert.ToString(db.getTheResult(sql, pms));
    }


    //查询用户最大借阅数量
    //服务CanBorrowNum，方便后期维护
    //前置条件：PSID必须存在
    public int MaxBorrowNum(string PSID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sqlMaxNum = "select MaxBorrow from tb_employee where PSID=@psid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",PSID)
        };
        int res = Convert.ToInt32(db.getTheResult(sqlMaxNum, pms));
        db.CloseConnection();
        return res;
    }

    /*更正Borrow的逻辑后发现此函数也白写了
     * 不完备的设计导致的无用的编码
    //Name转换为PSID,如果是PSID则返回PSID，如果不是name也不是psid返回null
    //borrow的辅助函数
    //前置条件：无（查询的对象可能不存在）
    public string NameToPSID(string NameOrPSID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select count(*) from tb_employee where PSID=@psid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",NameOrPSID)
        };
        int cnt = Convert.ToInt32(db.getTheResult(sql, pms));
        if (cnt != 0)
        {
            return NameOrPSID;
        }
        //输入不是PSID
        sql = "select PSID from tb_employee where Name=@name";
        pms = new SqlCeParameter[]{
            new SqlCeParameter("@name",NameOrPSID)
        };
        //??object为null是否可以ToString？？
        object obj = db.getTheResult(sql, pms);
        db.CloseConnection();
        string res="null";
        if (Convert.IsDBNull(obj))
        {
            return res;
        }
        return Convert.ToString(obj) ;
    }
    */

    ////Title转换为BID
    ////borrow的辅助
    //private string TitleToBID(string title)
    //{
    //    return "";
    //}
}