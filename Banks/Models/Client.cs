using Banks.Interfaces;

namespace Banks.Models
{
    public class Client
    {
        public Client()
        {
        }

        public Client(string firstName, string lastName, string address = null, string passportId = null)
        {
            FirstName = firstName;
            LastName = lastName;
            if (address != null)
            {
                Address = address;
            }

            if (passportId != null)
            {
                PassportId = passportId;
            }

            ConfirmedCheck();
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string PassportId { get; private set; }

        public INotification Notification { get; private set; }

        public bool Confirmed { get; private set; } = false;

        public void SetFirstName(string smth)
        {
            FirstName = smth;
        }

        public void SetLastName(string smth)
        {
            LastName = smth;
        }

        public void SubscribeToNotification(INotification notification)
        {
            Notification = notification;
        }

        public void SetAddress(string address)
        {
            Address = address;
            ConfirmedCheck();
        }

        public void SetPassportId(string passportId)
        {
            PassportId = passportId;
            ConfirmedCheck();
        }

        private void ConfirmedCheck()
        {
            if (this.Address != null && this.PassportId != null)
            {
                Confirmed = true;
            }
        }
    }
}