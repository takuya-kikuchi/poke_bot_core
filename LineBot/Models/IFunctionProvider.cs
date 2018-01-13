using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Models
{
    interface IFunctionProvider
    {
        string GetReplyMessage(string message);
    }
}
