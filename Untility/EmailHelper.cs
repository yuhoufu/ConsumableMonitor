using System;
using System.Collections.Generic;
using MimeKit;
using MailKit.Security;
using System.IO;

namespace Untility
{

    /// <summary>
    /// EmailHelper类是专门提供给用于Email的发送
    /// </summary>
    public static class EmailHelper
    {


        /// <summary>
        /// Smtp服务器地址
        /// </summary>
        private static readonly string SmtpServer = "smtphz.qiye.163.com";

        /// <summary>
        /// SMTP端口
        /// </summary>
        private static readonly int SmtpPort = Convert.ToInt32("465");


        /// <summary>
        /// 默认发件人
        /// </summary> 
        private static MailboxAddress fromAddress = new MailboxAddress("实验室管理系统", "lims@mthorizon.com");

        /// <summary>
        /// 默认发件邮箱
        /// </summary> 
        public static string FROMMAIL = "lims@mthorizon.com";


        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="mailFromAccount">发送邮箱账号</param>
        /// <param name="mailPassword">发送邮箱密码</param>
        /// <param name="message">邮件</param>
        public static void SendEmail(string mailFromAccount, string mailPassword, MimeMessage message)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(SmtpServer, SmtpPort, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(mailFromAccount, mailPassword);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public static async void SendEmailAsync(MimeMessage message)
        {

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(SmtpServer, SmtpPort, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync("lims@mthorizon.com", "Cont0708");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }

        public static string SendEmail(MimeMessage message)
        {
            string msg = "";


            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {

                client.MessageSent += (sender, args) =>
                {
                    msg = args.Response.ToString();
                };

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(SmtpServer, SmtpPort, SecureSocketOptions.Auto);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("lims@mthorizon.com", "Cont0708");
                client.Send(message);
                client.Disconnect(true);
            }
            return msg;

        }
        /// <summary>
        /// 创建文本消息
        /// </summary>
        /// <param name="fromAddress">发件地址</param>
        /// <param name="toAddressList">收件地址</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="IsPostFiles">是否将POST上传文件加为附件</param>
        /// <returns></returns>
        public static MimeMessage CreateTextMessage(MailboxAddress fromAddress, IList<MailboxAddress> toAddressList, IList<MailboxAddress> ccAddressList
            , string title, string content, bool IsPostFiles = false)
        {
            var message = new MimeMessage();
            message.From.Add(fromAddress);
            message.To.AddRange(toAddressList);
            message.Cc.AddRange(ccAddressList);
            message.Subject = title; //设置消息的主题
            message.Body = new TextPart("plain")
            {
                Text = content,
            };
            return message;
        }

        /// <summary>
        /// 创建文本消息
        /// </summary>
        /// <param name="toAddressList">收件地址</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="relativePathAndFileName">附件的相对路径和文件名称，如：“/attachments/2022-04-29_101031.xls”</param>
        /// <returns></returns>
        public static MimeMessage CreateTextMessage(IList<MailboxAddress> toAddressList, IList<MailboxAddress> ccAddressList
            , string title, string content, string relativePathAndFileName = null)
        {
            var message = new MimeMessage();
            message.From.Add(fromAddress);
            message.To.AddRange(toAddressList);
            message.Cc.AddRange(ccAddressList);
            message.Subject = title; //设置消息的主题

            //message.Body = new TextPart("plain")
            //message.Body = new TextPart("html")
            //{
            //    Text = content,
            //};

            var body = new TextPart("html")
            {
                Text = content,
            };

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            multipart.Add(body);

            if (relativePathAndFileName != null) // 有附件文件需要发送时。
            {
                string path = Environment.CurrentDirectory + relativePathAndFileName;
                // create an image attachment for the file located at path
                var attachment = new MimePart()
                {
                    Content = new MimeContent(File.OpenRead(path), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = Path.GetFileName(path)
                };
                multipart.Add(attachment);

            }

            // now set the multipart/mixed as the message body
            message.Body = multipart;



            return message;
        }


    }
}
