using MarcusMedinaPro.ClassEnumerator;

Console.OutputEncoding = System.Text.Encoding.UTF8; // Enable emoji support

Console.WriteLine("🔎 Enumerating sample workflows via interface...");
var workflows = EnumerateClasses<IWorkflow>.GetClassesByInterface();
foreach (var workflow in workflows)
{
    Console.WriteLine($" • {workflow.Name} → {workflow.Description}");
}

Console.WriteLine();
Console.WriteLine("🧱 Listing registered base workflows without instantiating...");
var workflowTypes = EnumerateClasses<WorkflowBase>.ListClassesByInheritance();
foreach (var type in workflowTypes)
{
    Console.WriteLine($" • {type.FullName}");
}

Console.WriteLine();
Console.WriteLine("⚡ Demonstrating cache reuse");
var cached = EnumerateClasses<IWorkflow>.GetClassesByInterface(useCache: true);
var cachedAgain = EnumerateClasses<IWorkflow>.GetClassesByInterface(useCache: true);
Console.WriteLine($"Same reference from cache: {ReferenceEquals(cached, cachedAgain)}");

Console.WriteLine();
Console.WriteLine("🧹 Clearing cache and reloading");
EnumerateClasses<IWorkflow>.ClearCache();
var reloaded = EnumerateClasses<IWorkflow>.GetClassesByInterface(useCache: true);
Console.WriteLine($"Cache cleared, reference equals cached? {ReferenceEquals(cached, reloaded)}");

Console.WriteLine();
Console.WriteLine("✅ Demo complete");

// sample plugin definitions
public interface IWorkflow
{
    string Name { get; }
    string Description { get; }
}

public abstract class WorkflowBase : IWorkflow
{
    public abstract string Name { get; }
    public abstract string Description { get; }
}

public sealed class PaymentWorkflow : WorkflowBase
{
    public override string Name => "payment";
    public override string Description => "Processes card and ledger entries";
}

public sealed class SupportWorkflow : WorkflowBase
{
    public override string Name => "support";
    public override string Description => "Routes tickets based on skills";
}

public sealed class ExperimentWorkflow : IWorkflow
{
    public string Name => "experiment";
    public string Description => "Launches feature flags on Tuesday";
}
