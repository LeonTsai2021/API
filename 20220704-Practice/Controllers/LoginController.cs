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
using _20220704_Practice.Helper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _20220704_Practice.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserDbContext _ctx;
        private readonly JwtHelpers _jwt;
//
        // 摘要:
        //     Creates a new Microsoft.AspNetCore.Mvc.HttpPostAttribute with the given route
        //     template.
        //
        // 參數:
        //   template:
        //     The route template. May not be null.
        public LoginController(UserDbContext ctx, JwtHelpers jwt)
        {
            _ctx = ctx;
            _jwt = jwt;
        }
        // GET: api/<ValuesController>
        [AllowAnonymous]
        [HttpPost("Login")]//輸入資料做驗證，所以用POST

        public IActionResult Login(SignUpModel data)
        {
            APIResponse<string> res = new APIResponse<string>();
            
            try
            {   //1.檢查是否有欄位沒有輸入
                if (string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
                {
                    res.result = APIResultCode.missing_parameter;
                    res.msg = "欄位不可為空";
                    return Ok(res);
                }

                //2.檢查使用者是否輸入錯誤
                SignUpModel input = _ctx.SignUpModel.Where(x => x.Username == data.Username).FirstOrDefault();
                if (input.Username == null || input.Password==null)
                {
                        res.result = APIResultCode.username_or_password_not_correct;
                        res.msg = "錯誤!名稱或密碼有誤";
                        return Ok(res);

                }


                res.result = APIResultCode.success;
                res.msg = "success";
                res.data = _jwt.GenerateToken(data.Username);

                return Ok(res); //給予user的jwt驗證
            }
            catch (Exception err)
            {
                res.result = APIResultCode.unknow_error;
                res.msg = err.Message;
                return Ok(res);
            }
        }
        [Authorize]
        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        [Authorize]
        [HttpGet("username")]
        public IActionResult GetUserName()
        {
            return Ok(User.Identity.Name);
        }

        [HttpGet("jwtid")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }


    }
}
