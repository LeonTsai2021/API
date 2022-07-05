using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greeny
{
    public enum APIResultCode
    {
        success = 1,
        unknow_error = 0,
        missing_parameter = 2,
        rqmt_not_found = 3,
        username_or_password_not_correct=4
    }

    public class APIResponse
    {
        public APIResultCode result { set; get; }
        public string msg { set; get; }
    }

    public class APIResponse<T>
    {
        public APIResultCode result { set; get; }
        public string msg { set; get; }

        public T data { set; get; }//data可以存任何被後宣告的型別
    }



}
