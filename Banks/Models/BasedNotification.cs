using System;
using Banks.Interfaces;

namespace Banks.Models
{
    public class BasedNotification : INotification
    {
        public void Notify(Client client)
        {
            Console.WriteLine($" Hi {client.FirstName}!" +
                              $"Our rules changed, so u should go on your phone app and check it!");
        }
    }
}