namespace Automerger.Model
{
    public class Addition : Change, IMergableChange
    {
        public Addition(int start, string[] newContent) : base(start, 0, newContent) { }
    }
}
