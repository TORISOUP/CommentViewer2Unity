using UnityEngine;
using System.Collections;

namespace CommentViewer2Unity
{
    /// <summary>
    /// あんこちゃんとの通信を管理するコンポーネント
    /// </summary>
    public class CommentViewerToUnityComponent : MonoBehaviour
    {
        CommentViewerToUnityClient ankochan;

        void Awake()
        {
            ankochan = new CommentViewerToUnityClient("127.0.0.1", 50082);
            if (ankochan.Connect())
            {
                Debug.Log("あんこちゃんに接続しました");
            }
        }

        public void SetMessageRecievedHandler(CommentViewerToUnityClient.MessageRecievedHandler handler)
        {
            this.ankochan.messageRecievedEvent += handler;
        }

        void OnApplicationQuit()
        {
            ankochan.Disconnect();
        }
    }
}