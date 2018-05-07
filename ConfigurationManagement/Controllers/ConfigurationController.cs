using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ConfigurationManagement.Business.Manager;
using ConfigurationManagement.Models;
using ConfigurationManagement.Business;

namespace ConfigurationManagement.Controllers
{
    public class ConfigurationController : Controller
    {
        private const string host ="mongodb://localhost:27017";
        private const string application = "console";
        private const int duration = 20;
        // GET: Configuration
        public ActionResult Index()
        {
            ConfigurationReader reader = new ConfigurationReader(application, host, duration);
            var list = reader.GetAll();
            var model = list.Select(p => new ConfigurationModel()
            {
                ApplicationName = p.ApplicationName,
                Guid = p.Guid,
                Name = p.Name,
                Type = p.Type,
                Value = p.Value
            }
            );
            return View(model);
        }

        // GET: Configuration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Configuration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Configuration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConfigurationModel model)
        {
            try
            {
                ConfigurationReader reader = new ConfigurationReader(application, host, duration);

                RecordDto recordDto = new RecordDto()
                {
                    ApplicationName = model.ApplicationName,
                    Name = model.Name,
                    Value = model.Value,
                    Type = model.Type,
                    Guid = model.Guid,
                };
                reader.Write(recordDto);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Configuration/Edit/5
        public ActionResult Edit(string id)
        {
            ConfigurationReader reader = new ConfigurationReader(application, host, duration);
            var dto = reader.GetValueWithId(id);
            var model = new ConfigurationModel()
            {
                ApplicationName = dto.ApplicationName,
                Guid = dto.Guid,
                Name = dto.Name,
                Type = dto.Type,
                Value = dto.Value
            };
            return View(model);
        }

        // POST: Configuration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ConfigurationModel model)
        {
            try
            {
                ConfigurationReader reader = new ConfigurationReader(application, host, duration);
                RecordDto recordDto = new RecordDto()
                {
                    ApplicationName = model.ApplicationName,
                    Name = model.Name,
                    Value = model.Value,
                    Type = model.Type,
                    Guid = model.Guid,
                };
                reader.Update(recordDto);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Configuration/Delete/5
        public ActionResult Delete(string id)
        {
            ConfigurationReader reader = new ConfigurationReader(application, host, duration);
            var dto = reader.GetValueWithId(id);
            var model = new ConfigurationModel()
            {
                ApplicationName = dto.ApplicationName,
                Guid = dto.Guid,
                Name = dto.Name,
                Type = dto.Type,
                Value = dto.Value
            };
            return View(model);
             
        }

        // POST: Configuration/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, ConfigurationModel model)
        {
            try
            {
                ConfigurationReader reader = new ConfigurationReader(application, host, duration);
                reader.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}