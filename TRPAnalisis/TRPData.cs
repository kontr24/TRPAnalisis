using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPAnalisis
{
    public class TRPData
    {
        public static int Id { get; set; }
        public string Surname { get; set; }
        public bool Gender { get; set; }
        public int Result { get; set; }
        public int AllPlace { get; set; }
        public int LagAllLeader { get; set; }
        public int GirlPlace { get; set; }
        public int LagGirlLeader { get; set; }
        public int ManPlace { get; set; }
        public int LagManLeader { get; set; }
        public string TRPIcon { get; set; }
        public bool Offset { get; set; }
    }
}
