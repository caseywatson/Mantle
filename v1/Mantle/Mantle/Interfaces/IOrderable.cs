using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantle.Interfaces
{
    public interface IOrderable<T>
    {
        int Order { get; set; }
        T Service { get; set; }
    }
}
