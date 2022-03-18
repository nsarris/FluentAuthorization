using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApplication.Authorization
{
    public class RecordResource
    {
        public RecordResource(EntityType type, int id)
        {
            Type = type;
            Id = id;
        }

        public EntityType Type { get; }
        public int Id { get; }
    }
}
