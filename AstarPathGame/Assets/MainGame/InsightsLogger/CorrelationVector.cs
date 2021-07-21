using System;

namespace InsightsLogger
{
    [Serializable]
    public class CorrelationVector
    {
        public string OperationId { get; private set; }
        public double TimeSinceLastEventInMs { get; private set; }

        private DateTime _timeOfLastStep;
        private string _baseVector;
        private int _currentStep;

        [ThreadStatic] public static CorrelationVector correlationVector = new CorrelationVector("");        

        public string Vector
        {
            get
            {
                return _baseVector + "." + _currentStep;
            }
        }

        public CorrelationVector(string operationId)
        {
            OperationId = operationId;
            _baseVector = operationId.ToString();
            _currentStep = 0;
            TimeSinceLastEventInMs = 0;
            _timeOfLastStep = DateTime.UtcNow;
        }

        public static void Reset(string operationId)
        {
            correlationVector.OperationId = operationId;
            correlationVector._baseVector = operationId.ToString();
            correlationVector._currentStep = 0;
            correlationVector.TimeSinceLastEventInMs = 0;
            correlationVector._timeOfLastStep = DateTime.UtcNow;
        }

        public CorrelationVector Extends()
        {
            var newVector = new CorrelationVector(this._baseVector + "." + this._currentStep);
            newVector.OperationId = this.OperationId;
            this._currentStep++;
            return newVector;
        }

        public CorrelationVector Increment()
        {
            TimeSinceLastEventInMs = (DateTime.UtcNow - _timeOfLastStep).TotalMilliseconds;
            _timeOfLastStep = DateTime.UtcNow;
            this._currentStep++;
            return this;
        }
    }
}
