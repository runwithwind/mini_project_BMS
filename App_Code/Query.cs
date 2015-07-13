using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlServerCe;

/// <summary>
/// Query 的摘要说明
/// </summary>
public class Query
{
	public Query()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /*错误的设计，Name位数可能多于PSID
    //通过employee信息查询出其借书状态
    //前置条件：输入为PSID，故需要Operation中NameToPSID辅助
    //返回DataTable, 借阅信息
    public DataTable QueryByEmployee(string PSID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select *from tb_borrowed where PSID=@psid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",PSID)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        db.CloseConnection();
        return dt;
    }
    */
    public DataTable QueryByPSID(string PSID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "SELECT tb_borrowed.PSID, tb_employee.Name, tb_employee.MaxBorrow, tb_borrowed.BID, tb_book.Title FROM tb_borrowed INNER JOIN tb_book ON tb_borrowed.BID = tb_book.BID INNER JOIN tb_employee ON tb_borrowed.PSID = tb_employee.PSID WHERE (tb_borrowed.PSID = @psid)";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",PSID)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        db.CloseConnection();
        return dt;
    }
    //不考虑同名情况
    public DataTable QueryByName(string Name)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        //认为不存在同名情况
        string sql = "select PSID from tb_employee where Name=@name";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@name",Name)
        };
        string psid = Convert.ToString(db.getTheResult(sql, pms));
        DataTable dt = QueryByPSID(psid);
        db.CloseConnection();
        return dt;
    }

    //返回人员检索结果，不显示借阅信息
    public DataTable GetEmployeeByPSID(string PSID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select PSID,Name,MaxBorrow from tb_employee where PSID=@psid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@psid",PSID)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        db.CloseConnection();
        return dt;
    }
    //不考虑同名情况
    public DataTable GetEmployeeByName(string Name)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        //认为不存在同名情况
        string sql = "select PSID from tb_employee where Name=@name";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@name",Name)
        };
        string psid = Convert.ToString(db.getTheResult(sql, pms));
        DataTable dt = GetEmployeeByPSID(psid);
        db.CloseConnection();
        return dt;
    }

    //返回书籍检索结果
    public DataTable GetBookByBID(string BID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select *from tb_book where BID=@bid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@bid",BID)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        db.CloseConnection();
        return dt;
    }
    //
    public DataTable GetBookByTitle(string Title)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        //认为不存在同名情况
        string sql = "select *from tb_book where Title=@title";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@title",Title)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        db.CloseConnection();
        return dt;
    }

    //还书后显示借阅者信息的辅助函数，通过BID查询该书的借阅者
    //该书一定被借阅了
    public string GetPSIDByBID(string BID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select PSID from tb_borrowed where BID=@bid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@bid",BID)
        };
        return Convert.ToString(db.getTheResult(sql, pms));
    }


    /* 错误的设计，书籍名称位数可能多于BID位数
    //查询书籍信息，借书的入口
    //输入可为BID或Title
    //返回DataTable，书籍信息
    public DataTable QueryByBook(string TitleOrBID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "select *from tb_book where BID=@bid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@bid",TitleOrBID)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        //判断BID是否查到结果，否则用Title再查
        if (dt.Rows.Count != 0)
        {
            return dt;
        }
        sql = "select *from tb_book where Title=@title";
        pms = new SqlCeParameter[]{
            new SqlCeParameter("@title",TitleOrBID)
        };
        dt = db.GetDataTable(sql, pms);
        db.CloseConnection();
        return dt;
    }
    */
    //BID一定全为数字，认为全为数字的不可能是Title
    //根据需求要返回每本书的状态，如被借阅需要给出借阅人信息
    //返回已借出的书籍
    public DataTable QueryByBIDBorrowed(string BID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "SELECT tb_book.BID, tb_book.Title, tb_employee.PSID, tb_employee.Name FROM tb_book right OUTER JOIN tb_borrowed ON tb_book.BID = tb_borrowed.BID LEFT OUTER JOIN  tb_employee ON tb_borrowed.PSID = tb_employee.PSID where tb_book.BID=@bid";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@bid",BID)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        return dt;
    }
    //返回未借出的书籍
    public DataTable QueryByBIDUnborrowed(string BID)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "SELECT *FROM tb_book WHERE(tb_book.BID NOT IN (SELECT   BID FROM   tb_borrowed)) AND (tb_book.BID =@bid)";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@bid",BID)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        return dt;
    }
    //含有字符则认为是Title
    //返回已借出的书籍
    public DataTable QueryByTitleBorrowed(string Title)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "SELECT tb_book.BID, tb_book.Title, tb_employee.PSID, tb_employee.Name FROM tb_book right OUTER JOIN tb_borrowed ON tb_book.BID = tb_borrowed.BID LEFT OUTER JOIN  tb_employee ON tb_borrowed.PSID = tb_employee.PSID where tb_book.Title=@title";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@title",Title)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        return dt;
    }
    //返回未借出的书籍
    public DataTable QueryByTitleUnborrowed(string Title)
    {
        DBConnectionLayer db = new DBConnectionLayer();
        db.OpenConnection();
        string sql = "SELECT *FROM tb_book WHERE(tb_book.BID NOT IN (SELECT   BID FROM   tb_borrowed)) AND (tb_book.Title =@title)";
        SqlCeParameter[] pms = new SqlCeParameter[]{
            new SqlCeParameter("@title",Title)
        };
        DataTable dt = db.GetDataTable(sql, pms);
        return dt;
    }

}