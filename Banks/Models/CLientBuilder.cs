using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Models
{
    public class ClientBuilder : IBuilder
    {
        private Client _client = new Client();

        public ClientBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this._client = new Client();
        }

        public void BuildStep1(string s1)
        {
            var arg = s1?.Split(' ').ToList();
            if (arg?.Count != 2)
            {
                throw new BanksException("Incorrect count of elements!");
            }

            this._client.SetFirstName(arg[0]);
            this._client.SetLastName(arg[1]);
        }

        public void BuildStep2(string s2)
        {
            this._client.SetAddress(s2);
        }

        public void BuildStep3(string s3)
        {
            this._client.SetPassportId(s3);
        }

        public Client GetClient()
        {
            Client result = this._client;
            this.Reset();
            return result;
        }
    }
}