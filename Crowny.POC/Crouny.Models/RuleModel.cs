using System.Collections.Generic;
using System.Linq;
using Crouny.Models.Helpers;
using Newtonsoft.Json;

namespace Crouny.Models
{
    public class RuleModel
    {
        private List<StateParameter> _stateDecoded;
        public int RuleId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public string State { get; set; }

        [JsonIgnore]
        public List<StateParameter> StateDecoded
        {
            get { return _stateDecoded ?? (_stateDecoded = GetParameters()); }
            set
            {
                _stateDecoded = value;
                State = JsonConvert.SerializeObject(value);
            }
        }

        public PluginModel Plugin { get; set; }

        private List<StateParameter> GetParameters()
        {
            return _stateDecoded ?? (State == null ? new List<StateParameter>() : JsonConvert.DeserializeObject<IEnumerable<StateParameter>>(State).ToList());
        }
    }
}
