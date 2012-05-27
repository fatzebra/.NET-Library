using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatZebra
{
    public interface IRecord
    {
         string ID { get; }
         bool Successful { get; }
    }
}
