namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 检查资源流程
    /// </summary>
    internal class BuiltinProcedureCheckResources:BuiltinProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
    }
}
