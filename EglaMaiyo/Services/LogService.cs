using EglaMaiyo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EglaMaiyo.Services
{
    public class LogService
    {
        private readonly List<ApiLog> _logs = new();

        public Task LogAsync(ApiLog log)
        {
            // Insert newest logs at the top
            _logs.Insert(0, log);
            return Task.CompletedTask;
        }

        public Task<List<ApiLog>> GetLogsAsync()
        {
            return Task.FromResult(_logs);
        }
        public List<ApiLog> GetLogs()
        {
            return _logs;
        }

        public void AddLog(string endpoint, string status, string message, string? response = null, string? source = null)
        {
            _logs.Insert(0, new ApiLog
            {
                Timestamp = DateTime.Now,
                Endpoint = endpoint,
                Status = status,
                Message = message,
                Response = response,
                Source = source
            });
        }

        internal Task AddLogAsync(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }
    }
}
