using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QQQidian.Models
{
    public class ReqBody
    {
        public string date { get; set; }
        public int count { get; set; }
        public int offset { get; set; }

        public ReqBody()
        {
            count = 500;
            offset = 0;
        }
    }
}
