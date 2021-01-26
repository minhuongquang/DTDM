using DTDM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DTDM.Controllers
{
    public class HomeController : Controller
    {

        static string _ip = "192.168.71.129";
        static string _user = "minh";
        static string _pass = "1";

        SshClient client = new SshClient(_ip, _user, _pass);

        private readonly ILogger<HomeController> _logger;
        private readonly pjdtdmContext _context;

        public HomeController(pjdtdmContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult StopDocker()
        {
            return View();
        }
        [HttpPost]
        public IActionResult StopDocker(StopModel model)
        {
            string message = "";

            if (ModelState.IsValid)
            {
                if (!_context.Users.Where(x => x.Username == model.username && x.Passwd == model.passwd && x.Role == 0).Count().Equals(0))
                {
                    client.Connect();
                    SshCommand sc = client.RunCommand("docker stop " + model.name + "");
                    sc.Execute();
                    SshCommand sc2 = client.RunCommand("docker rm " + model.name + "");
                    sc2.Execute();
                    SshCommand sc1 = client.RunCommand("docker ps");

                    ViewBag.MyMessage = sc1.Execute();
                    client.Disconnect();
                }
                else
                {
                    message = "permission denied";
                    ViewBag.MyMessage = message;
                }
            }
            else
            {
                message = "Failed to stop. Please try again";
                ViewBag.MyMessage = message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Setup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Setup(SetupModel model)
        {
            string message = "";

            if (ModelState.IsValid)
            {
                if (!_context.Users.Where(x => x.Username == model.username && x.Passwd == model.passwd && x.Role == 0).Count().Equals(0))
                {
                    string cmd = "docker run -it --name " + model.Name + " --cpuset-cpus=" + model.CPU + " --memory=" + model.Ram + " ubuntu /bin/bash";
                    client.Connect();
                    SshCommand sc = client.RunCommand(cmd);
                    message = "Đã tạo";
                    ViewBag.MyMessage = message;
                    client.Disconnect();
                }
                else
                {
                    message = "permission denied";
                    ViewBag.MyMessage = message;
                }

            }
            else
            {
                message = "Failed to run. Please try again";
                ViewBag.MyMessage = message;
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult Set_up()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Set_up(SetupModel model)
        {
            string message = "";

            if (ModelState.IsValid)
            {
                if (!_context.Users.Where(x => x.Username == model.username && x.Passwd == model.passwd && x.Role == 0).Count().Equals(0))
                {
                    string cmd = "docker run -it --name " + model.Name + " --cpuset-cpus=" + model.CPU + " --memory=" + model.Ram + " ubuntu /bin/bash";
                    client.Connect();
                    SshCommand sc = client.RunCommand(cmd);
                    message = "Đã tạo";
                    ViewBag.MyMessage = message;
                    client.Disconnect();
                }
                else
                {
                    message = "permission denied";
                    ViewBag.MyMessage = message;
                }

            }
            else
            {
                message = "Failed to run. Please try again";
                ViewBag.MyMessage = message;
            }
            return View();
        }
    }
}
