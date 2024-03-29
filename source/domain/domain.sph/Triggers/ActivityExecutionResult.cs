﻿namespace Bespoke.Sph.Domain
{
    public class ActivityExecutionResult : DomainObject
    {
        public ActivityExecutionStatus Status { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public string[] NextActivities { get; set; }
        public string ActivityId { get; set; }
        public string Correlation { get; set; }

        public override string ToString()
        {
            return this.ToJsonString();
        }
    }

    public enum ActivityExecutionStatus
    {
        None,
        WaitingAsync,
        NotRun,
        Failed,
        Success,
    }
}