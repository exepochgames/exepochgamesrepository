using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        ///----------------------------------------------------------| EDITOR ONLY PUBLIC VARIABLEs
        [SerializeField] float health = 100f;

        ///----------------------------------------------------------| PRIVATE VARIABLEs
        bool isDead =false;

        ///----------------------------------------------------------| MAIN METHODs
        public void TakeDamage(float damage) 
        {
            health = Mathf.Max(health - damage, 0); // kalan can miktarını tut ya da healt<=0 durumunu da kontrol et

            if(health==0) // Can sıfırlandıgında
            {
                Die(); //Öldürme işlemi
            }
        }

        private void Die() 
        {
            if(isDead) { return; } // TRUE ise geri kalan ayarlamalar zaten aynı demektir(ölü adamın tekrar ölme animasyonu calısmaması icin)

            isDead=true;  // Karakter öldü ataması
            GetComponent<Animator>().SetTrigger("die"); // Ölüm animasyonu triggerı aktif etme
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        ///----------------------------------------------------------| CONTROL STATEMENTs
        public bool IsDead()
        {
            return isDead;
        }
    }   
}
