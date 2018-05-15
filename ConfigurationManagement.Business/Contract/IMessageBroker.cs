using ConfigurationManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManagement.Business.Contract
{
    public interface IMessageBroker
    {
        void AddMessage(Record record);
        void ConsumeMessage();
    }
}
