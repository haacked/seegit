using System.Diagnostics;

namespace SeeGit
{
    [DebuggerDisplay("{Sha}: {Message}")]
    public class CommitVertex
    {
        public string Sha { get; private set; }
        public string Message { get; private set; }

        public CommitVertex(string sha, string message)
        {
            Sha = sha;
            Message = message;
        }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Sha, Message);
        }
    }
}