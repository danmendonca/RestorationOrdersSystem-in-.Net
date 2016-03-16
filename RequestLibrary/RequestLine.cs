namespace RequestLibrary
{
    class RequestLine
    {
        enum reqState { waiting, inProgress, ready };
        public Product Prod { get; protected set; }
        public int Qtt { get; protected set; }
        public int RequestNr { get; set; }
        public string Description { get; private set; }
        public int RequestState{get;set;}

        public RequestLine(Product prod, int qtt, string desc)
        {
            Prod = prod;
            Qtt = qtt;
            RequestNr = -1;

            if (desc != null)
                Description = desc;
            else Description = "";

            RequestState = (int)reqState.waiting;
        }

        public bool changeState()
        {
            switch((reqState)RequestState)
            {
                case reqState.waiting:
                    RequestState = (int)reqState.inProgress;
                    return true;
                case reqState.inProgress:
                    RequestState = (int)reqState.ready;
                    return true;
                default:
                    return false;
            }
        }
    }
}