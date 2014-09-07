using UnityEngine;
using System.Collections;

namespace CommentViewer2Unity
{
    public class CommentViewerToUnitySample : MonoBehaviour
    {
        CommentViewerToUnityComponent commentViewerComponent;
        
        void Start()
        {
            this.commentViewerComponent = GetComponent<CommentViewerToUnityComponent>();
            
            //イベントハンドラ登録
            commentViewerComponent .SetMessageRecievedHandler(MessageRecievedHandler);
        }

        /// <summary>
        /// コメント受信時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MessageRecievedHandler(object sender, CommentInfoRecievedEventArgs e)
        {
            Debug.Log(e.commentInfo.Message);
        }

    }

}