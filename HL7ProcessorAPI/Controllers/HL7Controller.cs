using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HL7ProcessorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HL7Controller : ControllerBase
    {
        private readonly HL7ProcessorService _hl7ProcessorService;

        public HL7Controller(HL7ProcessorService hl7ProcessorService)
        {
            _hl7ProcessorService = hl7ProcessorService;
        }

        /// <summary>
        /// Processes an HL7 message.
        /// </summary>
        /// <param name="hl7Message">The HL7 message to process.</param>
        /// <returns>The unique ID of the processed message.</returns>
        [HttpPost("process")]
        public async Task<IActionResult> ProcessHL7Message([FromBody] string hl7Message)
        {

            hl7Message = "MSH|^~\\&|HIS|HOSPITAL|LAB|HOSPITAL|20250215120000||ADT^A01|MSG123456|P|2.3\r\nEVN|A01|20250215120000\r\nPID|1||123456^^^HOSPITAL^MR||DOE^JOHN^A||19850515|M|||123 MAIN ST^^NEW YORK^NY^10001||555-555-1000||S||123456789|987-65-4321\r\nNK1|1|DOE^JANE^M|SPOUSE|456 MAIN ST^^NEW YORK^NY^10001|(555)555-2000\r\nPV1|1|I|ICU^101^1^HOSPITAL||||12345^PHYSICIAN^DAVID^J|||||||||1234567^INSURANCE||||||||||||||||||20250215115959\r\nOBX|1|ST|1234-5^BLOOD PRESSURE^LN||120/80|mmHg|90-140|N|||F\r\nAL1|1|DA|300|PENICILLIN|SEVERE REACTION\r\n";
            var id = await _hl7ProcessorService.ProcessHL7Message(hl7Message);
            return Ok(new { MessageId = id });
        }

        /// <summary>
        /// Retrieves an HL7 message by its unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the HL7 message.</param>
        /// <returns>The HL7 message.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHL7Message(Guid id)
        {
            var message = "Swagger working"; // Replace with actual logic: await _hl7ProcessorService.GetHL7MessageById(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }
    }
}