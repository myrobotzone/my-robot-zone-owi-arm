using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot_arm_server
{
    public interface IMessageHandler
    {
        bool Start();

        void HandleMessage(string message);

        void Stop();
    }
}
