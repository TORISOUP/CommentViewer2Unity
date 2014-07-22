using UnityEngine;
using System.Collections;

namespace Ankochan2Unity
{
    /// <summary>
    /// あんこちゃんとの通信を管理するコンポーネント
    /// </summary>
    public class AnkoChanComponent : MonoBehaviour
    {
        AnkochanClient ankochan;

        void Awake()
        {
            ankochan = new AnkochanClient("127.0.0.1", 50082);
            if (ankochan.Connect())
            {
                Debug.Log("あんこちゃんに接続しました");
            }
        }

        public void SetMessageRecievedHandler(AnkochanClient.MessageRecievedHandler handler)
        {
            this.ankochan.messageRecievedEvent += handler;
        }

        void OnApplicationQuit()
        {
            ankochan.Disconnect();
        }
    }
}