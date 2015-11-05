using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Addition : Change
    {
        public override string[] NewContent { get { return _newContent; } }

        public Addition(int start, string[] newContent) : base(start)
        {
            if (newContent == null)
            {
                throw new ArgumentNullException();
            }

            _newContent = newContent;
        }

        private string[] _newContent;
    }
}
