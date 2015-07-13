using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// BusinessHandlerLayer 的摘要说明
/// </summary>
public class BusinessHandlerLayer
{
	public BusinessHandlerLayer()
	{
	}

    //借书
    //传入参数为ID（辅助函数将name，title信息转换为ID）
    //返回是否借阅成功
    //之后配套函数需要执行，返回employee可借数量
    public bool Borrow(string PSID, string BID)
    {
        return true;
    }

    //还书
    //同借书后三条
    public bool Return(string PSID, string BID)
    {
        return true;
    }



}