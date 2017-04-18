using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public interface IPoolAble
    {
        bool IsInActive { get; }
        event EventHandler<PoolAbleEventArgs> Inactivated;
        void SetInActive();

        void Init(int hashCode);
    }

    public class PoolAbleEventArgs : EventArgs
    {

        public PoolAbleEventArgs(int hashCode)
        {
            HashCode = hashCode;
        }
        public Int32 HashCode
        {
            get; private set;
            
        }
    }
}
