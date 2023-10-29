using System.Text;
namespace UGHGame.GameEditor
{
    public delegate void DataTableCodeGenerator(DataTableProcessor dataTableProcessor , StringBuilder codeContent , object userData , bool isHotfixCSharp);
}
