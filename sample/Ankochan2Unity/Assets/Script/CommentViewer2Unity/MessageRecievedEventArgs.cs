using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommentViewer2Unity
{
    public class CommentInfoRecievedEventArgs : EventArgs
    {
        public CommentInfo commentInfo;
        public CommentInfoRecievedEventArgs(CommentInfo commentInfo)
        {
            this.commentInfo = commentInfo;
        }
    }
}
