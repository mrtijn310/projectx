using System.Collections.Generic;
using System.Linq;
using Crouny.Models.Helpers;
using Newtonsoft.Json;

namespace Crouny.Models
{
    public class PluginModel
    {
        private List<StateParameter> _parametersDecoded;
        public int PluginId { get; set; }
        public string Script { get; set; }
        public string ScriptName { get; set; }

        [JsonIgnore]
        public string Parameters { get; set; }

        [JsonIgnore]
        public List<StateParameter> ParametersDecoded
        {
            get { return _parametersDecoded ?? (_parametersDecoded = GetParameters()); }
            set
            {
                _parametersDecoded = value;
                Parameters = JsonConvert.SerializeObject(value);
            }
        }

        private List<StateParameter> GetParameters()
        {
            return _parametersDecoded ?? ( Parameters == null ? new List<StateParameter>() : JsonConvert.DeserializeObject<IEnumerable<StateParameter>>(Parameters).ToList());
        }

        [JsonIgnore]
        public int PluginType { get; set; }
    }
}
