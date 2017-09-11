using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Spike.Models
{
    public class ApplicationOptions
    {
        public ApplicationOptions()
        {
            // Set default value.
            
        }

        public Uri  weatherURL { get; set; }
        public string weatherToken { get; set; }
        public Uri BeerURL { get; set; }
        public string BeerToken { get; set; }
    }
}
