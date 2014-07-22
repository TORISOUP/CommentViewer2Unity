using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Ankochan2Unity
{
    /// <summary>
    /// あんこちゃんにSocketを繋いで通信するクラス
    /// Unity非依存なのでDebug.Logだけ消せば他での使える
    /// </summary>
    public class AnkochanClient
    {
        private String hostIp;
        private int hostPort;
        TcpClient tcpClient;
        byte[] buffer;

        /// <summary>
        /// メッセージ受信時に発行されるイベント
        /// </summary>
        public event MessageRecievedHandler messageRecievedEvent;
        public delegate void MessageRecievedHandler(object sender, MessageRecievedEventArgs e);

        /// <summary>
        /// コネクションを張った状態であるか？
        /// </summary>
        public bool isConnected
        {
            get { return this.tcpClient != null && this.tcpClient.Connected; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hostIp">ホストIP</param>
        /// <param name="hostPort">ホストポート</param>
        public AnkochanClient(string hostIp, int hostPort)
        {
            this.hostIp = hostIp;
            this.hostPort = hostPort;
            buffer = new byte[2048];
        }

        /// <summary>
        /// あんこちゃんに接続を試みる
        /// 失敗したら例外飛びます
        /// </summary>
        public bool Connect()
        {
            tcpClient = new TcpClient(hostIp, hostPort);
            tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, new AsyncCallback(CallBackBeginReceive), null);

            return tcpClient.Connected;
        }

        /// <summary>
        /// パケット受信時のコールバック
        /// </summary>
        /// <param name="ar"></param>
        private void CallBackBeginReceive(IAsyncResult ar)
        {
            try
            {
                var bytes = this.tcpClient.GetStream().EndRead(ar);
                if (bytes == 0)
                {
                    //bytesが０はソケットが切れている
                    Disconnect();
                    return;
                }

                //メッセージを文字列に変換
                String recievedMessage = Encoding.UTF8.GetString(buffer, 0, bytes);

                //JSONをCommentInfoにマッピング
                var commentInfo = LitJson.JsonMapper.ToObject<CommentInfo>(recievedMessage);
                
                //イベント通知
                messageRecievedEvent(this, new MessageRecievedEventArgs(commentInfo));
                
                //次の受信待機
                tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, new AsyncCallback(CallBackBeginReceive), null);
                
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void Disconnect()
        {
            Debug.Log("Disconnect");
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
    }
}
