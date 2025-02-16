using NHapi.Base.Model;
using NHapi.Model.V21.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Utilities
{

    public static class HL7ToJsonConverter
    {
        public static string ConvertToJson(IMessage message)
        {
            var messageDict = new Dictionary<string, object>();

            // Cast to a specific message type (e.g., ADT_A01)
            if (message is ADT_A01 adtMessage)
            {
                // Access segments
                messageDict["MSH"] = adtMessage.MSH.ToString();
                messageDict["EVN"] = adtMessage.EVN.ToString();
                messageDict["PID"] = adtMessage.PID.ToString();
                // Add more segments as needed
            }
            else
            {
                throw new NotSupportedException($"Message type {message.GetType().Name} is not supported.");
            }

            // Serialize to JSON
            return JsonSerializer.Serialize(messageDict);
        }
    }
}
