namespace Automerger.Model
{
    public class Addition : Change
    {
        public Addition(int start, string[] newContent) : base(start, 0, newContent) { }
    }
}
