using Banks.Models;

namespace Banks.Interfaces
{
    public interface INotification
    {
        void Notify(Client client);
    }
}