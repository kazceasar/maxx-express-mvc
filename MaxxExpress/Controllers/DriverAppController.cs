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
    public class DriverAppController : ApiController
    {
        public HttpResponseMessage Post([FromBody] tbl_web_driver_application driver_application)
        {
            try
            {
                using (cascadiaEntities entities = new cascadiaEntities())
                {
                    entities.tbl_web_driver_application.Add(driver_application);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, driver_application);
                    SendEmail();
                    message.Headers.Location = new Uri(Request.RequestUri + driver_application.ID.ToString());
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
                string FirstName;
                string LastName;
                string Phone;
                string Email;
                string DateofBirth;
                string Address;
                string City;
                string State;
                string Zip;
                string CDLNumber;
                string CDLState;
                string YearsofExprience;
                string CleanMVR;
                string DrugTest;
                string mailBody;
                string to = "maxxexpresstrucks@gmail.com";
                string from = "web@maxx-express.com";
                MailMessage message = new MailMessage(from, to);
                message.Subject = "Maxx Express Driver Application";
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("maxxexpress-com01b.mail.protection.outlook.com");
                client.UseDefaultCredentials = false;
                client.Port = 25;
                client.Host = "relay-hosting.secureserver.net";           
                client.Credentials = new System.Net.NetworkCredential("web@maxx-express.com", "BlueExpress13!");
                client.EnableSsl = client.Port == 587;

                var IND = '0';
                string IND_x = IND.ToString();
                var cascadia = cascadia_db.tbl_web_driver_application.FirstOrDefault(e => e.MailSent == IND_x);
                FirstName = cascadia.FirstName.ToString();
                LastName = cascadia.LastName.ToString();
                Phone = cascadia.Phone.ToString();
                Email = cascadia.Email.ToString();
                DateofBirth = cascadia.DateofBirth.ToString();
                Address = cascadia.Address.ToString();
                City = cascadia.City.ToString();
                State = cascadia.State.ToString();
                Zip = cascadia.Zip.ToString();
                CDLNumber = cascadia.CDLNumber.ToString();
                CDLState = cascadia.CDLState.ToString();
                YearsofExprience = cascadia.YearsofExprience.ToString();
                CleanMVR = cascadia.CleanMVR.ToString();
                DrugTest = cascadia.DrugTest.ToString();

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
                            " <th colspan='2'class='headers' style='text-align:left; font-family: 'Fjalla One', sans-serif;'><img src='http://maxx-express.com/images/img-contact-email-header-driver-app.png'></th>" +
                         "</tr>" +
                         "<tr>" +
                             " <td class='headers'>Name</td>" +
                             " <td class='values'>" + FirstName + ' ' + LastName + "</td>" +
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
                           "   <td class='headers'>Date of Birth</td>" +
                           "   <td class='values'>" + DateofBirth + "</td>" +
                        " </tr>" +
                        " <tr>" +
                           "   <td class='headers'>Address</td>" +
                           "   <td class='values'>" + Address + ", " + City + ", " + State + ", " + Zip  + "</td>" +
                        " </tr>" +
                        " <tr>" +
                           "   <td class='headers'>CDL Number</td>" +
                           "   <td class='values'>" + CDLNumber + "</td>" +
                        " </tr>" +
                        " <tr>" +
                           "   <td class='headers'>CDL State</td>" +
                           "   <td class='values'>" + CDLState + "</td>" +
                        " </tr>" +
                        " <tr>" +
                           "   <td class='headers'>Years of Exprience</td>" +
                           "   <td class='values'>" + YearsofExprience + "</td>" +
                        " </tr>" +
                        " <tr>" +
                           "   <td class='headers'>Clean MVR</td>" +
                           "   <td class='values'>" + CleanMVR + "</td>" +
                        " </tr>" +
                        " <tr>" +
                           "   <td class='headers'>Can you pass a Drug Test?</td>" +
                           "   <td class='values'>" + DrugTest + "</td>" +
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
                var aventus = cascadia_db.tbl_web_driver_application.FirstOrDefault(e => e.MailSent == IND_x);

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
