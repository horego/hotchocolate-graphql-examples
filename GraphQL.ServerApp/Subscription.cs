using System;
using HotChocolate;
using HotChocolate.Types;
using System.Reactive.Linq;
namespace GraphQL.ServerApp
{
    public class Subscription
    {
        private readonly IObservable<TestResult> m_Source;

        public Subscription()
        {
            m_Source = Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(5))
                .Select(l => new TestResult(l, $"My name is {l}"));
        }

        [Subscribe(With = nameof(Subscribe))]
        public SubscriptionTestResult OnTestChanged([EventMessage] TestResult changedRecord) =>
            changedRecord.Map();

        public IObservable<TestResult> Subscribe() =>
            m_Source;
    }

    public class SubscriptionTestResult
    {
        public long Id { get; }
        public string Name { get; }

        public SubscriptionTestResult(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public static class TestResultExtensions
    {
        public static SubscriptionTestResult Map(this TestResult item)
        {
            return new SubscriptionTestResult(item.Id, item.Name);
        }
    }

    public class TestResult
    {
        public long Id { get; }
        public string Name { get; }

        public TestResult(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
