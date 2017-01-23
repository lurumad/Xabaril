namespace Xabaril
{
    public class XabarilOptions
    {
        FailureMode _failureMode = FailureMode.Throw;

        public FailureMode FailureMode
        {
            get { return _failureMode; }
            set { _failureMode = value; }
        }
    }
}
