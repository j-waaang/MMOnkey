namespace JYW.ThesisMMO.Common.Codes {
    public enum ReturnCode : byte{
        OK = 0,
        Declined,
        InvalidOperationParameter = 50,
        OperationNotAllowed,
        OperationNotSupported
    }
}
