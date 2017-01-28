using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Crouny.DAL.EntityModel;
using Crouny.DAL.Interfaces;
using Crouny.Models;

namespace Crouny.DAL.Repositories
{
    public sealed class RuleRepository: BaseRepository, IRuleRepository
    {
        public RuleRepository(CrounyEntities context) : base(context)
        {
        }

        public IEnumerable<RuleModel> GetRules(int accountId)
        {
            return Context.Rules.Where(d => d.Device.AccountId == accountId)
                .Include(r=>r.Plugin)
                .ToList()
                .Select(Mapper.Map<RuleModel>);
        }

        public void EditRule(int ruleId, RuleModel ruleModel)
        {
            // todo: concurrency checks? maybe version timestamp?
            var plugin = Context.Rules.FirstOrDefault(p => p.RuleId == ruleId);
            if (plugin != null)
                plugin.State = ruleModel.State;

            Save();
        }
    }
}
