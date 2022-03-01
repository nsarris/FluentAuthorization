//using FluentAuthorization;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//namespace SampleApplication.Authorization
//{

//    public class MutableLookup<TKey,TValue> : ILookup<TKey, TValue>
//    {
//        Dictionary<TKey, List<TValue>> data = new Dictionary<TKey, List<TValue>>();

//        public void Add(TKey key, TValue policy)
//        {
//            if (!data.TryGetValue(key, out var list))
//            {
//                list = new List<TValue>();
//                data.Add(key, list);
//            }

//            list.Add(policy);
//        }

//        public IEnumerable<TValue> this[TKey key]
//        {
//            get
//            {
//                if (!data.TryGetValue(key, out var result))
//                    return Enumerable.Empty<TValue>();
//                else
//                    return result;
//            }
//        }

//        public int Count => data.SelectMany(x => x.Value).Count();

//        public bool Contains(TKey key)
//        {
//            return data.ContainsKey(key);
//        }

//        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
//        {
//            foreach (var item in data)
//                yield return new Grouping(item.Key, item.Value);
//        }

//        public class Grouping : IGrouping<TKey, TValue>
//        {
//            TKey key;
//            IEnumerable<TValue> policies;
//            public Grouping(TKey key, IEnumerable<TValue> policies)
//            {
//                this.key = key;
//                this.policies = policies;
//            }
//            public TKey Key => key;

//            public IEnumerator<TValue> GetEnumerator()
//            {
//                return policies.GetEnumerator();
//            }

//            IEnumerator IEnumerable.GetEnumerator()
//            {
//                return policies.GetEnumerator();
//            }
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return data.Values.GetEnumerator();
//        }
//    }
//}
