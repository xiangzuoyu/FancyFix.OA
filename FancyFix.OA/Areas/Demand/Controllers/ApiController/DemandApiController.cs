using FancyFix.Core.Enum;
using FancyFix.OA.Areas.Demand.Models;
using FancyFix.OA.Base;
using FancyFix.OA.Bll.Model;
using FancyFix.OA.Helper;
using FancyFix.OA.Model;
using FancyFix.OA.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FancyFix.OA.Areas.Demand.Controllers
{
    public class DemandApiController : BaseApiController
    {
        [HttpGet]
        public IHttpActionResult PageList(int deptId, int demandType, int page, int pageSize)
        {
            List<DemandModel> resultList = new List<DemandModel>();
            long records = 0;
            int loginUserId = MyInfo.Id;
            DemandSearch demandSearch = new DemandSearch { DeptId = deptId, DemandType = demandType, LoginUserId = loginUserId };
            var adminList = DemandTypeHelper.GetDeamndAdminUserIdList("DemandAdminUserId");
            if (adminList.Contains(loginUserId.ToString()))
            {
                demandSearch.IsAdmin = true;
            }
            var showList = DemandTypeHelper.GetDeamndAdminUserIdList("DemandShowUserId");
            if (showList.Contains(loginUserId.ToString()))
            {
                demandSearch.IsShow = true;
            }
            var list = Bll.BllDevelop_Demand.PageList(demandSearch, page, pageSize, out records);
            try
            {
                if (list != null && list.Count > 0)
                {
                    var userIdList = new List<int>();
                    userIdList.AddRange(list.Where(p => p.CreateUserId > 0).Select(p => p.CreateUserId).Distinct());
                    userIdList.AddRange(list.Where(p => p.ExecutorId.HasValue && p.ExecutorId > 0).Select(p => p.ExecutorId.Value).Distinct());
                    var userList = Bll.BllMng_User.GetListByIds(userIdList);
                    resultList = list.MapperConvert<List<DemandModel>>();
                    var deptList = DepartmentData.GetList();
                    foreach (var item in resultList)
                    {
                        item.StatusStr = ((Demand_StatusEnum)item.Status).ToString();
                        item.TypeStr = ((Demand_TypeEnum)item.Type).ToString();
                        if (userList != null)
                        {
                            var createUser = userList.Where(p => p.Id == item.CreateUserId).FirstOrDefault();
                            if (createUser != null)
                            {
                                item.CreateUser = createUser.RealName;
                            }
                            var executorUser = userList.Where(p => p.Id == item.ExecutorId).FirstOrDefault();
                            if (executorUser != null)
                            {
                                item.ExecutorUser = executorUser.RealName;
                            }
                        }
                        if (deptList != null)
                        {
                            var dept = deptList.Where(p => p.Id == item.DeptId).FirstOrDefault();
                            if (dept != null)
                            {
                                item.DeptName = dept.ClassName;
                            }
                        }

                        item.IsAdmin = demandSearch.IsAdmin;
                        item.IsShow = demandSearch.IsShow;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return Ok(new { rows = resultList, total = records });
        }
        [HttpPost]
        public IHttpActionResult Save(JObject obj)
        {
            var model = obj["model"];
            var headdata = obj["headdata"];
            if (model == null || headdata == null)
            {
                return ReturnResult(false, "参数有误");
            }
            var developDemand = JsonConvert.DeserializeObject<Develop_Demand>(headdata.ToString());
            var demandTypeModel = JsonConvert.DeserializeObject<DemandTypeModel>(model.ToString());
            if (developDemand == null || demandTypeModel == null)
            {
                return ReturnResult(false, "实体转换有误");
            }
            if (developDemand.EstimateCompleteTime < DateTime.Today)
            {
                return ReturnResult(false, "预计完成时间不能小于当前日期");
            }
            //if (string.IsNullOrWhiteSpace(developDemand.AffixAddress))
            //{
            //    return ReturnResult(false, "请上传附件");
            //}
            developDemand.Content = JsonConvert.SerializeObject(demandTypeModel);
            try
            {
                if (developDemand != null && developDemand.Id > 0)
                {
                    developDemand.UpdateTime = DateTime.Now;
                    developDemand.AffixAddress = developDemand.AffixAddress ?? "";
                    if (Bll.BllDevelop_Demand.Update(developDemand) < 0)
                    {
                        return ReturnResult(false, "操作失败");
                    }
                }
                else
                {
                    developDemand.Status = (int)Demand_StatusEnum.未完成;
                    developDemand.CreateTime = DateTime.Now;
                    developDemand.MissionNumber = DateTime.Now.ToString("yyyyMMddHHssmm");
                    developDemand.CreateUserId = MyInfo.Id;
                    if (Bll.BllDevelop_Demand.Insert(developDemand) < 0)
                    {
                        return ReturnResult(false, "操作失败");
                    }
                }
            }
            catch (Exception ex)
            {
                return ReturnResult(false, ex.Message);
            }

            return ReturnResult();
        }
        /// <summary>
        /// 设置状态为完成
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CompleteDemand(int id)
        {
            if (id == 0)
            {
                return ReturnResult(false, "id参数有误");
            }
            try
            {
                if (!Bll.BllDevelop_Demand.SetStatus(id, (int)Demand_StatusEnum.完成))
                {
                    return ReturnResult(false, "操作失败");
                }
            }
            catch (Exception ex)
            {

                return ReturnResult(false, ex.Message);
            }
            return ReturnResult();
        }
        /// <summary>
        /// 设置执行人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="executorId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExecutorDemand(JObject obj)
        {
            var id = Convert.ToInt32(obj["id"]);
            var executorId = Convert.ToInt32(obj["executorId"]);
            if (id == 0)
            {
                return ReturnResult(false, "id参数有误");
            }
            if (executorId == 0)
            {
                return ReturnResult(false, "executorId参数有误");
            }
            try
            {
                if (!Bll.BllDevelop_Demand.SetExecutorId(id, executorId))
                {
                    return ReturnResult(false, "操作失败");
                }
            }
            catch (Exception ex)
            {

                return ReturnResult(false, ex.Message);
            }
            return ReturnResult();
        }
    }
}
