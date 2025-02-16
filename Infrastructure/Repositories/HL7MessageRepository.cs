using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class HL7MessageRepository : IHL7MessageRepository
    {
        private readonly AppDbContext _context;

        public HL7MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(HL7Message message)
        {
            _context.HL7Messages.Add(message);
            await _context.SaveChangesAsync();
            return message.Id;
        }

        public async Task<HL7Message> GetByIdAsync(Guid id)
        {
            return await _context.HL7Messages.FindAsync(id);
        }
    }
}
