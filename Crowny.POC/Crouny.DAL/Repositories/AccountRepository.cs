using Crouny.DAL.Interfaces;
using System.Linq;
using Crouny.Models;
using Crouny.DAL.EntityModel;
using System.Data.Entity;
using AutoMapper;

namespace Crouny.DAL.Repositories
{
    public sealed class AccountRepository : BaseRepository, IAccountRepository
    {
        private readonly CrounyEntities _context;

        public AccountRepository(CrounyEntities context)
            : base(context)
        {
            _context = context;
        }

        public AccountModel GetAccount(int accountId)
        {
            return Mapper.Map<AccountModel>(_context.Accounts                
                .Include(a=>a.Devices.Select(d=>d.DeviceSettings))
                               .FirstOrDefault(a => a.AccountId == accountId));
        }
    }
}
