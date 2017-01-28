using Crouny.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Crouny.Web.Controllers.API
{
    [RoutePrefix("api/Device")]
    public class DeviceController : ApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IDeviceRepository _deviceRepository;

        public DeviceController(IAccountRepository accountRepository, IDeviceRepository deviceRepository)
        {
            _accountRepository = accountRepository;
            _deviceRepository = deviceRepository;
        }

        [HttpGet]        
        public async Task<IHttpActionResult> Get(Guid? deviceId)
        {
            var account = await Task.Run(()=> _accountRepository.GetAccount(1));
            return Ok(account);
        }

        [HttpGet]
        [Route("GetScripts")]
        public async Task<IHttpActionResult> GetScripts(Guid? deviceId)
        {
            var deviceScripts = await Task.Run(() => _deviceRepository.GetDeviceScripts(deviceId.Value));
            return Ok(deviceScripts);
        }
    }
}
