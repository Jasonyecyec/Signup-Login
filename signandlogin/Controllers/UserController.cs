using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using signandlogin.Models;
using signandlogin.DBModel;
using Newtonsoft.Json;
using System.Net.Mail;

namespace signandlogin.Controllers
{
    public class UserController : Controller
    {


        // GET: User
        public ActionResult SignUp()
        {
            UserModel user = new  UserModel();
            return View(user);
        }

        [HttpPost]
        public ActionResult SignUp(UserModel userObject)
        {

            if (ModelState.IsValid)
            {
                using (users_dbEntities userDBEntities = new users_dbEntities())
                {
                    if (!userDBEntities.users.Any(m => m.email == userObject.email))
                    {
                        Random r = new Random();
                        int randNum = r.Next(1000000);
                        string userOtp = randNum.ToString("D6");

                        user userTable = new DBModel.user();
                        userTable.first_name = userObject.firstName;
                        userTable.last_name = userObject.lastName;
                        userTable.email = userObject.email;
                        userTable.password = userObject.password;
                        userTable.address = userObject.address;
                        userTable.gender = userObject.gender;
                        userTable.phone_number = userObject.phoneNumber;
                        userTable.birthdate = userObject.birthdate;
                        userTable.date_created = DateTime.Now;
                        userTable.otp = userOtp;

                 
                        Session["otp"] = userOtp;
                        TempData["email"] = userObject.email;
                        TempData["userTable"] = JsonConvert.SerializeObject(userTable);
                        return RedirectToAction("EmailValidation");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Account already exist");
                        return View();
                    }
                  
                }
            }
            
            return View();
        }

        public ActionResult EmailValidation()
        {
      
            using (MailMessage mail = new MailMessage())
                {
                  
                    mail.To.Add(TempData["email"].ToString());
                    mail.From = new MailAddress("jason.yecyec023@gmail.com");
                    mail.Subject = "OTP";
                    string Body = Session["otp"].ToString();
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("jason.yecyec023@gmail.com", "hjjwsqbmhhppqemk"); // Enter sender User name and password       
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            
            return View();
        }

        [HttpPost]
        public ActionResult EmailValidation(string otp)
        {
          
         
            if (otp == Session["otp"].ToString())
            {
                return RedirectToAction("SaveUserToDatabase");
            }
            else
            {
                ModelState.AddModelError("Error", "Invalid OTP");
                return View();
            }

      
        }
        public ActionResult SaveUserToDatabase()
        {
            user userTable = JsonConvert.DeserializeObject<user>(TempData["userTable"].ToString());
            using (users_dbEntities userDBEntities = new users_dbEntities())
            {
                userDBEntities.users.Add(userTable);
                userDBEntities.SaveChanges();
            }

            Session["name"] = userTable.first_name + " " + userTable.last_name;
            return RedirectToAction("Index", "Home");
  
        }


        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel loginObject)
        {
            if (ModelState.IsValid)
            {
                using (users_dbEntities userDBEntities = new users_dbEntities())
                {
                    var findUser = userDBEntities.users.Where(m => m.email == loginObject.email && m.password == loginObject.password).FirstOrDefault();
                   if (findUser != null)
                    {

                        Session["name"] = findUser.first_name + " " + findUser.last_name;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Email and password do not match");
                        return View();
                    }
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }
        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
