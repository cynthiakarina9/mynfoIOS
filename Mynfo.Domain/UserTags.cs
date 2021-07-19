namespace Mynfo.Domain
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserTags
    {
        public int UserTagsId { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        public int TagId { get; set; }

        public string Name { get; set; }
    }
}
