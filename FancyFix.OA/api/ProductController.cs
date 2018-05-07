using FancyFix.OA.Base;
using FancyFix.OA.Model;
using FancyFix.Tools.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FancyFix.OA.api
{
    public class ProductController : BaseApiController
    {
        /// <summary>
        /// 获取产品属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProAttr(int id)
        {
            ResultJson result = new ResultJson();
            Product_Class model = Bll.BllProduct_Class.First(o => o.Id == id);
            if (id == 0 || (model == null || model.ChildNum > 0))
            {
                result.state = 0;
                result.error = "无效分类";
                return Json(result);
            }
            else
            {
                result.state = 1;
                result.attr = new List<Attr>();
                Bll.BllProduct_Attribute.LisAttr(id, result.attr);
                return Json(result);
            }
        }

        /// <summary>
        /// 验证产品sku是否存在
        /// </summary>
        /// <param name="prono"></param>
        /// <returns></returns>
        [HttpPost]
        public bool CheckSku([FromBody]string sku)
        {
            return !Bll.BllProduct_Info.Any(o => o.Sku == sku.Trim());
        }

        /// <summary>
        /// 获取产品分类下拉框
        /// </summary>
        /// <param name="parId"></param>
        /// <param name="selectId"></param>
        /// <returns></returns>
        [HttpGet]
        public string ShowClass(int parId, int selectId)
        {
            return Bll.BllProduct_Class.Instance().ShowClass(parId, selectId, true);
        }

        /// <summary>
        /// 获取分类编号
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetClassCode([FromBody]int classId)
        {
            return Bll.BllProduct_Class.GetCode(classId) + "xxx";
        }
    }
}