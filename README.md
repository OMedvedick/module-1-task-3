# Work Order Status Manager

A simple, strict state machine for managing Work Order statuses in .NET.

This library decouples the **state** (Work Order) from the **rules** (Validator), ensuring that a Work Order can only transition through a specific, pre-defined sequence of steps. It uses [FluentResults](https://github.com/altmann/FluentResults) to avoid throwing exceptions for validation failures.

## Why use this?

Instead of hardcoding `if-else` statements inside your entities, this library forces status changes to follow a strict chain (e.g., `Created` -\> `InProgress` -\> `Done`). If you try to skip a step (like jumping from `Created` straight to `Done`), the update fails gracefully (without Exception, check the Result object for error messages).

## Quick Start

Here is how the logic works in practice. You define a "chain" of allowed transitions, and the `WorkOrder` respects it.

```csharp
// 1. Define the rules: Key = Current Status, Value = The ONLY allowed Next Status
var transitionRules = new Dictionary<string, string>
{
    { "Created", "InProgress" },
    { "InProgress", "Completed" },
    { "Completed", "Archived" }
};

// 2. Initialize the validator and the work order
var validator = new StrictChainValidator(transitionRules);
var order = new WorkOrder("Created", validator);

// 3. Try to update
// Success: "Created" -> "InProgress" is allowed
var result1 = order.TryUpdateStatus("InProgress"); 

// Fail: "InProgress" cannot jump to "Archived" directly
var result2 = order.TryUpdateStatus("Archived"); 

if (result2.IsFailed)
{
    Console.WriteLine($"Update failed: {result2.Errors.First().Message}");
}
```

## Project Overview

The solution is split into two straightforward layers:

### 1\. Domain (The Contracts)

  * **`IWorkOrder<T>`**: Defines an object that has a status and strict update logic.
  * **`IStatusValidator<T>`**: The logic gatekeeper. It decides if `Status A` can become `Status B`.

### 2\. Infrastructure (The Logic)

  * **`StrictChainValidator`**: Implements a strict "one-way" street logic. It holds a dictionary where every status points to exactly one valid next step.
  * **`WorkOrder`**: The concrete entity.

## Setup

Standard .NET setup. No magic here.

1.  **Clone & Build:**

    ```bash
    git clone <REPO_URL>
    dotnet build
    ```

2.  **Reference it:**
    Add the compiled DLL (or project reference) to your main application.

3.  **Dependencies:**
    Make sure you restore NuGet packages to pull in `FluentResults`.
