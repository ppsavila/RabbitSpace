using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
  public void setTime()
  {

        UIController.GetInstance().gameOverWindow.Show();
        //Time.timeScale = 0;
        this.gameObject.SetActive(false);
        
  }
}
