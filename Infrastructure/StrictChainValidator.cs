using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using module_1_task_3.Domain.Validator;

namespace module_1_task_3.Infrastructure
{
    public class StrictChainValidator : IStatusValidator<string>
    {
        private readonly Dictionary<string, string> _allowedTransitions;

        public StrictChainValidator(Dictionary<string, string> allowedTransitions)
        {
            _allowedTransitions = allowedTransitions;
        }
        public Result Validate(string currentStatus, string newStatus)
        {
            if (_allowedTransitions.TryGetValue(currentStatus, out var requiredNextStatus))
            {
                if (newStatus == requiredNextStatus)
                {
                    return Result.Ok();
                }
        
                return Result.Fail($"Invalid transition: Status \"{currentStatus}\" must transition to \"{requiredNextStatus}\" (not to \"{newStatus}\")");
            }

            if (currentStatus == _allowedTransitions.Values.Last())
            {
                return Result.Fail($"Invalid transition: Status {_allowedTransitions.Values.Last()} is final and cannot be changed.");
            }

            return Result.Fail($"Invalid transition: Status \"{currentStatus}\" not found in rules.");
        }
    }
}