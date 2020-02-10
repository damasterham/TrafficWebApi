using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafikWebAPI.Models
{
    // A base model that will hold properties that would be reused between all models
    // So for example if it was required to have time stamps for creation and updates
    // for all our models we could just set it here
    public abstract class BaseModel
    {
        public int Id { get; set; }
    }
}
