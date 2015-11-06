using System;
using System.Collections.Generic;

namespace Automerger.Model
{
    public class Conflict : Change
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Creation
        public Conflict(IMergableChange change1, IMergableChange change2,
                        string[] source, int start)
        {
            if ((change1 == null) || (change2 == null) || (source == null))
            {
                throw new ArgumentNullException();
            }

            if ((start < 0) || (start > source.Length))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (start != Math.Min(change1.Start, change2.Start))
            {
                throw new ArgumentOutOfRangeException();
            }

            if ((change1.Start >= source.Length) || (change2.Start >= source.Length))
            {
                throw new ArgumentOutOfRangeException();
            }

            int afterlastFinish = Math.Max(change1.Start + change1.RemovedAmount,
                                           change2.Start + change2.RemovedAmount);

            int removedAmount = afterlastFinish - start;

            string[] originalBlock = Utils.GetSubArray(source, start, afterlastFinish);
            string[] changedBlock1 = GetChangedBlock(source, start, afterlastFinish, change1);
            string[] changedBlock2 = GetChangedBlock(source, start, afterlastFinish, change2);

            var newContent = new List<string>();
            newContent.Add(Consts.CONFLICT_BLOCK_BEGIN);
            newContent.Add(Consts.CONFLICT_BLOCK_SOURCE);
            newContent.AddRange(originalBlock);
            newContent.Add(Consts.CONFLICT_BLOCK_CHANGED1);
            newContent.AddRange(changedBlock1);
            newContent.Add(Consts.CONFLICT_BLOCK_CHANGED2);
            newContent.AddRange(changedBlock2);
            newContent.Add(Consts.CONFLICT_BLOCK_END);

            Initialize(start, removedAmount, newContent.ToArray());
        }

        private string[] GetChangedBlock(string[] source, int start, int afterFinish,
                                         IMergableChange change)
        {
            var result = new List<string>();

            for (int i = start; i < change.Start; ++i)
            {
                result.Add(source[i]);
            }

            result.AddRange(change.NewContent);

            for (int i = change.Start + change.RemovedAmount; i < afterFinish; ++i)
            {
                result.Add(source[i]);
            }

            return result.ToArray();
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
