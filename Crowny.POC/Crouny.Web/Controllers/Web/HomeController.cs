using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Crouny.DAL.Interfaces;
using Crouny.Models;
using Crouny.Web.Models;

namespace Crouny.Web.Controllers.Web
{
    public class HomeController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRuleRepository _ruleRepository;

        public HomeController(IDeviceRepository deviceRepository, IRuleRepository ruleRepository)
        {
            _deviceRepository = deviceRepository;
            _ruleRepository = ruleRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Regels");
        }

        public ActionResult Regels()
        {
            // TODO: keep account id in session.
            return View(new RulesViewModel { Rules = _ruleRepository.GetRules(1).ToList() });
        }

        [HttpPost]
        public ActionResult Regels(int ruleId, string status)
        {
            var rules = _ruleRepository.GetRules(1);
            var ruleToEdit = rules.FirstOrDefault(r => r.RuleId == ruleId);
            ruleToEdit.StateDecoded = ruleToEdit.Plugin.ParametersDecoded.Where(p => p.Name != (status == "true" ? "OffValue" : "OnValue")).ToList();

            _ruleRepository.EditRule(ruleId, ruleToEdit);

            return View(new RulesViewModel() {Rules= rules.ToList() });
        }

        public ActionResult Apparaten()
        {
            // TODO: keep account id in session.
            return View(new DevicesViewModel() {Devices= _deviceRepository.GetDevices(1) });
        }

        [HttpPost]
        public ActionResult Apparaten(int pluginId, PluginModel scriptModel)
        {
            _deviceRepository.EditPlugin(pluginId, scriptModel);

            return View(new DevicesViewModel() { Devices = _deviceRepository.GetDevices(1) });
        }
    }
}
