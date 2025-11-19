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
        
                return Result.Fail($"Недопустимый переход: Статус \"{currentStatus}\" должен перейти в \"{requiredNextStatus}\" (а не в \"{newStatus}\")");
            }

            if (currentStatus == "Завершен")
            {
                return Result.Fail($"Недопустимый переход: Статус \"Завершен\" является финальным и не может быть изменен.");
            }

            return Result.Fail($"Недопустимый переход: Статус \"{currentStatus}\" не найден в правилах.");
        }
    }
}