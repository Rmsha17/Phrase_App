using Phrase_App.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrase_App.Infrastructure.Services
{
    public class LogService : ILogService
    {
        private readonly PhraseDbContext _context;

        public LogService(PhraseDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LogErrorAsync(ErrorLog log)
        {
            try
            {
                _context.ErrorLogs.Add(log);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical Database Error: {ex.Message}");
                return false;
            }
        }
    }
}
