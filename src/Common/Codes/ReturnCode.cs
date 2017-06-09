namespace JYW.ThesisMMO.Common.Codes {
    public enum ReturnCode : byte{
        OK = 0,
        StartCasting,
        Declined,
        InvalidOperationParameter = 50,
        OperationNotAllowed,
        OperationNotSupported
    }
}
