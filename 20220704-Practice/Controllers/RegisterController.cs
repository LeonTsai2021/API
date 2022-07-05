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

namespace Greeny.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserDbContext ctx; //宣告已建立的資料庫物件

        public RegisterController(UserDbContext ctx)
        {
            this.ctx = ctx;
        }

        /// <summary>
        /// 使用者註冊
        /// </summary>
        [AllowAnonymous]
        [HttpPost("Signup")] //輸入資料所以用POST
        public IActionResult SignUp(SignUpModel data)
        {
            APIResponse res = new APIResponse();

            try
            {
                // 1. 檢查輸入參數(帳號密碼欄位是否為空)
                if (string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
                {
                    res.result = APIResultCode.missing_parameter;
                    res.msg = "欄位不可為空";
                    return Ok(res);
                }

                // 2. 檢查使用者是否存在
                var signUpModel = ctx.SignUpModel.Where(x => x.Username == data.Username).FirstOrDefault();//提取資料庫的"username"來判斷使用者名稱是否重複
                if (signUpModel != null)
                { 
                    res.result = APIResultCode.rqmt_not_found;
                    res.msg = "錯誤!使用者名稱已重複";
                    return Ok(res);
                }

                // 3. 加入資料庫
                // 3.1 new 一個使用者物件
                signUpModel = new SignUpModel()
                {
                    Username = data.Username,
                    Password = data.Password
                };
                // 3.2 新增 使用者物件 到 資料庫
                ctx.SignUpModel.Add(signUpModel);
                // 3.3 儲存 資料庫
                ctx.SaveChanges();

                res.result = APIResultCode.success;
                res.msg = "success";
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

