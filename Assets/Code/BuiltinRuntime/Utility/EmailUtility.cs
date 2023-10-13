using System;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 邮箱工具
    /// </summary>
    public static class EmailUtility
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="senderEmail">发送者邮箱</param>
        /// <param name="recipients">接收者邮箱</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="codeKey">邮箱授权码</param>
        /// <param name="CallCompelent">完成时回调</param>
        /// <param name="exceptionAction">异常事件</param>
        public static void SendEmail(string senderEmail , string recipients , string title , string content , string codeKey , RemoteCertificateValidationCallback CallCompelent = null , Action<Exception> exceptionAction = null)
        {
            try
            {
                MailMessage mail = new MailMessage( );
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(recipients);
                mail.Subject = title;
                mail.Body = content;
                string temp = senderEmail.Split('@')[1];
                SmtpClient smtpClient = new SmtpClient($"smtp.{temp}");
                smtpClient.Credentials = new NetworkCredential(senderEmail , codeKey);
                //启用ssl安全发送
                smtpClient.EnableSsl = true;
                //TODO:回调
                ServicePointManager.ServerCertificateValidationCallback = CallCompelent;
                smtpClient.Send(mail);
            }
            catch(Exception e)
            {
                exceptionAction?.Invoke(e);
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="senderEmail">发送者邮箱</param>
        /// <param name="recipients">接收者邮箱</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="accessory">附件路径</param>
        /// <param name="codeKey">邮箱授权码</param>
        /// <param name="CallCompelent">完成时回调</param>
        /// <param name="exceptionAction">异常事件</param>
        public static void SendEmail(string senderEmail , string recipients , string title , string content , string accessory , string codeKey , RemoteCertificateValidationCallback CallCompelent = null , Action<Exception> exceptionAction = null)
        {
            try
            {
                MailMessage mail = new MailMessage( );
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(recipients);
                mail.Subject = title;
                mail.Body = content;
                Attachment att = new Attachment(accessory);
                mail.Attachments.Add(att);
                string temp = senderEmail.Split('@')[1];
                SmtpClient smtpClient = new SmtpClient($"smtp.{temp}");
                smtpClient.Credentials = new NetworkCredential(senderEmail , codeKey);
                smtpClient.EnableSsl = true;
                //TODO:回调
                ServicePointManager.ServerCertificateValidationCallback = CallCompelent;

                smtpClient.Send(mail);
            }
            catch(Exception e)
            {
                exceptionAction?.Invoke(e);
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="senderEmail">发送者邮箱</param>
        /// <param name="recipients">接收者邮箱</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="codeKey">邮箱授权码</param>
        /// <param name="CallCompelent">完成时回调</param>
        /// <param name="exceptionAction">异常事件</param>
        public static void SendEmail(string senderEmail , string[] recipients , string title , string content , string codeKey , RemoteCertificateValidationCallback CallCompelent = null , Action<Exception> exceptionAction = null)
        {
            try
            {
                MailMessage mail = new MailMessage( );
                mail.From = new MailAddress(senderEmail);
                foreach(string rec in recipients)
                {
                    mail.To.Add(rec);
                }
                mail.Subject = title;
                mail.Body = content;
                string temp = senderEmail.Split('@')[1];
                SmtpClient smtpClient = new SmtpClient($"smtp.{temp}");
                smtpClient.Credentials = new NetworkCredential(senderEmail , codeKey);
                smtpClient.EnableSsl = true;
                //TODO:回调
                ServicePointManager.ServerCertificateValidationCallback = CallCompelent;
                smtpClient.Send(mail);
            }
            catch(Exception e)
            {
                exceptionAction?.Invoke(e);
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="senderEmail">发送者邮箱</param>
        /// <param name="recipients">接收者邮箱</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="accessory">附件路径</param>
        /// <param name="codeKey">邮箱授权码</param>
        /// <param name="CallCompelent">完成时回调</param>
        /// <param name="exceptionAction">异常事件</param>
        public static void SendEmail(string senderEmail , string[] recipients , string title , string content , string accessory , string codeKey , RemoteCertificateValidationCallback CallCompelent = null , Action<Exception> exceptionAction = null)
        {
            try
            {
                MailMessage mail = new MailMessage( );
                mail.From = new MailAddress(senderEmail);
                foreach(string rec in recipients)
                {
                    mail.To.Add(rec);
                }
                mail.Subject = title;
                mail.Body = content;
                Attachment att = new Attachment(accessory);
                mail.Attachments.Add(att);
                string temp = senderEmail.Split('@')[1];
                SmtpClient smtpClient = new SmtpClient($"smtp.{temp}");
                smtpClient.Credentials = new NetworkCredential(senderEmail , codeKey);
                smtpClient.EnableSsl = true;

                //TODO:回调
                ServicePointManager.ServerCertificateValidationCallback = CallCompelent;

                smtpClient.Send(mail);
            }
            catch(Exception e)
            {
                exceptionAction?.Invoke(e);
            }
        }
    }
}
