using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class GenericObjectPool : MonoBehaviour
    {
        private static GenericObjectPool current = new GenericObjectPool();
        public static GenericObjectPool Current
        {
            get { return current; }
        }

        private Dictionary<Int32, PoolingElementContext> pools = new Dictionary<int, PoolingElementContext>();


        public delegate IPoolAble InstantiationMethod(MonoBehaviour behaviour);


        public void Init(int hashCode, InstantiationMethod instantiationMethod,int min  = 10,int max = 20)
        {
            if (!pools.ContainsKey(hashCode))
            {
                PoolingElementContext poolingElementContext = new PoolingElementContext();
                poolingElementContext.InstationMethod = instantiationMethod;
                poolingElementContext.Min = min;
                poolingElementContext.Max = max;
                poolingElementContext.PoolingObjects = new List<IPoolAble>();
                pools.Add(hashCode, poolingElementContext);

                List<IPoolAble> poolAbles = pools[hashCode].PoolingObjects;

                for (int i = 0; i < min; i++)
                {
                    IPoolAble poolAble = CreateNew(instantiationMethod);
                    poolAbles.Add(poolAble);
                }
            }


          
        }



        public IPoolAble Get(int hashCode)
        {
            var context = pools[hashCode];

            IPoolAble poolAbleToReturn = null;
            foreach (var poolAble in context.PoolingObjects)
            {
                if (poolAble.IsInActive)
                {
                    poolAbleToReturn = poolAble;
                    break;
                }

            }

            if (poolAbleToReturn == null)
            {             
                IPoolAble poolAble = CreateNew(context.InstationMethod);
                context.PoolingObjects.Add(poolAble);
                poolAbleToReturn = poolAble;
            }
            return poolAbleToReturn;
        }

        private IPoolAble CreateNew(InstantiationMethod instantiationMethod)
        {
            IPoolAble poolAble = instantiationMethod(this);
            poolAble.Inactivated += PoolAble_Inactivated;
            return poolAble;

        }

        private void PoolAble_Inactivated(object sender, PoolAbleEventArgs e)
        {
            var hashCode = e.HashCode;
            int max = pools[hashCode].Max;

            if (pools[hashCode].PoolingObjects.Count >= max)
            {
                IPoolAble poolAble = sender as IPoolAble;
                poolAble.Inactivated -= PoolAble_Inactivated;
                MonoBehaviour behaviour = poolAble as MonoBehaviour;
                pools[hashCode].PoolingObjects.Remove(poolAble);
                Destroy(behaviour);
            }
        }

        public class PoolingElementContext
        {
            public List<IPoolAble> PoolingObjects { get; set; }
            public InstantiationMethod InstationMethod { get; set; }
            public int Min { get; set; }
            public int Max { get; set; }

        }

    }



}
