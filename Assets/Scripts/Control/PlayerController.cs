using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control {

// Aktorlerin ana kontrolünün saglandıgı sınıf
public class PlayerController : MonoBehaviour
{
    private void Update()
        {
            if(InteractWithCombat()) return;  // Dovüs durumu icin ana fonksiyon
            if(IntercatWithMovement()) return; // Hareket durumu icin ana fonksiyon
        }

        private bool InteractWithCombat() // Dovüs mekanizması calısma prensibi

        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()); // Mouse imlecinin üzerinde durdugu tum nesnelere raycast cizimi, bu raycastlerin bir dizide tutulması

            foreach (RaycastHit hit in hits) // Raycastın degdıgı tum nesnelerin ne oldugunun kontrolü (Hedef ise saldırılması / haritada bir nokta ise o noktaya gidilebilmesi icin)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>(); // raycastın carptıgı nesnenin "CombatTarget" componentinin instanceı alınır

                if(!GetComponent<Fighter>().CanAttack(target)) // "CanAttack" FALSE ise can yok vey saldırılabilir nesne degil demektir
                {
                    continue;             // Saglanan sart icin raycastın carptıgı bu cisme saldırı uygulanamaz, sıradaki nesneye gecmek icin
                }

                if(Input.GetMouseButton(1))    // Yukarıdaki sartlar saglanmıs ve nesne saldırılabilirse buraya gelinmistir. Mouse1 tıklanırsa eger <
                {
                    GetComponent<Fighter>().Attack(target);   // Mouseun üzerinde durdugu nesne hedef olarak belirlenir
                }
                return true; // Saldırma islemi biter ve InteractWithCombat fonksiyonu TRUE return eder
            }
            return false; //InteractWithCombat fonksiyonu FALSE dondurerek dovus durumu olmadıgı anlasılır ve sıradaki kontrol(hareket) yapılır(Update icerisinde)
        }


        private bool IntercatWithMovement() // Hareket kabiliyeti ana prensibi
        {
            Ray ray = GetMouseRay(); // Mouse ray cizilir haritada hangi koordinata denk geldigi ölcülür

            RaycastHit hit; // Raycastın carptıgı nokta icin tutucu referans deger
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit); // raycast cizilir,haritada(terrain) bir yere carpmıssa true atanır

            if (hasHit) // RayCast bir yere carpıyorsa
            {
                if(Input.GetMouseButton(1)) //Mouse1 basıldı
                {
                GetComponent<Mover>().StartMoveAction(hit.point); // Hareket islemi icin Mover fonksiyonuna gidilecek yerin kordınatı gonderilir
                }
                return true; // Hareket islemi biter IntercatWithMovement islemi TRUE dondürür
            }
            return false; // IntercatWithMovement FALSE hareket islemi olmadıgı anlasılır
        }
        private static Ray GetMouseRay() // Mouse icin raycast cizimi
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition); //cameradan mouseun oldugu yone dogru raycast cizilir
        }
    }
} 