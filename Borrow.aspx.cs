using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

public partial class Borrow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    String s;

    protected override void InitializeCulture()
    {
        if (Request.QueryString["currentculture"] != null)
            Session["lang"] = Request.QueryString["currentculture"];

        s = Session["lang"].ToString();

        if (!String.IsNullOrEmpty(s))
        {
            //UICulture - 决定了采用哪一种本地化资源，也就是使用哪种语言
            //Culture - 决定各种数据类型是如何组织，如数字与日期
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(s);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(s);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //输入异常检查
        string strIn = TextBox1.Text.Trim();
        Regex r = new Regex(@"^[0-9]");
        if (r.IsMatch(strIn) & strIn.Length > 6)
        {
            if (Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('PSID的长度必须小于6！');window.location.href='./Query_employee.aspx'</script>");
            else
                Response.Write("<script>alert('The length of PSID must be less than 6！');window.location.href='./Query_employee.aspx'</script>");
            return;
        }

        Query qr = new Query();
        DataTable dt = new DataTable();
        //Query by PSID or Name
        if (r.IsMatch(strIn))
        {
            dt = qr.GetEmployeeByPSID(strIn);
        }
        else
        {
            dt = qr.GetEmployeeByName(strIn);
        }

        //不存在的查询
        if (dt.Rows.Count == 0)
        {
            if ( Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('" + TextBox1.Text.Trim() + " 不存在！');window.location.href='./Borrow.aspx'</script>");
            else
            {
                Response.Write("<script>alert('" + TextBox1.Text.Trim() + " does not exist！');window.location.href='./Borrow.aspx'</script>");
            }
            return;
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    //借书操作
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey keys = GridView1.DataKeys[e.RowIndex];
        string psid = keys.Value.ToString();
        Operation op = new Operation();
        //达到最大借阅量，借阅失败
        string employeeName = op.PSIDToName(psid);
        if(!op.Borrow(psid,Session["BID"].ToString()))
        {
            if (Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('借阅失败！" + employeeName + " 已经达到最大借阅量！');window.location.href='./Query_employee.aspx'</script>");
            else
                Response.Write("<script>alert('Borrow failed！"+employeeName+" already reach the MaxBorrowNumber！');window.location.href='./Query_employee.aspx'</script>");
        }
        //借阅成功
        else
        {
            if (Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('借阅成功！" + employeeName + " 还可以继续借阅 " + op.CanBorrowNum(psid) + " books.');window.location.href='./Query_employee.aspx'</script>");
            else
                Response.Write("<script>alert('Borrow success！" + employeeName + " still can borrow " + op.CanBorrowNum(psid) + " books.');window.location.href='./Query_employee.aspx'</script>");
        }
    }
}