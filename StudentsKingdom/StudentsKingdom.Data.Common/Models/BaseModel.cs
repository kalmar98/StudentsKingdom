using Newtonsoft.Json;
using System;

namespace StudentsKingdom.Data.Common
{
    public class BaseModel<T>
    {
        public T Id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
