using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    class CountersFormClass
    {
        private string collectorId;
        private string firstName;
        private string lastName;
        private int countersCount;

        public CountersFormClass(string cid, string fn, string ln, int cc)
        {
            this.collectorId = cid;
            this.firstName = fn;
            this.lastName = ln;
            this.countersCount = cc;
        }

        public string CollectorId
        {
            get
            {
                return collectorId;
            }
            set
            {
                value = collectorId;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                value = firstName;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                value = lastName;
            }
        }

        public int CountersCount
        {
            get
            {
                return countersCount;
            }
            set
            {
                value = countersCount;
            }
        }
    }
}
