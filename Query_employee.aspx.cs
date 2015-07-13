using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlServerCe;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

public partial class Query_employee : System.Web.UI.Page
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

    //查询用户
    protected void Button1_Click(object sender, EventArgs e)
    {
        //输入异常检查
        string strIn = TextBox1.Text.Trim();
        //有点问题，数字挂字母？？
        Regex r=new Regex(@"\d{1,100}");
        if(r.IsMatch(strIn) & (strIn.Length<5|strIn.Length>6))
        {
            if (Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('PSID的长度必须是5或者6！');window.location.href='./Query_employee.aspx'</script>");
            else
                Response.Write("<script>alert('The length of PSID must be 5 or 6！');window.location.href='./Query_employee.aspx'</script>");
            return;
        }

        Query qr = new Query();
        DataTable dt = new DataTable();
        DataTable dtEmployeeExist = new DataTable();
        //Query by PSID or Name
        if (r.IsMatch(strIn))
        {
            dt = qr.QueryByPSID(strIn);
            Session["PSID"] = strIn;
            dtEmployeeExist = qr.GetEmployeeByPSID(strIn);
        }
        else
        {
            dt = qr.QueryByName(strIn);
            Operation op = new Operation();
            Session["PSID"] = op.NameToPSID(strIn);
            dtEmployeeExist = qr.GetEmployeeByName(strIn);
        }

        //该employee不存在的处理
        if (dtEmployeeExist.Rows.Count == 0)
        {
            if (Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('" + TextBox1.Text.Trim() + " 不存在！');window.location.href='./Query_employee.aspx'</script>");
            else
                Response.Write("<script>alert('" + TextBox1.Text.Trim() + " does not exist！');window.location.href='./Query_employee.aspx'</script>");
            return;
        }

        //有该employee，但借阅记录为空的处理
        if (dt.Rows.Count == 0)
        {
            if (Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('" + TextBox1.Text.Trim() + " 没有借阅任何图书！');window.location.href='./Query_employee.aspx'</script>");
            else
                Response.Write("<script>alert('" + TextBox1.Text.Trim() + " did not borrow any book！');window.location.href='./Query_employee.aspx'</script>");
            return;
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    //还书
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey keys = GridView1.DataKeys[e.RowIndex];
        string bid = keys.Value.ToString();
        //还书
        Operation op = new Operation();
        op.Return(bid);
        string Employee = op.PSIDToName(Session["PSID"].ToString());
        int StillCanBorrow = op.CanBorrowNum(Session["PSID"].ToString());
        TextBox1.Text = "";
        if (Session["lang"].ToString() == "zh-cn")
            Response.Write("<script>alert('还书成功！" + Employee + " 仍然可以借阅 " + StillCanBorrow + " books.');window.location.href='./Query_employee.aspx'</script>");
        else
            Response.Write("<script>alert('Return success！"+Employee+" still can borrow " + StillCanBorrow + " books.');window.location.href='./Query_employee.aspx'</script>");
    }
}