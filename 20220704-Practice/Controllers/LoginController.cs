using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using _20220704_Practice.Models;
using Greeny;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _20220704_Practice.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserDbContext _ctx;
//
        // 摘要:
        //     Creates a new Microsoft.AspNetCore.Mvc.HttpPostAttribute with the given route
        //     template.
        //
        // 參數:
        //   template:
        //     The route template. May not be null.
        
        public LoginController(UserDbContext ctx)
        {
            _ctx = ctx;
        }
        // GET: api/<ValuesController>
        [HttpPost("Login")]//輸入資料做驗證，所以用POST

        public IActionResult Login(SignUpModel data)
        {
            APIResponse res = new APIResponse();

            try
            {   //1.檢查是否有欄位沒有輸入
                if (string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
                {
                    res.result = APIResultCode.missing_parameter;
                    res.msg = "欄位不可為空";
                    return Ok(res);
                }

                //2.檢查使用者是否輸入錯誤
                var inputName = _ctx.SignUpModel.Where(x => x.Username == data.Username).FirstOrDefault();
                var inputPassword = _ctx.SignUpModel.Where(x => x.Password == data.Password).FirstOrDefault();
                if (inputName == null || inputPassword == null)
                {
                    res.result = APIResultCode.username_or_password_not_correct;
                    res.msg = "錯誤!名稱或密碼有誤";
                    return Ok(res);
                }

                res.result = APIResultCode.success;
                res.msg = "登入成功";
                return Ok(res);
            }
            catch (Exception err)
            {
                res.result = APIResultCode.unknow_error;
                res.msg = err.Message;
                return Ok(res);
            }

        }
        
    }
}
