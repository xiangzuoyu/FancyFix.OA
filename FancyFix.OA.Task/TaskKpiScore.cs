using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Task
{
    public class TaskKpiScore
    {
        public void Run()
        {
            var date = DateTime.Now.AddMonths(-1);
            Tools.Tool.Log.WritePure($"------------------------------------------------------");
            Tools.Tool.Log.WritePure($"开始统计{date.Year}年{date.Month}月每个员工的的KPI得分");
            Tools.Tool.Log.WritePure($"------------------------------------------------------");

            try
            {
                int lastYear = date.Year;
                int lastMonth = date.Month;
                var userlist = Bll.BllMng_User.GetAllUser();
                foreach (var user in userlist)
                {
                    //先查找进程
                    var process = Bll.BllKpi_Process.First(o => o.UserId == user.Id && o.Year == lastYear && o.Month == lastMonth);
                    if (process != null)
                    {
                        int score = 0;
                        int selfscore = 0;
                        var recordlist = Bll.BllKpi_Records.GetListByUserId(user.Id, lastYear, lastMonth);
                        if (recordlist != null && recordlist.Count > 0)
                        {
                            foreach (var record in recordlist)
                            {
                                //统计上级评分
                                if (record.IsApprove.HasValue && record.IsApprove.Value)
                                {
                                    score += (int)((record.ParScore * record.Score) / 100);
                                }
                                //统计自评分
                                if (record.IsSelfApprove.HasValue && record.IsSelfApprove.Value)
                                {
                                    selfscore += (int)((record.SelfScore * record.Score) / 100);
                                }
                            }
                        }
                        else
                        {
                            //进程下没有任务，则把进程设为未生成
                            process.IsCreated = false;
                            process.IsApprove = false;
                        }

                        process.Score = score;
                        process.SelfScore = selfscore;
                        int row = Bll.BllKpi_Process.Update(process);
                        Tools.Tool.Log.WritePure($"【统计】{user.RealName}-{date.Year}年{date.Month}月的进程分数{(row > 0 ? "成功" : "失败")}!");
                    }
                    else
                    {
                        //没有该进程就默认帮用户创建一个
                        int id = Bll.BllKpi_Process.Insert(new Model.Kpi_Process()
                        {
                            UserId = user.Id,
                            Score = 0,
                            Year = lastYear,
                            Month = lastMonth,
                            IsApprove = false,
                            Remark = "",
                            IsCreated = false,
                            SelfScore = 0,
                        });
                        Tools.Tool.Log.WritePure($"【新增】{user.RealName}-{date.Year}年{date.Month}月的进程{(id > 0 ? "成功" : "失败")}!");
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Tool.Log.WritePure($"任务失败：{ex.ToString()}");
            }
        }
    }
}
