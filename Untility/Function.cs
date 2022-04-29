using System;
using System.IO;

namespace Untility
{
    class Function
    {

        private static void SavePortData(string Data)
        {
            string fileDirCount = "测试写入文本";
            string Date = DateTime.Now.ToString("yyyy-MM-dd");//获取日期
            string Time = DateTime.Now.ToString("yyyy年MM月dd日HH时");//获取时间24H
                                                                  //获取当前运行程序的目录
            string fileName = Environment.CurrentDirectory;
            //设置数据保存目录名称
            string SavefileName = fileName + "/SaveData/";
            if (!Directory.Exists(SavefileName))
                Directory.CreateDirectory(SavefileName);
            //设置保存数据日期目录名称
            string SavefileTime = fileName + "/SaveData/" + Date + "//";
            if (!Directory.Exists(SavefileTime))
                Directory.CreateDirectory(SavefileTime);
            //设置保存文本名称
            String SaveDirName = fileName + "/SaveData/" + Date + "//" + Time + fileDirCount + ".txt ";
            FileInfo file = new FileInfo(SaveDirName);
            StreamWriter sw = file.AppendText();
            //检查文本存不存在
            if (!file.Exists)
            {
                //新建一个新的文本
                FileStream fs = file.Create();
                fs.Close();
                fs.Dispose();
            }
            sw.WriteLine(Data);
            sw.Flush();
            sw.Close();
        }


       
    }
}
