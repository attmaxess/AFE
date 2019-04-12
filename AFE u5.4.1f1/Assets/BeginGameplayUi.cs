using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Com.Beetsoft.AFE;
using TMPro;

public class BeginGameplayUi : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUI;
    public Transform panelCountTime;

    public Transform waitOtherPlayer;
    public List<TestYasuo> testYasuos = new List<TestYasuo>();

    private void Awake()
    {
        MessageBroker.Default.Receive<MessageChangedCharacterYasuo>().Subscribe(mess =>
        {
            if (mess.addOrRemove)
            {
                testYasuos.Add(mess.yasuo);
            }
            else
            {
                testYasuos.Remove(mess.yasuo);
            }
            if (testYasuos.Count == 1)
            {
                panelCountTime.gameObject.SetActive(false);
                waitOtherPlayer.gameObject.SetActive(true);
            }
            if (testYasuos.Count == 2)
            {
                panelCountTime.gameObject.SetActive(true);
                waitOtherPlayer.gameObject.SetActive(false);
                int count = 3;
                Observable.Interval(System.TimeSpan.FromSeconds(1)).TakeWhile(_ => count >= 0 && testYasuos.Count == 2).Subscribe(_ =>
                     {
                         if (count == 0)
                         {
                             panelCountTime.gameObject.SetActive(false);
                             waitOtherPlayer.gameObject.SetActive(false);
                         }
                         textMeshProUI.text = count.ToString();
                         count--;
                     });
            }
            if (testYasuos.Count == 0 || testYasuos.Count >= 3)
            {
                panelCountTime.gameObject.SetActive(false);
                waitOtherPlayer.gameObject.SetActive(false);
            }
        });
    }



}
