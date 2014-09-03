using UnityEngine;
using System.Collections;

namespace CommentViewer2Unity
{
    /// <summary>
    /// コメビュとの通信を管理するコンポーネント
    /// </summary>
    public class CommentViewerToUnityComponent : MonoBehaviour
    {
        /// <summary>
        /// 切断時に何秒毎に接続を再確認するか
        /// 0秒以下なら再接続しない
        /// </summary>
        public float reconnectTimeSpan = 2;

        CommentViewerToUnityClient commentViewerToUnityClient;

        /// <summary>
        /// コメビュに接続できているか
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return commentViewerToUnityClient.isConnected;
            }
        }

        void Awake()
        {
            //Awake時に初期化
            commentViewerToUnityClient = new CommentViewerToUnityClient("127.0.0.1", 50082);
        }

        // Use this for initialization
        void Start()
        {
            //Startで接続開始
            commentViewerToUnityClient.Connect();

            if (reconnectTimeSpan > 0)
            {
                //指定秒間で再接続を試みる
                InvokeRepeating("Reconnect", reconnectTimeSpan, reconnectTimeSpan);
            }
        }

        /// <summary>
        /// コメント受信時のイベントハンドラを登録する
        /// </summary>
        /// <param name="handler"></param>
        public void SetMessageRecievedHandler(CommentViewerToUnityClient.MessageRecievedHandler handler)
        {
            this.commentViewerToUnityClient.messageRecievedEvent += handler;
        }

        /// <summary>
        /// 再接続機構
        /// </summary>
        void Reconnect()
        {
            //接続が切れていたら再接続を試みる
            if (!IsConnected)
            {
                commentViewerToUnityClient.Connect();
            }
        }

        void OnDestroy()
        {
            commentViewerToUnityClient.Disconnect();
        }

        void OnApplicationQuit()
        {
            commentViewerToUnityClient.Disconnect();
        }
    }
}
