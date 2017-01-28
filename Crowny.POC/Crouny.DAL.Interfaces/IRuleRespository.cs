using System;
using System.Collections.Generic;
using Crouny.Models;

namespace Crouny.DAL.Interfaces
{
    public interface IRuleRepository
    {
        IEnumerable<RuleModel> GetRules(int accountId);
       
        void EditRule(int ruleId, RuleModel ruleModel);
    }
}
