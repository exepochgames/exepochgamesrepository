using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {

    //Combat kabiliyeti olan actorler icin sınıf
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 50f;  //Hedef ve aktor icin saldırı menzili durma limiti
        [SerializeField] float timeBetweenAttacks = 2f;  //Silah cesidine gore saldırı hızı animasyon tekrarı süresi cooldown
        float timeSinceLastAttack = 0;  //Son saldırı anim. sonrası gecen süre
        [SerializeField] float weaponDamage=30f;  //Silah hasarı (Değişkenleştirilecek)
        Health target;   //Mevcut aktroun etkileşime girdiği-gireceği actor (bullable)
        


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

        private void Attackbehaviour() //++^ -- Hedefe saldırabilmek için yeterince yakın
        {
            transform.LookAt(target.transform.position);  // Saldırı animasyonu oncesi aktorun yönünü hedefe doğru çevir
            if(timeSinceLastAttack > timeBetweenAttacks)  // Onceki saldırıdan sonra yeterli zaman gecmisse sıradaki saldırı veya ilk saldırıyı gerçekleştir
            {
            GetComponent<Animator>().ResetTrigger("stopAttack");  // Saldırıyı durdurma triggerını deaktif etme
            GetComponent<Animator>().SetTrigger("attack");  //Saldırıyı başlatan animasyonu aktif etme
            timeSinceLastAttack = 0;  // Gerçeklesen saldırı sonrası, son saldırıdan sonra gecen sure sayacını sıfırlama
            }
        }

        //Animasyon eventi
        void Hit()  // Middle animasyon anı gerçeklemesi
        {
            if(target == null ) { return; }  //Hedef yoksa return dondur. (try catch için fazla basit ama kullanılabilir)
            target.TakeDamage(weaponDamage);  // Hedefin Healt clasının hasar alma fonksiyonuna mevcut aktorun saldırı gücü degiskeni instance'ı gönderilir ve hedefin canı azalır.
        }

        private bool GetIsInRange() //Aktor ve hedef arası saldırı menzili hesabı
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;  // Hedef aktor arası mesafe saldırı menzilinden kısaysa TRUE döndür
        }


        // Mouseun üzerinde durdugu nesne saldırılabilir mi kontrolü
        public bool CanAttack(CombatTarget combatTarget)  //Gelen parametredeki nesne bir combatTarget nesnesi değilse saldırılabilir degildir(combatTarget scriptine sahip degil demek)
        {   if(combatTarget == null ) { return false; }  // Hedef yoksa FALSE dondur (Yani bu bir saldırı islemi degil)
            Health targetToTest = combatTarget.GetComponent<Health>();   //Gelen nesnenin healt componenti tutulur
            return targetToTest !=null && !targetToTest.IsDead();  //Sartlar saglanmıyorsa False doner ve hedefin healthı yok ve combat yapılamaz
        }

        public void Attack(CombatTarget combatTarget)  // Saldırı fonksiyonu backend
        {
            GetComponent<ActionScheduler>().StartAction(this); // Action managing --< currentAction = Fighter.cs
            target = combatTarget.GetComponent<Health>();  // Hedefin Healt componenti tutulur 
        }

        public void Cancel()  //Saldırı durdurulur // Animason + backend
        {
            StopAttack(); // Saldırı durdurma animasyon trigger Init()
            target = null;  // Hedefe null atanır
        }

        private void StopAttack()  //Animasyon eventi
        {
            GetComponent<Animator>().ResetTrigger("attack"); // Saldırma animasyonu deaktif edilir
            GetComponent<Animator>().SetTrigger("stopAttack");  // Saldırı durdurma animasyonu aktif edilir
        }
    }
}