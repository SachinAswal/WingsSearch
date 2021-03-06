﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaniShop.Models;
using SaniShop.DAL;
using System.Web.Security;
using log4net;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace SaniShop.Controllers
{
    public class LoginController : Controller 
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(LoginController).Name);

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Users user)
        {
            
            Session["key"] = string.Empty;
            Session["key"] = user.UserName;
            log.Info("Page Load failed : " + user.UserName + user.Password);
            if (user.IsValid(user.UserName, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                    return RedirectToAction("Index", "Home");                    
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }

            return View(user);    
                    
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
        [HttpGet]
        public ActionResult register1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register1(RegiterModal regiterModal)
        {
            try
            {
                
                if (ModelState.IsValid)
                {                    
                    Regitration regi = new Regitration();
                    //regi.username = regiterModal.username;
                    //regi.UserPassword = regiterModal.UserPassword;
                    //regi.Email = regiterModal.Email;
                    //regi.mobile = regiterModal.mobile;
                    //regi.registerdate = DateTime.Now.ToString();
                    //regi.city = regiterModal.city;
                              
                    var finalData = Mapper.Map<RegiterModal, Regitration>(regiterModal);
                    using (SainiShopEntities1 objDb = new SainiShopEntities1())
                    {
                        objDb.Regitrations.Add(finalData);
                        objDb.SaveChanges();
                    }
                }
                return View();
            }
            catch(Exception ex)
            {
                log.Error("Page Load failed : " + ex.InnerException + ex.StackTrace);
                return View();
            }
            
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
        }
    }
}