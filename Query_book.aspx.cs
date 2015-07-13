using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

public partial class Return : System.Web.UI.Page
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
        //
        Regex r=new Regex(@"\d{1,6}");
        if(r.IsMatch(strIn) & strIn.Length!=6)
        {
            if (Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('BID的长度必须是6位！');window.location.href='./Query_book.aspx'</script>");
            else
                Response.Write("<script>alert('The length of BID must be 6！');window.location.href='./Query_book.aspx'</script>");
            return;
        }

        Query qr = new Query();
        DataTable dtBorrowed = new DataTable();
        DataTable dtUnborrowed = new DataTable();
        DataTable dtBookExist = new DataTable();
        //按BID or Title Query
        if (r.IsMatch(strIn))
        {
            dtBorrowed = qr.QueryByBIDBorrowed(strIn);
            dtUnborrowed = qr.QueryByBIDUnborrowed(strIn);
            dtBookExist = qr.GetBookByBID(strIn);
        }
        else
        {
            dtBorrowed = qr.QueryByTitleBorrowed(strIn);
            dtUnborrowed = qr.QueryByTitleUnborrowed(strIn);
            dtBookExist = qr.GetBookByTitle(strIn);
        }
        //书籍不存在
        if (dtBookExist.Rows.Count == 0)
        {
            if (Session["lang"].ToString() == "zh-cn")
                Response.Write("<script>alert('" + TextBox1.Text.Trim() + " 不存在！');window.location.href='./Query_book.aspx'</script>");
            else
                Response.Write("<script>alert('" + TextBox1.Text.Trim() + " does not exist！');window.location.href='./Query_book.aspx'</script>");
            return;
        }

        GridView1.DataSource = dtBorrowed;
        GridView1.DataBind();
        GridView2.DataSource = dtUnborrowed;
        GridView2.DataBind();
    }

    //查询书籍
    //还书操作
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey keys = GridView1.DataKeys[e.RowIndex];
        string bid = keys.Value.ToString();
        //还书
        Operation op = new Operation();
        Query qr = new Query();
        string psid = qr.GetPSIDByBID(bid);
        string employeeName = op.PSIDToName(psid);
        op.Return(bid);
        int StillCanBorrow = op.CanBorrowNum(psid);
        TextBox1.Text = "";
        if (Session["lang"].ToString() == "zh-cn")
            Response.Write("<script>alert('还书成功！" + employeeName + " 仍然可以借阅 " + StillCanBorrow + " books.');window.location.href='./Query_employee.aspx'</script>");
        else
            Response.Write("<script>alert('Return success！"+employeeName+" still can borrow " + StillCanBorrow + " books.');window.location.href='./Query_employee.aspx'</script>");
    }
    //借书操作
    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey keys = GridView2.DataKeys[e.RowIndex];
        Session["BID"] = keys.Value.ToString();
        Response.Redirect("./Borrow.aspx");
    }
}