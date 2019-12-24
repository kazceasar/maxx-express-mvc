using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Validation;
using System.Net.Mail;

namespace MaxxExpress.Controllers
{
    public class ContactController : ApiController
    {
        public HttpResponseMessage Post([FromBody] tbl_web_contact_form contact)
        {
            try
            {
                using (cascadiaEntities entities = new cascadiaEntities())
                {
                    entities.tbl_web_contact_form.Add(contact);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, contact);
                    SendEmail();
                    message.Headers.Location = new Uri(Request.RequestUri + contact.ID.ToString());
                    return message;
                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public static void SendEmail()
        {
            using (cascadiaEntities cascadia_db = new cascadiaEntities())
            {
                string Name;
                string Phone;
                string Email;
                string Message;
                string mailBody;
                string to = "maxxexpresstrucks@gmail.com";
                string from = "web@maxx-express.com";
                MailMessage message = new MailMessage(from, to);
                message.Subject = "Maxx Express web contact form inquiry";
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("maxxexpress-com01b.mail.protection.outlook.com");
                client.UseDefaultCredentials = false;
                client.Port = 25;
                client.Host = "relay-hosting.secureserver.net";           
                client.Credentials = new System.Net.NetworkCredential("web@maxx-express.com", "BlueExpress13!");
                client.EnableSsl = client.Port == 587;

                var IND = '0';
                string IND_x = IND.ToString();
                var cascadia = cascadia_db.tbl_web_contact_form.FirstOrDefault(e => e.MailSent == IND_x);
                Name = cascadia.Name.ToString();
                Phone = cascadia.Phone.ToString();
                Email = cascadia.Email.ToString();
                Message = cascadia.Message.ToString();

                mailBody =
                    "<html>" +
                     "<head>" +
                       "<style>" +
                       "@import url(https://fonts.googleapis.com/css?family=Fjalla+One|Oxygen);" +
                       "th, td {padding: 15px;text-align: left; border: 1px solid #ddd;}" +
                       ".bold {font-weight:bold} " +
                       ".left-text {text-align:left} " +
                       ".headers {font-family: 'Fjalla One', sans-serif; font-weight: bold;}" +
                       ".values {font-family: 'Oxygen', sans-serif;}" +
                       "</style>" +
                    "</head>" +
                    "<body>" +
                       "<table style='width: 600px'>" +
                       "<colgroup>" +
                       "<col style='width: 100px'>" +
                       "<col style='width: 300px'>" +
                       "</colgroup>" +
                        " <tr> " +
                            " <th colspan='2'class='headers' style='text-align:left; font-family: 'Fjalla One', sans-serif;'><img src='http://maxx-express.com/images/img-contact-email-header.png'></th>" +
                         "</tr>" +
                         "<tr>" +
                             " <td class='headers'>Name</td>" +
                             " <td class='values'>" + Name + "</td>" +
                        " </tr>" +
                        " <tr>" +
                             " <td class='headers'>Phone</td>" +
                             " <td class='values'>" + Phone + "</td>" +
                        " </tr>" +
                        " <tr>" +
                           "   <td class='headers'>Email</td>" +
                           "   <td class='values'>" + Email + "</td>" +
                        " </tr>" +
                        " <tr>" +
                          " <td class='values' colspan='2' rowspan='8'>" +
                          "<span class='headers left-text'>Message: </span>" +
                          "" + Message + 
                          "</td>" +
                          " </tr>" +
                      " </table>" +
                      "</body>" +
                     "</html>"
                            ;
                message.Body = mailBody;
                try
                {
                    client.Send(message);
                    ResetEmailRecord("0");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                }
            }
        }

        public static void ResetEmailRecord(string sendmail)
        {
            using (cascadiaEntities cascadia_db = new cascadiaEntities())
            {
                string IND_x = sendmail.ToString();

                var IND_Value = '1';
                string IND_Value_x = IND_Value.ToString();
                var aventus = cascadia_db.tbl_web_contact_form.FirstOrDefault(e => e.MailSent == IND_x);

                if (aventus != null)
                {
                    try
                    {
                        aventus.MailSent = IND_Value_x;
                        cascadia_db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception caught in CreateTestMessage2(): {0}", ex.ToString());
                    }
                }
            }
        }



    }
}
