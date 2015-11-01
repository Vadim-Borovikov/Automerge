using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Addition : Change
    {
        public override int LinesAmount { get { return 0; } }

        public string[] Content;

        public Addition(int line, string[] content) : base(line)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            Content = content;
        }
    }
}
