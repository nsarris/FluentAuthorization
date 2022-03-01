using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApplication.Authorization
{
    public class RecordResource
    {
        public RecordResource(EntityTypeResource type, int id)
        {
            Type = type;
            Id = id;
        }

        public EntityTypeResource Type { get; }
        public int Id { get; }
    }
}
