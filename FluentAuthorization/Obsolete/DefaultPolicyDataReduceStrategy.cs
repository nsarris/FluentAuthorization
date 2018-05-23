//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FluentAuthorization
//{
//    public class DefaultPolicyDataReduceStrategy : IPolicyDataReduceStrategy
//    {
//        readonly static Lazy<DefaultPolicyDataReduceStrategy> instance = new Lazy<DefaultPolicyDataReduceStrategy>(() => new DefaultPolicyDataReduceStrategy());
//        internal static DefaultPolicyDataReduceStrategy Instance => instance.Value;
//        internal static object defaultValue = new object();
//        public virtual IPolicyData Merge(IPolicyData current, IPolicyData next)
//        {
//            if (current == null && next == null)
//                throw new Exception("Cannot merge null policy data");
//            else if (current == null || next == null)
//                return current ?? next;

//            if (current.GetType() != next.GetType())
//                throw new Exception("Cannot merge policy data of different types");

//            var currentValues = current.GetValues().ToList();
//            var nextValues = next.GetValues().ToList();

//            if (currentValues.Count != nextValues.Count)
//                throw new Exception($"Policy value count mismatch when trying to merge policy data of type {this.GetType().Name}");

//            var effectiveValues = new List<KeyValuePair<string, object>>();
//            foreach(var items in currentValues.Zip(nextValues, (x,y) => new { Current = x, Next = y }))
//            {
//                if (items.Current.Key != items.Next.Key)
//                    throw new Exception($"Policy value sequence mismatch for data of type {this.GetType().Name}");

//                object v = null;
//                if (items.Current.Value == null && items.Next.Value == null)
//                    v = null;
//                else if (items.Current.Value == null || items.Next.Value == null)
//                    v = items.Current.Value ?? items.Next.Value;
//                else
//                {
//                    if (items.Current.Value.GetType() != items.Next.Value.GetType())
//                        throw new Exception($"Policy value type mismatch for data of type {this.GetType().Name}");

//                    if (typeof(IConfigurable).IsAssignableFrom(items.Current.Value.GetType()))
//                    {
//                        if (((IConfigurable)items.Next.Value).IsConfigured)
//                            v = Configurable.Create(((IConfigurable)items.Next.Value).Value);
//                        else if (((IConfigurable)items.Current.Value).IsConfigured)
//                            v = Configurable.Create(((IConfigurable)items.Current.Value).Value);
//                        else
//                            v = defaultValue;
//                    }
//                    else
//                        v = items.Next.Value;
//                }

//                if (v != defaultValue)
//                    effectiveValues.Add(new KeyValuePair<string, object>(items.Current.Key, v));
//            }

//            var result = current.CreateNew(effectiveValues);

//            if (result == null)
//                throw new Exception($"Null result returned from PolicyData CreateNew for data of type {this.GetType().Name}");
//            if (result.GetType() != this.GetType())
//                throw new Exception($"Different type returned from PolicyData CreateNew for data of type {this.GetType().Name}");

//            return result;
//        }
//    }
//}
