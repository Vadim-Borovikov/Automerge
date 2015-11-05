using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Replacement : Change
    {
        public override int RemovedAmount { get { return _removal.RemovedAmount; } }
        public override string[] NewContent { get { return _addition.NewContent; } }

        public Replacement(int line, string[] newContent, int removedLinesAmount) : base(line)
        {
            _addition = new Addition(line, newContent);
            _removal = new Removal(line, removedLinesAmount);
        }

        private readonly Addition _addition;
        private readonly Removal _removal;
    }
}
