using UnityEngine;
using UnityEngine.UI;

public class SelectedItemScript : MonoBehaviour {
	public void Selected()
    {
        GetComponent<Image>().color = Color.gray;
    }
}
