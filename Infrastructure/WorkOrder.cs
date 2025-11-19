using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using FluentResults;
using module_1_task_3.Domain.Validator;

namespace module_1_task_3.Infrastructure
{

    public class WorkOrder : IWorkOrder<string>
    {
        public string CurrentStatus
        {
            get => field;
            protected set => field = value;
        }

        public IStatusValidator<string> StatusValidator { get; protected set; }


        public WorkOrder(IStatusValidator<string> statusValidator, string initialStatus)
        {
            StatusValidator = statusValidator ?? throw new ArgumentNullException(nameof(statusValidator));
            CurrentStatus = initialStatus ?? throw new ArgumentNullException(nameof(initialStatus));
           
        }


        public Result TryUpdateStatus(string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
            {
                return Result.Fail("New status cannot be empty.");
            } 

            var result = StatusValidator.Validate(CurrentStatus,newStatus);
            if (result.IsSuccess)
            {
                CurrentStatus = newStatus;
                
            }
            return result;

            
        }

    }
}