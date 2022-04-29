using MimeKit;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Untility;

namespace ConsumableMonitor
{

    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class WeeklyJob : IJob
    {
        /// <summary>
        /// 作业调度定时执行的方法,每周执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            var ConsumableCount1 = Convert.ToInt32(context.JobDetail.JobDataMap["ConsumableCount1"]);
            context.JobDetail.JobDataMap["ConsumableCount1"] = (ConsumableCount1 + 1).ToString();

            //SavePortData($"execute {ConsumableCount1} ConsumableCount1");
            //Log4NetHelper.Instance.Info($"execute {ConsumableCount1} ConsumableCount1");


            await Console.Out.WriteLineAsync($"execute {ConsumableCount1} times");

            //调用Get方法
            string result = GetDataByWebApi.RequestData("http://localhost:2020/server/api/consumables/getconsumablesbyminstock");
            //根据request.ContentType = "application/json"知道返回的是json字符串,下面可以将json字符串序列化为对象
            
            JObject jobject = JObject.Parse(result);
            int count = jobject["meta"]["Count"].Value<int>();
            List<Consumable> listConsumable = JsonConvert.DeserializeObject<List<Consumable>>(jobject["consumables"].ToString());
            Meta meta = JsonConvert.DeserializeObject<Meta>(jobject["meta"].ToString());

            await Console.Out.WriteLineAsync($"Count：{count}");
            await Console.Out.WriteLineAsync($"Count：{meta.Count}");
            await Console.Out.WriteLineAsync($"Count：{listConsumable.Count}");

            if (meta.Count == 0)
            {
                await Console.Out.WriteLineAsync($"没有需补货的库存。");
                return;
            }




            //用于支持gb2312         
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //设置保存数据日期目录名称
            string SavefileTime = Environment.CurrentDirectory + "/Attachments/";
            if (!Directory.Exists(SavefileTime))
                Directory.CreateDirectory(SavefileTime);
            string Time = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");//获取时间24H
            string attachmentName = "/Attachments/" + Time + ".xls";
            string fileName = Environment.CurrentDirectory + attachmentName;

            //创建一个空表
            DataTable dt = new DataTable();
            dt = ExcelHelper.ToDataTable<Consumable>(listConsumable);

            /*删除列（通过列名称）*/
            dt.Columns.Remove("Creater");
            dt.Columns.Remove("CreatedDate");
            dt.Columns.Remove("Modifier");
            dt.Columns.Remove("ModifiedDate");

            //DataTable的列名和excel的列名对应字典，因为excel的列名一般是中文的，DataTable的列名是英文的，字典主要是存储excel和DataTable列明的对应关系，当然我们也可以把这个对应关系存在配置文件或者其他地方
            Dictionary<string, string> dir = new Dictionary<string, string>();
            dir.Add("Id", "编号");
            dir.Add("ConsumableCategory", "类别");
            dir.Add("Name", "名称");
            dir.Add("SuppierName", "供应商");
            dir.Add("Brand", "品牌");
            dir.Add("Specification", "规格");
            dir.Add("Unit", "单位");
            dir.Add("SupplierNumber", "供应商编码");
            dir.Add("DeliveryDay", "货期");
            dir.Add("Warehousing", "入库");
            dir.Add("Delivery", "出库");
            dir.Add("Stock", "库存");
            dir.Add("MinStock", "最低库存");
            dir.Add("State", "状态");
            dir.Add("Description", "备注");

            //使用helper类导出DataTable数据到excel表格中,参数依次是 （DataTable数据源;  excel表名;  excel存放位置的绝对路径; 列名对应字典; 是否清空以前的数据，设置为false，表示内容追加; 每个sheet放的数据条数,如果超过该条数就会新建一个sheet存储）
            ExcelHelper.ExportDTtoExcel(dt, "耗材表", fileName, dir, false, 500);



            string mailMsg = "";
            IList<MailboxAddress> toAddressList = new List<MailboxAddress>();   // 收件人邮箱列表
            IList<MailboxAddress> ccAddressList = new List<MailboxAddress>();   // 抄送人邮箱列表

            toAddressList.Add(new MailboxAddress("耗材管理员", "1981250700@qq.com"));      //添加接收人
            ccAddressList.Add(new MailboxAddress("抄送人", "yuhoufu@mthorizon.com"));  //添加抄送人

            string title = "耗材库存-周提醒";                            //主题
            string content = $"<div>你好，耗材有<strong>{listConsumable.Count}</strong>项需补货，详情见附件！</div>";   //内容
            string attachmentPathAndName = null;
            FileInfo file = new FileInfo(fileName);
            //检查附件存不存在
            if (file.Exists)
            {
                attachmentPathAndName = attachmentName;
            }


            MimeMessage message = EmailHelper.CreateTextMessage(toAddressList, ccAddressList, title, content, attachmentPathAndName);
            mailMsg = EmailHelper.SendEmail(message);


            
            await Console.Out.WriteLineAsync($"mail：{mailMsg}");
            await Task.CompletedTask;
        }
        

    }
}