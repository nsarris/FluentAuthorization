using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Model
{
    public class Account
    {
        public int Id { get; set; }
        public Product Product { get; set; } 
        public decimal Balance { get; set; }
    }
}
