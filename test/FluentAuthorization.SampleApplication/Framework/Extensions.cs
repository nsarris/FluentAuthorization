using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApplication.Authorization
{
    internal static class Extensions
    {
        public static bool IsConfigured(this bool? value, bool check)
           => value.HasValue && value.Value == check;
    }
}
