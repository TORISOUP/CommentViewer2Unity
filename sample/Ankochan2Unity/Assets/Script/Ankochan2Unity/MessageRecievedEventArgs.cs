using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ankochan2Unity
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public CommentInfo commentInfo;
        public MessageRecievedEventArgs(CommentInfo commentInfo)
        {
            this.commentInfo = commentInfo;
        }
    }
}
