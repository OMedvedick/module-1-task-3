using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;

namespace module_1_task_3.Domain.Validator
{
    public interface IStatusValidator <TValuet>
    {
        Result Validate(TValuet currentStatus, TValuet newStatus);
        
    }
}