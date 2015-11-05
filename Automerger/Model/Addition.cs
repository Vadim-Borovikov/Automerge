using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Addition : Change
    {
        public Addition(int start, string[] newContent) : base(start, 0, newContent) { }
    }
}
