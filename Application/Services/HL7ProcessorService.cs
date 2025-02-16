using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Messaging;
using Newtonsoft.Json;
using NHapi.Base.Parser;




namespace Application.Services
{
    public class HL7ProcessorService
    {
        private readonly IHL7MessageRepository _repository;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public HL7ProcessorService(IHL7MessageRepository repository, RabbitMQPublisher rabbitMQPublisher)
        {
            _repository = repository;
            _rabbitMQPublisher = rabbitMQPublisher;
        }


        public async Task<Guid> ProcessHL7Message(string hl7Message)
        {
            try
            {
                Console.WriteLine("Received HL7 message for processing.");

                var parsedData = ParseHl7(hl7Message);
                Console.WriteLine("HL7 message parsed successfully.");

                var jsonData = JsonConvert.SerializeObject(parsedData);
                Console.WriteLine("Converted HL7 to JSON.");

                var hl7Entity = new HL7Message
                {
                    Id = Guid.NewGuid(),
                    //PatientName = parsedData.ContainsKey("PID") ? parsedData["PID"].PatientName : "Unknown",
                    //RecordNumber = parsedData.ContainsKey("PID") ? parsedData["PID"].PatientID : "Unknown",
                    JsonData = jsonData
                };

                var id = await _repository.AddAsync(hl7Entity);
                Console.WriteLine($"Saved HL7 message to database with ID: {id}");

                 _rabbitMQPublisher.Publish(id, jsonData);
                Console.WriteLine("Published HL7 message to RabbitMQ.");

                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing HL7 message: {ex.Message}");
                throw;
            }
        }

        private dynamic ParseHl7(string hl7Message)
        {
            // Parse the HL7 message
            var parser = new PipeParser();
            var message = parser.Parse(hl7Message);  // This may throw an exception if parsing fails

            // Serialize to JSON to check if it works
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,  // Ignore circular references
                Formatting = Formatting.Indented  // Make the JSON more readable
            };

            var json = JsonConvert.SerializeObject(message, jsonSettings);
            return json;

        }

        




    }
}
