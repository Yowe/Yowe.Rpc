using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork.Internal
{
    /// <summary>
    /// 一个元素的列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class RoundList<T>
    {
        private volatile IList<T> _list;
        private volatile int _index;

        public RoundList(IEnumerable<T> list)
        {
            _list = new List<T>(list);
        }

        public RoundList()
        {
            _list = new List<T>();
        }

        public IEnumerable<T> ALL
        {
            get
            {
                return _list;
            }
        }

        public void ClearALL()
        {
            _list.Clear();
        }

        public void Add(T item)
        {
            lock (_list)
                _list.Add(item);
        }

        public T Current
        {
            get
            {
                lock (_list)
                {
                    return _list[_index];
                }
            }
        }

        public T GetNext()
        {
            if (_list.Count == 0)
                return default(T);

            lock (_list)
            {
                _index++;
                if (_index >= _list.Count)
                    _index = 0;

                return _list[_index];
            }
        }
    }
}
