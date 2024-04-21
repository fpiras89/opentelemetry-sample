using System.Diagnostics.Metrics;

namespace Examples.Service.Application.Metrics
{
    public class TodoMetrics
    {
        private readonly Counter<int> todoCreatedCounter;
        private readonly Counter<int> todoDoneCounter;
        private readonly Counter<int> todoDeletedCounter;

        public TodoMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("Todo");
            todoCreatedCounter = meter.CreateCounter<int>("todo.created", description: "Number of created todos");
            todoDoneCounter = meter.CreateCounter<int>("todo.done", description: "Number of done todos");
            todoDeletedCounter = meter.CreateCounter<int>("todo.deleted", description: "Number of deleted todos");  
        }

        public void TodoCreated()
        {
            todoCreatedCounter.Add(1);
        }

        public void TodoDone()
        {
            todoDoneCounter.Add(1);
        }

        public void TodoDeleted()
        {
            todoDeletedCounter.Add(1);
        }
    }
}
