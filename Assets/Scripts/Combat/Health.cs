using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f; // Can miktarı
        bool isDead=false;  // Karakterin canlılıgı kontrolu

        public bool IsDead() // Saldıran aktorün saldırı anımasyonu iptali icin kontrol fonksiyonu (Geliştirilecek)
        {
            return isDead;
        }

        public void TakeDamage(float damage)   // Saldıran aktorun gücüne oranlı canı azaltma
        {
            health = Mathf.Max(health - damage, 0); // kalan can miktarını tut ya da healt<=0 durumunu da kontrol et

            if(health==0) // Can sıfırlandıgında
            {
                Die(); //Öldürme işlemi
            }
        }

        private void Die() // Öldürme işlemi
        {
            if(isDead) { return; } // TRUE ise geri kalan ayarlamalar zaten aynı demektir(ölü adamın tekrar ölme animasyonu calısmaması icin)

            isDead=true;  // Karakter öldü ataması
            GetComponent<Animator>().SetTrigger("die"); // Ölüm animasyonu triggerı aktif etme
        }
    }   
}
