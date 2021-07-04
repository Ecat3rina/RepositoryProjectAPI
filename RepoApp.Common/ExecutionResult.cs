using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.Common
{
    public class ExecutionResult
    {
        public ExecutionResult()
        {
            this.ExecutionStatus = ResultOutcome.OK;
            this.ValidationMessages = new Dictionary<string, string>();
        }

        public ResultOutcome ExecutionStatus { get; set; }
        public string validationResult { get; set; }

        public string Message { get; set; }

        public Dictionary<string, string> ValidationMessages { get; set; }

        public string LongMessage { get; set; }

        public object Data { get; set; }
    }
}
