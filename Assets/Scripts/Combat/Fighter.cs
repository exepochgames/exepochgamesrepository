using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {

    //Combat kabiliyeti olan actorler icin sınıf
    public class Fighter : MonoBehaviour, IAction
    {
        ///----------------------------------------------------------| EDITOR ONLY PUBLIC VARIABLES
        [SerializeField] float weaponRange = 50f; 
        [SerializeField] float timeBetweenAttacks = 2f;
        [SerializeField] float weaponDamage=30f;

        ///----------------------------------------------------------| PRIVATE VARIABLES
        float timeSinceLastAttack = Mathf.Infinity;

        ///----------------------------------------------------------| CLASS INSTANCES
        Health target;

        ///----------------------------------------------------------| UNITY METHODS
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;  //Son saldırıdan sonra gecen süre hesaplaması

            if(target==null) return;      //"target" degerine bir aktor atanmamıs ise geri kalan saldırı işlevlerini atla
            if(target.IsDead()) return;   //hedef ölü(healt<0) ise saldırmayı kesmek için return komutu
            
            //aktor ve hedef arası mesafe
            if (!GetIsInRange())    //Birbirlerine uzak iseler yakınına git
            {
                GetComponent<Mover>().MoveTo(target.transform.position);   // Saldırı menziline girene dek hedefe doğru ilerle
            }
            else                //Yeterince yakınsa hedefe ilerlemeyi kes ve saldırı fonk. cagir
            {
                GetComponent<Mover>().Cancel();     // Hareket islemi iptal/durdur
                Attackbehaviour();          // Saldırı anı animasyon eventleri
            }

        }

        ///----------------------------------------------------------| MAIN METODs
        
        public void Attack(GameObject combatTarget) 
        {
            GetComponent<ActionScheduler>().StartAction(this); // Action managing --< currentAction = Fighter.cs
            target = combatTarget.GetComponent<Health>();  // Hedefin Healt componenti tutulur 
        }

        void Hit() 
        {
            if(target == null ) { return; }  //Hedef yoksa return dondur. (try catch için fazla basit ama kullanılabilir)
            target.TakeDamage(weaponDamage);  // Hedefin Healt classının hasar alma fonksiyonuna mevcut aktorun saldırı gücü degiskeni instance'ı gönderilir ve hedefin canı azalır.
        }
        public void Cancel() 
        {
            StopAttack(); // Saldırı durdurma animasyon trigger Init()
            target = null;  // Hedefe null atanır
        }

        ///----------------------------------------------------------| CONTROL STATEMENTs
        public bool CanAttack(GameObject combatTarget) 
        {   
            if(combatTarget == null ) { return false; }  // Hedef yoksa FALSE dondur (Yani bu bir saldırı islemi degil)

            Health targetToTest = combatTarget.GetComponent<Health>();   //Gelen nesnenin healt componenti tutulur
            return targetToTest !=null && !targetToTest.IsDead();  //Sartlar saglanmıyorsa False doner ve hedefin healthı yok ve combat yapılamaz
        }

        ///----------------------------------------------------------| ANIMATION EVENTS
        private void Attackbehaviour() 
        {
            transform.LookAt(target.transform.position);  // Saldırı animasyonu oncesi aktorun yönünü hedefe doğru çevir
            if(timeSinceLastAttack > timeBetweenAttacks)  // Onceki saldırıdan sonra yeterli zaman gecmisse sıradaki saldırı veya ilk saldırıyı gerçekleştir
            {
            GetComponent<Animator>().ResetTrigger("stopAttack");  // Saldırıyı durdurma triggerını deaktif etme
            GetComponent<Animator>().SetTrigger("attack");  //Saldırıyı başlatan animasyonu aktif etme
            timeSinceLastAttack = 0;  // Gerçeklesen saldırı sonrası, son saldırıdan sonra gecen sure sayacını sıfırlama
            }
        }
        private void StopAttack() 
        {
            GetComponent<Animator>().ResetTrigger("attack"); // Saldırma animasyonu deaktif edilir
            GetComponent<Animator>().SetTrigger("stopAttack");  // Saldırı durdurma animasyonu aktif edilir
        }

        ///----------------------------------------------------------| CALCULATORs
        private bool GetIsInRange() 
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;  // Hedef aktor arası mesafe saldırı menzilinden kısaysa TRUE döndür
        }
    }
}