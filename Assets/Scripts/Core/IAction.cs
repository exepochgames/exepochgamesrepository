namespace RPG.Core
{

    // Dependency Injection  
    public interface IAction {
        void Cancel(); // Aktif aksiyon icin iptal cagrısı
    }
}