using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;

public partial class SiteMaster : MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string s;
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
}