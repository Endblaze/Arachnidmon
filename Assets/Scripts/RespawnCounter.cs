using UnityEngine;

public class RespawnCounter : MonoBehaviour
{

    private TextMesh txt;

    private float counter;

    private void Awake()
    {

        txt = GetComponent<TextMesh>();

    }

    private void OnEnable()
    {

        counter = int.Parse(txt.text);

    }

    private void Update()
    {

        counter -= Time.deltaTime;

        if(counter > 0)
        {
            txt.text = (int)counter + "";
        }
        else
        {
            txt.text = "";
        }

    }

}