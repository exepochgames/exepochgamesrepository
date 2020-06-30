using UnityEngine;

namespace RPG.Core
{

    public class ActionScheduler : MonoBehaviour
    {
        ///----------------------------------------------------------| PRIVATE VARIABLES
        IAction currentAction;
        ///----------------------------------------------------------| MAIN METHODs
        public void StartAction(IAction action)
        {
            if(currentAction == action) return; // Mevcut aksiyon, parametre olarak gelen aksiyon ise return;
            if(currentAction !=null) // Islem anında aktif bir aksiyon var ise (Hareket, Saldırı vs.)
            {
                //print("Cancelling  " + currentAction); // Debug.Log'a aktif olan aksiyon iptal ediliyor yazdırılır. // TEST icin //
                currentAction.Cancel();
            }
            currentAction = action; // Yeni bir aksiyon talebi geldigi icin aktif aksiyon güncellenir. (Hareket ederken saldırı yapıldı ise hareketi durdur ve saldır(Yeteri kadar yakınsa))
        }
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}