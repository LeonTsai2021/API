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
using System.Data.Entity.Validation;

namespace Greeny.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]//把api應用到所有控制器
    public class PracticeController : ControllerBase
    {
        private readonly UserDbContext ctx;

        public PracticeController(UserDbContext ctx)
        {
            this.ctx = ctx;
        }

        /// <summary>
        /// 取得所有
        /// </summary>
        [AllowAnonymous]//用於跳過授權，方便開發debug?
        [HttpGet]
        public IActionResult GetItems()
        {
            APIResponse<List<tblitem>> res = new APIResponse<List<tblitem>>();//型別被改為class tblitem

            try
            {
                // tolist (列出所有)
                var items = ctx.tblitems.ToList();

                res.result = APIResultCode.success;
                res.msg = "success";
                res.data = items;//data因轉換為class tblitem型別，存取其資料
                return Ok(res);
            }
            catch (Exception err)
            {
                res.result = APIResultCode.unknow_error;
                res.msg = err.Message;
                return Ok(res);
            }
        }

        /// <summary>
        /// 取得一個
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{Id}")]
        public IActionResult GetItem(int Id)
        {
            APIResponse<tblitem> res = new APIResponse<tblitem>();

            try
            {
                // where (查詢指定欄位)
                var item = ctx.tblitems.Where(x=>x.Id == Id).FirstOrDefault();

                res.result = APIResultCode.success;
                res.msg = "success";
                res.data = item;
                return Ok(res);
            }
            catch (Exception err)
            {
                res.result = APIResultCode.unknow_error;
                res.msg = err.Message;
                return Ok(res);
            }
        }
        /// <summary>
        /// 新增一個
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult PostItem(tblitem data)
        {
            APIResponse<tblitem> res = new APIResponse<tblitem>();

            try
            {
                var item = new tblitem()
                {
                    Id = data.Id,
                    ItemName = data.ItemName,
                    ItemDetail = data.ItemDetail,
                    DateTime = data.DateTime
                };
                ctx.tblitems.Add(item);
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
        /// <summary>
        /// 更新一個
        /// </summary>
        [AllowAnonymous]
        [HttpPut("{Id}")]
        public IActionResult PutItem(int Id, tblitem data)
        {
            APIResponse<tblitem> res = new APIResponse<tblitem>();

            try
            {
                // where (查詢指定欄位)
                var item = ctx.tblitems.Where(x => x.Id == Id).FirstOrDefault();
                // 更新資料
                item.ItemName = data.ItemName;
                item.ItemDetail = data.ItemDetail;
                item.DateTime = data.DateTime;
                // 回存
                ctx.SaveChanges();

                res.result = APIResultCode.success;
                res.msg = "success";
                res.data = item;
                return Ok(res);
            }
            catch (Exception err)
            {
                res.result = APIResultCode.unknow_error;
                res.msg = err.Message;
                return Ok(res);
            }
        }
        /// <summary>
        /// 刪除一個
        /// </summary>
        [AllowAnonymous]
        [HttpDelete("{Id}")]
        public IActionResult DeleteItem(int Id)
        {
            APIResponse<tblitem> res = new APIResponse<tblitem>();

            try
            {
                // where (查詢指定欄位)
                var item = ctx.tblitems.Where(x => x.Id == Id).FirstOrDefault();

                // 刪除資料
                ctx.Remove(item);
                // 回存
                ctx.SaveChanges();

                res.result = APIResultCode.success;
                res.msg = "success";
                res.data = item;
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

