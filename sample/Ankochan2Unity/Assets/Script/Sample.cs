using UnityEngine;
using System.Collections;

namespace Ankochan2Unity
{
    public class Sample : MonoBehaviour
    {
        AnkoChanComponent ankochan;
        
        void Start()
        {
            this.ankochan = GetComponent<AnkoChanComponent>();
            ankochan.SetMessageRecievedHandler(MessageRecievedHandler);
        }

        /// <summary>
        /// コメント受信時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MessageRecievedHandler(object sender, MessageRecievedEventArgs e)
        {
            Debug.Log(e.commentInfo.Hiragana);
        }

    }

}