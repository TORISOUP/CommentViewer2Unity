using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace CommentViewer2Unity
{
    /// <summary>
    /// あんこちゃんにSocketを繋いで通信するクラス
    /// Unity非依存なのでDebug.Logだけ消せば他での使える
    /// </summary>
    public class CommentViewerToUnityClient
    {
        private String hostIp;
        private int hostPort;
        TcpClient tcpClient;
        byte[] buffer;

        /// <summary>
        /// メッセージ受信時に発行されるイベント
        /// </summary>
        public event MessageRecievedHandler messageRecievedEvent;
        public delegate void MessageRecievedHandler(object sender, CommentInfoRecievedEventArgs e);

        public bool isConnected
        {
            get { return this.tcpClient != null && this.tcpClient.Connected; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hostIp">ホストIP</param>
        /// <param name="hostPort">ホストポート</param>
        public CommentViewerToUnityClient(string hostIp, int hostPort)
        {
            this.hostIp = hostIp;
            this.hostPort = hostPort;
            buffer = new byte[2048];
        }

        /// <summary>
        /// コメビュに接続を試みる
        /// </summary>
        public void Connect()
        {
            //コネクト時にプチフリするのを防ぐために別スレッドで起動する
            var thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    tcpClient = new TcpClient(hostIp, hostPort);
                    tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, new AsyncCallback(CallBackBeginReceive), null);
                }
                catch (Exception e)
                {
                    //もみ消し
                }
            }));
            thread.Start();
        }

        private void CallBackBeginReceive(IAsyncResult ar)
        {
            try
            {
                var bytes = this.tcpClient.GetStream().EndRead(ar);

                if (bytes == 0)
                {
                    //接続断
                    Disconnect();
                    return;
                }

                String recievedMessage = Encoding.UTF8.GetString(buffer, 0, bytes);
                var commentInfo = LitJson.JsonMapper.ToObject<CommentInfo>(recievedMessage);
                //イベント通知
                messageRecievedEvent(this, new CommentInfoRecievedEventArgs(commentInfo));
                tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, new AsyncCallback(CallBackBeginReceive), null);
            }
            catch (Exception e)
            {
                Disconnect();
                Debug.Log(e.Message);
            }
        }

        public void Disconnect()
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                tcpClient.GetStream().Close();
                tcpClient.Close();
            }
        }
    }
}