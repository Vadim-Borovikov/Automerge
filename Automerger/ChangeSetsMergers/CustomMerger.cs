using System;
using System.Collections.Generic;
using System.Linq;
using Automerger.Changes;
using Automerger.ChangeSets;

namespace Automerger.ChangeSetsMergers
{
    /// <summary>
    /// Merge rules:
    /// 1. Two identical changes don't yield a conflict
    /// 2. Two different additions before same line yield a confilct
    /// 3. In other cases two changes without collision don't yield a conflict
    /// 4. Addition vs removal yield a replacement
    /// 5. In other cases two changes with collision yield a conflict
    /// </summary>
    public class CustomMerger : IChangeSetMerger
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region IChangeSetMerger implementation
        public IDictionary<int, IChange> Merge(IReadOnlyDictionary<int, IMergableChange> changes1,
                                               IReadOnlyDictionary<int, IMergableChange> changes2,
                                               string[] source)
        {
            if ((changes1 == null) || (changes2 == null) || (source == null))
            {
                throw new ArgumentNullException();
            }

            ChangeSetVerifier.Verify(changes1, source.Length);
            ChangeSetVerifier.Verify(changes2, source.Length);

            var result = new Dictionary<int, IChange>();

            Dictionary<int, IMergableChange> currentChanges = Utils.ToDictionary(changes1);
            Dictionary<int, IMergableChange> otherChanges = Utils.ToDictionary(changes2);

            for (int line = 0; line <= source.Length; ++line)
            {
                if (!currentChanges.ContainsKey(line))
                {
                    if (!otherChanges.ContainsKey(line))
                    {
                        continue;
                    }

                    Utils.SwapDictionaries(ref currentChanges, ref otherChanges);
                }

                IMergableChange currentChange = currentChanges[line];
                currentChanges.Remove(line);

                IChange newChange = currentChange;

                IMergableChange collidingChange =
                    FindCollision(otherChanges, line, currentChange.AfterFinish);
                if (collidingChange != null)
                {
                    otherChanges.Remove(collidingChange.Start);

                    if (!collidingChange.Equals(currentChange))
                    {
                        IChange mergedChange = TryMerge(currentChange, collidingChange);
                        if (mergedChange != null)
                        {
                            newChange = mergedChange;
                        }
                        else
                        {
                            newChange = new Conflict(currentChange, collidingChange, source);
                        }
                    }
                }

                result.Add(line, newChange);
                line = newChange.AfterFinish - 1;
            }

            return result;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Helpers
        private IMergableChange FindCollision(IReadOnlyDictionary<int, IMergableChange> changes,
                                              int start, int afterFinish)
        {
            if (changes.Keys.Contains(start))
            {
                return changes[start];
            }

            for (int i = start; i < afterFinish; ++i)
            {
                if (changes.ContainsKey(i))
                {
                    return changes[i];
                }
            }
            return null;
        }

        private IChange TryMerge(IMergableChange change1, IMergableChange change2)
        {
            if (change1.Start != change2.Start)
            {
                return null;
            }

            Addition addition = TryCastOneOf<Addition>(change1, change2);
            if (addition == null)
            {
                return null;
            }
            Removal removal = TryCastOneOf<Removal>(change1, change2);
            if (removal == null)
            {
                return null;
            }

            return new Replacement(removal.Start, removal.RemovedAmount, addition.NewContent);
        }

        private static T TryCastOneOf<T>(object o1, object o2)
            where T : class
        {
            T result = o1 as T;
            if (result == null)
            {
                result = o2 as T;
                if (result == null)
                {
                    return null;
                }
            }
            return result;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
