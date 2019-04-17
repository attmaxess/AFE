using Com.Beetsoft.AFE;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

public class ShowCube : MonoBehaviour
{
    private List<TestYasuo> testYasuos = new List<TestYasuo>();

    public List<GameObject> listCube;

    void Start()
    {
        MessageBroker.Default.Receive<MessageChangedCharacterYasuo>().TakeUntilDestroy(gameObject).Subscribe(mess =>
        {
            if (mess.addOrRemove)
            {
                testYasuos.Add(mess.yasuo);
            }
            else
            {
                testYasuos.Remove(mess.yasuo);
            }

            if (testYasuos.Count >= 1)
            {
                listCube.ForEach(cube => cube.SetActive(true));
            }
            else
            {
                listCube.ForEach(cube => cube.SetActive(false));
            }
        });
    }
}
